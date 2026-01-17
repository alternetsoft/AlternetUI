SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.

"%SCRIPT_HOME%\MSW.BuildAndRunSample.net10.bat" "%SCRIPT_HOME%\..\..\Source\Samples\" ControlsSample %1 %2
