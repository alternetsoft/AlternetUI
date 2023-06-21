SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set "CMD=%~1"

if "!CMD!"=="" ( set "CMD=help" )


pushd "Source\Tools\Alternet.UI.RunCmd\"
dotnet run -- -r="%CMD%"
popd

