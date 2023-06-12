#!/bin/bash
set -euo pipefail

pushd ../../Alternet.UI.Build.Tasks
dotnet build --property WarningLevel=0
popd

