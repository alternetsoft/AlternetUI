rem call 04-DotNet.Nuget.Locals.Clear.bat

del /s /q obj
del /s /q bin

dotnet build
dotnet run --project TestDotNetNew.csproj