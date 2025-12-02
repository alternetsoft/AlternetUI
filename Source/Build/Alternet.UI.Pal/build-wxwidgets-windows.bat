
pushd ..\..\..\External\WxWidgets\build\msw\ 

:: msbuild *.vcxproj /p:configuration=debug /p:platform=win32

SET msb=C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\amd64\MSBuild.exe

"%msb%" wx_vc17.sln -maxcpucount /p:configuration=debug /p:platform=x64

popd