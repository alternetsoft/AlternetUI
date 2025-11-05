SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set PublishRoot=%SOURCE_DIR%\..\Publish\

:: Set up publish folder

if not exist "%PublishRoot%" (mkdir "%PublishRoot%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

set PackagesPublishDirectory=%PublishRoot%\Packages\
::REM set SourcePublishDirectory=%PublishRoot%\Sources\

echo ====================================5

pushd "%PackagesPublishDirectory%"

for /D %%f in (*.Folder) do (rmdir /S /Q %%f)

for /R %%f in (*.nupkg) do call "C:\Program Files\WinRar\WinRAR.exe" x "%%f" "%%f.Folder/"

for /R %%f in (*.vsix) do call "C:\Program Files\WinRar\WinRAR.exe" x "%%f" "%%f.Folder/"

popd



echo ====================================5


exit /b