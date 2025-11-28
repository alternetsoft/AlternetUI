:: Requires Alternet.UI.Pal\bin\NuGet\*.nupkg
:: Calls MSW.Publish.Build.Managed.bat
:: Calls MSW.Publish.Build.Integration.bat
:: =========================================

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

del "%PackagesPublishDirectory%\*.vsix"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: del "%PackagesPublishDirectory%\*.zip"
:: if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Build managed packages.

echo ====================================2
echo SubTool.2
call "%SCRIPT_HOME%\MSW.Publish.SubTool.2.Build.Integration.bat" %CERT_PASSWORD%
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Publish Visual Studio extension.

set VersionToolProject=%SOURCE_DIR%\Tools\Versioning\Alternet.UI.VersionTool.Cli\Alternet.UI.VersionTool.Cli.csproj

copy "%SOURCE_DIR%\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2022\Release\*.vsix" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================3
echo SET VSIX VERSION

dotnet run --project "%VersionToolProject%" --property WarningLevel=0 -- append-version-suffix "%PackagesPublishDirectory%\Alternet.UI.Integration.VisualStudio.vsix"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Publish command line templates.

copy "%SOURCE_DIR%\Integration\Templates\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================5


exit /b