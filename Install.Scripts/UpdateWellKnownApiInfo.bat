ECHO This script updates xml with control declarations.
ECHO You need to call Install.bat before this script.

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

dotnet msbuild /nodeReuse:false /t:UpdateWellKnownApiInfo "%SCRIPT_HOME%\..\Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0