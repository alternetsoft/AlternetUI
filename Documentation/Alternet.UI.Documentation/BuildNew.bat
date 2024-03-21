call Del.AllResults.bat
dotnet build /p:LatestDocFx=true Alternet.UI.Documentation.csproj
call calljobdone.bat "UI Documentation was built ok"