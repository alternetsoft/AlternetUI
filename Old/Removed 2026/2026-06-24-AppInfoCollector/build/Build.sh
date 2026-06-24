#!/bin/bash
pushd ../Alternet.UI.Build.Tasks.ApiInfoCollector
dotnet build
popd

pushd ..
dotnet build
popd

