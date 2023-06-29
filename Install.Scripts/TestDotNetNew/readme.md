
# Installing Alternet.UI custom beta nuget and vsix from files

**STEP 1**. Exit Visual Studio.

**STEP 2**. Copy nuget and vsix beta files into AlternetUI\Publish\Packages\ folder.
These files must be there:

* Alternet.UI.\*.nupkg
* Alternet.UI.\*.snupkg
* Alternet.UI.Integration.VisualStudio.VS2022-\*.vsix
* Alternet.UI.Pal.\*.nupkg
* Alternet.UI.Templates.\*.nupkg
* PublicExamples-\*.zip


\* in file names is version number (for example 0.9.200-beta)

**STEP 3.** Run script 00-Run.Steps.01-13.bat. This script changes contents 
of the "C:\AlternetTest\" folder by deletting all the contents and 
installing beta nugets there.
