#!/bin/bash

help() {
    echo "Usage: $(basename "$0") <output_file> [TARGET_DIR]"
    echo ""
    echo "Merges all .cs files into a single output file."
    echo ""
    echo "Arguments:"
    echo "  <output_file>   Path to the output file (required)"
    echo "  [TARGET_DIR]    Directory to search in (default: .)"
    echo ""
    echo "Options:"
    echo "  -h, --help      Show this help"
    echo ""
    echo "Example:"
    echo "  $(basename "$0") out.cs ./src"
}

if [ "$1" = "--help" ] || [ "$1" = "-h" ]; then
    help
    exit 0
fi

OUT="$1"

if [ -z "$OUT" ]; then
    echo "Error: output_file not set. See --help for more."
    exit 1
fi

# Tests if $2 (2nd cli arg) is zero-length
if [ -z "$2" ]; then
  TARGET_DIR="."
else
  TARGET_DIR="$2"
fi

# Tests if target dir exists
if [ ! -d "$TARGET_DIR" ]; then
    echo "Error: directory '$TARGET_DIR' does not exist."
    exit 1
fi

# Clear / create output file
> "$OUT"

COUNT=0

while IFS= read -r -d '' FILE; do
    if [ "$(realpath "$FILE")" = "$(realpath "$OUT")" ]; then
        echo "Skipping file $FILE..."
        continue
    fi

    echo "Writing file $FILE..."
    echo "// ============================================================" >> "$OUT"
    echo "// FILE: $FILE" >> "$OUT"
    echo "// ============================================================" >> "$OUT"
    cat "$FILE" >> "$OUT"
    echo "" >> "$OUT"

    COUNT=$((COUNT + 1))
done < <(find "$TARGET_DIR" -name "*.cs" -not -path "*/bin/*" -not -path "*/obj/*" -print0)

echo "$COUNT Files found."
echo "Content merged into: $OUT."