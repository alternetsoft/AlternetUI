#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./release.sh
popd
pushd ..
dotnet build PrintingSample.csproj -c Release
popd