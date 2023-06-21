function Invoke-Dotnet {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory = $true)]
        [System.String]
        $Command,

        [Parameter(Mandatory = $true)]
        [System.String]
        $Arguments
    )

    $DotnetArgs = @()
    $DotnetArgs = $DotnetArgs + $Command
    $DotnetArgs = $DotnetArgs + ($Arguments -split "\s+")

    # [void]($Output = & dotnet $DotnetArgs)

    & dotnet $DotnetArgs

    # Should throw if the last command failed.
    #if ($LASTEXITCODE -ne 0) {
    #    Write-Warning -Message ($Output -join "; ")
    #    throw "There was an issue running the specified dotnet command."
    #}
}
