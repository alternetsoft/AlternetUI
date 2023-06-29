echo ================
set SCRIPT_HOME=%~dp0.
mkdir "c:\AlternetTest"
dotnet nuget add source "c:\AlternetTest" -n AlternetTest

call Info.DotNet.Nuget.Source.List.bat
echo ================