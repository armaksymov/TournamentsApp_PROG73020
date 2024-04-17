import requests
import json
from urllib3.exceptions import InsecureRequestWarning

# Suppress only the single InsecureRequestWarning from urllib3
requests.packages.urllib3.disable_warnings(category=InsecureRequestWarning)

# API URLs
API_BASE_URL = 'https://localhost:7082'
TEAMS_API_URL = f'{API_BASE_URL}/Teams/REST/Add'
TOURNAMENTS_API_URL = f'{API_BASE_URL}/Tournaments/REST/Add'
MEMBERS_API_URL = f'{API_BASE_URL}/Members/REST/Add'
LIST_TEAMS_API_URL = f'{API_BASE_URL}/Teams/REST/List'
LIST_TOURNAMENTS_API_URL = f'{API_BASE_URL}/Tournaments/REST/List'
LIST_PLAYERS_API_URL = f'{API_BASE_URL}/Members/REST/List'

def make_request(method, url, json_data=None, headers=None):
    headers = {'Content-Type': 'application/json'} if headers is None else headers
    response = requests.request(method, url, json=json_data, headers=headers, verify=False)
    print('Status Code:', response.status_code)
    if method.lower() == 'get':
        try:
            parsed = json.loads(response.text)
            print('Response Content:', json.dumps(parsed, indent=4))
        except json.JSONDecodeError:
            print('Response Content:', response.text)

def test_add_team():
    json_data = {"TeamName": "Freedom Fighters", "MainTeamGameId": "val"}
    make_request('post', TEAMS_API_URL, json_data=json_data)

def test_add_tournament():
    json_data = {
        "Address": {"StreetAddress": "123 Some St", "TournamentCity": "Toronto", "TournamentCountry": "Canada", "TournamentPostalCode": "H0H 0H0"},
        "Tournament": {"TournamentName": "Valorant Esports", "TournamentDate": "2024-07-01", "TournamentGameId": "val", "TeamIds": ["ConCE", "FreFig"]}
    }
    make_request('post', TOURNAMENTS_API_URL, json_data=json_data)

def test_add_player():
    json_data = {"firstName": "Joe", "lastName": "Doe", "email": "janedoe@example.com", "dateOfBirth": "1990-01-01", "playerRoleId": "P"}
    make_request('post', MEMBERS_API_URL, json_data=json_data)

def get_teams():
    make_request('get', LIST_TEAMS_API_URL)

def get_tournament_details():
    make_request('get', LIST_TOURNAMENTS_API_URL)

def test_get_player():
    make_request('get', LIST_PLAYERS_API_URL)

def quit_program():
    print("Exiting program...")
    exit()

def main():
    actions = {
        "1": test_add_team,
        "2": test_add_tournament,
        "3": test_add_player,
        "4": get_teams,
        "5": get_tournament_details,
        "6": test_get_player,
        "7": quit_program
    }
    
    while True:
        print("\nMenu:")
        for i in range(1, 8):
            print(f"{i}. {actions[str(i)].__name__.replace('_', ' ').title()}")
        choice = input("Enter your choice: ")
        action = actions.get(choice)
        if action:
            action()
        else:
            print("Invalid choice. Please try again.")

if __name__ == "__main__":
    main()
