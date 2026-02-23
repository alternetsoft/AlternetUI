## What's New in 0.9.803-1.0.1

Main Improvements:

- Support multiple columns in StdTreeView and VirtualListBox.
- Add GenericBorder, HiddenGenericBorder controls. They are similar to Border and HiddenBorder but are inherited from GenericControl.
- The generator now embeds the UI XML content directly into generated C# by emitting a private static readonly string. This speeds up form loading.
- Derive PictureBox from HiddenGenericBorder. This allows PictureBox to paint optional border around the image if needed.
- Fix print dialog was not shown on Windows 11.
- Fix exeption in PrintPreview dialog.
- Maui: improve checked menu items support.
- Fix measure text issue in Graphics.
- Fixed OpenGL issue on macOs 26.
- ListControlItem: Introduced ToolTip and IsToolTipVisible properties, allowing items to display tooltips and track their visibility state.
- Maui: Improve context menu handling.
- Maui: Add cross-platform keyboard visibility service and platform-specific implementations.
- VirtualListBox: Allow users to scroll the items vertically by dragging the interior. This feature is particularly useful on mobile devices.

