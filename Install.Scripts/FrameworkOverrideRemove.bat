SETLOCAL EnableDelayedExpansion

call Install.Clean.All.Samples.bat

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source

pushd "%SOURCE_DIR%\Version\"
del FrameworksOverride.props
popd

pushd "%SOURCE_DIR%\Samples\CommonData\"
del FrameworksOverride.props
popd
