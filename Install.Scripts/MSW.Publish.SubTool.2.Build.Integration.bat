::if %USERNAME% EQU %COMPUTERNAME%$ (
::    set NUGET_PACKAGES=C:\Windows\system32\config\systemprofile\.nuget\packages\)

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set CERT_PASSWORD=%1

:: Clean =========================

set RELEASE_DIRECTORY_VS_2022=%SOURCE_DIR%\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2022\Release
set RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES=%SOURCE_DIR%\Integration\Templates\bin\Release

if not exist "%RELEASE_DIRECTORY_VS_2022%" (mkdir "%RELEASE_DIRECTORY_VS_2022%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

IF EXIST "%RELEASE_DIRECTORY_VS_2022%\*.vsix" (
    del /q /s "%RELEASE_DIRECTORY_VS_2022%\*.vsix")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

if not exist "%RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES%" (mkdir "%RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES%")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

IF EXIST "%RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES%\*.nupkg" (
    del /q /s "%RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES%\*.nupkg")
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Build =========================

:: VS 2022

"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version [17.0,19.0) -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > tmpFile 
set /p MSBUILD_2022= < tmpFile 
del tmpFile 

:: Command Line Templates  =========================

echo ====================================
echo Build Alternet.UI.Templates
echo ====================================

set UITemplates=%SOURCE_DIR%\Integration\Templates\Alternet.UI.Templates.csproj

dotnet msbuild -tl:off /restore /t:Clean,Build,Pack /p:Configuration=Release /p:WarningLevel=0 "%UITemplates%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

::::::::::::::::::::::::::::::::::::::

echo ====================================
echo Sign Alternet.UI.Templates
echo ====================================

set NugetSignProj=%SCRIPT_HOME%\Dotnet.Nuget.Sign.proj

dotnet msbuild -tl:off /t:DotNetNugetSign /p:NUGET_PATH="%SOURCE_DIR%\Integration\Templates\bin\Release\*.nupkg" "%NugetSignProj%"

::::::::::::::::::::::::::::::::::::::

:: echo ====================================
:: echo Restore Alternet.UI.Integration.VisualStudio
:: echo ====================================

set IntVSPath=%SOURCE_DIR%\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio
set IntVS=%IntVSPath%\Alternet.UI.Integration.VisualStudio.csproj

:: "%MSBUILD_2022%" /restore /p:WarningLevel=0 "%IntVS%"
:: if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

::::::::::::::::::::::::::::::::::::::

echo ====================================
echo Build Alternet.UI.Integration.UIXmlHostApp
echo ====================================

set UIXmlHostApp=%SOURCE_DIR%\Integration\Components\Alternet.UI.Integration.UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp.csproj

:: "%MSBUILD_2022%" /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0 "%UIXmlHostApp%"

dotnet msbuild -tl:off /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0 "%UIXmlHostApp%"

if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

::::::::::::::::::::::::::::::::::::::

:: echo ====================================
:: echo Sign Dlls
:: echo ====================================

:: call "%SCRIPT_HOME%\SignTools\Sign.Pal.InFolder.bat" %IntVSPath%

::::::::::::::::::::::::::::::::::::::

echo ====================================
echo Build Alternet.UI.Integration.VisualStudio
echo ====================================

"%MSBUILD_2022%" /restore /t:Clean,Build /p:Configuration=Release;DeployExtension=False /p:WarningLevel=0 "%IntVS%"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

