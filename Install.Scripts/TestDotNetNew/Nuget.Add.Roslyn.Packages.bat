echo ================
set SCRIPT_HOME=%~dp0

mkdir "c:\AlternetTest.6AB52"

pushd "%SCRIPT_HOME%..\..\Publish\Packages\"

forfiles /s /m "Alternet.Studio.*.nupkg" /c "cmd /c "%SCRIPT_HOME%\nuget" add @path -Source "c:\AlternetTest.6AB52"
popd

pushd "c:\AlternetTest.6AB52"
dir
popd
echo ================