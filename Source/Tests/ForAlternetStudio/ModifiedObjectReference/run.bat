SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0

pushd "bin\Debug\net8.0"

for /R %%f in (*.exe) do "%%f"

popd
