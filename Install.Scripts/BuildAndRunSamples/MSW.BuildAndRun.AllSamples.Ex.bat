SETLOCAL EnableDelayedExpansion
set SCRIPT_HOME=%~dp0.

set SAMPLE_FOLDER=%1

call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% ControlsSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% ExplorerUISample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% DrawingSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% CustomControlsSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% DataBindingSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% LayoutSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% InputSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% PaintSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% EmployeeFormSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% WindowPropertiesSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% MenuSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% CommonDialogsSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% DragAndDropSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% ThreadingSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% PrintingSample %2 %3
call "%SCRIPT_HOME%\MSW.BuildAndRunSample.bat" %SAMPLE_FOLDER% HelloWorldSample %2 %3

