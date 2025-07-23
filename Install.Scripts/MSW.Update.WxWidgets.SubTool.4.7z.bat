SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set PROJ=%SCRIPT_HOME%\..\Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj

set EXTERNAL=%SCRIPT_HOME%\..\External\
set A7Z="C:\Program Files\7-Zip\7z"
set RESULT=wxWidgets-bin-noobjpch-3.3.1.7z
pushd "%EXTERNAL%"
del "%RESULT%"
%A7Z% a "%RESULT%" "WxWidgets\" -r -xr^^!*.obj -xr^^!*.pch -mqs -mx9 -m0=lzma2:a=1:mf=bt4:d=512m:fb=273:mc=10000 -mmt=off
popd
