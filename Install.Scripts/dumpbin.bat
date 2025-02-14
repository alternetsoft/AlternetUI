@echo off
setlocal

:: Find the path to vswhere.exe
for /F "tokens=*" %%i in ('dir /b /s /a-d "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"') do set VSWHERE=%%i

:: Use vswhere.exe to find the Visual Studio installation path
for /F "tokens=*" %%i in ('"%VSWHERE%" -latest -products * -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath') do set VSINSTALLPATH=%%i

:: Output the installation path
echo Visual Studio is installed at: %VSINSTALLPATH%

set DumpbinPath=%VSINSTALLPATH%\VC\Tools\MSVC\14.42.34433\bin\Hostx64\x64\dumpbin.exe

 

rem Run dumpbin with the provided arguments
if not exist "%DumpbinPath%" (
    echo dumpbin.exe not found
    exit /b 1
)

"%DumpbinPath%" /headers %*
