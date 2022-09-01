SETLOCAL EnableDelayedExpansion

set ScriptHome=%~dp0.

set SourceRoot=%ScriptHome%\..\..\..\..\

set RefsRoot=%ScriptHome%\..\Refs\

rem .NET 4.6.1
set net461OutputDirectory=%RefsRoot%\refs-net461\bin\Debug\net461\

copy "%SourceRoot%\Alternet.UI\bin\Debug\net461\*.*" "%net461OutputDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

set net461TargetPalDirectory=%net461OutputDirectory%\x64\
if not exist "%net461TargetPalDirectory%" (mkdir "%net461TargetPalDirectory%")
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

copy "%SourceRoot%\Alternet.UI.Pal\bin\x64\Debug\*.*" "%net461TargetPalDirectory%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)