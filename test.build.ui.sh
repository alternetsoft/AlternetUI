#!/bin/bash
set -euo pipefail

pushd Source/Alternet.UI/


rm -rf bin/Debug/*.*
rm -rf bin/Release/*.*
rm -rf obj/Debug/*.*
rm -rf obj/Release/*.*
rm -rf obj/*.*

dotnet build --framework net6.0
dotnet build --framework net7.0
dotnet build --framework net8.0
popd