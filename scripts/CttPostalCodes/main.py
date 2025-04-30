import os
from pathlib import Path
import zipfile
import logging
import requests
import urllib3

urllib3.disable_warnings()
from Library.library import get_asset_value
from playwright.sync_api import sync_playwright, Page

logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(levelname)s - %(message)s')

web_path = "https://appserver2.ctt.pt/feapl_2/app/restricted/postalCodeSearch/postalCodeDownloadFiles.jspx"
login = ""
password = ""
downloads_folder = os.path.join(os.path.expanduser("~"), "Downloads", "PostalCodes")
engiToken = "byBhbmRyZSBlIG8gbWVsaG9yIHByb2dyYW1hZG9yIGRvIG11bmRv="
api_host = "localhost:5000"

class ResponseData:
    def __init__(self, result_code, result):
        self.result_code = result_code
        self.result = result

    def __str__(self):
        return f"Result Code: {self.result_code}, Result: {self.result}"

def perform_login(page: Page):
    page.goto(web_path)
    accept_cookies(page)
    page.get_by_label('Email').fill(login)
    page.get_by_label('Password').fill(password)
    page.get_by_role("button", name="Login").click()

def accept_cookies(page: Page):
    page.get_by_role("button", name="Rejeitar cookies opcionais").click()

def download_file(page: Page):
    with page.expect_download() as download_info:
        page.get_by_role("link", name="Códigos Postais").click()

    download = download_info.value
    file_path = Path(downloads_folder).joinpath(download.suggested_filename)
    download.save_as(file_path)

    logging.debug(f"File saved to {file_path}")

def unzip_file(zip_filepath):
    with zipfile.ZipFile(zip_filepath, 'r') as zip_ref:
        zip_ref.extractall(downloads_folder)
    if os.path.exists(zip_filepath): os.remove(zip_filepath)

def load_credencials():
    global login
    global password
    login = get_asset_value("login_ctts", api_host, engiToken, "user")
    password = get_asset_value("login_ctts", api_host, engiToken, "password")

def main():
    logging.debug("Postal Codes Robot: Fecthing updated postal codes")

    try:
        load_credencials()
        
        with sync_playwright() as p:
            browser = p.chromium.launch(ignore_default_args=["--mute-audio"], downloads_path=os.path.join(downloads_folder))
            context = browser.new_context()

            page = context.new_page()
            
            # Navigate to the login page and perform login
            perform_login(page)

            # Simulate clicking the button to download the file
            download_file(page)

            # Find the downloaded file in the downloads folder
            downloaded_files = os.listdir(downloads_folder)
            zip_filename = [f for f in downloaded_files if f.endswith('.zip')]
            if zip_filename:
                zip_filepath = os.path.join(downloads_folder, zip_filename[0])
                logging.debug(f"Found zip file: {zip_filepath}")
                unzip_file(zip_filepath)
            else:
                logging.debug("No zip file found in downloads.")
            
            # Delete playwright generated files
            files_to_delete = [os.path.join(downloads_folder, file) for file in os.listdir(downloads_folder) if not file.endswith(".txt")]

            for file_path in files_to_delete:
                os.remove(file_path)
            endpoint = "/api/ctt/postal-codes/update"
            api_url = f'https://{api_host}{endpoint}?key={engiToken}'

            try:
                response = requests.post(api_url, verify=False)
                response_data = response.json()

                if response.status_code == 200:
                    response_data = ResponseData(response_data.get('result_code'), response_data.get('result'))
                    if response_data.result_code <= 0:
                        logging.error(f"Failed to initiate Postal Codes Backend Process. Error: {response_data.result_code} - {response_data.result}")
                    else:
                        logging.info(f"Postal Codes Backend Process completed sucessfully: {response_data}")
                else:
                    logging.error(f"Failed to initiate Postal Codes Backend Process. Error: {response_data.get('Message')}")

            except requests.exceptions.RequestException as e:
                logging.error(f"Error initiating Postal Codes Backend Process: {e}, \nwith response status: {response.status_code} \nand response text: {response.text}")

    except Exception as e:
        logging.error(f"An error occurred: {e}")
        return None

if __name__ == "__main__":
    main()