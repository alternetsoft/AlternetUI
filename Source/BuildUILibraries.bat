SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

dotnet restore Alternet.UI.Common\Alternet.UI.Common.csproj
dotnet restore Alternet.UI\Alternet.UI.csproj
dotnet restore Alternet.UI.Maui\Alternet.UI.Maui.csproj

dotnet build Alternet.UI.Common\Alternet.UI.Common.csproj
dotnet build Alternet.UI\Alternet.UI.csproj
dotnet build Alternet.UI.Maui\Alternet.UI.Maui.csproj