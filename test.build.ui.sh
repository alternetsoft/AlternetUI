#!/bin/bash
set -euo pipefail

pushd Source/Alternet.UI/


rm -rf bin/Debug/*.*
rm -rf bin/Release/*.*
rm -rf obj/Debug/*.*
rm -rf obj/Release/*.*
rm -rf obj/*.*

dotnet build
popd