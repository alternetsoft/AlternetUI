dotnet clean
dotnet run --framework net8.0 -c Release /p:Optimize=false /p:DebugType=portable /p:DebugSymbols=true --no-incremental