:: Look for the Machine type:
:: 8664 - x64
:: AA64 - ARM64
:: 14C - x86

pushd bin\x64\Debug
"C:\Program Files\Microsoft Visual Studio\18\Community\VC\Tools\MSVC\14.50.35717\bin\Hostx64\x64\dumpbin.exe" /headers Alternet.UI.Pal.dll | findstr machine
popd

