default:
    @just --list

count:
    find . -name "*.cs" \
        -not -path "*/Migrations/*" \
        -not -path "*/bin/*" \
        -not -path "*/obj/*" \
        | xargs wc -l \
        | sort -n
    @echo "C# line count, without */Migrations/*, */bin/*, */obj/*"

clean:
    dotnet clean Broke-Manager.sln
    find . -type d \( -name bin -o -name obj \) -exec rm -rf {} +

migrate name:
    dotnet ef migrations add {{name}} \
        --project src/Infrastructure \
        --output-dir Persistence/Migrations

clear-migrations:
    rm -rf src/Infrastructure/Persistence/Migrations

db-update:
    dotnet ef database update \
        --project src/Infrastructure

db-drop:
    dotnet ef database drop \
        --project src/Infrastructure