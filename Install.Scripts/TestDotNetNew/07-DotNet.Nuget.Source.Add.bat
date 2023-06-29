echo ================
set SCRIPT_HOME=%~dp0.
mkdir "c:\AlternetTest.6AB52"
dotnet nuget add source "c:\AlternetTest.6AB52" -n AlternetTest

call Info.DotNet.Nuget.Source.List.bat
echo ================