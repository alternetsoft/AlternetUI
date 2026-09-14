@echo off
REM === Configure FontForge path ===
set FONTFORGE="G:\PortableApps\PortableApps\FontForgePortable\App\FontForge\fontforge.bat"

REM === Check arguments ===
if "%~1"=="" (
    echo Usage: %~nx0 inputfont.woff2
    exit /b 1
)

REM === Run FontForge conversion ===
%FONTFORGE% -lang=ff -c "Open($1); Generate($1:r + '.ttf')" "%~1"

echo Done. Converted %~1 to TTF.



