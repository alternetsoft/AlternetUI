
# 0.9.500 (2024 January 15)

Changed build number for the new beta release.

# 0.9.412 (2024 January 15)

## 2024 January 15

- Font.DefaultMono, Control.DefaultMonoFont.
- SplittedPanel: Create side panels virtual methods, All panels are Control descendants, DefaultPanelSize virtual property,
Splitters property.
- Improved PropertyGridSample and other demos.
- CardPanelHeader.SelectLastTab.
- Vertically aligned StackPanel stretches last child control if child's VerticalAlignment = Stretch and
 StackPanel.AllowStretch = true.
- Fixed splitter color in Dark/Light schemes.
- GenericToolBar.DeleteAl.
- PropertyGrid: CalcScrolledPositionI, CalcScrolledPositionD, GetHitTestColumn, GetHitTestProp, CalcUnscrolledPositionI, 
CalcUnscrolledPositionD.
- Added Reset menu item in PropertyGridSample.
- Activated, Deactivated events are now declared in the Control (previously in Window).
- Fixed single control layout in the Window.
- PropertyGrid improved Double property editor.
- Control: Change to SuggestedSize, SuggestedWidth, SuggestedHeight calls Layout.
- GenericLabel: Fixed painting when IsBold=true.
- GenericToolbar: Fixed IsBold.
- Control now invalidates when Enabled is changed. This fixed user controls drawing when Enabled is changed.
- GenericLabel: Repainted with correct color when Enabled is changed. Better Layout (SuggestedSize is used for layout).

## 2024 January 11

- Add FindReplaceControl, SpeedTextButton controls.
- Splitter.SizeDelta and resize speed improvement.
- Fixed MenuItem.cpp SetDisabledBitmap under Linux.
- GenericToolBar: DefaultSpeedBtnMargin, DefaultStickyBtnMargin, DefaultTextBtnMargin, AddStickyBtn, ToggleToolSticky.
- Add GenericToolBarSet control. Impleements multiple toolbars in one control.
- Fixed Color compare operator.
- CardPanelHeader improved. Used SpeedButton instead of Label. Hover border is shown when mouse is over. TabClick event
 is not fired if Tab is already selected.
- Fixed CardPanelHeader painting Linux/Macos.
- Splitter improved. Optionally draw background and foreground line in the center. New props: DefaultNormalColors, NormalColors.
 Better painting under Linux.

## 2024 January 9

- SpeedButton: Defaults, DisabledImageSet, ImageSet.
- Bitmap new constructor parameter Depth.
- GenericImage: SetAlpha(byte value), new constructor GenericImage(SizeI size, bool clear = false).
- Control: GetChildren, SetChildrenFont, SetChildrenBackgroundColor, SetChildrenForegroundColor.
- ControlSet: Font(Font? value), BackgroundColor(Color? value), ForegroundColor(Color? value).
- StackPanel right aligns child if it's HorizontalAlignment = Right.
- Color now is class, AsBrush, AsPen and GetAsPen(1) are cached.
- SpeedButton: ShortcutKeys, Shortcut, ShortcutKeyInfo, TextVisible, ImageToText.
- GenericToolBar: ImageToText, Items, TextVisible, ImageVisible, SetToolShortcut.
- UserPaintControl.DropFownMenu, fixed Click event.
- GenericToolBar: DefaultImageSize, DefaultNormalImageColor, DefaultDisabledImageColor, NormalImageColor, DisabledImageColor,
 ImageSize, NormalSvgImages, NormalSvgImages, AddSpeedBtn, SetToolAction, AddToolAction, RemoveToolAction,
SetToolImage, SetToolDisabledImage, GetToolImage, GetToolDisabledImage.
- SpeedButton: DefaultShortcutToolTipTemplate, ShortcutToolTipTemplate.
- ContextMenu.Closing event.

# 0.9.411 (2024 January 5)

## 2024 January 5

- Added GenericToolBar control. Demo is in PropertyGridSample.
- MenuItem: Image, DisabledImage. Now it's possible to specify images in menus.
- SpeedButton: Sticky, DropDownMenu, ClickAction.
- Fixed SvgColors default Disabled color.
- Grid: AddStarRow, AddStarColumn.
- PictureBox.DisabledImage.

## 2024 January 1

