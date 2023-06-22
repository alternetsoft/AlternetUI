cd ..\Alternet.UI.Build.Tasks.ApiInfoCollector
dotnet build -c Debug
dotnet build -c Release

cd ..
dotnet build -c Debug
dotnet build -c Release


