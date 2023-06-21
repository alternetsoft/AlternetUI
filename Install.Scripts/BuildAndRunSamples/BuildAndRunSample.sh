#!/bin/bash
set -euo pipefail

pushd ../../Source/Samples/$1
#dotnet clean
dotnet build --nologo --property WarningLevel=0
dotnet run --nologo --property WarningLevel=0 --framework net6.0
popd
