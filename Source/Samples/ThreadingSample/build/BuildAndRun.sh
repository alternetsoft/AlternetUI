#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build ThreadingSample.csproj
popd
dotnet ../bin/Debug/net6.0/ThreadingSample.dll

