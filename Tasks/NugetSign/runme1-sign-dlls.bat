SETLOCAL EnableDelayedExpansion
cls

setlocal

set SCRIPT_HOME=%~dp0.

pushd "%SCRIPT_HOME%\..\..\Source\Tools\Alternet.UI.RunCmd\"

dotnet run -- -r=rezip Settings="%SCRIPT_HOME%\NugetSignTaskParams.xml"

popd
