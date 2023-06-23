:: Requires Alternet.UI.Pal\bin\NuGet\*.nupkg
:: Calls MSW.Publish.Build.Managed.bat
:: Calls MSW.Publish.Build.Integration.bat
:: =========================================

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set CERT_PASSWORD=%1

:: Set up publish folder.

set PublishRoot=%SCRIPT_HOME%\..\..\..\Publish\
if not exist "%PublishRoot%" (mkdir "%PublishRoot%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

set PackagesPublishDirectory=%PublishRoot%\Packages\
@REM set SourcePublishDirectory=%PublishRoot%\Sources\

if not exist "%PackagesPublishDirectory%" (mkdir "%PackagesPublishDirectory%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.snupkg"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.vsix"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del "%PackagesPublishDirectory%\*.zip"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Publish PAL packages.

copy "%SCRIPT_HOME%\..\Alternet.UI.Pal\bin\NuGet\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Build managed packages.

echo ====================================

call "%SCRIPT_HOME%\MSW.Publish.SubTool.1.Build.Managed.bat" %CERT_PASSWORD%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Publish managed packages.

copy "%SCRIPT_HOME%\..\..\Alternet.UI\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%SCRIPT_HOME%\..\..\Alternet.UI\bin\Release\*.snupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Build integration components.

echo ====================================
echo SubTool.2
call "%SCRIPT_HOME%\MSW.Publish.SubTool.2.Build.Integration.bat" %CERT_PASSWORD%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Publish Visual Studio extension.

set VersionToolProject=%SCRIPT_HOME%\..\..\Tools\Versioning\Alternet.UI.VersionTool.Cli\Alternet.UI.VersionTool.Cli.csproj

REM copy "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2019\Release\*.vsix" "%PackagesPublishDirectory%"
REM if not !ERRORLEVEL! EQU 0 (
REM     exit /b !ERRORLEVEL!)

REM dotnet run --project "%VersionToolProject%" -- append-version-suffix "%PackagesPublishDirectory%\Alternet.UI.Integration.VisualStudio.VS2019.vsix"
REM if not !ERRORLEVEL! EQU 0 (
REM     exit /b !ERRORLEVEL!)

copy "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2022\Release\*.vsix" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

echo ====================================

dotnet run --project "%VersionToolProject%" --property WarningLevel=0 -- append-version-suffix "%PackagesPublishDirectory%\Alternet.UI.Integration.VisualStudio.VS2022.vsix"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Publish command line templates.

copy "%SCRIPT_HOME%\..\..\Integration\Templates\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Generate public source.

set PublicSourceGeneratorToolProject=%SCRIPT_HOME%\..\..\Tools\PublicSourceGenerator\Alternet.UI.PublicSourceGenerator.csproj

echo ====================================

dotnet run --project "%PublicSourceGeneratorToolProject%" --property WarningLevel=0  -- components
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

echo ====================================

dotnet run --project "%PublicSourceGeneratorToolProject%" --property WarningLevel=0  -- samples
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)


::copy "%SCRIPT_HOME%\..\..\Alternet.UI\bin\Release\Alternet.UI.*.nupkg" "%SCRIPT_HOME%\..\..\..\Publish\Packages"
::copy "%SCRIPT_HOME%\..\..\Alternet.UI\bin\Release\Alternet.UI.*.snupkg" "%SCRIPT_HOME%\..\..\..\Publish\Packages"

exit /b