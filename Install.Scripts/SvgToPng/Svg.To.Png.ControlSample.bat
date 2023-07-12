SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\..\Source
set SVG_DIR=%SOURCE_DIR%\Samples\ControlsSample\Resources\Svg\
set WIDTH=24

call Svg.To.Png.bat "%SVG_DIR%" "arrow-left" %WIDTH%
call Svg.To.Png.bat "%SVG_DIR%" "arrow-right" %WIDTH%
call Svg.To.Png.bat "%SVG_DIR%" "arrow-rotate-right" %WIDTH%
call Svg.To.Png.bat "%SVG_DIR%" "caret-right" %WIDTH%
call Svg.To.Png.bat "%SVG_DIR%" "minus" %WIDTH%
call Svg.To.Png.bat "%SVG_DIR%" "plus" %WIDTH%
call Svg.To.Png.bat "%SVG_DIR%" "search" %WIDTH%
call Svg.To.Png.bat "%SVG_DIR%" "xmark" %WIDTH%

