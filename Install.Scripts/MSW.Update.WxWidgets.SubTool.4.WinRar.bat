SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set PROJ=%SCRIPT_HOME%\..\Source\Build\Alternet.UI.Pal\Alternet.UI.Pal.proj

set EXTERNAL=%SCRIPT_HOME%\..\External\
set WINRAR="C:\Program Files\WinRAR\WinRAR.exe"
set RESULT=wxWidgets-bin-noobjpch-3.2.3.zip
pushd "%EXTERNAL%"
del "%RESULT%"
%WINRAR% a -s -r -x*.obj -x*.pch "%RESULT%" "WxWidgets\*.*"
popd
