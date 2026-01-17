pushd Install.Scripts
pushd BuildAndRunSamples

start "" cmd /c "MSW.BuildAndRun.ControlsSample.Release.bat"

popd
popd
exit /b

