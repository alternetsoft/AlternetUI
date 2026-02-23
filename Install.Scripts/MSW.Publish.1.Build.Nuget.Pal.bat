:: =========================================

cls

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set PublishRoot=%SOURCE_DIR%\..\Publish\

:: Set up publish folder

if not exist "%PublishRoot%" (mkdir "%PublishRoot%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

set PackagesPublishDirectory=%PublishRoot%\Packages\
::REM set SourcePublishDirectory=%PublishRoot%\Sources\

if not exist "%PackagesPublishDirectory%" (mkdir "%PackagesPublishDirectory%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\Alternet.UI.Pal.*.nupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\Alternet.UI.Pal.*.snupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: =========================================

dotnet msbuild -tl:off /t:Nuget "%SCRIPT_HOME%\..\Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj"

if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Publish PAL packages.

copy "%SOURCE_DIR%\Build\Alternet.UI.Pal\bin\NuGet\*.nupkg" "%PackagesPublishDirectory%"

echo ====================================5

pushd "%PackagesPublishDirectory%"

for /D %%f in (*.Folder) do (rmdir /S /Q %%f)
for /R %%f in ("Alternet.UI.Pal.*.nupkg") do call "C:\Program Files\WinRar\WinRAR.exe" x "%%f" "%%f.Folder/"

popd
:: =========================================

exit /b 0