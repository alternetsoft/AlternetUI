cls
pushd "..\Source\Tools\Alternet.UI.RunCmdStudio\"
dotnet build
popd

pushd "..\Source\Tools\Alternet.UI.RunCmdStudio\bin\Debug\net10.0\"
Alternet.UI.RunCmdStudio.exe -r=logLicense
popd