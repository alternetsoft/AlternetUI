#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build ControlsTest.csproj
popd

