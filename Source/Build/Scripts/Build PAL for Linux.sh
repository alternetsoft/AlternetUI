#!/bin/bash
set -euo pipefail
SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

# Prepare variables.
. "$SCRIPT_HOME/Set Artifact Paths for Linux.sh"

# Build
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
dotnet msbuild $SCRIPT_HOME/../Alternet.UI.Pal/Alternet.UI.Pal.proj

# Copy Alternet.UI.Pal build results.
rm -rf "$UI_PAL_ARTIFACTS_PATH"
mkdir -p "$UI_PAL_ARTIFACTS_PATH"
cp -a "$SCRIPT_HOME/../Alternet.UI.Pal/bin/Linux/Release/." "$UI_PAL_ARTIFACTS_PATH"