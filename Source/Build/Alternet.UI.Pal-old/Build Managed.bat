SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Clean packages

set ALTERNET_UI_RELEASE_DIRECTORY=%SCRIPT_HOME%\..\..\Alternet.UI\bin\Debug

if not exist "%ALTERNET_UI_RELEASE_DIRECTORY%" (mkdir "%ALTERNET_UI_RELEASE_DIRECTORY%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_UI_RELEASE_DIRECTORY%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Build

dotnet msbuild /restore /t:Clean,Build,Pack /p:Configuration=Debug /p:AlternetUIPackagesBuild=true "%SCRIPT_HOME%\..\..\Alternet.UI\Alternet.UI.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%SCRIPT_HOME%\..\..\Alternet.UI\bin\Debug\Alternet.UI.0.0.1.nupkg" *.*