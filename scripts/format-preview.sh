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

dotnet format $TYPE --severity $SEVERITY --verify-no-changes

echo "Type 'y' to accept: "
read input

if [ "$input" = "y" ]; then
    echo "Modifying files..."
    dotnet format $TYPE --severity $SEVERITY
    echo "Done."
else
    echo "Aborted, no files where modifyed."
    exit 1
fi