- Added SplittedPanel. Panel with top, bottom, left, right sub-panels and splitters.
- Added SpeedButton control.
- Cursors: HSplit, VSplit.
- Control.ContextMenuStrip. You can assign here ContextMenu and it will be shown on mouse right-click.
- ContextMenu: Opening, OnOpening.
- Fixed Keys to Key convertion.
- Fixed PictureBox.Image #76.
- Fixed Control mouse capture lost handling.
- RectD: Left, Top, Right, Bottom set methods.
- DrawingUtils: new drawing methods (DrawVertLine, DrawHorzLine, FillRectangleBorder, etc.).
- Fixed PictureBox Height not work #77.
- Control: new focus related props (AcceptsFocus, AcceptsFocusFromKeyboard, AcceptsFocusRecursively, AcceptsFocusAll). 
- GenericLabel control made public.
- Fixed Graphics.GetClippingBox.
- Control.IgnoreLayout property. If True control is not used in container layout.
- LayoutPanel.Layout property now works.

## 2023 December 27

- Added Splitter (can see demo in ControlsSample - Layout - LayoutPanel).
- Control keyboard handling speed up.
- SystemColors made all colors non readonly.
- Fixed string to color comversion for named colors.
- Control: IndexInParent, NextSibling, Right, Bottom, DefaultForeColor, DefaultBackColor, QueryContinueDrag (dummy).

## 2023 December 24

- Control.Scroll event.
- Fixed NewtonSoft.Json vulnerability (updated to new nuget version).
- Fixed Alternet.UI.Integration.Remoting compilation #72.
- Fixed WebBrowser.NavigateToString #71.
- Control: TextChanged, OnTextChanged.
- DrawingContext -> Graphics (WinForms related).
- Graphics: SetPixel, GetPixel.
- Control: Resize, OnResize (WinForms related).

## 2023 December 19

- Control.KeyPress event.
- Control.Invalidate with Region param.
- UserPaintControl: Scrollbars, WantChars.
- Optimized structures (Rect, Point, Size, Color, etc.) in UI C++ code. As a result, all structures are passed
 faster from C# to C++ code.
- Control: IsScrollable, Dock (works when inside LayoutPanel), BackgroundImage (dummy), BackgroundImages (dummy).
- Control Scroll methods:
    * void SetScrollBar(bool isVertical, bool visible, int value, int largeChange, int maximum)
    * bool IsScrollBarVisible(bool isVertical)
    * int GetScrollBarValue(bool isVertical)
    * int GetScrollBarLargeChange(bool isVertical)
    * int GetScrollBarMaximum(bool isVertical)
- Region: IsEmpty, IsOk, Clear(), Contains(Point), Contains(Rect), new constrcutor.
- Renamed Rect, Size, Point. This rename is done to make drawing structures more compatible with WinForms and less confusing.
Rect -> RectD, Point -> PointD, Size -> SizeD, Int32Rect -> RectI, Int32Size -> SizeI, Int32Point -> PointI.

# 0.9.410 (2023 December 15)

## 2023 December 15

- Finished ScrolBar. Added HScrollBar, VScrollBar.
- MouseWheelEventArgs -> MouseEventArgs.
- MouseButtonEventArgs -> MouseEventArgs.
- MouseEventArgs: Button, ClickCount, Delta.
- Control: IsFocused -> Focused.
- SystemInformation: DoubleClickTime, MouseWheelScrollLines.
- Cursor new constructors (from image, from stream).
- New constructor: Bitmap(Stream stream, BitmapType bitmapType = BitmapType.Any).
- Cursors.Default.
- Control: SendToBack, BringToFront, SetChildIndex, OnFontChanged, ImeMode.
- Operators: Color == System.Drawing.Color.
- GenericLabel used in CardPanelHeaderButton. So all problems with CardPanelHeader on Linux and MacOs are fixed.
- Collection: SetItemIndex, GetItemIndex.

## 2023 December 13

- Working on the ScrollBar control.
- Added WindowLogListBox. A Window with LogListBox control.
- Improved Font:
    * Speed and memory optimization.
    * public Font Base {get;}
    * public static Font Get(string familyName, double emSize, FontStyle style = FontStyle.Regular);
    * public Font GetWithStyle(FontStyle style);
    * FontFamily is not created when not needed.
