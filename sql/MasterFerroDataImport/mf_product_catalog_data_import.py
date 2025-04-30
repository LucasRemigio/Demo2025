import pandas as pd
import os
from dotenv import load_dotenv
from sqlalchemy import create_engine, text  
from unidecode import unidecode


'''

Execute this command to install the required packages:

pip install -r requirements.txt

'''


# Load the Excel file, specifying the sheet name and skipping the first row
excel_file = 'MF-Pricing-20240620.xlsx'
df = pd.read_excel(excel_file, sheet_name='BD_SKU', skiprows=1)


print(f"Data from {excel_file} loaded successfully")


# Drop the first column (Unnamed: 0) if it exists
if 'Unnamed: 0' in df.columns:
    df = df.drop('Unnamed: 0', axis=1)


# Rename columns to match SQL table schema
df = df.rename(columns={
    'Artigo': 'product_code',
    'Descricao': 'description',
    'Unidade': 'unit',
    'STKActual': 'stock_current',
    'Moeda': 'currency',
    'PVP1': 'price_pvp',
    'PCMedio': 'price_avg',
    'PCUltimo': 'price_last',
    'DataUltEntrada': 'date_last_entry',
    'DataUltSaida': 'date_last_exit',
    'Familia': 'family_id',
    'PVP_REF_Mercado': 'price_ref_market'
})


if df['description'].isnull().any():
    print("Warning: Missing descriptions found. Filling with placeholder text.")
    df['description'] = df['description'].fillna('No Description Available')


# Load environment variables from .env file
load_dotenv()


# Retrieve database credentials from environment variables
username = os.getenv("DB_USERNAME")
password = os.getenv("DB_PASSWORD")
host = os.getenv("DB_HOST")
port = os.getenv("DB_PORT")
database = os.getenv("DB_NAME")


# Define your database connection string
db_connection_string = f"mysql+pymysql://{username}:{password}@{host}:{port}/{database}"


# Create a database connection
engine = create_engine(db_connection_string)


# Load lookup tables into dictionaries for fast access
def load_lookup_tables(engine):
    return {
        'type': pd.read_sql("SELECT name, id FROM mf_product_type", engine).set_index('name')['id'].to_dict(),
        'material': pd.read_sql("SELECT name, id FROM mf_product_material", engine).set_index('name')['id'].to_dict(),
        'shape': pd.read_sql("SELECT name, id FROM mf_product_shape", engine).set_index('name')['id'].to_dict(),
        'finishing': pd.read_sql("SELECT name, id FROM mf_product_finishing", engine).set_index('name')['id'].to_dict(),
        'surface': pd.read_sql("SELECT name, id FROM mf_product_surface", engine).set_index('name')['id'].to_dict()
    }


lookup_tables = load_lookup_tables(engine)


# Normalize the description to handle uppercase and accents
def normalize_text(input_text):
    return unidecode(input_text).upper()


# Define mappings for each attribute
type_mapping = {
    'BARRA': 'Barra',
    'VIGA': 'Viga',
    'VARAO': 'Varão',
    'CH.': 'Chapa',
    'CHAPA': 'Chapa',
    'TUBO': 'Tubo',
    'T.ACO': 'Tubo',
    'T. ACO': 'Tubo',
    'CANT.': 'Barra',
    'PAREDE': 'Diversos',
    'REDE': 'Rede',
    'MALHA': 'Rede',
    'PAINE': 'Painel',
    'PAINEIS': 'Painel',
}


material_mapping = {
    'FERRO': 'Ferro',
    'ACO': 'Aço',
    'ALUMINIO': 'Alumínio',
    'INOX': 'Inox',
    'COBRE': 'Cobre',
    'PVC': 'PVC',
    'TUBO EST.': 'Aço',
    'PLAST.': 'Plástico',
    'MALHA': 'Malha',
}


