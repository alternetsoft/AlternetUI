ECHO OFF

ECHO ===========================

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0

set DEMO_FRAMEWORK=%1
set DEMO_NAME=%2
set DEMO_PLATFORM=%3

pushd %SCRIPT_HOME%

dotnet msbuild -tl:off /t:BuildExeDemo -property:DEMO_NAME=%DEMO_NAME%;DEMO_FRAMEWORK=%DEMO_FRAMEWORK%;DEMO_PLATFORM=%DEMO_PLATFORM% build-demo.proj

popd

ECHO ===========================