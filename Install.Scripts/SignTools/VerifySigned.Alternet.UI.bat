SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\..\Source
set SIGNTOOL=%SCRIPT_HOME%\..\SignToolVerify.bat

pushd "%SOURCE_DIR%\Alternet.UI\bin\Release\"

for /R %%f in (Alternet.UI.dl?) do call "%SIGNTOOL%" "%%f"

popd