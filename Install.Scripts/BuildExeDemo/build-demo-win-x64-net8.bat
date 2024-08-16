CLS

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_HOME=%SCRIPT_HOME%\..\..\Source
set SAMPLES_HOME=%SOURCE_HOME%\Samples
set DEMO_HOME=%SAMPLES_HOME%\ControlsSample

:: delete bin folder contents here

pushd %DEMO_HOME%

dotnet build -c Release --framework net8.0

popd

:: zip demo here

if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0