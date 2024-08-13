SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set PUBLISH_HOME=%SCRIPT_HOME%\..\..\Publish\Packages

set PDF_MANUAL=%SCRIPT_HOME%\site\pdf\toc.pdf

del "%PUBLISH_HOME%\alternet-ui-*.pdf"

copy "%PDF_MANUAL%" "%PUBLISH_HOME%\alternet-ui-manual.pdf"



