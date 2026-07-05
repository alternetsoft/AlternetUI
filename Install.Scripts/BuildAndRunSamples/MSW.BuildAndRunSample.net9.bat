SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.
set sampleHome=%1
set sampleName=%2

pushd %sampleHome%
pushd %sampleName%
dotnet build -tl:off --property WarningLevel=0
start /b dotnet run --property WarningLevel=0 --framework net9.0
popd
popd


