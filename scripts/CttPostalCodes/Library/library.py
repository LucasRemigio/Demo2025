import mimetypes
import shutil
import json
import ssl
import os
from shutil import copyfile
import subprocess
import time
from docx import Document
import pandas as pd
import requests
import urllib.request
from openpyxl.styles import PatternFill
from datetime import datetime, timedelta
from openpyxl import load_workbook
import xlwings as xw # type: ignore
from dateutil.relativedelta import relativedelta

engiToken = "byBhbmRyZSBlIG8gbWVsaG9yIHByb2dyYW1hZG9yIGRvIG11bmRv="
referer = "https://hrplus.hrbsolutions.eu/engimatrix"

def kill_excel_process():
    try:
        # For Windows
        if os.name == 'nt':
            subprocess.call(["taskkill", "/F", "/IM", "excel.exe"])
        # For MacOS and Linux
        else:
            subprocess.call(["pkill", "-f", "Microsoft Excel"])
    except Exception as e:
        print(f"Error killing Excel process: {e}")

def call_ss_api_and_get_result(endpoint, body):
    base_url = "https://hrplus.hrbsolutions.eu"
    url = f'{base_url}{endpoint}'

    # Convert data to JSON payload
    payload = json.dumps(body)

    # Define headers
    headers = {
        'Content-Type': 'application/json',
        'Referer': referer
    }

    # Create SSL context
    context = ssl.create_default_context()
    context.check_hostname = False
    context.verify_mode = ssl.CERT_NONE

    try:
        # Make the API request with proper payload
        res = requests.post(url, headers=headers, data=payload, verify=False)

        # Check if the response status is 200 (OK)
        if res.status_code == 200:
            return res.json()
        else:
            # Print error message
            print(f"Error calling API: {res.status_code}")
            return None

    except Exception as e:
        # Print the exception message
        print("An error occurred:", e)
        return None

def process_smartsheet_data(token, sheet_name, save_attachments, endpoint, body):
    # Make API call and retrieve data
    response = call_ss_api_and_get_result(endpoint, body)

    if response:
        # Extract the data from the response
        data = response.get('data')
        if data is not None:
            # Check if 'data' field is present in the response and is a non-empty list
            if isinstance(data, str):
                try:
                    # Parse the JSON string to convert it into a Python object
                    data = json.loads(data)
                except json.JSONDecodeError as e:
                    print("Error parsing JSON data:", e)
                    return None
            if isinstance(data, list) and len(data) > 0:
                # Convert the list of dictionaries to a DataFrame
                df = pd.DataFrame(data)
                return df
            else:
                print("Data should be a non-empty list.")
                print("Data:", data)
        else:
            print("No 'data' field found in API response.")
    else:
        print("Failed to retrieve data from Smartsheet.")
    return None

def update_smartsheet_column(token, sheet_name, column_to_check, value_to_check, column_name_to_update, value_to_update):
    try:
        # Define the endpoint for updating the Smartsheet column
        endpoint = '/engimatrix/api/intelligentDocWeb/UpdatSmartsheetColumn'

        value_to_check = str(value_to_check)
        value_to_update = str(value_to_update)
        column_to_check = str(column_to_check)
        column_name_to_update = str(column_name_to_update)

        # Define the body for the API request
        body = {
            "token": token,
            "smarsheetName": sheet_name,
            "columnToCheck": column_to_check,
            "valueToCheck": value_to_check,
            "columnNameToUpdate": column_name_to_update.strip(),
            "valueToUpdate": value_to_update
        }

        # Make the API call to update the Smartsheet column
        response = process_smartsheet_data(token, sheet_name, False, endpoint, body)

        print(f"Column '{column_name_to_update}' updated successfully.")
        return response

    except Exception as e:
        print(f"An error occurred: {str(e)}")
        return None

def download_templates_from_ss(attachments, templates_folder_path):
    # Create the attachment folder if it doesn't exist
    os.makedirs(templates_folder_path, exist_ok=True)

    downloaded_templates = []

    for attachment in attachments:
        file_name_all = attachment.get("fileName", "")
        file_name = os.path.splitext(file_name_all)[0] #Removing extension
        if file_name in [extract_file_name(templates_folder_path)]:
            attachment_url = attachment["url"]
            attachment_path = os.path.join(templates_folder_path, file_name_all)

            try:
                if urllib.parse.urlparse(attachment_url).scheme in ('http', 'https'):
                    urllib.request.urlretrieve(attachment_url, attachment_path)
                    print(f"Template '{file_name}' downloaded successfully.")

                    # Return the attachment file name if downloaded successfully
                    downloaded_templates.append(file_name)
            except Exception as e:
                print(f"Error downloading attachment '{file_name}': {str(e)}")
                continue

    if downloaded_templates:
        return downloaded_templates
    else:
        print("No matching templates found for download.")
        # Return None if no suitable attachment is found or downloaded
        return None

