SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Prepare variables.

call "%SCRIPT_HOME%\Set Artifact Paths.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Receive Alternet.UI PAL Windows Artifact.

if not exist "%UI_WINDOWS_PAL_BIN%" (mkdir "%UI_WINDOWS_PAL_BIN%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /q /s "%UI_WINDOWS_PAL_BIN%\*"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

xcopy /s /y "%UI_PAL_WINDOWS_ARTIFACTS_PATH%\*.*" "%UI_WINDOWS_PAL_BIN%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Receive Alternet.UI PAL macOS Artifact.

if not exist "%UI_MACOS_PAL_BIN%" (mkdir "%UI_MACOS_PAL_BIN%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /q /s "%UI_MACOS_PAL_BIN%\*"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

xcopy /s /y "%UI_PAL_MACOS_ARTIFACTS_PATH%\*.*" "%UI_MACOS_PAL_BIN%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Receive Alternet.UI PAL Linux Artifact.

if not exist "%UI_LINUX_PAL_BIN%" (mkdir "%UI_LINUX_PAL_BIN%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /q /s "%UI_LINUX_PAL_BIN%\*"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

xcopy /s /y "%UI_PAL_LINUX_ARTIFACTS_PATH%\*.*" "%UI_LINUX_PAL_BIN%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Build the packages.

dotnet msbuild "%SCRIPT_HOME%\..\Alternet.UI.Pal\Alternet.UI.Pal.proj" /t:NuGet
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0