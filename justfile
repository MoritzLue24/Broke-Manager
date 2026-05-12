default:
    @just --list --unsorted

# Counts C# lines without */Migrations/*, */bin/*, */obj/*
count target=".":
    @find "{{target}}" -name "*.cs" \
        -not -path "*/Migrations/*" \
        -not -path "*/bin/*" \
        -not -path "*/obj/*" \
        | xargs wc -l \
        | sort -n
    @echo "C# line count"
    @echo "   $( find "{{target}}" -name "*.cs" \
        -not -path "*/Migrations/*" \
        -not -path "*/bin/*" \
        -not -path "*/obj/*" \
        | wc -l ) files"
    @echo "C# file count"

# Dotnet clean & removes bin & obj
clean:
    dotnet clean Broke-Manager.sln
    find . -type d \( -name bin -o -name obj \) -exec rm -rf {} +

# Creates a new migration in src/Infrastructure/Persistence/Migrations
migrate name:
    dotnet ef migrations add {{name}} \
        --project src/Infrastructure \
        --output-dir Persistence/Migrations

# Removes all migrations from src/Infrastructure/Persistence/Migrations
clear-migrations:
    rm -rf src/Infrastructure/Persistence/Migrations

# Updates database from src/Infrastructure
db-update:
    dotnet ef database update \
        --project src/Infrastructure

# Drops database from src/Infrastructure
db-drop:
    dotnet ef database drop \
        --project src/Infrastructure

test:
    dotnet test --nologo --verbosity minimal