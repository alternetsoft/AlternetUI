@echo off
dotnet build
pushd bin\Debug\net10.0\
start NativeApi.Generator.exe
popd