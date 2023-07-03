SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.

"%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" "%SCRIPT_HOME%\..\..\Source\Samples\" WindowPropertiesSample %1 %2
