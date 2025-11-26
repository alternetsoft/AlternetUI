cls
pushd Install.Scripts
call MSW.Publish.1.Build.Nuget.Pal.bat 
call MSW.Publish.2.Build.NuGet.Managed.bat
popd