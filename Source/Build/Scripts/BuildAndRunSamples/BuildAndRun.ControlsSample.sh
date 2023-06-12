#!/bin/bash
set -euo pipefail

pushd ../../../Samples/ControlsSample
dotnet build --property WarningLevel=0
dotnet run --property WarningLevel=0 --framework net6.0
popd



