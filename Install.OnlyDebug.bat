SETLOCAL EnableDelayedExpansion
cls

set SCRIPT_HOME=%~dp0.

dotnet msbuild -tl:off /t:InstallAlternetUI /nodeReuse:false -property:OnlyDebug=true "%SCRIPT_HOME%\Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0