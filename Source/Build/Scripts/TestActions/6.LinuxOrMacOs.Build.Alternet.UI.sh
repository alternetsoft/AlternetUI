#!/bin/bash
set -euo pipefail

pushd ../../Alternet.UI
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
dotnet build --nologo --property WarningLevel=0
popd



