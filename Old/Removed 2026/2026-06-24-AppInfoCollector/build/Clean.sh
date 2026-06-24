#!/bin/bash
pushd ../Alternet.UI.Build.Tasks.ApiInfoCollector
dotnet clean
popd

pushd ..
dotnet clean
popd

