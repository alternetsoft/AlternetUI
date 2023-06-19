SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do (
    set FOUND_MSBUILD_PATH=%%i)


dotnet msbuild /p:CppMsBuildPath="%FOUND_MSBUILD_PATH%" /t:InstallAlternetUI "%SCRIPT_HOME%\Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

exit /b 0