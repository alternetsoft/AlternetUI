SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Prepare variables.

call "%SCRIPT_HOME%\Set Artifact Paths.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Build Alternet.UI.

for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\amd64\MSBuild.exe`) do (
    set FOUND_MSBUILD_PATH=%%i)

dotnet msbuild /p:CppMsBuildPath="%FOUND_MSBUILD_PATH%" "%SCRIPT_HOME%\..\Alternet.UI.Pal\Alternet.UI.Pal.proj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Copy Alternet.UI build results.

if not exist "%UI_PAL_WINDOWS_ARTIFACTS_PATH%" (mkdir "%UI_PAL_WINDOWS_ARTIFACTS_PATH%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /q /s "%UI_PAL_WINDOWS_ARTIFACTS_PATH%\*"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

xcopy /s /y "%UI_WINDOWS_PAL_BIN%\*.*" "%UI_PAL_WINDOWS_ARTIFACTS_PATH%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0