#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build HelloWorldSample.csproj
popd
dotnet ../bin/Debug/net6.0/HelloWorldSample.dll

