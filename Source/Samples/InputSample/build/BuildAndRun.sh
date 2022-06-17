#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build InputSample.csproj
popd
dotnet ../bin/Debug/net6.0/InputSample.dll

