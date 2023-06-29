# AlternetUI

AlterNET UI is a cross-platform .NET UI framework that allows the development of light-footprint .NET desktop applications that runs on Windows, macOS, and Linux with Microsoft Visual Studio or Visual Studio Code.

It is built on top of the .NET Framework and uses a XAML-like approach to define user interface and layout. It provides a set of standard controls that looks native on the target OS, such as Text Box, Label, CheckBox, Button, Image, TreeView, ListView, WebBrowser and more.

The framework includes a platform-independent graphic device interface for rendering graphical objects, such as fonts, brushes, images, and a layout engine.

For increased developer productivity, AlterNET UI extension for Visual Studio is available.

------------

Steps needed to build AlterNET UI

- 3 build machines are needed: Windows, macOS, Linux.
- First, AlterNET UI need to be built on all 3 platforms.
- Then, to build the final packages, the artifacts (build results) from the 3 platforms need to be combined together in the last, final step.

Step 1. Windows Machine Setup:
- Install Visual Studio 2022, with C# Desktop development, C++ Desktop development workloads, VS Extenstion Development installed. Net 6.0, 7.0, 4.62, 4.81 targeting packs are required.
- Clone the AlterNET UI repo.

Step 2. macOS Machine Setup:
- macOS 10.15 or newer is required.
- Install XCode and CMake.
- Make folder with Windows Alternet.UI installation accessible for macOS Machine.

Step 3. Linux Machine Setup:
- Install Ubuntu 20.04 or newer.
- Install CMake and C compilers.
- Install required packages, run Install.Scripts/Ubuntu.Install.Packages.sh.
- Make folder with Windows Alternet.UI installation accessible for Linux Machine.

Step 4. Build:
- Run Install.sh (or Install.ps1) on macOs Machine. It collects artifacts in Publish/Artifacts/Build/macOS.
- Run Install.sh (or Install.ps1) on Linux Machine. It collects artifacts in Publish/Artifacts/Build/Linux.
- Run Install.bat (or Install.ps1) on Windows Machine. It collects artifacts in Publish/Artifacts/Build/Windows.

Step 5. Build NuGet packages on Windows Machine (optional):
- Collect all the artifacts from all the 3 previous builds in the locations as they were on the source machines.
- Run Install.Scripts/MSW.Publish.1.Build.Nuget.Pal.bat.
- Run Install.Scripts/MSW.Publish.2.Build.NuGet.Managed.bat.
- The results will be in Publish/Packages.

Please run the Install script each time you switch development platform 
(for example, when you switch from Linux to MSW, run Install.bat on MSW).

------------

Steps needed to create new wxWidgets control
- In the NativeApi project, add a new control class using some existing control as an example.
- Run NativeApi.Generator to generate to interop code.
- Add new managed classes and their respective control handlers in the Alternet.UI project, using some existing controls as an example.
- In the C++ project (Alternet.UI.Pal), include the generated source files (MyNewControl.cpp and MyNewControl.h) to the project.
- Make sure the C++ class is inherited from the Control class.
- The generated public methods definitions are in Api/MyNewControl.inc.
- You will need to create the corresponding implementations of this methods in the MyNewControl.cpp.
- Each of the controls is based on the corresponding wxWidgets control, see the existing controls for how they are implemented
- To check what functionality is supported by wxWidgets, inspect the documentation at https://docs.wxwidgets.org/3.0/ .
