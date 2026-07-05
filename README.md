# AlterNET UI

AlterNET UI is a cross-platform .NET UI framework that allows the development of light-footprint .NET desktop 
applications that runs on Windows, macOS, and Linux with Microsoft Visual Studio or Visual Studio Code.

AlterNET UI is also compatible with .NET MAUI 10.0 platform. It provides a ControlView and its various descendants, 
which allow you to use AlterNET UI controls inside MAUI applications.

It is built on top of the .NET Framework and uses a XAML-like approach to define user interface and layout. 
It provides a set of standard controls that looks native on the target OS, such as TextBox, Label, CheckBox, Button, 
Image, TreeView, ListView, WebBrowser and much more.

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
- On all machines installations of .NET SDK [8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), 
[9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) and [10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
are required.
- If you want to use UI on single platform, skip installation steps for other platforms.

### <b>STEP 1.</b> Windows Machine Setup:

- Windows 11 is required.
- Install Visual Studio 2026 Version 18.7.3 or later, with C# Desktop development, C++ Desktop development workloads, 
VS Extenstion Development installed. Net 10.0, 9.0, 8.0 targeting packs are required.
- For development with Alternet.UI for Maui, install these dotnet workloads: android, ios, maccatalyst, macos, maui-windows, mono-android.
- Update workloads to the latest version: ```dotnet workload update```.
- Clone the AlterNET UI repo.
- Use the "C:\Alternet.UI" folder or any other root folder. The folder name should not contain spaces or non-English characters, 
and it should be as short as possible.

### <b>STEP 2.</b> macOS Machine Setup:

- macOs 26.2 (Tahoe) or newer is required.
- Install appropriate XCode version. See this [page](https://developer.apple.com/support/xcode/). XCode 26.2 or newer is required.
- Install [CMake](https://cmake.org/download/). Copy CMake.app into /Applications (or another location), double click to run it. 
Follow the "How to Install For Command Line Use" menu item.
- For development with Alternet.UI for Maui, install these dotnet workloads: android, ios, maccatalyst, macos, mono-android.
- Update workloads to the latest version: ```dotnet workload update```.

### <b>STEP 3.</b> Linux Machine Setup:

- Install Ubuntu 26.04 or newer.
- Install CMake and C compilers.
- Install required packages, run ```Install.Scripts/Ubuntu.Install.Packages.sh```.

### <b>STEP 4.</b> (Optional) Specify target frameworks for samples:
You can modify list of target frameworks in file
"Source\Samples\CommonData\TargetFrameworks.props".

### <b>STEP 5.</b> Build:

If you want to use UI on single platform, run install script only there.

- Exit Visual Studio before running Install script.
- By default installation script doesn't download and compile used WxWidgets library if it was done previously. For overriding this 
behavior, you can delete ```WxWidgets``` sub-folder in the ```External``` folder of the Alternet.UI installation. 
In this case full installation will be performed. 
This could be useful when, for example, we switch to the new WxWidgets builds.
- Run ```Install.sh``` on macOs or Linux Machine.
- Run ```Install.bat``` on Windows Machine.
- Now you can open solution "\Source\Alternet.UI.slnx".

### <b>STEP 6.</b> (Optional) Build NuGet packages on Windows Machine:

- In folder ``` Publish\Packages\ ``` you need to have the following files:
                                                                         
	- Alternet.UI.Pal-1.1.0.0-linux-arm64-debug.zip
	- Alternet.UI.Pal-1.1.0.0-linux-arm64-release.zip
	- Alternet.UI.Pal-1.1.0.0-linux-x64-debug.zip
	- Alternet.UI.Pal-1.1.0.0-linux-x64-release.zip
	- Alternet.UI.Pal-1.1.0.0-maccatalyst-arm64-debug.zip
	- Alternet.UI.Pal-1.1.0.0-maccatalyst-arm64-release.zip
	- Alternet.UI.Pal-1.1.0.0-win-arm64-debug.zip
	- Alternet.UI.Pal-1.1.0.0-win-arm64-release.zip
	- Alternet.UI.Pal-1.1.0.0-win-x64-debug.zip
	- Alternet.UI.Pal-1.1.0.0-win-x64-release.zip
	- Alternet.UI.Pal-1.1.0.0-win-x86-debug.zip
	- Alternet.UI.Pal-1.1.0.0-win-x86-release.zip

If some of the files are missing they will not be included in the result Pal nuget.

- Run Install2.Build.Nugets.bat.
- The result nugets will be in Publish/Packages.

## Template for use with GitHub source instead of nuget:

If you want to develop with latest AlterNET.UI source from GitHub instead of using it from 
[NuGet](https://www.nuget.org/packages/Alternet.UI), you can use 
[MinMasterTemplate](https://github.com/alternetsoft/AlternetUI/tree/master/Install.Scripts/MinMasterTemplate).
