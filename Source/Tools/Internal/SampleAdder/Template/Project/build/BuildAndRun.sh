#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build ##SampleName##.csproj
popd
dotnet ../bin/Debug/netcoreapp3.1/##SampleName##.dll

