# 0.9.400 (2023 September ?)

- Added AuiManager, AuiToolbar, AuiNotebook controls.
- Added AuiManager sample.
- Added PropertyGrid control and sample.

# 0.9.300 (2023 August 9)

- Added LinkLabel, SplitterPanel, LayoutPanel controls.
- Added FontDialog.
- Improved TextBox. Added many new properties and methods. Added simple RichEdit functionality.
- Added time editor to DateTimePicker. Added PopupKind, different MinDate and MaxDate properties.
- General UI work speedup. Speedup in control creation, in events processing, in drawing (no more rect, 
point, size, thickness conversions), in NativeApi.Generator (less full recompilation of PAL dll is needed). 
Also removed different static dictionaries used to convert native controls to UI controls. 
- Improved Border control. Added BorderWidth and BorderColor properties. Now it is possible to specify 
whether to draw individual border side (BorderWidth is Thickness). Currently values are limited to 0 and 1.
- Fixed ListView, TabControl, CheckListBox, Button, NumericUpDown bugs.
- Improved ListView speed with large number of items.
- Improved Toolbars.
- Added Image GrayScale methods.
- Improved Samples.
- Control.Font now works.
- Added TreeView.HasBorder, ToolbarItem.DisabledImages, Control.GetDPI(), Toolbar.GetDefaultImageSize, 
Control.SetBounds(), Control.Width and Height, Font.AsBold, Font.AsUnderlined, NumericUpDown.Increment, 
WebBrowser.HasBorder, Control.GetVisibleChildren, Control.HasChildren, Control.GetVisibleChild, Application.Idle,
Color.GetKnownColors, Button.HasBorder, Toolbar.IsRight, Toolbar.IsVertical, Button.TextVisible, Button.TextAlign,
Button.SetImagePosition, Button.SetImageMargins, TreeView.VariableRowHeight, TreeView.TwistButtons, 
TreeView.StateImageSpacing, TreeView.Indentation, TreeView.RowLines, TreeView.HideRoot, Control.BeginIgnoreRecreate, 
Control.EndIgnoreRecreate
and other properties and methods.
- Improved work on Linux and MacOs.


# 0.9.201 (2023 July 9)

- Fixed painting problems on window resize and move.
- Fixed not working scrollbars in ListView, TreeView, ListBox, CheckedListBox.
- Improved look of the Sample projects on Linux and macOS.
- Improved behavior and fixed bugs in the Sample projects.
- Fixed some of the UIXml previewer problems in Sample projects.
- Added WebBrowser.Url property.
- Fixed Border control painting on Linux.
- Added UserPaintControl control.
- Improvements in the installation scripts.
- New static properties in Border control: DefaultBorderPen, DefaultBorderWidth, DefaultBorderColor.
- New methods Grid.SetRowColumn, Image.FromUrl, ImageSet.FromUrl.
- New properties ListBox.SelectedIndicesDescending, CheckListBox.CheckedIndicesDescending.
- New methods ListBox.RemoveSelectedItems, ListBox.RemoveItems, CheckListBox.RemoveCheckedItems, 
	ListBox.IsValidIndex, ListBox.GetValidIndexes.
- CheckListBox now fires SelectionChanged event and keeps corect SelectedIndices.
- Added HasBorder property to ListBox, CheckListBox.
- Removed TextBox.EditControlOnly property. Added TextBox.HasBorder property. 
- Fixed bug in toolbar and statusbar when additional strange items appeared if items were added from code.

# 0.9.200 (2023 Jun 29)

- Added WebBrowser control.
- Added WebBrowser control demo page in the ControlsSample project.
- Added ControlsTest project with WebBrowser.
- Added Install.bat, Install.sh, Install.ps1 scripts. This greatly simplified the installation process.
- Added complete Net 6, Net 7, and Net 4.81 support.
- Removed Alternet.DateTime. Now you can use System.DateTime in all places. 
- Added binary wxWidgets download instead of compilation during the installation. This speeds up the process of installation.
- Removed VS2019 support.
- Removed GTK debug messages on Linux.
- Added Application properties: DisplayName, AppClassName, VendorName, VendorDisplayName.
 
