- Alternet.UI is now fully crossplatform. It was separated from WxWidgets. 
We also started to port Alternet.UI to MAUI and Avalonia.UI. 
It will be possible to use any Alternet.UI.Control descendant with these libraries.
We plan to support at least three platforms: WxWidgets (Window, Linux, MacOs), MAUI (Windows, Android, iOS), Avalonia.UI.
- SkiaContainer control allows to use any Alternet.UI.Control inside Maui.
- Integrated SkiaSharp into the library. Added SkiaSharp mega demo in ControlsSample.
- Updated to use WxWidgets 3.2.5. 
- Added possibility to specify the layout method for any Control descendant using Layout property. Implemented layout methods: None, Dock, Basic, Vertical, Horizontal.
- Control.Dock property is used in all layouts. For example, if some child of the StackPanel has Dock=DockStyle.Right, it will be aligned right. All other align rules are applied after docking child controls.
- Improved StackPanel alignment: Horizontally aligned StackPanel can have centered children, 
- Improved StackPanel alignment: VerticalAlignment property of the child controls is used to specify vertical alignment inside the container. 
- Element 'Fill' was added to VerticalAlignment and HorizontalAlignment enums. It allows to layout the child control so it will occupy the remaining empty space of the container.
- VListBox control. This is ListBox descendant which can have a huge number of items, has owner draw feature for the items, 
uses properties of the ListControlItem to customize the item's painting.
- TabControl, Toolbar are implemented inside the library.
- New properties, methods and events in the existing controls and classes.
- New controls: ColorComboBox, ColorListBox, VCheckListBox, FileListBox, PreviewFile, PreviewFileSplitted, PreviewInBrowser, PreviewTextFile, PreviewUixml, PreviewUixmlSplitted, FontListBox.
- TextureBrush.
- Fixed known Linux related problems.
- Used netstandard2.0 in the library.
- Bug fixes and optimizations.
