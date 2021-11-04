SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set PublishRoot=%SCRIPT_HOME%\..\..\..\Publish\Artifacts\Deploy
set PublishManifest=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Publish\extension.manifest.json
set PublisherTool=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VSSDK\VisualStudioIntegration\Tools\Bin\VsixPublisher.exe
set PackagePath=%PublishRoot%\Alternet.UI.Integration.VisualStudio.VS2019-0.1.68-beta.vsix
set AzureDevOpsAccessToken=4u542gllvtm2sl3zkuquchj6ma4aexme5qykv7dyozkrmz26cora


"%PublisherTool%" publish -payload "%PackagePath%" -publishManifest "%PublishManifest%" -personalAccessToken %AzureDevOpsAccessToken%