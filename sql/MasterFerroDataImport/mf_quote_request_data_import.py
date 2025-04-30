import pandas as pd
from sqlalchemy import create_engine


'''

Execute this command to install the required packages:

pip install -r requirements.txt

'''

# Load the Excel file, specifying the sheet name and skipping the first row
excel_file = 'MF-Pricing-20240620.xlsx'
df = pd.read_excel(excel_file, sheet_name='BD-PRICING', skiprows=2)

# Drop any unnamed index columns that may exist
df = df.loc[:, ~df.columns.str.contains('^Unnamed')]

# Rename columns to match SQL table schema
df = df.rename(columns={
    'ID-ORC-ERP': 'quote_id_erp',
    'DATA': 'quote_date',
    'ID_CLT': 'client_id',
    'NOME_CLT': 'client_name',
    'SKU': 'product_code',
    'QD': 'quantity_requested',
    'PRECO_ERP': 'erp_price',
    'MODvsERP': 'erp_price_modification_percent',
    'ALERTA': 'alert_flag',
    'ESPECIAL (0/1)': 'special_flag',
    'PRECO_FINAL': 'final_price',
    'QD ENCOMENDA': 'order_quantity',
    'ID ENCOMENDA': 'order_id',
    'OBSERVACOES': 'observation',
    'PREÇO_UN': 'unit_price',
    'MB_CL': 'margin_percent',
    'DIF.P_ERP': 'price_difference_erp',
    'DIF._ERP%': 'price_difference_percent_erp',
    'DIF_Total_ERP': 'total_difference_erp',
    'DIF_Total_FIN': 'total_difference_final'
})

# Select only the columns required for the SQL table
df = df[[
    'quote_id_erp', 'quote_date', 'client_id', 'client_name', 'product_code',
    'quantity_requested', 'erp_price', 'erp_price_modification_percent',
    'alert_flag', 'special_flag', 'final_price', 'order_quantity', 'order_id',
    'observation', 'unit_price', 'margin_percent', 'price_difference_erp',
    'price_difference_percent_erp', 'total_difference_erp', 'total_difference_final'
]]

# Convert `client_id` to numeric, coerce errors to NaN, and filter out rows with NaN in `client_id`
df['client_id'] = pd.to_numeric(df['client_id'], errors='coerce')
df = df.dropna(subset=['client_id'])

# Clean and convert `quantity_requested` to decimal format
df['quantity_requested'] = (
    df['quantity_requested']
    .astype(str)  # Convert to string to handle text replacements
    .str.replace('\xa0', '', regex=True)  # Remove non-breaking spaces
    .str.replace(',', '.')  # Replace comma with dot for decimal format
    .astype(float)  # Convert to float for decimal precision
)

# Check and fill any missing data if necessary
if df['client_name'].isnull().any():
    print("Warning: Missing client names found. Filling with placeholder text.")
    df['client_name'] = df['client_name'].fillna('Unknown Client')

# Set data types to match SQL table requirements
df['quote_date'] = pd.to_datetime(df['quote_date'], errors='coerce')
df['alert_flag'] = df['alert_flag'].fillna(0).astype(int)
df['special_flag'] = df['special_flag'].fillna(0).astype(int)

# Define your database connection details
username = "root"
password = "#password#"  # Replace with your actual MySQL password
host = "localhost"
port = 3306
database = "masterferro_engimatrix"

# Create the database connection string
db_connection_string = f"mysql+pymysql://{username}:{password}@{host}:{port}/{database}"

# Create a connection to the database
engine = create_engine(db_connection_string)

# Insert the data into the `mf_quote_request` table
table_name = 'mf_quote_request'
try:
    df.to_sql(table_name, con=engine, if_exists='append', index=False)
    print("Data inserted successfully")
except Exception as e:
    print("Error inserting data:", e)
