
SETLOCAL EnableDelayedExpansion
cls

setlocal

set SCRIPT_HOME=%~dp0.


pushd "%SCRIPT_HOME%\..\..\Install.Scripts\TestDotNetNew"
call 05-Nuget.Download.Exe.bat
popd

set NUGET_EXE=%SCRIPT_HOME%\..\..\Install.Scripts\TestDotNetNew\nuget.exe


pushd "%SCRIPT_HOME%\Output\"

for %%f in (*.nupkg) do (
    "%NUGET_EXE%" sign "%%f" ^
        -CertificateSubjectName "ALTERNET SOFTWARE PTY LTD" ^
        -Timestamper http://timestamp.digicert.com
)


popd

