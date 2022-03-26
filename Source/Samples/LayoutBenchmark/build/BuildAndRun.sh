#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build LayoutBenchmark.csproj
popd
dotnet ../bin/Debug/netcoreapp3.1/LayoutBenchmark.dll

