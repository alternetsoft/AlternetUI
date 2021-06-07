SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Prepare variables.

call "%SCRIPT_HOME%\Set Artifact Paths.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet msbuild /t:BuildWxWidgets /p:ReleaseOnlyWxWidgetsBuild=true "%SCRIPT_HOME%\..\Alternet.UI.Pal\Alternet.UI.Pal.proj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0