#!/bin/bash
pushd ../../../Alternet.UI.Pal/build
./debug.sh
popd
pushd ..
dotnet build EmployeeFormSample.csproj
popd
dotnet ../bin/Debug/netcoreapp3.1/EmployeeFormSample.dll

