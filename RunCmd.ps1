# https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_automatic_variables?view=powershell-7.3

    Param (
        [System.String]
        $CommandName
    )

 if ($CommandName) {  } else { $CommandName="help" }

. Install.Scripts\Common.ps1

# Write-Host $PSScriptRoot

Push-Location "Source\Tools\Alternet.UI.RunCmd\"

# dotnet run -- -r="%CMD%"
# $DotnetArgs = @()
# $DotnetArgs = $DotnetArgs + "run"
# $DotnetArgs = $DotnetArgs + "--"
# $DotnetArgs = $DotnetArgs + "-r=$CmdName"
# & dotnet $DotnetArgs

Invoke-Dotnet -Command run -Arguments "-- -r=$CommandName"

Pop-Location



