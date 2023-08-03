pushd %1
dotnet build --nologo --property WarningLevel=0
start /b dotnet run --nologo --property WarningLevel=0 --framework net6.0
popd


