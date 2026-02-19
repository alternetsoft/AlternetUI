#!/bin/bash
set -euo pipefail

pushd ../../Source/Samples/$1


rm -rf bin/Debug/*.*
rm -rf bin/Release/*.*
rm -rf obj/Debug/*.*
rm -rf obj/Release/*.*
rm -rf obj/*.*

dotnet build -c Release -tl:off --property WarningLevel=0
dotnet run -c Release --property WarningLevel=0 --framework net8.0

popd