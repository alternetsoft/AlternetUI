
SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

dotnet msbuild -tl:off /t:BuildWxWidgets_Windows "%SCRIPT_HOME%\Alternet.UI.Pal.proj"
 if not !ERRORLEVEL! EQU 0 (
     exit /b !ERRORLEVEL!)

 exit /b 0