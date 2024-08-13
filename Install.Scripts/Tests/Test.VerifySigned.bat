SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\..\Source
set SIGNTOOL=%SCRIPT_HOME%\..\SignToolVerify.bat

call "%SIGNTOOL%" "%SOURCE_DIR%\Alternet.UI\bin\Debug\net8.0\Alternet.UI.dll"

