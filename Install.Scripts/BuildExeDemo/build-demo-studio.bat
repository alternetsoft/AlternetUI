ECHO OFF

ECHO ===========================

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0

pushd %SCRIPT_HOME%

dotnet msbuild -tl:off /t:BuildExeDemo -property:DEMO_NAME_CODE=studio-ui;TARGET_CONFIG=Debug;VERSION_SUFFIX=11.0.0 build-demo.proj

popd

ECHO ===========================