Get-CimInstance Win32_Process -Filter "Name = 'dotnet.exe'" |
  Where-Object { $_.CommandLine -match 'MSBuild\.dll' } |
  ForEach-Object {
    Write-Host "Killing PID $($_.ProcessId): $($_.CommandLine)"
    Stop-Process -Id $_.ProcessId -Force
  }