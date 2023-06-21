if %USERNAME% EQU %COMPUTERNAME%$ (
    set NUGET_PACKAGES=C:\Windows\system32\config\systemprofile\.nuget\packages\)

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Clean

:: REM set RELEASE_DIRECTORY_VS_2019=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2019\Release
set RELEASE_DIRECTORY_VS_2022=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2022\Release
set RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES=%SCRIPT_HOME%\..\..\Integration\Templates\bin\Release

:: REM if not exist "%RELEASE_DIRECTORY_VS_2019%" (mkdir "%RELEASE_DIRECTORY_VS_2019%")
:: REM if not !ERRORLEVEL! EQU 0 (
:: REM     exit /b !ERRORLEVEL!)

:: REM IF EXIST "%RELEASE_DIRECTORY_VS_2019%\*.vsix" (
:: REM     del /q /s "%RELEASE_DIRECTORY_VS_2019%\*.vsix")
:: REM if not !ERRORLEVEL! EQU 0 (
:: REM     exit /b !ERRORLEVEL!)

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
:: REM if not !ERRORLEVEL! EQU 0 (
:: REM     exit /b !ERRORLEVEL!)

:: REM "%FOUND_MSBUILD_PATH_VS_2019%" /restore /t:Clean,Build /p:Configuration=Release;DeployExtension=False "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"
:: REM if not !ERRORLEVEL! EQU 0 (
:: REM     exit /b !ERRORLEVEL!)

:: VS 2022

"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version [17.0,18.0) -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > tmpFile 
set /p FOUND_MSBUILD_PATH_VS_2022= < tmpFile 
del tmpFile 

"%FOUND_MSBUILD_PATH_VS_2022%" /restore /p:VsTargetVersion=VS2022 "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"

"%FOUND_MSBUILD_PATH_VS_2022%" /restore /t:Clean,Build /p:Configuration=Release "%SCRIPT_HOME%\..\..\Integration\Components\Alternet.UI.Integration.UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

"%FOUND_MSBUILD_PATH_VS_2022%" /restore /t:Clean,Build /p:Configuration=Release;DeployExtension=False "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Command Line Templates

dotnet msbuild /restore /t:Clean,Build,Pack /p:Configuration=Release "%SCRIPT_HOME%\..\..\Integration\Templates\Alternet.UI.Templates.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet nuget sign "%SCRIPT_HOME%\..\..\Integration\Templates\bin\Release\*.nupkg" --certificate-path "%SCRIPT_HOME%\..\..\Keys\Alternet.pfx" --timestamper http://timestamp.digicert.com --certificate-password Alternet^^!
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)