SETLOCAL EnableDelayedExpansion

set ScriptHome=%~dp0.

set /p "NewVersion=Enter new NuGet packages version: "

set ProjectsRoot=%ScriptHome%\..\NuGet\

pushd "%ProjectsRoot%\nuget-net6.0\"
dotnet add package Alternet.UI --version %NewVersion%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)
popd

pushd "%ProjectsRoot%\nuget-net7.0\"
dotnet add package Alternet.UI --version %NewVersion%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)
popd

pushd "%ProjectsRoot%\nuget-net462\"
dotnet add package Alternet.UI --version %NewVersion%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)
popd