set arg1=%1
dotnet build /p:Platform=x64 "..\..\Samples\%arg1%\%arg1%.csproj"
start /b ..\..\Samples\%arg1%\bin\X64\debug\net6.0\%arg1%.exe
