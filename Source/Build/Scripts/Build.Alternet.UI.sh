#!/bin/bash
set -euo pipefail

pushd ../../Alternet.UI
dotnet build --property WarningLevel=0
popd



