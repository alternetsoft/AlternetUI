## How to create new control:

- In the NativeApi project, add a new control class using some existing control as an example.
- Run NativeApi.Generator to generate to interop code.
- Add new managed classes and their respective control handlers in the AlterNET.UI project, using 
some existing controls as an example.
- In the C++ project (Alternet.UI.Pal), include the generated source files (MyNewControl.cpp and MyNewControl.h) to the project.
Also in "Api" sub-folder include the generated source files (MyNewControl.Api.h and MyNewControl.inc) to the project.
- Make sure the C++ class is inherited from the Control class.
- The generated public methods definitions are in Api/MyNewControl.inc.
- You will need to create the corresponding implementations of this methods in the MyNewControl.cpp.
- Each of the controls is based on the corresponding wxWidgets control, see the existing controls for how they are implemented
- To check what functionality is supported by wxWidgets, inspect the documentation at https://docs.wxwidgets.org/3.2/ .
- After you created new UI control or added new event, call Install.Scripts\UpdateWellKnownApiInfo.bat.
This script allows to use new control in uixml. If you create controls in cs, you don't need to run it.
