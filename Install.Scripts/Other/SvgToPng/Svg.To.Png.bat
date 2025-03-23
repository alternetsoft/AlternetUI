SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0
set SOURCE_DIR=%SCRIPT_HOME%\..\..\Source
set INKSCAPE=E:\INSTALL\papps\PortableApps\InkscapePortable\App\Inkscape\bin\inkscape.exe

set PATHONLY=%~1
set FILENAMEONLY=%~2
set WIDTH=%~3

pushd "%PATHONLY%"
"%INKSCAPE%" --export-background-opacity=0 --export-width=%WIDTH% --export-height=%WIDTH% --export-type=png --export-filename="%FILENAMEONLY%-%WIDTH%.png" "%FILENAMEONLY%.svg"
popd