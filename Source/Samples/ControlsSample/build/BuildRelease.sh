#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./release.sh
popd
pushd ..
dotnet build ControlsSample.csproj -c Release
popd