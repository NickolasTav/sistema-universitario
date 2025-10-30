#!/usr/bin/env bash
# Run the Web project with dotnet watch from repository root
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR/Sistema.Universitario.Web" || exit 1
URL=${1:-http://localhost:8080}
echo "Starting Web project with Hot Reload on $URL"
dotnet watch run --urls "$URL"
