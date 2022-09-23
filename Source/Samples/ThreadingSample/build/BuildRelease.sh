#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./release.sh
popd
pushd ..
dotnet build ThreadingSample.csproj -c Release
popd