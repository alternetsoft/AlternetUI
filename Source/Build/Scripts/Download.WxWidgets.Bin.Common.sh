#!/bin/bash
set -euo pipefail
SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

# Build
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
dotnet msbuild /t:DownloadAndExtractWxWidgetsBinWindows $SCRIPT_HOME/../Alternet.UI.Pal/Alternet.UI.Pal.proj

