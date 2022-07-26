#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build CommonDialogsSample.csproj
popd
dotnet ../bin/Debug/net6.0/CommonDialogsSample.dll

