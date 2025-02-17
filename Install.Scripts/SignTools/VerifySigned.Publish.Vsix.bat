ECHO OFF

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\..\Source
set SIGNTOOL=%SCRIPT_HOME%\..\SignToolVerify.bat

pushd "%SCRIPT_HOME%\..\..\Publish\Packages"

for /R %%f in (*.vsix) do call (
	ECHO "%%f"
	sign code verify "%%f"
)

popd




