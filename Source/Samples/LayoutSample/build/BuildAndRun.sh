#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build LayoutSample.csproj
popd
dotnet ../bin/Debug/net6.0/LayoutSample.dll

