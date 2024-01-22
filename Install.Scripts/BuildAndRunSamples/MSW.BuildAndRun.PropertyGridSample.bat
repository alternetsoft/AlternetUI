SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.

"%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" "%SCRIPT_HOME%\..\..\Source\Samples\" PropertyGridSample %1 %2
