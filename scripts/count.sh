#!/bin/bash

help() {
    echo "Usage: $(basename "$0") [TARGET_DIR] [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  -ip, --ignore_path <dir>    Ignore a directory"
    echo "  -if, --ignore_file <file>   Ignore a file"
    echo "  -h,  --help                 Show this help"
}

IGNORE_PATHS=()
IGNORE_FILES=()

# Parse arguments
while [ $# -gt 0 ]; do
    case "$1" in
        --help|-h)
            help
            exit 0
            ;;
        --ignore_path|-ip)
            IGNORE_PATHS+=("$2")
            shift 2
            ;;
        --ignore_file|-if)
            IGNORE_FILES+=("$2")
            shift 2
            ;;
        *)
            TARGET_DIR="$1"
            shift
            ;;
    esac
done

# Tests if TARGET_DIR is zero-length
if [ -z "$TARGET_DIR" ]; then
  TARGET_DIR="."
fi

# Tests if target dir exists
if [ ! -d "$TARGET_DIR" ]; then
    echo "Error: directory '$TARGET_DIR' does not exist."
    exit 1
fi

# Build find command
FIND_CMD=(find "$TARGET_DIR" -name "*.cs"
    -not -path "*/bin/*"
    -not -path "*/obj/*"
)
for IGNORE in "${IGNORE_PATHS[@]}"; do
    FIND_CMD+=(-not -path "*/$IGNORE/*")
done
for IGNORE in "${IGNORE_FILES[@]}"; do
    FIND_CMD+=(-not -name "$IGNORE")
done

# Run
"${FIND_CMD[@]}" | xargs wc -l | sort -n