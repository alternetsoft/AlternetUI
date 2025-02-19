SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set PublishRoot=%SCRIPT_HOME%\..\..\Publish\

:: Set up publish folder

if not exist "%PublishRoot%" (mkdir "%PublishRoot%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

set PackagesPublishDirectory=%PublishRoot%\Packages\

echo ====================================5

pushd "%PackagesPublishDirectory%"

for /D %%f in (alternet-ui-demo-*) do (rmdir /S /Q %%f)

for /R %%f in (alternet-ui-demo-*.zip) do call "C:\Program Files\WinRar\WinRAR.exe" x "%%f"

popd



echo ====================================5


exit /b