#!/bin/bash
set -euo pipefail

pushd ../../../Samples/$1
dotnet build --nologo --property WarningLevel=0
dotnet run --nologo --property WarningLevel=0 --framework net6.0
popd



