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

del "%PackagesPublishDirectory%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.snupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.vsix"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.zip"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Publish PAL packages.

copy "%SOURCE_DIR%\Build\Alternet.UI.Pal\bin\NuGet\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Build managed packages.

echo ====================================1

call "%SCRIPT_HOME%\MSW.Publish.SubTool.1.Build.Managed.bat" %CERT_PASSWORD%
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Publish managed packages.

copy "%SOURCE_DIR%\Alternet.UI\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

copy "%SOURCE_DIR%\Alternet.UI\bin\Release\*.snupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Build integration components.

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

dotnet run --project "%VersionToolProject%" --property WarningLevel=0 -- append-version-suffix "%PackagesPublishDirectory%\Alternet.UI.Integration.VisualStudio.VS2022.vsix"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Publish command line templates.

copy "%SOURCE_DIR%\Integration\Templates\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Generate public source.

echo ====================================4

call "MSW.Publish.SubTool.3.Gen.Public.Components.bat"

if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================5

call "MSW.Publish.SubTool.4.Gen.Public.Samples.bat"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)


exit /b