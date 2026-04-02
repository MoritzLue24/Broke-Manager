# Development Guide

## Vorraussetzungen
* Docker
* .NET 8 SDK

## Setup
1. Repository Clonen
    * `git clone https://github.com/MoritzLue24/Broke-Manager.git`

2. Environment Datei `.env` im Root erstellen mit folgenden Variablen:
    * `MICROSOFT_SQL_PASSWORD`  (Groß- & Kleinbuchstaben, Zahlen, Sonderzeichen)

3. `Api/appsettings.Development.json` Erstellen mit 
    * Default-Connection-String: `Server=localhost;Database=BrokeManagerDb;User Id=sa;Password=<DEIN_PASSWORD>;TrustServerCertificate=True;`

4. (optional) Migrieren:
    `docker compose up migrations`

## Api Watch run
Erstelle neuen container & führe `dotnet watch run` in /Api aus:
`docker compose up api`

## Build
Todo