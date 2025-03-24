SETLOCAL EnableDelayedExpansion

set CERT_PASSWORD=%1

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source

:: Clean packages

set ALTERNET_UI_RELEASE_DIRECTORY=%SOURCE_DIR%\Alternet.UI\bin\Release
set ALTERNET_MAUI_RELEASE_DIRECTORY=%SOURCE_DIR%\Alternet.UI.Maui\bin\Release
set ALTERNET_COMMON_RELEASE_DIRECTORY=%SOURCE_DIR%\Alternet.UI.Common\bin\Release

if not exist "%ALTERNET_UI_RELEASE_DIRECTORY%" (mkdir "%ALTERNET_UI_RELEASE_DIRECTORY%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

if not exist "%ALTERNET_MAUI_RELEASE_DIRECTORY%" (mkdir "%ALTERNET_MAUI_RELEASE_DIRECTORY%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

if not exist "%ALTERNET_COMMON_RELEASE_DIRECTORY%" (mkdir "%ALTERNET_COMMON_RELEASE_DIRECTORY%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)


del /q /s "%ALTERNET_UI_RELEASE_DIRECTORY%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_UI_RELEASE_DIRECTORY%\*.snupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_MAUI_RELEASE_DIRECTORY%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_MAUI_RELEASE_DIRECTORY%\*.snupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_COMMON_RELEASE_DIRECTORY%\*.nupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

del /q /s "%ALTERNET_COMMON_RELEASE_DIRECTORY%\*.snupkg"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Build

echo ====================================
::Alternet.UI.Common
pushd "%SOURCE_DIR%\Alternet.UI.Common\"
dotnet msbuild Alternet.UI.Common.csproj -tl:off /restore /t:Clean,Build,Pack /p:Configuration=Release /p:AlternetUIPackagesBuild=true /p:WarningLevel=0
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

dotnet msbuild -tl:off /t:DotNetNugetSign /p:NUGET_PATH="%SOURCE_DIR%\Alternet.UI.Common\bin\Release\*.nupkg" "%SCRIPT_HOME%\Dotnet.Nuget.Sign.proj"

copy "%SOURCE_DIR%\Alternet.UI.Common\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

copy "%SOURCE_DIR%\Alternet.UI.Common\bin\Release\*.snupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================
::Alternet.UI
pushd "%SOURCE_DIR%\Alternet.UI\"
dotnet msbuild -tl:off /restore /t:Clean,Build /p:Configuration=Release /p:AlternetUIPackagesBuild=true /p:WarningLevel=0
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================BM1
::Alternet.UI.Build.Tasks.ApiInfoCollector
pushd "%SOURCE_DIR%\Alternet.UI.Build.Tasks\Alternet.UI.Build.Tasks.ApiInfoCollector\"
dotnet msbuild -tl:off /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0 
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================BM2
::Alternet.UI.Build.Tasks.csproj
pushd "%SOURCE_DIR%\Alternet.UI.Build.Tasks\"
dotnet msbuild -tl:off /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================BM3
::Alternet.UI Pack
pushd "%SOURCE_DIR%\Alternet.UI\"
dotnet msbuild -tl:off /restore /t:Pack /p:Configuration=Release /p:AlternetUIPackagesBuild=true /p:WarningLevel=0
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

dotnet msbuild -tl:off /t:DotNetNugetSign /p:NUGET_PATH="%SOURCE_DIR%\Alternet.UI\bin\Release\*.nupkg" "%SCRIPT_HOME%\Dotnet.Nuget.Sign.proj"

echo ====================================BM4
:: Publish managed packages.

copy "%SOURCE_DIR%\Alternet.UI\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

copy "%SOURCE_DIR%\Alternet.UI\bin\Release\*.snupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================BM5
::Alternet.UI.Maui
pushd "%SOURCE_DIR%\Alternet.UI.Maui\"
dotnet msbuild -tl:off /restore /t:Clean,Build,Pack /p:Configuration=Release /p:AlternetUIPackagesBuild=true /p:WarningLevel=0
popd
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

dotnet msbuild -tl:off /t:DotNetNugetSign /p:NUGET_PATH="%SOURCE_DIR%\Alternet.UI.Maui\bin\Release\*.nupkg" "%SCRIPT_HOME%\Dotnet.Nuget.Sign.proj"

copy "%SOURCE_DIR%\Alternet.UI.Maui\bin\Release\*.nupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

copy "%SOURCE_DIR%\Alternet.UI.Maui\bin\Release\*.snupkg" "%PackagesPublishDirectory%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================BM6



