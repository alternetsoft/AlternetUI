echo ================
set SCRIPT_HOME=%~dp0
set NUGET_HOME=%SCRIPT_HOME%\..\TestDotNetNew\nuget


forfiles /s /m "*.nupkg" /c "cmd /c "%NUGET_HOME%" add @path -Source "c:\AlternetTest.6AB52"
popd

pushd "c:\AlternetTest.6AB52"
dir
popd
echo ================