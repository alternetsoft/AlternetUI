@echo off
dotnet build
pushd bin\Debug\net8.0\
start NativeApi.Generator.exe
popd