CLS

ECHO OFF
ECHO ===========================================
ECHO ===========================================

SETLOCAL EnableDelayedExpansion

set FRAMEWORK=%1
set DEMO_NAME=%2
set ADD_PARAMS=%3

ECHO BuildExeDemo %DEMO_NAME% %FRAMEWORK%
ECHO ===========================================
ECHO ON

set SCRIPT_HOME=%~dp0
set SOURCE_HOME=%SCRIPT_HOME%\..\..\Source
set ALTERNET_HOME=%SCRIPT_HOME%\..\..\
set SAMPLES_HOME=%SOURCE_HOME%\Samples
set DEMO_HOME=%SAMPLES_HOME%\ControlsSample
set DEMO_HOME_BIN=%DEMO_HOME%\bin
set DEMO_HOME_BIN_RELEASE=%DEMO_HOME%\bin\Release
set SIGN_PAL_IN_FOLDER=%SCRIPT_HOME%\..\SignTools\Sign.Pal.InFolder.bat
set SIGN_EXE_IN_FOLDER=%SCRIPT_HOME%\..\SignTools\Sign.Exe.InFolder.bat
set CLEAN_PROJECT=%SCRIPT_HOME%\..\Tools\CleanProject.bat
set PUBLISH_FOLDER_PARENT=%ALTERNET_HOME%\Publish\Packages
set PUBLISH_FOLDER=%PUBLISH_FOLDER_PARENT%\%DEMO_NAME%
set PUBLISH_ZIP=%PUBLISH_FOLDER%.zip
set VersionToolProject=%SOURCE_HOME%\Tools\Versioning\Alternet.UI.VersionTool.Cli\Alternet.UI.VersionTool.Cli.csproj

call "%CLEAN_PROJECT%" "%DEMO_HOME%"

pushd %PUBLISH_FOLDER_PARENT%
if exist "%DEMO_NAME%" (rmdir /s /q "%DEMO_NAME%")
mkdir %DEMO_NAME%
popd

pushd %DEMO_HOME%
dotnet clean
dotnet build -c Release --framework %FRAMEWORK% %ADD_PARAMS%
popd

ECHO ====================================
ECHO SIGN PAL IN FOLDER: %DEMO_HOME_BIN%
call %SIGN_PAL_IN_FOLDER% %DEMO_HOME_BIN%

ECHO ====================================

ECHO SIGN EXE IN FOLDER: %DEMO_HOME_BIN%
call %SIGN_EXE_IN_FOLDER% %DEMO_HOME_BIN%
ECHO ====================================

xcopy "%DEMO_HOME_BIN_RELEASE%\%FRAMEWORK%" "%PUBLISH_FOLDER%" /s /e

copy %SCRIPT_HOME%\PublicFiles\ExeDemoREADME.md %PUBLISH_FOLDER%\README.md

pushd "%PUBLISH_FOLDER%"
del Alternet.UI.xml
del Alternet.UI.Common.xml
del Alternet.UI.Interfaces.xml
rmdir /s /q runtimes\linux-arm
rmdir /s /q runtimes\linux-arm64
rmdir /s /q runtimes\linux-musl-x64
rmdir /s /q runtimes\linux-x64
rmdir /s /q runtimes\osx
rmdir /s /q runtimes\win-arm64
rmdir /s /q arm
rmdir /s /q arm64
rmdir /s /q musl-x64

:::::::::::::::::::::::::::

del *.pdb
popd

:: powershell -command "Compress-Archive -Path '%PUBLISH_FOLDER%\*.*' -DestinationPath '%PUBLISH_ZIP%'"

pushd %PUBLISH_FOLDER_PARENT%
del %DEMO_NAME%.zip

"C:\Program Files\7-Zip\7z" a -tzip -r %DEMO_NAME% -mx7 "%DEMO_NAME%\*"

dotnet run --project "%VersionToolProject%" --property WarningLevel=0 -- append-version-suffix "%DEMO_NAME%.zip"

popd

if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0