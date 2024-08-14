ECHO OFF

ECHO ========================================================

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set PublishRoot=%SOURCE_DIR%\..\Publish\
set PackagesRoot=%SOURCE_DIR%\..\Publish\Packages
set SIGNTOOL=%SCRIPT_HOME%\SignToolVerify.bat


pushd "%PackagesRoot%"

for /R %%f in (*.dll) do call "%SIGNTOOL%" "%%f"

popd

ECHO ========================================================


