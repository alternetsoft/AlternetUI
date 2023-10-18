SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set PROJ=%SCRIPT_HOME%\..\Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj

dotnet msbuild /t:DownloadAndExtractWxWidgets "%PROJ%"

