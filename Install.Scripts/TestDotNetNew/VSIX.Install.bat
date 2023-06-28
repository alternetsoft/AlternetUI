SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set PublishRoot=%SCRIPT_HOME%\..\..\Publish\


pushd "C:\Program Files (x86)\Microsoft Visual Studio\Installer\resources\app\ServiceHub\Services\Microsoft.VisualStudio.Setup.Service\"
vsixinstaller "%PublishRoot%\Packages\Alternet.UI.Integration.VisualStudio.VS2022-0.9.2-beta.vsix"
popd