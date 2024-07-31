SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.

set SAMPLE_FOLDER=%SCRIPT_HOME%\..\..\Source\Samples

call MSW.BuildAndRun.AllSamples.Ex.bat "%SAMPLE_FOLDER%" %1 %2

