# AlternetUI

AlterNET UI is a cross-platform library for developing light-footprint .NET desktop applications that run on Windows, macOS, and Linux.

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
