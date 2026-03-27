SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set ROBOTS=%SCRIPT_HOME%\robots.txt
set ROBOTSRESULT=%SCRIPT_HOME%\site\robots.txt

del "%ROBOTSRESULT%"
copy "%ROBOTS%" "%ROBOTSRESULT%"
