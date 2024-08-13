SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set NUGET_HOME=%SCRIPT_HOME%\..\..\Publish\Packages

pushd %NUGET_HOME%
"%SCRIPT_HOME%\..\TestDotNetNew\nuget.exe" verify -All *.nupkg
popd