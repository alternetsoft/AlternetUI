:: Requires Alternet.UI.Pal\bin\NuGet\*.nupkg
:: Calls MSW.Publish.Build.Managed.bat
:: Calls MSW.Publish.Build.Integration.bat
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

del "%PackagesPublishDirectory%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.snupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.vsix"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: del "%PackagesPublishDirectory%\*.zip"
:: if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Publish PAL packages.

copy "%SOURCE_DIR%\Build\Alternet.UI.Pal\bin\NuGet\*.nupkg" "%PackagesPublishDirectory%"

:: Build managed packages.

echo ====================================1

call "%SCRIPT_HOME%\MSW.Publish.SubTool.1.Build.Managed.bat" %CERT_PASSWORD%
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================5

call "MSW.Publish.SubTool.4.Gen.Public.Samples.bat"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================5


exit /b