#!/bin/bash


help() {
    echo "Usage: $(basename "$0") [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  -s, --severity <'error'|'info'|'warn'>  Default='info'"
    echo "  -h,  --help                             Show this help"
}

SEVERITY="info"

# Parse arguments
while [ $# -gt 0 ]; do
    case "$1" in
        --help|-h)
            help
            exit 0
            ;;
        --severity|-s)
            SEVERITY="$2"
            shift 2
            ;;
        *)
            echo "Invalid option '$1'."
            echo "Type --help for help."
            ;;
    esac
done

if [[ "$SEVERITY" != "error" && "$SEVERITY" != "info" && "$SEVERITY" != "warn" ]]; then
    echo "Invalid severity option. --severity has to be in 'error', 'info', 'warn'."
    echo "Type --help for help."
    exit 1
fi

echo "Looking for formatting errors / warnings..."

output=$(dotnet format $TYPE --severity $SEVERITY --exclude src/Infrastructure/Persistence/Migrations --verify-no-changes 2>&1)
line_count=$(echo "$output" | wc -l)

echo ""

if [[ $line_count -le 1 && -z "$output" ]]; then
    echo "No errors / warnings found."
    exit 0
fi

echo "$output"
echo "$line_count errors / warnings found."
echo ""
echo -n "Type 'y' to format: "
read input

if [ "$input" = "y" ]; then
    echo "Modifying files..."
    dotnet format $TYPE --severity $SEVERITY --exclude src/Infrastructure/Persistence/Migrations
    echo "Done."
else
    echo "Aborted, no files where modified."
    exit 1
fi