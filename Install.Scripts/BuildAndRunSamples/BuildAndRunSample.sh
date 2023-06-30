#!/bin/bash
set -euo pipefail

pushd ../../Source/Samples/$1


rm -rf bin/Debug/*.*
rm -rf bin/Release/*.*
rm -rf obj/Debug/*.*
rm -rf obj/Release/*.*
rm -rf obj/*.*

dotnet build --nologo --property WarningLevel=0 --framework net6.0
dotnet run --nologo --property WarningLevel=0 --framework net6.0
popd