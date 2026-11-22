SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

pushd "%SCRIPT_HOME%\..\..\..\Install.Scripts"

call MSW.Publish.1.Build.Nuget.Pal.bat

popd