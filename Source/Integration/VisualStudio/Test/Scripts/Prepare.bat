SETLOCAL EnableDelayedExpansion

set ScriptHome=%~dp0.

call "%ScriptHome%\Prepare-net461.bat"
call "%ScriptHome%\Prepare-netcoreapp3.1.bat"

if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)