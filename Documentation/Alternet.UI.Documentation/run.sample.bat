SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.
set sampleHome=%1

pushd apidoc\%sampleHome%\examples
:: start /b dotnet run --framework net8.0

dotnet run --framework net8.0
popd


