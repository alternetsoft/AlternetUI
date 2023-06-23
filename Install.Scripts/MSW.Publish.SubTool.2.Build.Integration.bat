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

"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version [17.0,18.0) -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > tmpFile 
set /p FOUND_MSBUILD_PATH_VS_2022= < tmpFile 
del tmpFile 

echo ====================================
echo ====================================
echo ====================================
echo  Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio
"%FOUND_MSBUILD_PATH_VS_2022%" /restore /p:VsTargetVersion=VS2022 /p:WarningLevel=0  "%SOURCE_DIR%\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================
echo  Integration\Components\Alternet.UI.Integration.UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp
"%FOUND_MSBUILD_PATH_VS_2022%" /restore /t:Clean,Build /p:Configuration=Release /p:WarningLevel=0 "%SOURCE_DIR%\Integration\Components\Alternet.UI.Integration.UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp.csproj"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================
echo  Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio
"%FOUND_MSBUILD_PATH_VS_2022%" /restore /t:Clean,Build /p:Configuration=Release;DeployExtension=False /p:WarningLevel=0 "%SOURCE_DIR%\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio.csproj"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

:: Command Line Templates  =========================

echo ====================================
echo  Integration\Templates\Alternet.UI.Templates
dotnet msbuild /restore /t:Clean,Build,Pack /p:Configuration=Release /p:WarningLevel=0 "%SOURCE_DIR%\Integration\Templates\Alternet.UI.Templates.csproj"
if not !ERRORLEVEL! EQU 0 (exit /b !ERRORLEVEL!)

echo ====================================

call MSW.Publish.SubTool.3.Nuget.Sign.bat "%SOURCE_DIR%\Integration\Templates\bin\Release\*.nupkg" %CERT_PASSWORD%