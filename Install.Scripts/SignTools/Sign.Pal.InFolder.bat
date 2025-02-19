
ECHO OFF
ECHO ==========================================
ECHO Sign.Pal.InFolder.bat

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\..\Source
set SIGNTOOL=%SCRIPT_HOME%\..\SignTool.bat

ECHO Folder: %1%

pushd "%1%"

for /R %%f in (Alternet.UI.Pal.dl?) do (
echo sign "%%f"
call "%SIGNTOOL%" "%%f"
)

popd

ECHO ==========================================
