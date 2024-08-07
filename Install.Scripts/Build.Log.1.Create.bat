SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source

pushd "%SOURCE_DIR%\Alternet.UI\"
dotnet build /restore -t:Clean,Build -f:net8.0 -p:Configuration=Debug -flp:v=diag;logfile="Build.Result.Log"
popd