def extract_file_name(template_path):
    base_name = os.path.basename(template_path)  # Get the file name with extension
    file_name, _ = os.path.splitext(base_name)  # Remove the extension
    return file_name

# Function to zip a folder
def zip_folder(folder_path, output_path):
    shutil.make_archive(output_path, 'zip', folder_path)

def send_email_python(client_id, client_secret, tenant_id, user_id_or_email, subject, content, recipient_emails, attachments):
    # Authenticate and obtain access token
    token_url = f'https://login.microsoftonline.com/{tenant_id}/oauth2/v2.0/token'
    token_data = {
        'grant_type': 'client_credentials',
        'client_id': client_id,
        'client_secret': client_secret,
        'scope': 'https://graph.microsoft.com/.default'
    }
    response = requests.post(token_url, data=token_data)
    access_token = response.json()['access_token']

    # Prepare recipient list
    to_recipients = [{"emailAddress": {"address": email}} for email in recipient_emails]

    # Try to send an e-mail
    graph_url = f'https://graph.microsoft.com/v1.0/users/{user_id_or_email}/sendMail'
    email_data = {
        "message": {
            "subject": subject,
            "body": {
                "contentType": "Text",
                "content": content
            },
            "toRecipients": to_recipients
        }
    }

    if attachments:
        email_data['message']['attachments'] = attachments

    headers = {
        'Authorization': 'Bearer ' + access_token,
        'Content-Type': 'application/json'
    }
    response = requests.post(graph_url, json=email_data, headers=headers)

    # Process response
    if response.status_code == 202:
        print('Email sent successfully.')
    else:
        print('Failed to send email:', response.status_code, response.text)

def get_asset_value(asset_name, api_host,  engiToken = engiToken, field = "text"):
    endpoint = f"/api/runscripts/getAssetByName?assetName={asset_name}&engiToken={engiToken}"
    url = f'https://{api_host}{endpoint}'
    headers = {
        'Content-Type': 'application/json',
        'Referer': referer
    }

    # Create SSL context
    context = ssl.create_default_context()
    context.check_hostname = False
    context.verify_mode = ssl.CERT_NONE

    try:
        # Make the API request
        res = requests.get(url, headers=headers, verify=False)

        # Check if the response status is 200 (OK)
        if res.status_code == 200:
            #print(res.json()["asset"])
            data_json = res.json()["asset"]
            if field in data_json:
                print(f"Response Data ({field}):", data_json[field])
                return data_json[field]
            else:
                print(f"Field '{field}' not found in the response.")
                return None
        else:
            # Print error message
            print(f"Failed to get {asset_name}: {res.status_code}")
            return None

    except Exception as e:
        # Print the exception message
        print("An error occurred:", e)
        return None

def remove_transactions_with_status_new(api_host, engiToken):
    endpoint = "/engimatrix/api/runscripts/removeTransactionsWithStatusNew"
    api_url = f'https://{api_host}{endpoint}?engiToken={engiToken}'

    headers = {
        'Content-Type': 'application/json',
        'Referer': referer
    }

    try:
        response = requests.post(api_url, headers=headers, verify=False)
        response_data = response.json()

        if response.status_code == 200:
            print("Transactions with status New removed successfully")
        else:
            print(f"Failed to remove transactions. Error: {response_data.get('Message')}")

    except requests.exceptions.RequestException as e:
        print(f"Error removing transactions: {e}")

