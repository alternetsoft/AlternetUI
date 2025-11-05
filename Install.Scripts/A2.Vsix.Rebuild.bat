pushd
call "A2.Vsix.Uninstall.bat"
popd
pushd
call "MSW.Publish.7.Build.Vsix.bat"
popd
pushd
call "A2.Vsix.Install.bat"
popd
