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

:: NuGet packages.

dotnet nuget push "%PublishRoot%\*.nupkg" -k %NuGetApiKey% -s https://api.nuget.org/v3/index.json
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

:: Clean Up
RD /S /Q "%PublicRepo%"

exit /b