def add_transactions_to_queue(out_Df, queue_name, api_host, delete_new_transactions_of_queue):
    if delete_new_transactions_of_queue:
        endpoint = "/engimatrix/api/runscripts/removeTransactionsWithStatusNew"

        remove_transactions_with_status_new(api_host, engiToken)

    endpoint = f"/engimatrix/api/runscripts/addTransaction"
    api_url = f'https://{api_host}{endpoint}?engiToken={engiToken}'

    headers = {
        'Content-Type': 'application/json',
        'Referer': referer
    }

    failed_transactions = []

    for _, row in out_Df.iterrows():
        # Convert row to dictionary
        input_data = row.where(pd.notna(row), None).to_dict()

        payload = {
            "status_id": "NOVO",
            "queue_name": queue_name,
            "input_data": json.dumps(input_data)
        }

        try:
            response = requests.post(api_url, headers=headers, json=payload, verify=False)
            response_data = response.json()
            # print(response_data)

            if response.status_code == 200:
                print(f"Transaction added successfully for {row['Nº Ordem de Serviço']}")
            else:
                print(f"Failed to add transaction for {row['Nº Ordem de Serviço']}. Error: {response_data.get('Message')}")
                failed_transactions.append(row)

        except requests.exceptions.RequestException as e:
            print(f"Error adding transaction for {row['Nº Ordem de Serviço']}: {e}")

        # Add a 5 seconds delay after each transaction so we can prevent errors
        time.sleep(5)

    if failed_transactions:
        print("Retrying failed transactions...")
        for row in failed_transactions:
            input_data = row.to_dict()
            payload = {
                "queue_name": queue_name,
                "input_data": json.dumps(input_data),
                "engiToken": engiToken
            }

            try:
                response = requests.post(api_url, headers=headers, json=payload)
                response_data = response.json()

                if response.status_code == 200 and response_data.get("Message") == "Success":
                    print(f"Transaction re-added successfully for {row['Nº Ordem de Serviço']}")
                else:
                    print(f"Failed again to add transaction for {row['Nº Ordem de Serviço']}. Error: {response_data.get('Message')}")

            except requests.exceptions.RequestException as e:
                print(f"Error re-adding transaction for {row['Nº Ordem de Serviço']}: {e}")

def edit_transaction(transaction_id, engiToken, api_host, **kwargs):
    endpoint = f"/engimatrix/api/runscripts/editTransaction?id={transaction_id}&engiToken={engiToken}"
    api_url = f'https://{api_host}{endpoint}'

    headers = {
        'Content-Type': 'application/json',
        'Referer': referer
    }

    started = kwargs.get("started")
    ended = kwargs.get("ended")

    payload = {
        "id": transaction_id,
        "status_id": kwargs.get("status_id"),
        "exception": kwargs.get("exception"),
        "output_data": kwargs.get("output_data")
    }

    # Format 'started' field if provided
    if started:
        formatted_started = datetime.strptime(started, "%m/%d/%Y %I:%M:%S %p").strftime("%Y-%m-%d %H:%M:%S")
        payload["started"] = formatted_started
    else:
        payload["started"] = kwargs.get("started")

    # Format 'ended' field if provided
    if ended:
        formatted_ended = datetime.strptime(ended, "%m/%d/%Y %I:%M:%S %p").strftime("%Y-%m-%d %H:%M:%S")
        payload["ended"] = formatted_ended
    else:
        payload["ended"] = kwargs.get("ended")

    # Remove keys with None values
    payload = {k: v for k, v in payload.items() if v is not None}

    try:
        response = requests.post(api_url, headers=headers, json=payload, verify=False)
        response_data = response.json()

        if response.status_code == 200:
            print(f"Transaction updated successfully")
        else:
            print(f"Failed to update transaction. Error: {response_data.get('Message')}")
    except requests.exceptions.RequestException as e:
        print(f"Error updating transaction: {e}")

def retrieve_transactions(queue_name, api_host):
    endpoint = f"/engimatrix/api/runscripts/getTransactionByQueueName"
    api_url = f'https://{api_host}{endpoint}?engiToken={engiToken}'

    headers = {
        'Content-Type': 'application/json',
        'Referer': referer
    }

    try:
        response = requests.get(api_url, headers=headers, params={"queue_name": queue_name, "engiToken": engiToken}, verify=False)
        response_data = response.json()

        if response.status_code == 200:
            transactions = response_data.get("transactions", [])

            if transactions is None:
                print(f"Failed to retrieve transactions. Error: {response_data.get('Message')}")
                return

        else:
            print(f"Failed to retrieve transactions. Error: {response_data.get('Message')}")

    except requests.exceptions.RequestException as e:
        print(f"Error retrieving transactions: {e}")

    return transactions

