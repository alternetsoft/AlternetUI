pushd %1
dotnet build /p:Platform=x64 --arch x64 --nologo --property WarningLevel=0 --framework net6.0
start /b dotnet run /p:Platform=x64 --arch x64 --nologo --property WarningLevel=0 --framework net6.0
popd

:: set /p input= Type any input
