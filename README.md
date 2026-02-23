# AlterNET UI

AlterNET UI is a cross-platform .NET UI framework that allows the development of light-footprint .NET desktop 
applications that runs on Windows, macOS, and Linux with Microsoft Visual Studio or Visual Studio Code.

It is built on top of the .NET Framework and uses a XAML-like approach to define user interface and layout. 
It provides a set of standard controls that looks native on the target OS, such as Text Box, Label, CheckBox, Button, 
Image, TreeView, ListView, WebBrowser and more.

The framework includes a platform-independent graphic device interface for rendering graphical objects, such as fonts, 
brushes, images, and a layout engine.

For increased developer productivity, 
AlterNET UI [extension](https://marketplace.visualstudio.com/items?itemName=AlternetSoftwarePTYLTD.AlternetUIForVS2022) 
for Visual Studio is available.

## Useful links:

- [Home Page](https://www.alternet-ui.com/)
- [Documentation](https://docs.alternet-ui.com/)
- [AlterNET UI NuGet](https://www.nuget.org/packages/Alternet.UI)
- [Visual Studio Extension](https://marketplace.visualstudio.com/items?itemName=AlternetSoftwarePTYLTD.AlternetUIForVS2022)
- [Blog](https://www.alternet-ui.com/blog)
- [Forum](https://forum.alternet-ui.com/)

## How to use:

- [Visual Studio](https://docs.alternet-ui.com/tutorials/hello-world/visual-studio/hello-world-visual-studio.html)
- [Command-Line and Visual Studio Code](https://docs.alternet-ui.com/tutorials/hello-world/command-line/hello-world-command-line.html)

------------

## How to build:

It is better to use AlterNET UI from [NuGet](https://www.nuget.org/packages/Alternet.UI). If you need a custom build, 
here is step by step instruction:

- 3 build machines are needed: Windows, macOS, Linux.
- On all machines installations of .NET SDK [8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), [9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) 
are required.
- On Windows installation of .NET SDK [10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) is required.
- If you want to use UI on single platform, skip installation steps for other platforms.

<b>STEP 1.</b> Windows Machine Setup:

- Windows 11 is preferred.
- Install Visual Studio 2026 Version 18.0.0 or later, with C# Desktop development, C++ Desktop development workloads, 
VS Extenstion Development installed. Net 10.0, 9.0, 8.0 targeting packs are required.
- For development with Alternet.UI for Maui, install these dotnet workloads: android, ios, maccatalyst, macos, maui-windows, mono-android.
- Clone the AlterNET UI repo.
- Use the "C:\Alternet.UI" folder or any other root folder. The folder name should not contain any spaces or non-English letters.

<b>STEP 2.</b> macOS Machine Setup:

- macOS 10.15 (Catalina) or newer is required. For development on MAUI platform, macOs 26.2 (Tahoe) or newer is required.
We use for developmnent macOs 26.2.
- Install appropriate XCode version (for example Xcode 12.4 for macOS 10.15). See 
this [page](https://developer.apple.com/support/xcode/). For development on MAUI platform, XCode 26.2 or newer is required.
- Install [CMake](https://cmake.org/download/). Copy CMake.app into /Applications (or another location), double click to run it.
 Follow the "How to Install For Command Line Use" menu item.
- Make folder with Windows Alternet.UI installation accessible for macOS Machine.
- For development with Alternet.UI for Maui, install these dotnet workloads: android, ios, maccatalyst, macos, mono-android.

<b>STEP 3.</b> Linux Machine Setup:

- Install Ubuntu 25.10 or newer.
- Install CMake and C compilers.
- Install required packages, run Install.Scripts/Ubuntu.Install.Packages.sh.
- Make folder with Windows Alternet.UI installation accessible for Linux Machine.

<b>STEP 4.</b> (Optional) Specify target frameworks for samples:
You can modify list of target frameworks in file
"Source\Samples\CommonData\TargetFrameworks.props".

<b>STEP 5.</b> Build:

If you want to use UI on single platform, run install script only there.

- Exit Visual Studio before running Install script.
- By default installation script doesn't download and compile used WxWidgets library if it was done previously. For overriding this 
behavior, you can delete "WxWidgets" sub-folder in the "External" folder of the Alternet.UI installation. In this case full installation will be performed. 
This could be useful when, for example, we switch to the new WxWidgets builds.
- Run Install.sh (or Install.ps1) on macOs Machine.
- Run Install.sh (or Install.ps1) on Linux Machine.
- Run Install.bat (or Install.ps1) on Windows Machine.
- Now you can open solution "\Source\Alternet.UI.sln".

<b>STEP 6.</b> (Optional) Build NuGet packages on Windows Machine:

- Run Install2.Build.Nugets.bat.
- The results will be in Publish/Packages.

## Important:

Please run the Install script each time you switch development platform 
(for example, when you switch from Linux to MSW, run Install.bat on MSW).

## Sample template for use with latest source from GitHub instead of nuget:

If you want to develop with latest AlterNET.UI source from GitHub instead of using it from 
[NuGet](https://www.nuget.org/packages/Alternet.UI), you can use 
[MinMasterTemplate](https://github.com/alternetsoft/AlternetUI/tree/master/Install.Scripts/MinMasterTemplate).
