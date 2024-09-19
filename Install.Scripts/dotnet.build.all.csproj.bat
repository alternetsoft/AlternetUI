SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set ROOT_DIR=%SCRIPT_HOME%\..

pushd "%ROOT_DIR%"

for /R %%f in (*.csproj) do dotnet build "%%f"

popd