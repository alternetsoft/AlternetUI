ECHO OFF

ECHO ===============================
ECHO ===============================
ECHO ===============================

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\..\
set SIGNTOOL=%SCRIPT_HOME%\..\..\..\Install.Scripts\SignToolVerify.bat

pushd "%SCRIPT_HOME%\bin\Release\"

for /R %%f in (Alternet*.dll) do call "%SIGNTOOL%" "%%f"
for /R %%f in (*.exe) do call "%SIGNTOOL%" "%%f"

popd

ECHO ===============================
ECHO ===============================
ECHO ===============================
