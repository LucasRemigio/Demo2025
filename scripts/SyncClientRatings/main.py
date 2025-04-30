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

BASE_URL = f"https://{api_host}/api/clients/ratings/sync-primavera"

endpoints = {
    "credit",
    "payment-compliance", 
    "historical-volume"
}

def make_request(strategy_type):
    url = f"{BASE_URL}/{strategy_type}?key={engiToken}"
    
    try:
        logging.debug(f"Making request to {strategy_type}")
        response = requests.post(url, verify=False)
        
        if response.status_code == 200:
            data = response.json()
            response_data = ResponseData(data.get('result_code'), data.get('result'), data.get('statistics'))

            if response_data.result_code <= 0:
                logging.error(f"Failed to update client ratings. Error: {response_data.result_code} - {response_data.result}")
            else:
                logging.info(f"Client ratings updated sucessfully: {response_data}")
        else:
            logging.error(f"Failed to get data for {strategy_type}: {response.status_code} \nand response text: {response.text}")
    
    except requests.exceptions.RequestException as e:
        logging.error(f"An error occurred: {e}")

def main():
    logging.debug("Updating and creating the clients ratings")

    # Make requests for each type
    for strategy_type in endpoints:
        make_request(strategy_type)

if __name__ == "__main__":
    main()