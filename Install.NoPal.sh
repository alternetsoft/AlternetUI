#!/bin/bash
set -euo pipefail
SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

# Build
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
dotnet msbuild -tl:off /t:InstallAlternetUI "-property:OnlyDebug=true;NoPal=true" $SCRIPT_HOME/Source/Build/Alternet.UI.Pal/Alternet.UI.Pal.proj
