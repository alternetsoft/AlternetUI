#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build ControlsSample.csproj
popd
dotnet ../bin/Debug/netcoreapp3.1/HelloWorldSample.dll

