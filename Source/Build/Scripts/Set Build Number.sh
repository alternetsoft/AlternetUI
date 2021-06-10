#!/bin/bash
set -euo pipefail
SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
dotnet run --project "$SCRIPT_HOME/../../Tools/VersionTool/VersionTool.csproj" -- set-build-number $1