SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set PUBLIC_EXAMPLES_DIR=%SCRIPT_HOME%\..\Publish\Packages
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set VersionToolProject=%SOURCE_DIR%\Tools\Versioning\Alternet.UI.VersionTool.Cli\Alternet.UI.VersionTool.Cli.csproj

call Tools\CleanProject.bat "%PUBLIC_EXAMPLES_DIR%\PublicExamples\CommonData"
call Tools\CleanProject.bat "%PUBLIC_EXAMPLES_DIR%\PublicExamples\ControlsSample"
call Tools\CleanProject.bat "%PUBLIC_EXAMPLES_DIR%\PublicExamples\ControlsSampleDll"
call Tools\CleanProject.bat "%PUBLIC_EXAMPLES_DIR%\PublicExamples\HelloWorldSample"
call Tools\CleanProject.bat "%PUBLIC_EXAMPLES_DIR%\PublicExamples\SkiaSharpSampleDll"

pushd %PUBLIC_EXAMPLES_DIR%

del /q PublicExamples*.zip
"C:\Program Files\WinRar\WinRAR.exe" a -r -x@"%SCRIPT_HOME%\rarexclude.txt" PublicExamples.zip "PublicExamples\*.*"

dotnet run --project "%VersionToolProject%" --property WarningLevel=0 -- append-version-suffix "%PUBLIC_EXAMPLES_DIR%\PublicExamples.zip"

popd
