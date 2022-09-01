SETLOCAL EnableDelayedExpansion

set ScriptHome=%~dp0.

set SourceRoot=%ScriptHome%\..\..\..\..\
set RefsRoot=%ScriptHome%\..\Refs\
set ContentRoot=%ScriptHome%\Content\
set ProjectDirectory=%RefsRoot%\refs-net6.0\
set OutputDirectory=%ProjectDirectory%\bin\Debug\net6.0\

if not exist "%OutputDirectory%" (mkdir "%OutputDirectory%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%SourceRoot%\Alternet.UI\bin\Debug\netcoreapp3.1\*.*" "%OutputDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

set targetPalDirectory=%OutputDirectory%\runtimes\win-x64\native\
if not exist "%targetPalDirectory%" (mkdir "%targetPalDirectory%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%SourceRoot%\Alternet.UI.Pal\bin\x64\Debug\*.*" "%targetPalDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet build "%ProjectDirectory%\refs-net6.0.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%ContentRoot%\refs-net6.0\*.*" "%OutputDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)