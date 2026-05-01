ECHO OFF

ECHO ===========================

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0

pushd %SCRIPT_HOME%

dotnet msbuild -tl:off /t:BuildExeDemo -property:DEMO_NAME_CODE=licenseactivation;TARGET_CONFIG=Release;VERSION_SUFFIX=10.1.0 build-demo.proj

popd

ECHO ===========================