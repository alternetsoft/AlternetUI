@echo off
dotnet build
pushd bin\Debug\net6.0\
start NativeApi.Generator.exe
popd