- ControlSet: WidthToMax(), Width(double).
- Improved DrawingContext:
    * Added inline atrribite to all methods.
    * public Size GetTextExtent(string text, Font font, Control? control);
    * public Size GetTextExtent(string text, Font font);
    * GetDPI();
    * public void DrawText(string text, Point location, Font font, Color foreColor, Color backColor);
    * public void DestroyClippingRegion();
    * public void SetClippingRegion(Rect rect);
    * public Rect GetClippingBox();
- Application.CreateAndRun.
- Added GetButtonEvents sample.
- Improvements and bug fixes in Samples.
- Separated Key and Keys enums. This is for better WinForms compatibility.
- KeyEventArgs made compatible with WinForms.
- Improved Control:
    * Added Text virtual property. This is for better WinForms compatibility.
    * ModifierKeys, KeyModifiers.
    * Control now supports IComponent.
    * ResetCursor, ResetFont, ResetBackColor, SetStyle, MousePosition.
    * PointToScreen, PointToClient.
- MenuItem.ShortcutKeys and new constructor.
- Font.Clone().
- AssemblyUtils: EnumProps, IsReadOnly.

## 2023 December 9

- Improved Image/Bitmap:
    * public Bitmap(int width, int height, DrawingContext dc);
    * public Bitmap(GenericImage genericImage, DrawingContext dc);
    * public Bitmap(Int32Size size, Control control);
    * double ScaledWidth {get;}
    * public Int32Size ScaledSize {get;}
    * double ScaledHeight {get;}
    * public Int32Size DipSize {get;}
    * public double ScaleFactor {get;set;}
- Timer made more compatible with system timer. Interval property is now int. Added IntervalAsTimeSpan, AutoReset property.
Added Elapsed event. Timer is not inherited from Component and supports IComponent. Added sample in Source/Tests/Timer.
- Color.GetAsPen(double width, DashStyle dashStyle), Color.GetAsPen(double width).
- Clipboard.ContainsText(TextDataFormat).
- DrawingContext new methods. Allow to fill and stroke figure in one call.
    * void RoundedRectangle(Pen pen, Brush brush, Rect rectangle, double cornerRadius);
    * void Rectangle(Pen pen, Brush brush, Rect rectangle);
    * void Ellipse(Pen pen, Brush brush, Rect rectangle);
    * void Path(Pen pen, Brush brush, GraphicsPath path);
    * void Pie(Pen pen, Brush brush, Point center, double radius, double startAngle, double sweepAngle);
    * void Circle(Pen pen, Brush brush, Point center, double radius);
    * void Polygon(Pen pen, Brush brush, Point[] points, FillMode fillMode);
- Better painting in Border/PaintBox.
- Font: Smaller, Larger, Scaled.
- Rect.ToRect, Point.OffsetBy, Image.Canvas. 
- Ceiling method in Rect, Point, Size.

## 2023 December 7

- System.Drawing related changes in UI Image/Bitmap. This is done to make Image and Bitmap more compatible with System.Drawing.Image.
Main change: All constructors are changed so image width/height are in pixels like it's done in 
System.Drawing.Image. Previously some of the constructors had Dip sizes. BUT this was bad as there can be 
more than one display with different DPI and we don't know which DPI to use when converting pixel to/from dip. 
Added Size, Width, Height (in pixels). Added SizeDip(Control) and BoundsDip(Control) methods to get image 
size in dips (now this is control related). Added new constructors (Image, int, int) and other. All examples and were code updated
to reflect these changes.
- Control: ForeColor, BackColor.
- Made MinMaster.csproj template crossplatform.

## 2023 December 5

- Install.OnlyDebug scripts. They build only x64 Debug, so installation is much faster when needed.
- Control.Invalidate(Rect rect), RefreshRect(Rect rect, bool eraseBackground = true).
- HighDpi related improvements in Control, AuiToolbar, TextInputDialog, NumberInputDialog, RichToolTip, sample projects.
- GenericImage.ChangeToGrayScale().
- Image to ImageSet explicit conversion operator.
- New Image.ToGrayScale() approach. Thanks to @neoxeo, we improved Image.ToGrayScale(), #66
Also added static event GrayScale, it allows to implement custom gray scale for the images if needed.
Also added samples of new GrayScale methods usage to PaintSample and AuiManagerSample.
- Fixed app.manifest in all sample projects.
- RichToolTip: IgnoreImages, DefaultIgnoreImages.
- Improved Border/PictureBox painting.
- ImageSet: DefaultSize, AsImageFor, GetPreferredBitmapSizeAtScale, GetPreferredBitmapSizeFor.
- Rect/Point/Size/Color conversion from/to System.Drawing.*.
- Made Rect, Point, Size, Color methods inline where possible.
- Control.Log.
- MinSplitterSashSize is now HighDpi ready.
- AuiManager: Splitter color and size are like in SplitterPanel.
- WinForms related: Point.Offset, TextDataFormat, Clipboard/Contains* are now methods.

