@echo off
REM runps1.bat - Run a .ps1 file passed as the first argument, forwarding any additional args.
REM Usage: runps1.bat "C:\path\to\script.ps1" [arg1 arg2 ...]
setlocal

IF "%~1"=="" (
  echo Usage: %~nx0 "C:\path\to\script.ps1" [args...]
  exit /b 1
)

REM Capture script path and remaining args
set "script=%~1"
shift
set "args=%*"

REM Run the script with a process-scoped relaxed policy (-ExecutionPolicy Bypass) and no profile
pwsh -NoProfile -ExecutionPolicy Bypass -File "%script%" %args%
exit /b %ERRORLEVEL%