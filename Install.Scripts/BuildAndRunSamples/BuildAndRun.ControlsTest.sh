#!/bin/bash
set -euo pipefail

pushd ../../Source/Samples/ControlsTest

rm -rf bin/Debug/*.*
rm -rf bin/Release/*.*
rm -rf obj/Debug/*.*
rm -rf obj/Release/*.*
rm -rf obj/*.*

dotnet build --property WarningLevel=0
dotnet run --property WarningLevel=0 --framework net8.0
popd