## 2023 December 3

- AuiManager HighDpi support.
- Control.DefaultFont setter. Now is possible to change default font for the controls. So on high dpi screens we could change
this property in order to zoom every control if it wasn't already created. In the our samples default font is now increased
 if dpi > 96.
- HighDpi related. Window: IncFontSize, IncFontSizeHighDpi, UpdateDefaultFont.
- PictureBox.Canvas, Display.Primary.
- GenericImage conversion operator to Bitmap.
- Rect implicit convertion from tuple (Point, Size).
- Image: Save/Load now return bool, added Bounds.
- PaintSample better load/save #64.
- GenericImage: ForEachPixel, GetRGB(int x, int y), GetPixel(int x, int y, bool withAlpha = false), SetPixel(int x, int y, Color color, bool withAlpha = false).
- RGBValue and HSVValue improvements: Added standard methods for any structs.
- Optimized default colors declarations (speed up in Color use).
- Added sample images to PaintSample.
- Added BaseMemory class. It works with native memory buffers (alloc, realloc, free, fill, etc.). It can be used with GenericImage.
- Added ActionRef.
- MenuItem.Add(string title, Action onClick).
- RGBValue conversion operators.
- Added example actions to PaintSample. Show usage of get/set native pixel buffer methods.
- Sample pages with rich text use Control.DefaultFont.
- WinForms related dummy classes: ContextMenuStrip, IWin32Window, ToolStripMenuItem, Form, Graphics.
- WinForms related: Control.CreateGraphics.
- WinForms related: Key -> Keys. Added missing key declarations.
- WinForms related: PenDashStyle -> DashStyle.
- WinForms related: Added Border3dSide, GraphicsUnit.
- WinForms related: Font more compatible with WinForms #6.
- WinForms related: MessageBox made more compatible with WinForms. All related enums are updated
 to have all elements existing in WinForms. All static Show methods are made in the same way.
 MessageBoxResult renamed to DialogResult. Added static event
 MessageBox.ShowDialog which allows to override default show message box handler. Added MessageBoxInfo, HelpNavigator, HelpInfo.
- WinForms related: SolidBrush.Color set method.
- Fixed PaintSample error #67.
- MinMasterTemplate in Install.Scripts\MinMasterTemplate. This is minimal project template
 to use with Alternet.UI latest master version from github.
- Application: log messages are queued until log event is registered, Is64BitOS, Is64BitProcess, LogWarning, LogError.
- Font and FontFamily doesn't raise any exceptions anymore. If Font name is not found, a default
 font is used and error messages is logged. If bad parameters are passed to the font constructor, error message is output to log
and font is created with default parameters.

---

# 0.9.409 (2023 November 30)

## 2023 November 30

- GenericImage - platform independant image for pixel manipulations.
- Image and Bitmap: DefaultDisabledBrightness, AsGeneric (comverts to GenericImage), HasAlpha, PixelWidth, 
PixelHeight, DefaultBitmapType, Depth, Load(string, BitmapType), Save(string, BitmapType), Save(Stream, BitmapType), 
Load(Stream, BitmapType), GetSubBitmap(Int32Rect), ConvertToDisabled(brightness), Rescale(Int32Size), ResetAlpha().
- IconSet separated from ImageSet. As a result - ImageSet is mush faster (no conversion to icon is done on image load or add).
Also Window.Icon type is now IconSet.
- PictureBox now uses current control state to paint image. As a result, Enabled and other properties that change control state
 are fully functional and are used to select the appropriate image. Use StateObjects.Images to set images for
 different control states (see GenericControlState enum).
- HSVValue and RGBValue classes.
- Color : AlphaBlend, ChangeLightness, MakeMono, MakeGrey, MakeDisabled, AsImage(Int32Size).
 Color to SolidBrush converter. Color to and from RGBValue converters.
