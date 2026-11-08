SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.
set sampleHome=%1

pushd apidoc\%sampleHome%\examples

dotnet run --framework net10.0
popd


