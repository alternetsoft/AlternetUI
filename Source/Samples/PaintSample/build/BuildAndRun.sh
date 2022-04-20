#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build PaintSample.csproj
popd
dotnet ../bin/Debug/netcoreapp3.1/PaintSample.dll

