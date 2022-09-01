pushd ..
FOR /d /r . %%d IN ("bin") DO @IF EXIST "%%d" rd /s /q "%%d"
FOR /d /r . %%d IN ("obj") DO @IF EXIST "%%d" rd /s /q "%%d"
popd