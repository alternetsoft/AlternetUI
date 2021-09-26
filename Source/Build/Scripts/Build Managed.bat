SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Clean packages

set ALTERNET_UI_RELEASE_DIRECTORY=%SCRIPT_HOME%\..\..\Alternet.UI\bin\Release

if not exist "%ALTERNET_UI_RELEASE_DIRECTORY%" (mkdir "%ALTERNET_UI_RELEASE_DIRECTORY%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_UI_RELEASE_DIRECTORY%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Build

dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release /p:AlternetUIPackagesBuild=true "%SCRIPT_HOME%\..\..\Alternet.UI\Alternet.UI.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release "%SCRIPT_HOME%\..\..\Alternet.UI.Build.Tasks\Alternet.UI.Build.Tasks.ApiInfoCollector\Alternet.UI.Build.Tasks.ApiInfoCollector.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release "%SCRIPT_HOME%\..\..\Alternet.UI.Build.Tasks\Alternet.UI.Build.Tasks.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet msbuild /restore /t:Pack /p:Configuration=Release /p:AlternetUIPackagesBuild=true "%SCRIPT_HOME%\..\..\Alternet.UI\Alternet.UI.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)