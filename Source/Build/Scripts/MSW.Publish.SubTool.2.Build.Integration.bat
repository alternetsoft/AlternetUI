if %USERNAME% EQU %COMPUTERNAME%$ (
    set NUGET_PACKAGES=C:\Windows\system32\config\systemprofile\.nuget\packages\)

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set CERT_PASSWORD=%1

:: Clean

:: REM set RELEASE_DIRECTORY_VS_2019=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2019\Release
set RELEASE_DIRECTORY_VS_2022=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2022\Release
set RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES=%SCRIPT_HOME%\..\..\Integration\Templates\bin\Release

:: REM if not exist "%RELEASE_DIRECTORY_VS_2019%" (mkdir "%RELEASE_DIRECTORY_VS_2019%")

:: REM IF EXIST "%RELEASE_DIRECTORY_VS_2019%\*.vsix" (
:: REM     del /q /s "%RELEASE_DIRECTORY_VS_2019%\*.vsix")

if not exist "%RELEASE_DIRECTORY_VS_2022%" (mkdir "%RELEASE_DIRECTORY_VS_2022%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

IF EXIST "%RELEASE_DIRECTORY_VS_2022%\*.vsix" (
    del /q /s "%RELEASE_DIRECTORY_VS_2022%\*.vsix")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

if not exist "%RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES%" (mkdir "%RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

IF EXIST "%RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES%\*.nupkg" (
    del /q /s "%RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES%\*.nupkg")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Build

:: VS 2019

:: REM "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version [16.0,17.0) -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > tmpFile 
:: REM set /p FOUND_MSBUILD_PATH_VS_2019= < tmpFile 
:: REM del tmpFile 
:: REM "%FOUND_MSBUILD_PATH_VS_2019%" /restore /p:VsTargetVersion=VS2019 "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"
:: REM "%FOUND_MSBUILD_PATH_VS_2019%" /restore /t:Clean,Build /p:Configuration=Release "%SCRIPT_HOME%\..\..\Integration\Components\Alternet.UI.Integration.UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp.csproj"
:: REM "%FOUND_MSBUILD_PATH_VS_2019%" /restore /t:Clean,Build /p:Configuration=Release;DeployExtension=False "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"

:: VS 2022

"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version [17.0,18.0) -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > tmpFile 
set /p FOUND_MSBUILD_PATH_VS_2022= < tmpFile 
del tmpFile 

echo ====================================
:: Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio
"%FOUND_MSBUILD_PATH_VS_2022%" /restore /p:VsTargetVersion=VS2022 /p:WarningLevel=0  "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"

echo ====================================
:: Integration\Components\Alternet.UI.Integration.UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp
"%FOUND_MSBUILD_PATH_VS_2022%" /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0 "%SCRIPT_HOME%\..\..\Integration\Components\Alternet.UI.Integration.UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

echo ====================================
:: Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio
"%FOUND_MSBUILD_PATH_VS_2022%" /restore /t:Clean,Build /p:Configuration=Release;DeployExtension=False /p:WarningLevel=0 "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Command Line Templates

echo ====================================
:: Integration\Templates\Alternet.UI.Templates
dotnet msbuild /restore /t:Clean,Build,Pack /p:Configuration=Release /p:WarningLevel=0 "%SCRIPT_HOME%\..\..\Integration\Templates\Alternet.UI.Templates.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

echo ====================================

call MSW.Publish.SubTool.3.Nuget.Sign.bat "%SCRIPT_HOME%\..\..\Integration\Templates\bin\Release\*.nupkg" %CERT_PASSWORD%