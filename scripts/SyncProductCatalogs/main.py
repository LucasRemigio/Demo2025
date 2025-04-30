import logging
import requests
import urllib3

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)
logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(levelname)s - %(message)s')

engiToken = "byBhbmRyZSBlIG8gbWVsaG9yIHByb2dyYW1hZG9yIGRvIG11bmRv="
api_host = "localhost:5000"

class Statistics:
    def __init__(self, time_elapsed_ms, total_syncs):
        self.time_elapsed_ms = time_elapsed_ms
        self.total_syncs = total_syncs

    def __str__(self):
        return f"Time Elapsed: {self.time_elapsed_ms} ms, Total Syncs: {self.total_syncs}"

class ResponseData:
    def __init__(self, result_code, result, statistics):
        self.result_code = result_code
        self.result = result
        self.statistics = statistics

    def __str__(self):
        return f"Result Code: {self.result_code}, Result: {self.result}, Statistics: {str(self.statistics)}"

def main():
    logging.debug("Updating and creating the product catalogs")

    endpoint = "/api/products/catalogs/sync-primavera"
    api_url = f'https://{api_host}{endpoint}?key={engiToken}'

    try:
        response = requests.post(api_url, verify=False)
        response_data = response.json()

        if response.status_code == 200:
            response_data = ResponseData(response_data.get('result_code'), response_data.get('result'), response_data.get('statistics'))
            if response_data.result_code <= 0:
                logging.error(f"Failed to update product catalogs. Error: {response_data.result_code} - {response_data.result}")
            else:
                logging.info(f"Product catalogs updated sucessfully: {response_data}")
        else:
            logging.error(f"Failed to update product catalogs. Error: {response_data.get('Message')}")

    except requests.exceptions.RequestException as e:
        logging.error(f"Error updating product catalogs: {e}, \nwith response status: {response.status_code} \nand response text: {response.text}")


if __name__ == "__main__":
    main()