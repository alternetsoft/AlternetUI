SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set VsixsigntoolPath="C:\Users\yezo\.nuget\packages\microsoft.vssdk.vsixsigntool\16.2.29116.78\tools\vssdk\vsixsigntool.exe"
set SigntoolParameters=sign /v /f ..\..\Keys\Alternet.pfx /p Alternet^^! /sha1 "02c4f7817da47b568c3aa56dc3a3ebaf4ac59da0"

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

call "%SCRIPT_HOME%\Build Managed.bat"
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

call "%SCRIPT_HOME%\Build Integration.bat"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Publish Visual Studio extension.

set VersionToolProject=%SCRIPT_HOME%\..\..\Tools\Versioning\Alternet.UI.VersionTool.Cli\Alternet.UI.VersionTool.Cli.csproj

copy "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2019\Release\*.vsix" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

rem %VsixsigntoolPath% %SigntoolParameters% "%PackagesPublishDirectory%\Alternet.UI.Integration.VisualStudio.VS2019.vsix"
rem if not !ERRORLEVEL! EQU 0 (
rem     exit /b !ERRORLEVEL!)

dotnet run --project "%VersionToolProject%" -- append-version-suffix "%PackagesPublishDirectory%\Alternet.UI.Integration.VisualStudio.VS2019.vsix"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2022\Release\*.vsix" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

rem %VsixsigntoolPath% %SigntoolParameters% "%PackagesPublishDirectory%\Alternet.UI.Integration.VisualStudio.VS2022.vsix"
rem if not !ERRORLEVEL! EQU 0 (
rem     exit /b !ERRORLEVEL!)

dotnet run --project "%VersionToolProject%" -- append-version-suffix "%PackagesPublishDirectory%\Alternet.UI.Integration.VisualStudio.VS2022.vsix"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Publish command line templates.

copy "%SCRIPT_HOME%\..\..\Integration\Templates\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Generate public source.

set PublicSourceGeneratorToolProject=%SCRIPT_HOME%\..\..\Tools\PublicSourceGenerator\Alternet.UI.PublicSourceGenerator.csproj

dotnet run --project "%PublicSourceGeneratorToolProject%" -- components
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet run --project "%PublicSourceGeneratorToolProject%" -- samples
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b