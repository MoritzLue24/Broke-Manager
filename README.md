# Broke-Manager
Eine Webapp, welche dir hilft deine Finanzen vorrausschauend zu planen und nicht broke zu werden

## Vorraussetzungen
- .NET 8
- Docker

## Developement
1. **Clone**: `git clone https://github.com/MoritzLue24/Broke-Manager`

2. **Configuration**:
    - `Api/Properties/launchSettings.json` für Env-Variablen, Port, etc.
    - `Api/appsettings.Development.json` für ConnectionStrings & alles C#-Interne
    - `./docker-compose.yml` -> `mssql-dev` für MSSql-Port

3. **SQLServer** starten: `docker compose up mssql-dev -d`
4. **Api** starten: `cd Api && dotnet watch run`

**Migrate**: `cd Api && dotnet ef database update`

Standartmäßig läuft die Api auf `http://localhost:5180`, kein HTTPS in der Dev-Umgebung. Der SQL-Server läuft für Dev auf Port 1432.
Der Developement-Stack benutzt kein Docker, außer für MSSQL.

## Publish
1. **Clone**: `git clone https://github.com/MoritzLue24/Broke-Manager`
2. **Configuration**:
    - Erstelle eine `./.env`, mit `./example.env` als Vorlage
    - Ports:
        - Docker-Intern, Nginx-Ports -> `./nginx.conf`
        - Docker-Intern, Api -> `./Api/Dockerfile`
        - Außerhalb, Nginx -> `./docker-compose.yml` -> `nginx`
        - Außerhalb, MSSQL -> `./docker-compose.yml` -> `mssql`
3. **Publish**: `docker compose up -d --build`

**Migrate**: `docker compose up migrate`

Das Docker-Netzwerk:
- Nginx, Port 80 -> Api, Port 5033

Außerhalb:
- Port 1433 -> SQL-Server
- Port 80 -> Nginx, Api
- TODO: ^ remove, Port 443 -> Nginx, Web