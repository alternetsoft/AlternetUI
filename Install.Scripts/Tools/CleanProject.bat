set projectPath=%1
pushd %projectPath%
if exist "bin" (rmdir /s /q "bin")
if exist "obj" (rmdir /s /q "obj")
popd

