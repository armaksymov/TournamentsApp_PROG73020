# A Tournament Management System for Conestoga Condors E-Sports Team

## Intro
  Welcome to the Tournament Management System repository. Tournament Management System is an application, where you can create and manage e-sports tournaments or even virtually any tournaments.
  This repository serves as a central hub for the collaborative efforts of our team in designing and implementing this application.

## Repository Contents
  This repository contains various resources and code related to our Tournament Management System.
  Here's a brief overview of what you can find here:
  - Project Source Code: The heart of our project. The source code for the logic and user interfaces of our application.
  - Testing and Validation: Test scripts, testing data, and all the validation procedures we used to ensure the reliability and performance of our application.
  - Issues: The record of all the issues we raised and encountered while working on this project, our discussions and comments. Feel free to open an issue if you have any questions or suggestions.
  - Wikis and ReadMes: The place to go for an elaborate description of our application, installation and distribution information, details on various use-cases, limitations, and known issues. This README file is one of the files you can go through to get a feel of what is buing built in this repository.

## Team Members
  - Abdelrahman Hashad
  - Ali Noureddine
  - Artem Maksymov
  - Mostafa Elbasiouny

## Objective and System Requirements
  Our primary goal is to develop an ASP.NET MVC web application, which would allow event organizers, team leaders, and event followers to effectively manage and spectate e-sports tournaments from any device with access to Internet. 
  As the base functionality we aim to:
  - Provide a solution for event managers to create, post, and manage e-sports tournaments through the event manager dashboard.
  - Provide a solution for event managers to easily invite teams to participate in their events.
  - Provide a solution for e-sports team leaders to easily respond to event invitations in their invbox, apply to participate in the events, and send out the tournament information to all team members.
  - Provide unauthorized system users (event spectators) to easily check tournament logs and team statistics for the previous tournaments.

## Getting Started
  To get a real feel of our application, please feel free to download the latest release by navigating to the releases tab and selecting the freshest version.
  Once you have the freshest release installed you will just need to set up a local SQL Server database or a containerized one, select the appropriate ConnectionString, and run the application.

  Please follow the following steps to install and setup our application on your workstation.
  - [ ] Step 1. Cloning the repository on your workstation:
  - Open Git Bash or Terminal and run:
    ```
    git clone https://github.com/armaksymov/TournamentsApp_PROG73020.git
    ```

  - [ ] Step 2. Set up the database.
  - To set up the database locally, please navigate to https://www.microsoft.com/en-ca/sql-server/sql-server-downloads and install one of the specialized edition which applies to you. 
  We sugest using SQL Server Express for a swift localhost experience.
   - To set up the database inside a Docker container, make sure you have Docker installed, open your console and run, of course replacing <your_password> with an actual password:
      ```
      docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=your_password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
      ```

  - [ ] Step 3. Select the applicable Connection String.
  - For the locally hosted database you shouldn't need any changes, just navigate to the Program.cs file and make sure you are using "LocalDB" as your Connection String:
    ```
    string? connectionString = builder.Configuration.GetConnectionString("LocalDB");
    ```
  - For the database Docker image, please navigate to the Program.cs file and replace the "LocalDB" Connection String with the "DockerDB" one:
    ```
    string? connectionString = builder.Configuration.GetConnectionString("DockerDB");
    ```
    And make sure to configure the ConnectionString, located inside appsettings.json file to make sure it uses your password (replace the <your_password> field with an actual password):
    ```
    "DockerDB": "Server=localhost;Database=TournamentsDB;Trusted_Connection=False;MultipleActiveResultSets=true;TrustServerCertificate=true;User Id=sa;Password=<your_password>;",
    ```

  - [ ] Step 4. Updating the database:
  - Simply open the Package Manager Console in Visual Studio and run:
    ```
    Update-Database
    ```
  - [ ] Step 5. Finally run!
  - Hit Ctrl + 5 and enjoy :)
    ```
    Ctrl + F5
    ```

## Contact Us
  Should you have any questions, suggestions, or encounter any issues while working with our application, please don't hesitate to raise an issue stating your question or feel free to reach out to any of our team members listed above.
  We are committed to developing our application as the best version of itself and value your input.

## Thank you for your interest in our project and for reading this file!
