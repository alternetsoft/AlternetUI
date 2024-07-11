SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set BIN=%SCRIPT_HOME%\bin\Debug\net8.0-windows

set DEST=C:\AlternetUI\UIXmlHostApp

del "%DEST%\*.*" /s /q

xcopy "%BIN%" "%DEST%" /s /e