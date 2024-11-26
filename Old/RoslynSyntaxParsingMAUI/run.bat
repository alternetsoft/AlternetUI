SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0

call build.bat

pushd "bin\Debug\net9.0-windows10.0.19041.0\win10-x64"

for /R %%f in (*.exe) do "%%f"

popd
