SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source

pushd "%SOURCE_DIR%\Version\"
# del FrameworksOverride.props
# copy SampleFrameworksOverride.props FrameworksOverride.props
popd

pushd "%SOURCE_DIR%\Samples\CommonData\"
del FrameworksOverride.props
copy SampleFrameworksOverride.props FrameworksOverride.props
popd
