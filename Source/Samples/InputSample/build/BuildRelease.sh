#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./release.sh
popd
pushd ..
dotnet build InputSample.csproj -c Release
popd