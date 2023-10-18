# 0.9.405 (not released, work in progress)

- Implemented Sizer functionality. It allows to implement complex controls layouts.
- Add to TreeViewItem: TextColor, BackgroundColor, IsBold.
- TreeView speedup (removed handlesByItems usage).
- Updated webview to 2.1.0.2045.28.
- Updated wxWidgets to 3.2.3.
- Fixed exceptions in menu and toolbar.
- Fixed exception in DragAndDropSample.
- Moved PanelTreeAndCards control to the main library.
- Improved Xml documentation in cs files.
- Improved handling of Uixml errors (not finished).

---

# 0.9.404 (2023 October 10)

- Add CardsPanelHeader control.
- Improved Samples, fixed Linux and MacOs related problems.
- Add to Size: GetWidths, GetHeights, MaxWidthHeight, NaN, IsNan, AnyIsNan.
- Add ComboBox.UseChoiceControl.
- Fixed "Unrecognised keycode 393,394" under MacOs.
- Fixed DateTimePicker bugs on Linux.
- Add to LinkLabel: UseGenericVersion, UseShellExecute.

# 0.9.403 (2023 October 7)

- Added PanelWebBrowser, CardPanel controls.
- Added WebBrowserSearchWindow.
- Add to Control: MinimumSize, MaximumSize, MinHeight, MaxHeight, MinWidth, MaxWidth.
- Add ControlSet class to perform group operations on controls.
- Fixed exception in wxGetKeyStateGTK under Linux (Ubuntu 22 and higher).
- Fixed crashes in button demo under Linux.
- Fixed #11 and other bugs in PrintPreview.
- Add WebBrowser/PanelWebBrowser constructor with url param. Now is possible to specify default url which will be opened.
- Add to IAuiPaneInfo: GetBestSize, GetMinSize, GetMaxSize.
- Add to LinkLabel: HoverColor, NormalColor, VisitedColor, Visited.
- Add new Label, CheckBox, Button constructor with Text parameter.
- Added Application new props and methods.
- Fixed PropertyGrid color scheme bugs.
- Improved Sample projects.
- Add to PropertyGrid: different color props.
- Add ComboBox.BindSelectedItem.
- Add to Grid: AddAutoColumn, AddAutoRow.
- Add to NotifyIcon: IsOk, IsAvailable, IsIconInstalled.
- Removed many hints and warnings.

# 0.9.402 (2023 September 28)

- Border improvements: 
BorderWidth's can be any positive number (previously only 0 and 1).
Can set individual colors of the border sides.
Border painting is now compatible with Background brush and BackgroundColor.
- Added AuiManager.ArtProvider, AuiToolbar.ArtProvider. Now possible to specify colors and other settings
for Aui toolbars and panes.
- Fixed event handling in many cpp controls.
- Add to Control: SuggestedSize, SuggestedWidth, SuggestedHeight.
- Add to Button: ExactFit, StateImages.FocusedImage.
- Fixed flickering in Label, LinkLabel under Windows.
- Improved sample projects.
- Added Application.DoEvents.
- Added to Font: GetDefaultOrNew, custom Equals.
- Fixed KnownColorTable.cs not compiled under Linux.
- Improved TreeView.MakeAsListBox.
- Added Control.DoInsideLayout.
- Fixed ListViewItem cell index not assigned bug.
- Added IAuiNotebookPage and used in AuiNotebook.
- Fixed Button.cpp not updated images on recreate.
- Add to DrawingContext: DrawPoint, DrawDebugPoints.
- Add to Color: AsBrush, AsPen.
- Added Pen.AsBrush.
- NativeApiGenerator improved, do not gen try/catch on events.
- Fixed: Button images now work under Macos.
- AuiToolbar: Implemented methods to set min size of the controls.

# 0.9.401 (2023 September 22)

- Complete rewrite of the StatusBar control (many new features, fixed known bugs).
- Added PanelAuiManager control.
- Fixed ListView.Items were not added without created native control.
- Added to TreeView: FirstItem, LastItem, LastRootItem, Add, FocusAndSelectItem, Add(string).
- Added to Control: DoInsideUpdate, InUpdates, DragStart event.
- Added to Collection: SetCount.
- Added to (ListViewItrem, TreeViewItem): Assign, Clone.
- Added to ListControl: RemoveAll, ClearSelected, FindString.
- Added to Application: different loggind methods.
- Fixed DragDrop related issues.
- Fixed (ListBox, ComboBox) item change not applied to control.
- Add to Button: HoveredImage, PressedImage, DisabledImage.
- Fixed incorrect event handling in Control.cpp.
- Add static Button.ImagesEnabled (mostly for MacOs).
- TreeViewItem.IsSelected is now writable.
- Improved NativeApiGenerator: Now extra code is not generated for simple properties and methods (speed up overall lib performance), fixed bugs.
- Fixed Window exception in some cases

# 0.9.400 (2023 September 15)

- Added PropertyGrid, AuiManager, AuiToolbar, AuiNotebook, PanelOKCancelButtons controls.
- Added Svg images support.
- Added AuiManager, PropertyGrid samples.
- Added Validator class and interface (allows to apply constraints on input data in TextBox and other controls).
- Added to Control: CanAcceptFocus, IsFocusable, BackgroundColor, ForegroundColor, Disposed event.
- Added to TextBox: Validator, DefaultValidator.
- Added to TreeView: RemoveSelected, RemoveAll, RemoveItemAndSelectSibling.
- Added to TreeViewItem: NextOrPrevSibling.
- Added visual editor for list and collection properties.
- Improved Brush and Font handling in Control.
- Fixed bugs in ComboBox.
- Improved Font, Brush (and its descendants).
- Improved Native class generator (speedup in Native controls, bug fixes). 
- Speed up in Collection, TreeView, TreeViewItem, ListView, .
- Fixed (TreeView, ListBox) raised SelectionChanged twice.
- Fixed exceptions in DateTimePicker, StatusBar.
- Implemented localization related classes, methods and props (full localization is not completed yet).
- Improved ContextMenu.Show.
- Added to Window: HasSystemMenu.
- Implemented in Color: speedup, new methods and props, localization.
- Added Control.Parent set method.
- Speed up: MenuItem.Items are not created for items without childs.
- Added to ResourceLoader: Default, StreamFromUrl, etc.
- Fixed bug in application exit if no visible form.

---

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

---

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
 
