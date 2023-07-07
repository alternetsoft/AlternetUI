SETLOCAL EnableDelayedExpansion

set ScriptHome=%~dp0.

set SourceRoot=%ScriptHome%\..\..\..\..\
set RefsRoot=%ScriptHome%\..\Refs\

set ProjectDirectory=%RefsRoot%\refs-net461\
set OutputDirectory=%ProjectDirectory%\bin\Debug\net461\

if not exist "%OutputDirectory%" (mkdir "%OutputDirectory%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%SourceRoot%\Alternet.UI\bin\Debug\net462\*.*" "%OutputDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

set targetPalDirectory=%OutputDirectory%\x64\
if not exist "%targetPalDirectory%" (mkdir "%targetPalDirectory%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%SourceRoot%\Alternet.UI.Pal\bin\x64\Debug\*.*" "%targetPalDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet build "%ProjectDirectory%\refs-net462.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)