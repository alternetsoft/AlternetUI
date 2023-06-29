SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.

set SAMPLE_FOLDER=%SCRIPT_HOME%\..\..\Publish\Packages\PublicExamples

pushd "..\BuildAndRunSamples\"

call MSW.BuildAndRun.AllSamples.Ex.bat "%SAMPLE_FOLDER%" %1 %2

popd