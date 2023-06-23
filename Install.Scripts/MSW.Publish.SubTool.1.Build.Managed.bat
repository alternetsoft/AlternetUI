SETLOCAL EnableDelayedExpansion

set CERT_PASSWORD=%1

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source

:: Clean packages

set ALTERNET_UI_RELEASE_DIRECTORY=%SOURCE_DIR%\Alternet.UI\bin\Release

if not exist "%ALTERNET_UI_RELEASE_DIRECTORY%" (mkdir "%ALTERNET_UI_RELEASE_DIRECTORY%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_UI_RELEASE_DIRECTORY%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_UI_RELEASE_DIRECTORY%\*.snupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Build

echo ====================================
::Alternet.UI
pushd "%SOURCE_DIR%\Alternet.UI\"
dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release /p:AlternetUIPackagesBuild=true /p:WarningLevel=0
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================
::Alternet.UI.Build.Tasks.ApiInfoCollector
pushd "%SOURCE_DIR%\Alternet.UI.Build.Tasks\Alternet.UI.Build.Tasks.ApiInfoCollector\"
dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0 
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================
::Alternet.UI.Build.Tasks.csproj
pushd "%SOURCE_DIR%\Alternet.UI.Build.Tasks\"
dotnet msbuild /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================
::Alternet.UI Pack
pushd "%SOURCE_DIR%\Alternet.UI\"
dotnet msbuild /restore /t:Pack /p:Configuration=Release /p:AlternetUIPackagesBuild=true /p:WarningLevel=0
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================

call MSW.Publish.SubTool.3.Nuget.Sign.bat "%SOURCE_DIR%\Alternet.UI\bin\Release\*.nupkg" %CERT_PASSWORD%