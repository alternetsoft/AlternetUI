# Remove-BinObj.ps1
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$root = Resolve-Path "$scriptDir\..\.."

Write-Host "Starting cleanup in: $root`n"

Get-ChildItem -Path $root -Directory -Recurse |
    Where-Object {
        $_.FullName -notmatch '\\\.git\\' -and $_.Name -in @('bin', 'obj')
    } |
    ForEach-Object {
        try {
            Remove-Item -Path $_.FullName -Recurse -Force -ErrorAction Stop
            Write-Host "Removed: $($_.FullName)"
        } catch {
            Write-Host "Failed to remove: $($_.FullName) — $($_.Exception.Message)"
        }
    }

Write-Host "`nCleanup complete. All bin/obj folders removed (excluding .git)."
