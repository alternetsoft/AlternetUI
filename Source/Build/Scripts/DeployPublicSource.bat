:: USAGE: DeployPublicSource.bat SourceDirectory RepoDirectory CommitMessageFile ApiKey RepoName

SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set SourceDirectory=%1
set RepoDirectory=%2
set CommitMessageFile=%3
set ApiKey=%4
set RepoName=%5

set RepoUrl=https://%ApiKey%@github.com/alternetsoft/%RepoName%.git
set VersionToolProject=%SCRIPT_HOME%\..\..\Tools\Versioning\Alternet.UI.VersionTool.Cli\Alternet.UI.VersionTool.Cli.csproj

mkdir %RepoDirectory%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

git clone %RepoUrl% %RepoDirectory%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

xcopy /H /E /I "%RepoDirectory%\.git" "%RepoDirectory%\..\.git"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

del /S /Q /F %RepoDirectory%\*
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

xcopy /H /E /I %RepoDirectory%\..\.git %RepoDirectory%\.git
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

RD /S /Q "%RepoDirectory%\..\.git"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

xcopy "%SourceDirectory%" "%RepoDirectory%" /K /D /H /Y /S
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

dotnet run --project "%VersionToolProject%" -- generate-public-commit-message "%CommitMessageFile%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

pushd "%RepoDirectory%"

git add -A
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

git commit -F "%CommitMessageFile%"
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

git push %RepoUrl%

for /f %%i in ('dotnet run --project "%VersionToolProject%" -- get-version') do set ProductVersion=%%i

git tag %ProductVersion%
if not !ERRORLEVEL! EQU 0 (
    exit /b !ERRORLEVEL!)

git push %RepoUrl% --tags

popd