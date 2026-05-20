#!/bin/bash


help() {
    echo "Usage: $(basename "$0") [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  -s, --severity <'error'|'info'|'warn'>  Default='info'"
    echo "  -h,  --help                             Show this help"
}

# ich weiß es doch auch nicht
print_formatted_output() {
    local base_dir
    base_dir=$(pwd)

    # Strip \r and process real lines
    while IFS= read -r line; do
        # Skip empty lines
        if [ -z "$line" ]; then
            echo ""
            continue
        fi

        # Match pattern: /absolute/path/file.cs(line,col): severity code: message [project]
        local pattern='^(/[^(]+)\(([0-9]+,[0-9]+)\): (error|warn|info|warning) ([^:]+): (.+) \[.+\]$'
        if [[ "$line" =~ $pattern ]]; then
            local abs_path="${BASH_REMATCH[1]}"
            local location="${BASH_REMATCH[2]}"
            local severity="${BASH_REMATCH[3]}"
            local code="${BASH_REMATCH[4]}"
            local message="${BASH_REMATCH[5]}"

            # Make path relative
            local rel_path="./${abs_path#$base_dir/}"

            local CYAN='\033[0;36m'
            local YELLOW='\033[0;33m'
            local RED='\033[0;31m'
            local BLUE='\033[0;34m'
            local BOLD='\033[1m'
            local RESET='\033[0m'

            if [ "$severity" = "error" ]; then
                sev_color=$RED
            elif [ "$severity" = "warn" ] || [ "$severity" = "warning" ]; then
                sev_color=$YELLOW
            else
                sev_color=$BLUE
            fi

            printf "${CYAN}%s${RESET}${YELLOW}(%s)${RESET}: ${sev_color}${BOLD}%s${RESET} %s: %s\n" \
                "$rel_path" "$location" "$severity" "$code" "$message"
        else
            # Print non-matching lines only if not empty after stripping
            stripped=$(echo "$line" | tr -d '\r')
            [ -n "$stripped" ] && echo "$stripped"
        fi
    done <<< "$(echo "$output" | tr -d '\r' | grep -v '^$')"
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


if [[ -z "$output" && $? -eq 0 ]]; then
    echo ""
    echo "No errors / warnings found."
    exit 0
fi

echo ""
print_formatted_output
line_count=$(echo "$output" | wc -l)

echo ""
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