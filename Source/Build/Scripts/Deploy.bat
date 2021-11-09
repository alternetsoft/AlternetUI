SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

:: Set up.

set PublishRoot=%SCRIPT_HOME%\..\..\..\Publish\Artifacts\Deploy
if not exist "%PublishRoot%" (exit /b 1)

set PublicComponentsSources=%PublishRoot%\PublicComponents
if not exist "%PublicComponentsSources%" (exit /b 1)

set PublicExamplesSources=%PublishRoot%\PublicExamples
if not exist "%PublicExamplesSources%" (exit /b 1)

set PublicRepo=%PublishRoot%\Repo
mkdir %PublicRepo%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

set NuGetApiKey=oy2iknibdcdvlz5ztp3ae34lrw65b5uebhs2dcvfwrlitq
set GitHubApiKey=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
set AzureDevOpsAccessToken=4u542gllvtm2sl3zkuquchj6ma4aexme5qykv7dyozkrmz26cora

:: NuGet packages.

dotnet nuget push "%PublishRoot%\*.nupkg" --skip-duplicate -k %NuGetApiKey% -s https://api.nuget.org/v3/index.json
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Public Components Sources

call "%SCRIPT_HOME%\DeployPublicSource.bat" %PublicComponentsSources% %PublicRepo%\Components %PublicRepo%\ComponentsCommitMessage.txt %GitHubApiKey% alternet-ui
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: Public Examples Sources

call "%SCRIPT_HOME%\DeployPublicSource.bat" %PublicExamplesSources% %PublicRepo%\Examples %PublicRepo%\ExamplesCommitMessage.txt %GitHubApiKey% alternet-ui-examples
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

:: VS Extensions

set VSIXPublisherTool=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VSSDK\VisualStudioIntegration\Tools\Bin\VsixPublisher.exe
set VSIXPublishManifest2019=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Publish\extension.VS2019.manifest.json
set VSIXPublishManifest2022=%SCRIPT_HOME%\..\..\Integration\VisualStudio\Publish\extension.VS2022.manifest.json
set VSIXPackagePath2019=%PublishRoot%\Alternet.UI.Integration.VisualStudio.VS2019*.vsix
set VSIXPackagePath2022=%PublishRoot%\Alternet.UI.Integration.VisualStudio.VS2022*.vsix

for %%i in (%VSIXPackagePath2019%) do "%VSIXPublisherTool%" publish -payload "%%i" -publishManifest "%VSIXPublishManifest2019%" -personalAccessToken %AzureDevOpsAccessToken%
for %%i in (%VSIXPackagePath2022%) do "%VSIXPublisherTool%" publish -payload "%%i" -publishManifest "%VSIXPublishManifest2022%" -personalAccessToken %AzureDevOpsAccessToken%

:: Clean Up
RD /S /Q "%PublicRepo%"

exit /b