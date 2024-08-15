SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set DLL=%1

dotnet msbuild /t:SignVerify /p:DllPathForSignTool="%DLL%" "%SCRIPT_HOME%\SignTool.proj"
