## What's New in 0.9.736-0.9.759 

Major improvements:

- Completely reimplement MainMenu, MenuItem, and ContextMenu. They are now derived from FrameworkElement.
- Refactor DPI-change handling in controls and Window.
- Add an IsTransparent property to Label and PictureBox controls.
- Handle system color changes correctly.
- Add SVG image support to SpeedButton, PictureBox, and other controls.
- Change coordinates and bounds types to float.
- MAUI: add support for context menus and implement mouse capture.
- SpeedButton: add optional image, text, or shortcut painting on the right.
- Add OpenGL rendering support for UserControl.
- Add dark mode support for Windows.
- UserControl: add overlay support and several ShowOverlayToolTip overloads for flexible tooltip display and management.
- FileListBox: add an optional column header with column-sorting support.
- Merge GenericLabel and Label into a single control implemented inside the library.
- Refactor LinkLabel to be implemented inside the library.
- Linux: show popup windows in the correct position, disable scrollbar overlays, and implement GTK CSS styling.
- WebBrowser: fix background painting for the Edge backend.
- Implement picker controls for selecting color, date, time, enum, list, font name, and font size.
- Restore native ListBox and CheckedListBox implementations.
- Add StdSlider — slider control fully implemented in C# inside the library.
- Add automatic painting of child generic controls.

