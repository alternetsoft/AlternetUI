SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

dotnet msbuild /t:RunControlsSample -property:NoNet8=true "%SCRIPT_HOME%\Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0
