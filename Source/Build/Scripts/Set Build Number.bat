SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

dotnet run --project "%SCRIPT_HOME%\..\..\Tools\VersionTool\VersionTool.csproj" -- set-build-number %1

if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)