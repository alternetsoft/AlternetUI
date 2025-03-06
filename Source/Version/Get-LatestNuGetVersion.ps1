param (
    [string]$PackageName = "PACKAGE_NAME"
)

$url = "https://api.nuget.org/v3-flatcontainer/$($PackageName.ToLower())/index.json"
$versions = Invoke-RestMethod -Uri $url
$latestVersion = $versions.versions[-1]
Write-Output "Latest $PackageName = $latestVersion"
