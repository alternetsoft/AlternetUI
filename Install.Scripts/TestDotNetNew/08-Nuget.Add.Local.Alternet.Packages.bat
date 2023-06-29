echo ================
set SCRIPT_HOME=%~dp0

mkdir "c:\AlternetTest"
del /s /q "c:\AlternetTest\*.*"
for /d %%G in ("c:\AlternetTest\*") do rd /s /q "%%G" 

pushd "%SCRIPT_HOME%..\..\Publish\Packages\"

forfiles /s /m "*.nupkg" /c "cmd /c "%SCRIPT_HOME%\nuget" add @path -Source "c:\AlternetTest"
popd

pushd "c:\AlternetTest"
dir
popd
echo ================