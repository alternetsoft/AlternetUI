@echo off
dotnet build
pushd bin\Debug\netcoreapp3.1\
start NativeApi.Generator.exe
popd