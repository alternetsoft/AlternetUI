ECHO OFF

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\..\Source
set SIGNTOOL=%SCRIPT_HOME%\..\SignToolVerify.bat

pushd "%SCRIPT_HOME%\..\..\Publish\Packages"

for /R %%f in (*.nupkg) do (
	ECHO "%%f"
	dotnet nuget verify "%%f"
)

for /R %%f in (*.snupkg) do (
	ECHO "%%f"
	dotnet nuget verify "%%f"
)

popd




