SETLOCAL EnableDelayedExpansion

set ScriptHome=%~dp0.

set SourceRoot=%ScriptHome%\..\..\..\..\
set NuGetRoot=%ScriptHome%\..\NuGet\

dotnet build "%NuGetRoot%\nuget-net461\nuget-net461.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)


dotnet build "%NuGetRoot%\nuget-netcoreapp3.1\nuget-netcoreapp3.1.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet build "%NuGetRoot%\nuget-net6.0\nuget-net6.0.csproj"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)