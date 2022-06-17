#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build DrawingSample.csproj
popd
dotnet ../bin/Debug/net6.0/DrawingSample.dll

