"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version [17.0,18.0) -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > tmpFile 
set /p FOUND_MSBUILD_PATH_VS_2022= < tmpFile 
del tmpFile 

echo ==================================== BI3
echo  Integration\VisualStudio\Alternet.UI.Integration.VisualStudio\Alternet.UI.Integration.VisualStudio
"%FOUND_MSBUILD_PATH_VS_2022%" /restore /t:Clean,Build /property:Configuration=Release /p:WarningLevel=0 "Alternet.UI.Integration.VisualStudio.csproj"

