SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0

call build.bat

pushd "bin\Debug\net8.0"

for /R %%f in (*.exe) do "%%f"

popd
