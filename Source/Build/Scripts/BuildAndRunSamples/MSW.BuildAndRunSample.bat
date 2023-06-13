set arg1=%1
set platform=%2

if "%platform%"==x64 goto ok
if "%platform%"==x86 goto ok
set platform=x64
:ok
ECHO platform: %platform%

dotnet build /p:Platform=%platform% "..\..\..\Samples\%arg1%\%arg1%.csproj" --property WarningLevel=0
start /b dotnet run /p:Platform=%platform% --project "..\..\..\Samples\%arg1%\%arg1%.csproj" --property WarningLevel=0 --framework net6.0

rem start /b "..\..\..\Samples\%arg1%\bin\X64\debug\net6.0\%arg1%.exe"
