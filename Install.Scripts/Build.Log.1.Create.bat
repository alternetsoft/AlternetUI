SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source

pushd "%SOURCE_DIR%\Alternet.UI\"
dotnet msbuild /restore /t:Clean,Build /p:Configuration=Debug /flp:v=diag;logfile="Build.Result.Log"
popd

