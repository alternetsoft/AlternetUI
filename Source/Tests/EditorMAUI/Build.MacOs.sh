#!/bin/bash
set -euo pipefail
SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

echo =====================

dotnet build-server shutdown
set MSBUILDDISABLENODEREUSE=1
dotnet build -v n /m:1 -p:UseRazorBuildServer=false -p:UseSharedCompilation=false --disable-build-servers

