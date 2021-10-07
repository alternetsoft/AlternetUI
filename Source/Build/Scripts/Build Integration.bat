if %USERNAME% EQU %COMPUTERNAME%$ (
    set NUGET_PACKAGES=C:\Windows\system32\config\systemprofile\.nuget\packages\)

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Clean

set RELEASE_DIRECTORY_VS_2019=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2019\Release
set RELEASE_DIRECTORY_VS_2022=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\bin\VS2022\Release
set RELEASE_DIRECTORY_COMMAND_LINE_TEMPLATES=%SCRIPT_HOME%\..\..\Integration\Templates

if not exist "%RELEASE_DIRECTORY_VS_2019%" (mkdir "%RELEASE_DIRECTORY_VS_2019%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

IF EXIST "%RELEASE_DIRECTORY_VS_2019%\*.vsix" (
    del /q /s "%RELEASE_DIRECTORY_VS_2019%\*.vsix")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

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

"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -prerelease -version [16.0,17.0) -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > tmpFile 
set /p FOUND_MSBUILD_PATH_VS_2019= < tmpFile 
del tmpFile 

"%FOUND_MSBUILD_PATH_VS_2019%" /restore /p:VsTargetVersion=VS2019 "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"

"%FOUND_MSBUILD_PATH_VS_2019%" /restore /t:Clean,Build /p:Configuration=Release;DeployExtension=False "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: VS 2022

"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -prerelease -version [17.0,18.0) -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > tmpFile 
set /p FOUND_MSBUILD_PATH_VS_2022= < tmpFile 
del tmpFile 

"%FOUND_MSBUILD_PATH_VS_2022%" /restore /p:VsTargetVersion=VS2022 "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"

"%FOUND_MSBUILD_PATH_VS_2022%" /restore /t:Clean,Build /p:Configuration=Release;DeployExtension=False "%SCRIPT_HOME%\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Command Line Templates

dotnet msbuild /restore /t:Clean,Build,Pack /p:Configuration=Release "%SCRIPT_HOME%\..\..\Integration\Templates\Alternet.UI.Templates.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

    