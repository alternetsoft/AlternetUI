@REM SETLOCAL EnableDelayedExpansion

@REM set SCRIPT_HOME=%~dp0.

@REM :: Set up.

@REM set PublishRoot=%SCRIPT_HOME%\..\..\..\Publish\Artifacts\Deploy
@REM if not exist "%PublishRoot%" (exit /b 1)

@REM set PublicComponentsSources=%PublishRoot%\PublicComponents
@REM if not exist "%PublicComponentsSources%" (exit /b 1)

@REM set PublicExamplesSources=%PublishRoot%\PublicExamples
@REM if not exist "%PublicExamplesSources%" (exit /b 1)

@REM set PublicRepo=%PublishRoot%\Repo
@REM mkdir %PublicRepo%
@REM if not !ERRORLEVEL! EQU 0 (
@REM     exit /b !ERRORLEVEL!)

@REM set NuGetApiKey=oy2iknibdcdvlz5ztp3ae34lrw65b5uebhs2dcvfwrlitq
@REM set GitHubApiKey=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

@REM :: NuGet packages.

@REM dotnet nuget push "%PublishRoot%\*.nupkg" -k %NuGetApiKey% -s https://api.nuget.org/v3/index.json
@REM if not !ERRORLEVEL! EQU 0 (
@REM     exit /b !ERRORLEVEL!)

@REM :: Public Components Sources

@REM call "%SCRIPT_HOME%\DeployPublicSource.bat" %PublicComponentsSources% %PublicRepo%\Components %PublicRepo%\ComponentsCommitMessage.txt %GitHubApiKey% alternet-ui
@REM if not !ERRORLEVEL! EQU 0 (
@REM     exit /b !ERRORLEVEL!)

@REM :: Public Examples Sources

@REM call "%SCRIPT_HOME%\DeployPublicSource.bat" %PublicExamplesSources% %PublicRepo%\Examples %PublicRepo%\ExamplesCommitMessage.txt %GitHubApiKey% alternet-ui-examples
@REM if not !ERRORLEVEL! EQU 0 (
@REM     exit /b !ERRORLEVEL!)

@REM :: Clean Up
@REM RD /S /Q "%PublicRepo%"

@REM exit /b