shape_mapping = {
    'RECT': 'Retangular',
    'VERGALHAO': 'Vergalhão',
    'QUAD': 'Quadrado',
    ' NO ': 'Nó',
    'HEXAGONAL': 'Hexagonal',
    'RED': 'Redondo',
    'VARAO': 'Redondo',
    'UPN': 'UPN',
    'IPN': 'IPN',
    'IPE': 'IPE',
    'HEB': 'HEB',
    'HEA': 'HEA',
    'HEM': 'HEM',
    'BARRA T': 'T',
    'ABA': 'ABA',
    'CORRIMAO': 'Corrimão',
    'DECOR.': 'Decorativo',
    'S/M': 'Canalização',
    'S/L': 'Canalização',
    'CANT.': 'Cantoneira',
    'ELEC': 'Eletrossoldado',
    'HERC.': 'Hércules',
}


finishing_mapping = {
    'GALV': 'Galvanizado',
    'GAL.': 'Galvanizado',
    'ZINCOR': 'Zincor',
    'ZINC': 'Zincado',
    'PRETO': 'Preto',
    'PRETA': 'Preto',
    'DECAP': 'Decapado',
    'COR-TEN': 'Corten',
    'VERDE': 'Verde',
    'BRANCO': 'Branco',
    'CINZA': 'Cinza',
}


surface_mapping = {
    'LISO': 'Liso',
    'LISA': 'Liso',
    'NERV': 'Nervurado',
    'ONDUL': 'Ondulado',
    'OND.': 'Ondulado',
    'LAM.': 'Laminado',
    'XADREZ': 'Xadrez',
    'GOTAS': 'Gotas',
    'S/M': 'Série Média',
    'S/L': 'Série Ligeira',
    'S/COST': 'Sem Costura',
    'C/COST': 'Com Costura',
    'ABAS IGUAIS': 'Abas Iguais',
    'AB.IGUAIS': 'Abas Iguais',
    'ABAS DESIGUAIS': 'Abas Desiguais',
    'AB.DESIG.': 'Abas Desiguais',
    'ABAS': 'Abas',
}


# General function to map description to IDs using the provided mapping
def map_description_to_id(description, mapping, lookup_table):
    description = normalize_text(description)
    for keyword, value in mapping.items():
        if keyword in description:
            return lookup_table.get(value)
    return None


# Specific functions for each attribute using the general mapping function
def get_type_id(description):
    return map_description_to_id(description, type_mapping, lookup_tables['type'])


def get_material_id(description):
    material_id = map_description_to_id(description, material_mapping, lookup_tables['material'])
    return material_id if material_id is not None else lookup_tables['material'].get('Ferro')


def get_shape_id(description):
    return map_description_to_id(description, shape_mapping, lookup_tables['shape'])


def get_finishing_id(description):
    return map_description_to_id(description, finishing_mapping, lookup_tables['finishing'])


def get_surface_id(description):
    return map_description_to_id(description, surface_mapping, lookup_tables['surface'])


# Apply functions for 'type_id', 'material_id', 'shape_id', and 'finishing_id'
df['type_id'] = df['description'].apply(get_type_id)
df['material_id'] = df['description'].apply(get_material_id)
df['shape_id'] = df['description'].apply(get_shape_id)
df['finishing_id'] = df['description'].apply(get_finishing_id)
df['surface_id'] = df['description'].apply(get_surface_id)

# Fetch existing product codes from the database
def fetch_existing_product_codes(engine, table_name):
    query = f"SELECT product_code FROM {table_name}"
    existing_codes_df = pd.read_sql(query, engine)
    return set(existing_codes_df['product_code'])

def clear_table(engine, table_name):
    try:
        with engine.connect() as connection:
            # Use DELETE or TRUNCATE based on your needs
            connection.execute(text(f"TRUNCATE TABLE {table_name}"))
            print(f"Cleared all records from the table '{table_name}'.")
    except Exception as e:
        print(f"Error clearing table '{table_name}': {e}")


# Retrieve both IDs and names from `mf_product_family` table
family_df = pd.read_sql("SELECT id, name FROM mf_product_family", engine)


# Filter out family IDs where the name contains 'DESCONTINUADOS'
#valid_family_ids = family_df[~family_df['name'].str.contains("DESCONTINUADOS", case=False, na=False)]['id'].tolist()


# Get the list of all valid family IDs (including "DESCONTINUADOS")
all_family_ids = family_df['id'].tolist()

