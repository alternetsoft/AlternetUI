set SCRIPT_HOME=%~dp0


pushd "%SCRIPT_HOME%..\..\Publish\Packages\"

forfiles /s /m "*.nupkg" /c "cmd /c "%SCRIPT_HOME%\nuget" add @path -Source "c:\AlternetTest"
popd

