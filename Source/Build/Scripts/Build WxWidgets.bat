SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Prepare variables.

call "%SCRIPT_HOME%\Set Artifact Paths.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do (
    set FOUND_MSBUILD_PATH=%%i)

dotnet msbuild /t:BuildWxWidgets "%SCRIPT_HOME%\..\Alternet.UI.Pal\Alternet.UI.Pal.proj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0