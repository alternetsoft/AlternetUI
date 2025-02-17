SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set DLL=%1

dotnet msbuild -tl:off /t:DotNetSign /p:DllPathForSignTool="%DLL%" "%SCRIPT_HOME%\SignTool.proj"
