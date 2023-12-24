# MinMaster project template

This is minimal project template to use with AlterNET.UI latest master version from github.
If you are using Alternet.UI with Nuget, this sample is not for you.

## How to use:

As stated in AlterNET.UI README.md, you need to run Install.bat (or Install.sh) one time,
after you switched development platform OS or fetched new AlterNET.UI version from GitHub.

- STEP 1. Edit MinMaster.csproj file in the Notepad and change "AlternetUIPath" property
to the path where Alternet.UI folder is located.
- STEP 2. Run build.bat (or build.sh).
- STEP 4. Open MinMaster.csproj is Visual Studio and enjoy.

## Current limitations:

- Uixml forms are not supported by this template. You need to create forms and controls
from code. See MainWindow.cs for an example.

## Issue url

- You can comment or add bug report for MinMaster template here:
https://github.com/alternetsoft/AlternetUI/issues/68
- There is an improved template project created by @neoxeo.
It has script file that asks for project name, path. etc.
https://github.com/alternetsoft/AlternetUI/issues/68#issuecomment-1864016068
