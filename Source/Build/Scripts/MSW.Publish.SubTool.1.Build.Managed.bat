SETLOCAL EnableDelayedExpansion

set CERT_PASSWORD=%1

set SCRIPT_HOME=%~dp0.

:: Clean packages

set ALTERNET_UI_RELEASE_DIRECTORY=%SCRIPT_HOME%\..\..\Alternet.UI\bin\Release

if not exist "%ALTERNET_UI_RELEASE_DIRECTORY%" (mkdir "%ALTERNET_UI_RELEASE_DIRECTORY%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_UI_RELEASE_DIRECTORY%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_UI_RELEASE_DIRECTORY%\*.snupkg"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Build

echo ====================================
::Alternet.UI
dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release /p:AlternetUIPackagesBuild=true /p:WarningLevel=0 "%SCRIPT_HOME%\..\..\Alternet.UI\Alternet.UI.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

echo ====================================
::Alternet.UI.Build.Tasks.ApiInfoCollector
dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0 "%SCRIPT_HOME%\..\..\Alternet.UI.Build.Tasks\Alternet.UI.Build.Tasks.ApiInfoCollector\Alternet.UI.Build.Tasks.ApiInfoCollector.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

echo ====================================
::Alternet.UI.Build.Tasks.csproj
dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0  "%SCRIPT_HOME%\..\..\Alternet.UI.Build.Tasks\Alternet.UI.Build.Tasks.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

echo ====================================
::Alternet.UI Pack
dotnet msbuild /restore /t:Pack /p:Configuration=Release /p:AlternetUIPackagesBuild=true /p:WarningLevel=0 "%SCRIPT_HOME%\..\..\Alternet.UI\Alternet.UI.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

echo ====================================

call MSW.Publish.SubTool.3.Nuget.Sign.bat "%SCRIPT_HOME%\..\..\Alternet.UI\bin\Release\*.nupkg" %CERT_PASSWORD%