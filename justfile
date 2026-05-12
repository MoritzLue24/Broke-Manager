# Shows this message
default:
    @just --list --unsorted

# Counts C# lines without */Migrations/*, and without tmp-merge.cs
count target=".":
    ./scripts/count.sh {{target}} -if tmp-merge.cs -ip Migrations

# Merges all C# files in specified dir into one tmp-merge.cs
merge-cs target="./src":
    ./scripts/merge-cs.sh tmp-merge.cs {{target}}

# Searches for TODO's in each file of the filetype (default .cs)
todo filetype="cs":
    @grep -rn --color=always "TODO\|FIXME" --include="*.{{filetype}}" .

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

# Tests all projects
test:
    dotnet test --nologo --verbosity minimal