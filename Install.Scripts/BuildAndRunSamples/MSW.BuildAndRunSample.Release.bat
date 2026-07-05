SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.
set sampleHome=%1
set sampleName=%2

pushd %sampleHome%
pushd %sampleName%
dotnet build -c Release -tl:off --property WarningLevel=0
start /b dotnet run -c Release --property WarningLevel=0 --framework net8.0
popd
popd


