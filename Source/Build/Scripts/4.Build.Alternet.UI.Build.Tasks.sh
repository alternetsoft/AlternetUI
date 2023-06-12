#!/bin/bash
set -euo pipefail


pushd ../../Alternet.UI.Build.Tasks/Alternet.UI.Build.Tasks.ApiInfoCollector
dotnet build --property WarningLevel=0
popd

pushd ../../Alternet.UI.Build.Tasks
dotnet build --property WarningLevel=0
popd

