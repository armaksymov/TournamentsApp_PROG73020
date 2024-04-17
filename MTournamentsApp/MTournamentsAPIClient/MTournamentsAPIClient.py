import random
import requests
from urllib3.exceptions import InsecureRequestWarning

# Suppress only the single InsecureRequestWarning from urllib3 needed
requests.packages.urllib3.disable_warnings(category=InsecureRequestWarning)

# Global constants
TEAMS_API_URL = 'https://localhost:7082/Teams/REST/Add'
TOURNAMENTS_API_URL = 'https://localhost:7082/Tournaments/REST/Add'
MEMBERS_API_URL = 'https://localhost:7082/Members/REST/Add'
LIST_TEAMS_API_URL = 'https://localhost:7082/Teams/REST/List'
LIST_TOURNAMENTS_API_URL = 'https://localhost:7082/Tournaments/REST/List'
LIST_PLAYERS_API_URL = 'https://localhost:7082/Members/REST/List'


def test_add_team():
    json_data = {
        "TeamName": "Freedom Fighters",
        "MainTeamGameId": "halInf" 
    }
    headers = {'Content-Type': 'application/json'}
    response = requests.post(TEAMS_API_URL, json=json_data, headers=headers, verify=False)
    print('Status Code:', response.status_code)

def test_add_tournament():
    json_data = {
    "Address": {
        "StreetAddress": "a",
        "TournamentCity": "b",
        "TournamentCountry": "c",
        "TournamentPostalCode": "d"
    },
    "Tournament": {
        "TournamentName": "dedsdez",
        "TournamentDate": "2024-07-01",
        "TournamentGameId": "val",
        "TeamIds": ["ConCE", "dee"]
    }
}
    headers = {'Content-Type': 'application/json'}
    response = requests.post(TOURNAMENTS_API_URL, json=json_data, headers=headers, verify=False)
    print('Status Code:', response.status_code)

def test_add_player():
    json_data = {
    "firstName": "Joe",
    "lastName": "Mama",
    "email": "janedoe@example.com",
    "dateOfBirth": "1990-01-01",
    "playerRoleId": "P"
}
    headers = {'Content-Type': 'application/json'}
    response = requests.post(MEMBERS_API_URL, json=json_data, headers=headers, verify=False)
    print('Status Code:', response.status_code)

def get_teams():
    headers = {'Content-Type': 'application/json'}
    response = requests.get(LIST_TEAMS_API_URL, headers=headers, verify=False)
    print('Status Code:', response.status_code)
    print('Response Content:', response.text)

def get_tournament_details():
    headers = {'Content-Type': 'application/json'}
    response = requests.get(LIST_TOURNAMENTS_API_URL, headers=headers, verify=False)
    print('Status Code:', response.status_code)
    print('Response Content:', response.text)

def test_get_player():
    headers = {'Content-Type': 'application/json'}
    response = requests.get(LIST_PLAYERS_API_URL, headers=headers, verify=False)
    print('Status Code:', response.status_code)
    print('Response Content:', response.text)

def main():
    actions = {
       
        "1": test_add_team,
        "2": test_add_tournament,
        "3": test_add_player,
        "4": get_teams,
        "5": get_tournament_details,
        "6": test_get_player,
        "7": exit
    }
    
    while True:
        print("\nMenu:")
        print("1. Test Add Team")
        print("2. Test Add Tournament")
        print("3. Test Add Player")
        print("4. Get Teams")
        print("5. Get Tournament Details")
        print("6. Get Player Details")
        print("7. Exit")
        choice = input("Enter your choice: ")
        action = actions.get(choice)
        if action:
            action()
        else:
            print("Invalid choice. Please try again.")

if __name__ == "__main__":
    main()
