set projectPath=%1
pushd %projectPath%
if exist "bin" (rmdir /s /q "bin")
popd

