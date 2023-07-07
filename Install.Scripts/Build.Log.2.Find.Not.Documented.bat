SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source

pushd "%SOURCE_DIR%\Tools\Alternet.UI.RunCmd\"
dotnet run -- -r=filterLog Filter="CS1591" Log="%SOURCE_DIR%\Alternet.UI\Build.Result.Log" Result="%SOURCE_DIR%\Alternet.UI\Find.Not.Documented.log"
popd

:: 
