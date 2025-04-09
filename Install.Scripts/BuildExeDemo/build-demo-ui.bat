ECHO OFF

ECHO ===========================

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0

pushd %SCRIPT_HOME%

dotnet msbuild -tl:off /t:BuildExeDemo -property:DEMO_NAME_CODE=ui build-demo.proj

popd

ECHO ===========================