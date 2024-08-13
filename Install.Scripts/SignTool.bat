SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source


dotnet msbuild /t:DotNetSign "%SCRIPT_HOME%\SignTool.proj"