- PictureBox: CenterVert, CenterHorz.
- Control: Raise, Lower.
- Image: IsOk, IsEmpty, Rect, IsNullOrEmpty, IsNotNullAndOk, GetExtensionsForOpenSave, GetExtensionsForOpen, GetExtensionsForSave.
- Application.Alert, Size.AnyIsEmpty, Rect.CenterIn, Rect.ConvertToString, Display.IsVertical.
- Add FileMaskUtils and use it in PaintSample #64.
- Display: PixelFromDip for Size, Rect, Point, etc.
- ControlDefaults: HasBorderOnWhite, HasBorderOnBlack.
- Improved Border and PictureBox painting.

## 2023 November 26

- Added PopupPropertyGrid popup window.
- SplitterPanel: Now is possible to set min sash size.
- PropertyGrid: GetPropertyParent, GetFirstChild, GetPropertyCategory, GetFirst, GetProperty, GetPropertyByLabel,
 GetPropertyByName, GetPropertyByNameAndSubName, GetSelection.
- Bitmap.Clone, Image: GetDrawingContext, Clone.
- Control: Borders, StateObjects, SetImage, SetBackground, DrawDefaultBackground, BehaviorOptions, CurrentStateOverride.
- Control: AfterShow event, AfterHide event, SetBounds(Rect, SetBoundsFlags).
- Border and PictureBox: improved painting.
- New PlatformDefaults settings: AllowButtonHasBorder, AllowButtonBackground, AllowButtonForeground.
 This is related to bad look of customized buttons on MacOs and Linux.
- ControlSet.Enabled, CardPanelHeader.DefaultTabMargin, Application.LogNameValueIf, ListControl.AddEnumValues<T>(T selectValue).
- Improved animation page in ControlsSample (thanks to @neoxeo).
- Improved different samples.
- WindowPropertiesSample: Add 'Properties...' button.
- Control.RowIndex and Control.ColumnIndex used in Grid.
- Set Button.MinMargin = 3 on Linux.
- Designer.MouseLeftButton is called when mouse button is clicked.
- Optimized OS detection and defaults.

## 2023 November 24

- Control: CurrentState prop and CurrentStateChanged event.
- Added HeaderLabel, SplittedTreeAndCards, PanelMultilineTextBox controls.
- Added CardPanelHeader.Text, Application.AddIdleTask, TextBox.BindText, DateTimePicker.BindValue.
- ControlAndLabel: DefaultControlLabelDistance, CreateDefaultLabel, CreateLabel.
- Control: SetFocusIfPossible, SetMouseCapture, CanUserPaint, IsMouseOverChanged, IsMouseOver,
 Backgrounds, Foregrounds, IsMouseLeftButtonDown, OnIsMouseOverChanged.
