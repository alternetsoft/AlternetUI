SETLOCAL EnableDelayedExpansion

set ScriptHome=%~dp0.

call "%ScriptHome%\Prepare-net461.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

call "%ScriptHome%\Prepare-netcoreapp3.1.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

call "%ScriptHome%\Prepare-net6.0.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

call "%ScriptHome%\Prepare-build-nuget.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)