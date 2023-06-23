set ALTERNET_PFX=%SCRIPT_HOME%\..\..\Keys\Alternet.pfx
set CERT_PASSWORD=%2
set NUGET_PATH=%1

if not exist "%ALTERNET_PFX%" (goto nugetnotsignedkey)
if not exist "%CERT_PASSWORD%" (goto nugetnotsignedpwd)

dotnet nuget sign "%NUGET_PATH%" --certificate-path "%ALTERNET_PFX%" --timestamper http://timestamp.digicert.com --certificate-password %CERT_PASSWORD%


if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

goto endoffile

:nugetnotsignedpwd

echo WARNING: Alternety.UI NuGet was not signed (no password provided as a parameter)
goto endoffile

:nugetnotsignedkey

echo WARNING: Alternety.UI NuGet was not signed (no Source\Keys\Alternet.pfx)

:endoffile