# Ensure `df` has only numeric family IDs and exclude rows with null family IDs
df = df[df['family_id'].notnull() & df['family_id'].astype(str).str.match(r'^\d+$')]

# Filter rows where the family_id exists in the list of all family IDs
df = df[df['family_id'].astype(str).isin(all_family_ids)]


# Retrieve existing product codes
table_name = 't_product_catalog'

# ELIMINATE!!!
clear_table(engine, table_name)

existing_product_codes = fetch_existing_product_codes(engine, table_name)



# Filter out duplicates based on product_code
df['product_code'] = df['product_code'].apply(normalize_text)
df_to_insert = df[~df['product_code'].isin(existing_product_codes)]
    
# Filtrar os produtos
df = df[df['description'].str.contains(r'\b(?:BARRA|VIGA|VAR[ÃA]O|CHAPA|CH\.|TUBO|T\.A[CÇ]O|T\. A[CÇ]O|TE |T.|CANT.)\b', case=False, regex=True)]

# Filtrar apenas produtos com 'BARRA RECT.'
df_to_insert = df_to_insert[df_to_insert['description'].str.contains(r'\bBARRA RECT\.', case=False, regex=True)]


# Dynamically extract dimensions (length, width, height) in centimeters
if not df_to_insert.empty:
    # Set the length to 600 cm for all rows
    df_to_insert['length'] = 6000  # Fixed length in mm

   # Extract width and height using regex (accounting for optional spaces)
    df_to_insert['width'] = (
        df_to_insert['description']
        .str.extract(r'RECT\.\s*(\d+)')[0]  # Adjusted regex to allow optional spaces
        .astype(float)  # Allow float for NaN handling
        .fillna(0)      # Replace NaN with 0 (or drop them instead)
    )

    df_to_insert['height'] = (
        df_to_insert['description']
        .str.extract(r'x\s*(\d+)')[0]
        .astype(float)  # Allow float for NaN handling
        .fillna(0)      # Replace NaN with 0 (or you can drop them instead) 
    )

    df_to_insert['description_lucas'] = df_to_insert['description']

    #Remove dimensions from the description (anything after 'RECT.')
    df_to_insert['description'] = df_to_insert['description'].str.replace(
        r'(RECT\.\s*\d+\s*x\s*\d+\s*mm)', 'RECT.', regex=True
    )

# Filtrar produtos com 'VERGALHAO' na descrição
df_vergalhoes = df_to_insert[df_to_insert['description'].str.contains(r'VERGALHAO', case=False, regex=True)]

if not df_vergalhoes.empty:
    # Extract dimensions for width and height (assuming dimensions follow a similar pattern)
    df_vergalhoes['width'] = (
        df_vergalhoes['description']
        .str.extract(r'VERGALHAO\s*\(?\s*(\d+)\s*\)?')[0]  # Adjust regex for variations
        .astype(float)  # Convert to float for easier handling
        .fillna(0)  # Replace NaN with 0
    )

    df_vergalhoes['height'] = 0  # Vergalhões geralmente não têm altura; ajuste conforme necessário
    df_vergalhoes['length'] = 6000  # Comprimento fixo em mm para barras/vergalhões

    # Remover dimensões da descrição (deixar somente o nome do produto)
    df_vergalhoes['description'] = df_vergalhoes['description'].str.replace(
        r'\(?\s*\d+\s*\)?', '', regex=True  # Remove números associados às dimensões
    ).str.strip()  # Remove espaços desnecessários

    # Adicionar coluna para verificar como o produto foi extraído
    df_vergalhoes['description_lucas'] = df_vergalhoes['description']

    # Exibir os produtos extraídos
    print("Produtos isolados (VERGALHÃO) com dimensões:")
    print(df_vergalhoes[['product_code', 'description', 'width', 'height', 'length']])


if df_to_insert.empty:
    print("No new data to insert. All product codes already exist in the database.")
else:
    try:
        # Insert non-duplicate data into the database
        df_to_insert.to_sql(table_name, con=engine, if_exists='append', index=False)
        print(f"Inserted {len(df_to_insert)} new records into {table_name}.")
    except Exception as e:
        print("Error inserting data:", e)
