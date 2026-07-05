# Development Guide

## Vorraussetzungen
* Docker
* .NET 8 SDK

## Setup
1. Repository Clonen
    * `git clone https://github.com/MoritzLue24/Broke-Manager.git`

2. Packages runterladen, für IntelliSense
    * In ./Api/: `dotnet restore`

2. Environment Datei `.env`kopieren und anpassen:
    * `cp .env.example .env`
    * `nano .env` (oder dein Editor der Wahl)

4. Datenbank Migrationen ausführen:
    * (postgres docker service starten)
    * `just db-update`

## Ausführen
`just watch-run`
oder `just run`

## Bei Ownership Problemen ?
`sudo chown -R "$(id -u):$(id -g)" Api/bin Api/obj`
