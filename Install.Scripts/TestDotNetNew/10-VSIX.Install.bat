SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set PublishRoot=%SCRIPT_HOME%\..\..\Publish\
set VSIXInstaller="C:\Program Files (x86)\Microsoft Visual Studio\Installer\resources\app\ServiceHub\Services\Microsoft.VisualStudio.Setup.Service\vsixinstaller"
set VSIXInstaller="C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\vsixinstaller"

pushd "%PublishRoot%\Packages"

for /f %%f in ('dir /b Alternet.UI.Integration.VisualStudio*.vsix') do %VSIXInstaller% /q "%%f"

popd