- ControlSet: Size, InnerMaxWidth, InnerSuggestedWidthToMax.
- SplitterPanel: SashPositionDip, SplitVerticalDip, SplitHorizontalDip.
- Fixed LayoutPanelPage in ControlsSample.
- Added different ControlState* classes.
- Fixed LinkLabel (#62).
- No more wxWidgets code warnings in Visual Studio.
- Improved DialogFactory.GetTextFromUser.
- Fixed AuiToolbar MouseCapture problems on Linux/MacOs.
- Fixed TextBox text alignment enum choices.
- Border: Borders for different control states.

## 2023 November 23

- Fixed controls flickering on Windows: TextBox, Button, CheckBox, RadioButton, Label, ComboBox, LinkLabel,
 ListBox, DateTimePicker, Calendar, TabControl, TreeView, NumericUpDown.
- Improved Layout in controls: SplitterPanel.
- Border: UniformCornerRadius, UniformRadiusIsPercent. Allows to draw background with rounded corners. 
- CardPanelHeader: DefaultBorderWidth, DefaultBorderPadding, DefaultBorderMargin, DefaultAdditionalSpace, 
Border, TabHorizontalAlignment, BorderWidth, BorderPadding, DefaultBorderSideWidth, 
BorderMargin, TabGroupHorizontalAlignment, TabVerticalAlignment,
TabGroupVerticalAlignment, Orientation.
- Added CardPanelHeaderButton. Now it is possible to customize button used in CardPanelHeader, just assign
 CustomButton.CreateButton property or override CardPanelHeader.CreateHeaderButton() method.
- CardPanel: SelectedCardIndex, IndexOf, BackgroundColorActiveTab, BackgroundColorInactiveTab,
 ForegroundColorActiveTab, ForegroundColorInactiveTab, CardPropertyChanged.
- Draw design corners in PropertyGridSample. Also Border control can draw such borders.
- Ctrl+Shift+Alt+F12 if DEBUG shows Developer Tools. If it is not what is wanted, clear KnownKeys.ShowDeveloperTools.
- Added Application.DoInsideBusyCursor, Control.Group, TreeView.SelectFirstItem, Control.HasIndirectParent,
 FontAndColor.ChangeColor, ButtonBase.ClickAction, TextBox.ShowDialogGoToLine.
- RichTextBox: CurrentPosition, event CurrentPositionChanged.
- ControlSet: LabelMaxWidth, LabelSuggestedWidthToMax.
- Rect, Size, Point, Thickness implicit convert from tuple. Example: Size size = (5, 10);
- Control: DebugBacgkroundColor, UseDebugBackgroundColor.
- Fixed SplitterPanel cursor problems on Linux.
- Improved ControlsSample, PropertyGridSample and other.
- Rect: MinWidthHeight, MaxWidthHeight, PercentOfWidth, PercentOfHeight, PercentOfMinSize.

---

# 0.9.408 (2023 November 20)

- Added Net 8.0 support.
- Svg image default fill color is determined using IsDark (so dark themes are supported).
- Control: IsDarkBackground, Idle event.
- Font: PixelSize, IsUsingSizeInPixels, NumericWeight, IsFixedWidth, Weight.
- Application: DebugWriteLine, LogBeginSection, LogEndSection, LogSeparator.
- Added PanelDeveloperTools, WindowDeveloperTools controls. 'Developer tools' is added to context menu of LogListBox.
- Added IPropertyGridItem.FlagsAndAttributes, Control.FlagsAndAttributes, ICustomFlags, ICustomAttributes, IFlagsAndAttributes,
 Factory, SvgColors, StatusBar.Text, FontAndColor.
- RichTextBox: DefaultUrlColorOnWhite, DefaultUrlColorOnBlack, CreateUrlAttr.
- Color: GetLuminance, IsOk, RHex, GHex, BHex, RGBHex, IsBlack.
- Added svg load parameter: fill color override.
- Fixed Window.Icon=null didn't work.
- Collection: RemoveLast, RemoveFirst.
- Fixed exception in Control idle event unbind.
- Fixed Collection clear items event handling.
- StatusBar improvements: Added visual Panel editor to MenuSample. Fixed known bugs. Added StatusBarPanel: Tag, Clone, Assign.
 Added StatusBar: TextEllipsize, Window, Add, Clear. Improved StatusBar demo in MenuSample.
- UIDialogListEditWindow improved: Hide toolbar buttons if they are not allowed. Apply edited property value in PropertyGrid before Ok button is pressed.
- Add PropertyGrid methods: ClearSelection, ClearModifiedStatus, CollapseAll, EditorValidate, ExpandAll.
- Improved in Control: ResetBackgroundColor, ResetForegroundColor.
- Fixed Control OnGotFocus, OnLostFocus now are called.
- Control: BackgroundStyle, GetStaticDefaultFontAndColor, GetDefaultFontAndColor, IsTransparentBackgroundSupported,
 AlwaysShowScrollbars, GetChildrenRecursive.
- Fixed DrawingSample bad painting on Linux.
- CardPanel and CardPanelHeader can be used together.
- Improved CardPanelHeader: Now possible to specify active/inactive tab foreground and background colors and whether to use them.
 Also active tab bold style is now optional.
- Better exception handling.
- Fixed exception in key handling under Ubuntu 23.
- GenericTabControl.
- Display: ClientAreaDip, GeometryDip, PixelToDip.
- Control: GetDisplay, PixelFromDip, GetPixelScaleFactor, PixelToDip, PixelFromDipF, HideToolTip, UniqueId.
- Add PopupListBox.ResultItem, AuiToolbar.GetToolPopupLocation, PropertyGrid.SuggestedInitDefaults, StatusBar.SetText,
PropertyGrid.AddConstItem, Slider.BindValue.
- Add PopupListBox demo in AuiManagerSample (click on toolbar button opens popup list box).
- Add PopupWindow.ShowPopup at location.
- Control: ResetBackgroundColor, ResetForegroundColor now have 'method' parameter.
- TextBox: DefaultResetErrorBackgroundMethod, DefaultResetErrorForegroundMethod, ResetErrorBackgroundMethod, ResetErrorForegroundMethod.
- Application: IsAndroidOS, IsIOS, IsAndroidVersionAtLeast.
- Control now raises global events: ControlDefaults.InitDefaults, Designer.ControlDisposed, Designer.ControlCreated, Designer.ControlParentChanged.
- Moved some props TextBox -> CustomTextEdit.
- Custom event logger in Developer Tools.
- ListControl: SelectedAction, Add(string,Action).
- Changed base class of control items like TreeViewItem to BaseControlItem. All items have now UniqueId property.
- CardPanel: WaitControlContainer , WaitControl, UseWaitControl, SelectedCard, Find(Control).

---

# 0.9.407 (2023 November 13)

- RichTextBox is now full featured rich text editor not derived from TextBox.
- Added PanelRichTextBox, ValueEditorString, ValueEditorUrl controls.
- Added RichToolTip, ToolTip, Display, SystemSettings, Caret, Cursor, Cursors, SystemFonts classes.
- Rect speed optimization.
- ListControl: FindStringCulture, CompareOptions, DefaultFindStringCulture, DefaultCompareOptions. Improved FindString* behavior
 to use these new props. Added FindString demo to ControlsSample.
- TextBox: IsRequired, AutoUrlOpen, DefaultAutoUrlOpen, DefaultErrorUseBackgroundColor, DefaultErrorUseForegroundColor,
 DefaultAutoUrlModifiers, AutoUrlModifiers, IsNullOrEmpty, IsNullOrWhiteSpace.
- Grid: ColumnCount, RowCount.
- Control: Cursor, IsBold, ColumnIndex, RowIndex, GetColumnGroup, GetRowGroup.
- ControlSet: LabelSuggestedWidth, LabelColumnIndex, LabelColumnIndex.
- ControlAndLabel: LabelColumnIndex, LabelSuggestedWidth.
- ListBox: SelectedItemsAsText, ItemsAsText.
- CardPanelHeader: TabHasBorder, DefaultTabHasBorder.
- Fixed ColorUtils.FindKnownColor exception on MacOs.
- Fixed bad RichToolTip colors on Linux.
- TextBoxAndLabel: IsValidMail, IsNullOrEmpty, IsNullOrWhiteSpace.
- Add SaveFileDialog.OverwritePrompt, OpenFileDialog.FileMustExist.
- FileDialog: NoShortcutFollow, ChangeDir, PreviewFiles, ShowHiddenFiles.
- AuiToolbar better tool click handling.
- Fixed FileDialog: FileName not assigned to dialog.
- Fixed bad popup in CustomControlsSample.
- Fixed system colors under Linux and MacOs. Preivously system colors under Linux and MacOs were hard coded.
 Now we use SystemSettings to get them.
- Fixed #57, #58.
- Added Border.HasBorder, SoundUtils.Bell, CardPanel.UseBusyCursor, PlatformDefaults.TextBoxUrlClickModifiers,
 ModifierKeysConverter.ToString, KeyInfo.ToString, KeyInfo.GetCustomKeyLabel, KeyInfo.RegisterCustomKeyLabels,
Application.BackendOS.
- Application: IsBusyCursor, BeginBusyCursor, EndBusyCursor.
- DialogFactory: GetTextFromUser, GetNumberFromUser.
- RichTextBox: CaretLineNumber, LastLineNumber, ShowDialogGoToLine, SaveToStream, LoadFromStream.
- Added StringSearch class and used it in ListControl, TextBox, RichTextBox (Search property).
- Fixed MacOs related RichTextBox key combinations.

---

# 0.9.406 (2023 November 5)

- Add MultilineTextBox, RichTextBox, LogListBox, ComboBoxAndLabel, ControlAndLabel, TextBoxAndLabel, controls.
- Add popup windows: PopupWindow, PopupListBox, PopupCheckListBox, PopupPictureBox, PopupCalendar, PopupTreeView, PopupAuiManager.
- Add value editors: HexEditorUInt32, ValueEditorByte, ValueEditorCustom, ValueEditorDouble, ValueEditorEMail, ValueEditorInt16,
 ValueEditorInt32, ValueEditorInt64, ValueEditorSByte, ValueEditorSingle, ValueEditorUDouble, ValueEditorUInt16, ValueEditorUInt32, 
ValueEditorUInt64, ValueEditorUSingle.
- PictureBox: ImageVisible, ImageStretch.
- TextBox: MinValue, MaxValue, DefaultValidatorErrorText, DefaultErrorBackgroundColor, DefaultErrorForegroundColor,
 NumberStyles, FormatProvider, DataType, ValidatorReporter, ValidatorErrorText, EmptyTextAllow, EmptyTextValue,
 GetDataTypeCode, GetMinMaxRangeStr, CreateValidator, GetKnownErrorText, ReportValidatorError, DefaultFormat, Converter,
 DefaultText, SetTextAs* methods, MinLength, MaxLength, TextAsNumber, Options, CurrentPosition, CurrentPositionChanged event,
ReportErrorMinMaxLength, UseValidator, ErrorStatusChanged event, .
- ITextBoxTextAttr: Add GetFontStyle, SetFontStyle, GetFontItalic, GetFontInfo, SetFontInfo.
- TextBox: Add rich edit methods SetSelectionStyle, ToggleSelectionFontStyle, ToggleSelectionBold, ToggleSelectionItalic,
 ToggleSelectionUnderline, ToggleSelectionStrikethrough, SelectionSetColor, SelectionSetAlignment, SelectionAlignCenter,
 SelectionAlignLeft, HandleRichEditKeys, SelectionAlignRight, SelectionJustify.
- Fixed TextBox.SelectAll if it is rich editor.
- Improvments in Validator classes and methods.
- Fixed TabControl behavior and bad painting under Linux.
- Improved resize behavior of AuiManager controls and panels.
- AuiToolbar: All Add methods now return toolId.
- AuiToolbar: Add EventToolNameOrId, GetToolName(), GetToolTag(), SetToolName(), SetToolTag(), GetToolMinHeights(),
 GetToolMaxOfMinHeights().
- AuiToolbar: Fixed Clear, DeleteTool, ShowPopupMenu.
- Add Application.FirstWindow; Collection: First, Last; ControlSet: Visible, Action; ImageSet.AsImage.
- Add Window.MakeAsPopup, StringUtils.TryParseNumber.
- AssemblyUtils: Optimization of GetMinValue, GetMaxValue. Add consts for Default, MinValue
 and MaxValue for all number types. Add GetRealTypeCode, GetMinMaxRangeStr, GetDefaultNumberStyles, GetDefaultValue, 
UpgradeNumberType, IsNumberTypeCode.
- Add Slider.ClearTicks().
- PictureBox made unfocusable.
- Control: Add methods and props for grouping of children. Added GroupIndexes, GroupIndex, NewGroupIndex, GetGroup, MemberOfGroup.
- FontInfo improved: add Name property, FontInfo instance is not created if not needed, new constructor, validated SizeInPoints
 property on set.

---

# 0.9.405 (2023 October 25)

- Added Calendar and AnimationPlayer controls.
- Implemented Sizer functionality. It allows to implement complex controls layouts.
- Add to TreeViewItem: TextColor, BackgroundColor, IsBold.
- TreeView speedup (removed handlesByItems usage).
- Updated webview to 2.1.0.2045.28.
- Updated wxWidgets to 3.2.3.
- Improvements in Validators, added number validator demo to ControlsSample.
- Fixed TextBox.Validator
- Fixed exceptions in menu and toolbar.
- Fixed exception in DragAndDropSample.
- Moved PanelTreeAndCards control to the main library.
- Improved Xml documentation in cs files.
- Improved handling of Uixml errors (not finished).
- Add to CheckBox: CheckState, ThreeState, AlignRight, AllowAllStatesForUser.
- Add Label.BindText, ComboBox.BindEnumProp, CheckBox.BindBoolProp.
- Add to TextBox: TextAlign, TextWrap.
- Add to ListControl: FirstItem, SelectFirstItem().
- Minor improvements in PropertyGrid.
- Add to Control: LayoutDirection, ChildrenSet, AddStackPanel, AddButton, AddHorizontalStackPanel, AddVerticalStackPanel,
AddCheckBox, AddLabel, AddGroupBox.
- Fixed freeze/system hang on Linux in CustomColorPicker popup.
- Add to Popup: IsTransient, PuContainsControls, BorderStyle.
- Add to Window: BorderStyle, IsPopupWindow.

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
 
