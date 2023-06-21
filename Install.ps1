. Install.Scripts\Common.ps1

Write-Host Starting Alternet.UI Installation

Invoke-Dotnet -Command msbuild -Arguments '/t:InstallAlternetUI "Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj"'

Write-Host Done Alternet.UI Installation
