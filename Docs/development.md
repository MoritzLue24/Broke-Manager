# Development Guide

## Vorraussetzungen
* Docker
* .NET 8 SDK

## Setup
1. Repository Clonen
    * `git clone https://github.com/MoritzLue24/Broke-Manager.git`

2. Packages runterladen, für IntelliSense
    * In ./Api/: `dotnet restore`

2. Environment Datei `.env` im Root erstellen mit folgenden Variablen:
    * `MICROSOFT_SQL_PASSWORD`  (Groß- & Kleinbuchstaben, Zahlen, Sonderzeichen)

3. `Api/appsettings.Development.json` Erstellen mit folgender Vorlage. Für ARM basierte prozessoren ist der SQL-Server **nicht verfügbar**, benutzte dann SQLite
    ```
    {
    "ConnectionStrings": {
        "SqlServer": "Server=database;Database=BrokeManagerDb;User Id=sa;Password=<DEIN_MSSQL_PASSWORT_HIER_EINSETZTEN>;TrustServerCertificate=True;",
        "Sqlite": "Data source=./app.db"
    },
    "DatabaseProvider": <"Sqlite"ODER"SqlServer">
    }
    ```

4. (optional) Migrieren:
    `docker compose up migrations`

## Api Watch run
Erstelle neuen container & führe `dotnet watch run` in /Api aus:
`docker compose up api`

## Build
Todo

## Bei Ownership Problemen ?
`sudo chown -R "$(id -u):$(id -g)" Api/bin Api/obj`