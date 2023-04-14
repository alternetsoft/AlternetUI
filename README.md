# AlternetUI

AlterNET UI is a cross-platform .NET UI framework that allows the development of light-footprint .NET desktop applications that runs on Windows, macOS, and Linux with Microsoft Visual Studio or Visual Studio Code.

It is built on top of the .NET Framework and uses a XAML-like approach to define user interface and layout. It provides a set of standard controls that looks native on the target OS, such as Text Box, Label, CheckBox, Button, Image, TreeViews, ListView, and more.

The framework includes a platform-independent graphic device interface for rendering graphical objects, such as fonts, brushes, images, and a layout engine.

For increased developer productivity, AlterNET UI extension for Visual Studio is available.

------------
Steps needed to build AlterNET UI
- 3 build machines are needed: Windows, macOS, Linux
- first, the PAL native components need to be built on all 3 platforms
- then, to build the final packages, the artifacts (build results) from the 3 platforms need to be combined together in the last, final step

Windows Machine Setup
- Install Visual Studio 2022, with C# Desktop development, C++ Desktop development workloads, VS Extenstion Development installed
- Clone the AlterNET UI repo
- Build wxWidgets by running Source\Build\Scripts\Build WxWidgets.bat

macOS Machine Setup
- macOS 10.13.6 High Sierra or newer is required
- install XCode and CMake
- Build wxWidgets by running Source\Build\Scripts\Build WxWidgets.sh

Linux Machine Setup
- Install Ubuntu 20.04
- install CMake and C compilers
- Build wxWidgets by running Source\Build\Scripts\Build WxWidgets.sh


1. Windows Build
- run Source/Build/Scripts/Build PAL for Windows.bat
- Collect artifacts in Publish\Artifacts\Build\Windows\UI\PAL

2. macOS Build
- run Source/Build/Scripts/Build PAL for macOS.sh
- Collect artifacts in Publish\Artifacts\Build\macOS\UI\PAL

3. Linux Build
- run Source/Build/Scripts/Build PAL for Linux.sh
- Collect artifacts in Publish\Artifacts\Build\Linux\UI\PAL

4. Final Build on Windows Machine
- collect all the artifacts in from all the 3 previous builds in the locations as they were on the source machines
- run Source/Build/Scripts/Build PAL NuGet.bat
- run Source/Build/Scripts/Publish.bat
- the results will be in Publish\Packages
- 
------------
Steps needed to create new wxWidgets control
- In the NativeApi project, add a new control class using some existing control as an example
- Run NativeApi.Generator to generate to interop code
- Add new managed classes and their respective control handlers in the Alternet.UI project, using some existing controls as an example
- In the C++ project (Alternet.UI.Pal), include the generated source files (MyNewControl.cpp and MyNewControl.h) to the project
- Make sure the C++ class is inherited from the Control class
- The generated public methods definitions are in Api/MyNewControl.inc
- You will need to create the corresponding implementations of this methods in the MyNewControl.cpp
- Each of the controls is based on the corresponding wxWidgets control, see the existing controls for how they are implemented
- To check what functionality is supported by wxWidgets, inspect the documentation at https://docs.wxwidgets.org/3.0/ 