def send_feedback_email_python(client_id, client_secret, tenant_id, user_id_or_email, subject, recipient_emails, attachments=None, **kwargs):
    script_status = kwargs.get('script_status', '')
    direction = kwargs.get('direction', '')
    area = kwargs.get('area', '')
    process = kwargs.get('process', '')
    start_datetime = kwargs.get('start_datetime', '')
    end_datetime = kwargs.get('end_datetime', '')
    num_transactions = kwargs.get('num_transactions', 0)
    num_successes = kwargs.get('num_successes', 0)
    num_failures = kwargs.get('num_failures', 0)

    email_body = f"""
    <html>
    <body style="font-family: Arial, sans-serif; line-height: 1.4;">
        <div>Robot correu com {script_status}.</div>
        <div>Direção: {direction}</div>
        <div>Área: {area}</div>
        <div>Processo: {process}</div>
        <div>Data Hora Início: {start_datetime}</div>
        <div>Data Hora de Fim: {end_datetime}</div>
        <div>Nº Transações: {num_transactions}</div>
        <div>Nº Sucessos: {num_successes}</div>
        <div>Nº Insucessos: {num_failures}</div>
    </body>
    </html>
    """

    send_email(client_id, client_secret, tenant_id, user_id_or_email, subject, email_body, recipient_emails, attachments)

def insert_job_details(api_host, engiToken, queue_name, status_name, date_time, job_details):
    endpoint = f"/engimatrix/api/runscripts/insertJobDetails?engiToken={engiToken}"
    api_url = f'https://{api_host}{endpoint}'

    headers = {
        'Content-Type': 'application/json; charset=utf-8',
        'Referer': referer
    }

    payload = {
        "queue_name": queue_name,
        "status_name": status_name,
        "date_time": date_time,
        "job_details": job_details
    }

    try:
        response = requests.post(api_url, headers=headers, json=payload, verify=False)
        response_data = response.json()

        if response.status_code == 200:
            print("Job details inserted successfully")
        else:
            print(f"Failed to insert job details. Error: {response_data.get('Message')}")
    except requests.exceptions.RequestException as e:
        print(f"Error inserting job details: {e}")

def send_feedback_email(client_id, client_secret, tenant_id, user_id_or_email, subject, recipient_emails, api_host, engiToken, attachments=None, **kwargs):
    script_status = kwargs.get('script_status', '')
    direction = kwargs.get('direction', '')
    area = kwargs.get('area', '')
    process = kwargs.get('process', '')
    start_datetime = kwargs.get('start_datetime', '')
    end_datetime = kwargs.get('end_datetime', '')
    num_transactions = kwargs.get('num_transactions', 0)
    num_successes = kwargs.get('num_successes', 0)
    num_failures = kwargs.get('num_failures', 0)

    email_body = f"""
    <html>
    <body style="font-family: Arial, sans-serif; line-height: 1.4;">
        <div>Robot correu com {script_status}.</div>
        <div>Direção: {direction}</div>
        <div>Área: {area}</div>
        <div>Processo: {process}</div>
        <div>Data Hora Início: {start_datetime}</div>
        <div>Data Hora de Fim: {end_datetime}</div>
        <div>Nº Transações: {num_transactions}</div>
        <div>Nº Sucessos: {num_successes}</div>
        <div>Nº Insucessos: {num_failures}</div>
    </body>
    </html>
    """

    send_email(subject, email_body, recipient_emails, attachments, user_id_or_email, client_id, client_secret, tenant_id, api_host, engiToken)

def send_email(subject, email_body, recipient_emails, attachments, user_id_or_email, client_id, client_secret, tenant_id, api_host, engiToken):
    endpoint = "/engimatrix/api/runscripts/sendEmail"
    api_url = f'https://{api_host}{endpoint}'

    headers = {
        "Content-Type": "application/json",
        'Referer': referer
    }

    data = {
        "ClientId": client_id,
        "ClientSecret": client_secret,
        "TenantId": tenant_id,
        "Subject": subject,
        "Content": email_body,
        "RecipientEmails": recipient_emails,
        "UserIdOrEmail": user_id_or_email,
        "EngiToken": engiToken
    }

    if attachments is not None:
        for attachment in attachments:
            file_path = attachment['name']
            file_name = os.path.basename(file_path)
            file_mime_type, _ = mimetypes.guess_type(file_name)
            attachment['contentType'] = file_mime_type

        data["Attachments"] = attachments

    try:
        response = requests.post(api_url, json=data, headers=headers, verify=False)
        if response.status_code == 200:
            print('Email sent successfully from Python.')
        else:
            print(f'Failed to send email from Python: {response.status_code} - {response.text}')
    except Exception as e:
        print(f'Error sending email from Python: {e}')