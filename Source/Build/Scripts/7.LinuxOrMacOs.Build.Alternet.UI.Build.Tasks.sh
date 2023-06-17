#!/bin/bash
set -euo pipefail

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

pushd ../../Alternet.UI.Build.Tasks/Alternet.UI.Build.Tasks.ApiInfoCollector
dotnet build --nologo --property WarningLevel=0
popd

pushd ../../Alternet.UI.Build.Tasks
dotnet build --nologo --property WarningLevel=0
popd

