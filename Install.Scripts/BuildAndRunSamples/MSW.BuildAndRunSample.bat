SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.
set sampleHome=%1
set sampleName=%2
set platform=%3

if "%platform%"==x64 goto ok
if "%platform%"==x86 goto ok
set platform=x64
:ok
ECHO platform: %platform%

pushd %sampleHome%
pushd %sampleName%
dotnet build -tl:off /p:Platform=%platform% --property WarningLevel=0
start /b dotnet run /p:Platform=%platform% --property WarningLevel=0 --framework net8.0
popd
popd


