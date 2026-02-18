Get-CimInstance Win32_Process -Filter "Name = 'dotnet.exe'" |
  Select-Object ProcessId, ParentProcessId, CommandLine, ExecutablePath, CreationDate