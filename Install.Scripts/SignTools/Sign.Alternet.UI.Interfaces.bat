SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\..\Source
set SIGNTOOL=%SCRIPT_HOME%\..\SignTool.bat

call "%SIGNTOOL%" "%SOURCE_DIR%\Alternet.UI.Interfaces\bin\Debug\netstandard2.0\Alternet.UI.Interfaces.dll"

