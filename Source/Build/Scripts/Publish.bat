SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

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

@REM :: Generate public source.

@REM set PublicSourceGeneratorToolProject=%SCRIPT_HOME%\..\..\Tools\PublicSourceGenerator\PublicSourceGenerator.csproj
@REM set TargetPublicSource=%SourcePublishDirectory%\Public Source Code.zip
@REM set TargetPublicSamples=%SourcePublishDirectory%\Public Samples.zip
@REM set RepoRoot=%SCRIPT_HOME%\..\..\..

@REM FOR /F %%i IN ('call "%SCRIPT_HOME%\xpath.bat" "%SCRIPT_HOME%\..\..\Mastering\Version\Version.props" //Project/PropertyGroup/Version') DO set PACKAGES_VERSION=%%i
@REM if not !ERRORLEVEL! EQU 0 (
@REM     exit /b !ERRORLEVEL!)

@REM dotnet run --project "%PublicSourceGeneratorToolProject%" -- components --repoRoot "%RepoRoot%" --targetFile "%TargetPublicSource%" --packagesVersion "%PACKAGES_VERSION%"
@REM if not !ERRORLEVEL! EQU 0 (
@REM     exit /b !ERRORLEVEL!)

@REM dotnet run --project "%PublicSourceGeneratorToolProject%" -- samples --repoRoot "%RepoRoot%" --targetFile "%TargetPublicSamples%" --packagesVersion "%PACKAGES_VERSION%"
@REM if not !ERRORLEVEL! EQU 0 (
@REM     exit /b !ERRORLEVEL!)

exit /b