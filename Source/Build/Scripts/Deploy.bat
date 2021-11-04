SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Set up.

set PublishRoot=%SCRIPT_HOME%\..\..\..\Publish\Artifacts\Deploy
if not exist "%PublishRoot%" (exit /b 1)

set NuGetApiKey=oy2iknibdcdvlz5ztp3ae34lrw65b5uebhs2dcvfwrlitq

:: Publish PAL packages.

dotnet nuget push "%PublishRoot%\*.nupkg" -k %NuGetApiKey% -s https://api.nuget.org/v3/index.json
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b