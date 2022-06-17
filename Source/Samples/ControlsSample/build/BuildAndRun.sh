#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build ControlsSample.csproj
popd
dotnet ../bin/Debug/net6.0/ControlsSample.dll

