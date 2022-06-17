# AlterNET UI Beta 3

AlterNET UI Beta 3 adds important features like menus, keyboard shortcuts, commands, modal windows, advanced window
management, and more.

## Menus

`MainMenu`, `Menu`, and `MenuItem` classes allow to build complex menu hierarchies, either from UIXML or from code.
macOS gets support of its system UI guidelines, including automatically moving items like **"About"** or **"Quit"** to
the macOS application menu. Users can override this behavior by setting `MenuItem.Role` property.

<Menu Screenshots/videos here>

The menu capabilities are demonstrated in the new **MenuSample** demo project.

## Keyboard Shortcuts

Related to menus is the keyboard shortcuts support. The programmer can easily specify the shortcuts like **"Ctrl+O"** or
**"Shift+F1"** for menu items directly from UIXML. macOS also gets special support here, where shortcut modifier keys
get automatically translated to its proper conterparts, so the most of the shortcuts can be specified once for all the
platforms. Specifying the modifier keys specifically for macOS is also possible.

## Commands

Commands allow to separate the semantics and the object that invokes a command from the logic that executes the command.
Another purpose of commands is to indicate whether an action is available. The programmer can set `MenuItem.Command`
property to link an `ICommand` with a `MenuItem`. As the `Command` can be data-bound from UIXML, this allows for MVVM
approach to implementing user interfaces.

## Modal Windows and Window Management

The programmer now can show a window modally by calling the `Window.ShowModal` method. A `ModalResult` is then returned to
indicate the result with which a modal dialog window is closed.

Also in this release we added a lot of means to control your windows, most of them as members of the `Window` class. These
include control of the window ownership, state, activation, title bar and border features, icons, window bounds, and more.

These new window management capabilities are shown in the newly added **WindowPropertiesSample** demo project.