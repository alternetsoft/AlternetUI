SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set PUBLISH_HOME=%SCRIPT_HOME%\..\..\Publish\Packages

set PDF_MANUAL=%SCRIPT_HOME%\site\pdf\toc.pdf
set RESULT_MANUAL=%SCRIPT_HOME%\site\pdf\alternet-ui-manual.pdf

call Del.AllResults.bat

dotnet build /p:LatestDocFx=true Alternet.UI.Documentation.csproj

del "%RESULT_MANUAL%"
copy "%PDF_MANUAL%" "%RESULT_MANUAL%"

call Publish.Pdf.bat

call calljobdone.bat "UI Documentation was built ok"





