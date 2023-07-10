echo ================
set SCRIPT_HOME=%~dp0

mkdir "c:\AlternetTest.6AB52"
del /s /q "c:\AlternetTest.6AB52\*.*"
for /d %%G in ("c:\AlternetTest.6AB52\*") do rd /s /q "%%G" 

pushd "%SCRIPT_HOME%..\..\Publish\Packages\"

forfiles /s /m "*.nupkg" /c "cmd /c "%SCRIPT_HOME%\nuget" add @path -Source "c:\AlternetTest.6AB52"
popd

pushd "c:\AlternetTest.6AB52"
dir
popd
echo ================