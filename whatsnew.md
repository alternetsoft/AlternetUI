# 1.1.9 (2026 September 5)

- Add controls: XCalendar, BoldLabel, MonthSpeedButton.
- PanelSettings: Support live changes for some controls.
- ListControlItem: Fix accent paint when disabled or unfocused.
- Demo: Improve ColorListBox sample.
- Add classes: LightDarkBackColors, RestrictedDate.
- Improve repeat pattern rules.
- PanelFormSelector: Improve appearance.
- PanelSettings: Introduce a container stack so new editors can be added inside custom controls, including a convenience helper for horizontal groups.
- ToolTipWindow: Fix tooltip not showing.
- SpeedButtonWithListPopup: Support null item values.
- Demo: Add Other page to DateTime section.
- AbstractControl: Add a generic Add{TControl}(string text) overload that creates a new child control, sets its text, and attaches it to the current parent. This simplifies constructing and initializing child controls in a single call.
- TimePicker: Respect the system time format.
- Adjust exception-window button spacing.
- Refactor RichToolTip to depend on IRichToolTipTemplate instead of the concrete TemplateControls.RichToolTipTemplate.
- PictureBox: Fix preferred size to support partial suggested sizes.
- ListControlItem: Draw background for selected list cells. Add a new IsSelectedCell property to explicitly mark cell-level selection state for rendering.
- Improve format-provider support in various controls.
- Use XCalendar in PopupCalendar.
- ListControlItem: Refactor column cell rendering to call new virtual DrawCellBackground and DrawCellForeground methods, enabling custom per-cell drawing in derived classes.
- SpeedButtonWithListPopup: Make popup showing cancellable and configurable.
- AbstractControl: Add protected methods OnBackColorChanged and OnForeColorChanged.
- AbstractControl: Add BackColorChanged and ForeColorChanged events.
- VirtualListBox: Implement cell-click detection for list boxes with columns. Add a CellClick event that fires when a user clicks a cell, along with a HitTestColumn() method to determine which column was clicked. The event provides access to the item index, item, column, and mouse event information.
- Support None in the selection mode of various controls.
- EnumPicker: Refactor to support speed-button presentation by introducing a new overridable ShowAsSpeedButton() method.
- ScrollViewer.SetScrollBarVisible, VirtualListBox.RequestScrollBarSize, Border.GetDefaultPreferredSize, XListView.SelectionMode, MonthPicker.ValueAsInt.
- Fix cell painting in multi-column list boxes. Item border is now painted if specified. Item foreground color is used if specified.
- AbstractControl.Add{TControl} method.
- Graphics: SetClip with CombineMode parameter.
- Add DrawDualColorLine method to Graphics. It draws a line alternating between two colors/brushes using configurable segment sizes, supporting both horizontal and vertical orientations, clipped to the specified bounds.
- Add static Color.IsVisible helper to check whether a nullable color should be treated as visible by rejecting null, uninitialized, and fully transparent values. This centralizes a common visibility check and makes calling code simpler.

---

# 1.1.8 (2026 August 31)

- Create controls: PinCodePicker, YearPicker, GenericControlAndLabel, MonthAndDayPicker, RelativeWeekdayOfMonthPicker, ControlAndSuffix, XRadioButtonAndSuffix, XIntPickerWithLabels, ScrollableRepeatPatternPicker, ScrollViewer{TControl}, DayOfWeekPicker, MonthPicker, RelativeWeekdayPicker, ScrollableGenericControl, RelativeWeekdayPicker.
- PanelSettings: Refactor type-to-control mapping. Replaced hardcoded type checks and method calls with extensible delegate maps (TypeCodeToControl, DefaultTypeDelegates and ItemToControlMethods).
- PanelSettings: Introduced configurable defaults for button and link-label margins, and applied them when creating those controls.
- PanelSettings.AddInputs.
- Calendar: Add MarkWithRule method. It marks calendar days matching a RepeatPatternRule with the given date attributes.
- DateUtils: Add DateOnly related helper methods.
- Demo: Add RepeatPatternPicker to calendar page and implement marking of days with repeat pattern rules.
- Implement ShowAccentMarker feature in VirtualListBox and its descendants.
- ColorListBox: Fix checkboxes painting position.
- Extend ListBoxItemPaintEventArgs with customization properties: ImageOverride and HideCheckboxes.
- VirtualListBox: Refactor list item checkbox drawing flow. Extracted checkbox rendering into a new static DrawCheckBox helper.
- Add GenericImageList - a faster and more flexible replacement for the existing ImageList class. It can be used with generic controls.
- ListControlItem: Allow to specify multiple images using AdditionalImages property and related methods.
- Make SvgImage image APIs non-nullable.
- Graphics.DrawLabelWithImages: allows drawing a label with multiple linked images, respecting text visibility and image order.
- Fix default layout for different picker controls.
- Thickness: WithTopBottom and WithLeftRight.
- PopupCalendar: Add Today button in the bottom toolbar.
- PopupWindow: BottomToolBarPanel, LeftBottomToolBar.
- BaseObjectWithNotify: Add new SetProperty overload.
- BaseObjectWithNotify: Remove GetNewFieldValue because it raised the changed event before setting the value.
- Create classes: ImageLists.
- Inherit TransparentPanel from HiddenGenericBorder.
- Inherit GenericControlAndPicture from TransparentPanel.
- Change VerticalLine to inherit from GenericControl.
- PanelSettings: Fix exception when value is a nullable enum.
- ColorListBox: TextOverItemImageStyle, DrawTextOverItemImage.
- XIntPicker.DefaultMinWidth, BaseCollection.EnsureCountAtLeast, VirtualListBox.PaintText, Font.HasGlyph, SpeedButtonWithListPopup.ChangeItemsCase.
- Calculator: IsDisplayReadOnly, IsPasswordDisplay.
- Add IsPassword prop to Label, InnerPopupTextBox, EditableListPicker, SpeedButton
- XRadioButton: Introduce a new RadioSiblings property to allow callers to define the radio buttons participating in group auto-uncheck behavior. 
- Make font family names getter thread-safe.
- Fix exception in system settings called on generic control.
- FontFactory now provides a cached DefaultSymbolFont with lazy initialization and a setter for overrides.
- Calculator: Reworks button system to use a typed ButtonKind enum, centralizing button text mapping and click handling. Adds public accessors and helpers to get buttons by kind and control visibility for operator, clear, clear-last, sign, decimal, parenthesis, and display elements. 
- Fix generic control repaint on layout change.
- Fix TabControl.ContentVisible property behavior.
- Inherit VerticalLine from GenericControl.
- Add culture-aware month and day name helpers.
- SpeedEnumButton: Fix display text for enum elements.
- Create date related enums and classes: MonthNamesKind, ExtendedDayOfWeek, DayNamesKind, DaysOfWeek, CalendarMonth, ScheduleRepeatPattern, RelativeWeekday.
- Add RotateFlip to PictrureBox and ImageDrawable.
- Inherit XScrollBar from generic control
- Crteate RepeatPatternRule and other repeat pattern rule classses (for daily, weekly, monthly, yearly repeat patterns).
- ImageDrawable and PictureBox: Fixed painting in disabled state.
- PictureBox: ClearImage now also clears Icon property.
- PictureBox: Remove extra invalidate on size changed.
- ImageDrawable: Add Images and ImageSets properties
- Bitmap: new constructor from SKBitmap.
- IconSet: Fixed load exception.
- New RepeatPatternPicker control (unfinished).

---

# 1.1.7 (2026 August 23)

- Add controls: ScrollablePanelSettings, GenericDateEdit, GenericControlAndButton, GenericControlAndPicture, DrawingResourcePicker.
- PanelSettings: support DateTime, DateOnly and TimeOnly editing.
- PanelSettings: Add DefaultMinChildMargin.
- PanelSettings: DefaultHorizontalLineMargin is Thickness now.
- PanelSettings: AddSpacer now has optional minHeight param.
- PanelSettings: allow to use up/down for int values.
- PanelSettings: Add MinHeight support for multiline input controls.
- PanelSettings: AddFlagCheckBoxes, AddRadioButtons.
- PanelSettings: GetItemControlEditor, GetItemControlLabel, GetItemControl.
- PanelSettings: Add support for "CheckBoxInLabel" custom param for value editors with textBox. This allows to show checkbox instead of label.
- DateTimePicker.Kind now accepts DateTime.
- DateTimePicker: show icons in DateTime mode.
- PanelSettings: Fix CheckBox image margins.
- SpeedDateButton.DefaultTextLeftPadding.
- DateTimePicker now derives from GenericDateEdit.
- Inherit NumericUpDown from XIntPicker.
- PanelRichTextBox: add margin around rich editor.
- TimePicker: implement round border.
- Inherit from generic control: TimePicker, ColorPickerAndButton, EnumPickerAndButton.
- TimePicker: Expose segment controls for the hours, minutes, seconds buttons, plus the separator controls between them.
- DateTimePicker: AsTimeOnly, AsDateOnly.
- Fix TimePicker layout.
- Image.RotateFlip, ToolBar.IsTransparent, DrawingUtils.GetMaxStringSize.
- SpeedButton: KeepSquareShape, IsNormalTransparent.
- Add IsSquare helpers for RectD and SizeD.
- Remove obsolete error strings.
- Add new svg: clock, calendar, rotate.
- Calendar.AsDateOnly, SpeedDateButton.AsDateOnly, TimePicker.AsTimeOnly.
- PanelMultilineTextBox: Add textbox margin and color sync options.
- PanelSettingsItem.ItemToControl. This property allows to override the default control creation behavior with a custom delegate. 
- PanelSettingsItem.Owner. This property allows to get the owner PanelSettings instance for a given item.
- Refactors TextBoxAndButton and ControlAndButtonto accept an optional control type in its constructor.
- CustomEventArgs: new constructors and members.
- Expose PanelSettings.Items as public and enforce explicit item ownership when items are inserted or removed..
- XRadioButton: GetSiblingButtons, RadioGroupId, AutoUncheckSiblings.
- ControlAndLabel: Add constructor overload with typeOfControl and typeOfLabel params.
- ListControlItem.IsCheckBoxEnabled.
- SpeedButtonWithListPopup: now works fine on MAUI platform.
- ColorListBox: ItemImageBorder, ItemImageShape.
- SpeedColorButton: AsImageWithBorder, ColorImageShape.
- Brush and Color: add shape param to AsImageWithBorder.
- LinkLabel: show underline under the text.
- GenericBorder: GetDefaultBorderWidth, OnBorderPropertyChanged.
- Brush.AsImageWithBorder.
- PopupWindow and PopupListBox: New constructor with initial settings param.
- Create DrawingResource.
- PopupColorListBox, ColorPicker and SpeedColorButton: new constructor with useDefaultColors param.

---

# 1.1.6 (2026 August 17)

- ColorListBox: ColorImageSizeKind, ColorImageSize, ColorImageRatio, IsColorRightAligned, AddBrushItem, AddEmptyColor.
- ListControlItem: Change GetItemImageRect declaration to accept CoerceItemImageSizeDelegate param.
- Use latest SkiaSharp nuget version (4.151.1).
- Create StringFormat, CharacterRange, StringTrimming, StringFormatFlags, StringDigitSubstitute, HotkeyPrefix.
- SpeedButtonWithPopup: SelectedValueChanged, SelectedItem.
- ListPicker.DropDownStyle.
- ListPicker: Items property is now WinForms compatible. Old Items property was renamed to ListItems.
- CardPanelHeader: ImageHorizontalAlignment, ImageVerticalAlignment.
- TabControl and CardPanelHeader: DrawTabSeparatorLines.
- CardPanelHeader: Fixes close button paint when tabs are left/right aligned and add OnCloseButtonClick.
- CardPanelHeader: Made inner panels paint methods overridable.
- Graphics: Fix vertical text paint in DrawLabel.
- Add VertDirection to Label, SpeedButton, TabControl, CardPanelHeader.
- Add Graphics.DrawVerticalText with VerticalTextDirection parameter.
- Graphics.DrawVertTextFromBottom.
- GraphicsPath: Path, PathBuilder and new constructor.
- TextFormat.TextBackColor.
- PanelSettings: Allow to customize editor with ColorListBox using custom args: HasEmptyColor and HasTransparentColor.
- ColorListBox..
- XScrollBar minor improvements and bug fixes.
- Graphics: Fix DrawArc.

---

# 1.1.5 (2026 August 10)

- TextRenderer: Use ReadOnlySpan{char} for text.
- Graphics.ClipBounds.
- HatchBrush: Add constructor with HatchStyle param to make it more compatible with WinForms.
- HatchBrush: Add new constructors with background color parameter.
- Reorder elements in BrushHatchStyle to make it compatible with WinForms and HatchStyle enum.
- Implement XScrollBar. This control is implemented inside the library and doesn't use native control.
- Move inner ScrollBar classes to root level.
- Use PaintEventHandler for compatibility with legacy code
- IDataObject: GetDataPresent and GetData overloads
- DragEventArgs: Add X, Y, OriginalTarget, MouseScreenLocation.
- AbstractControl: Enter and Leave events
- LinearGradientBrush: SetSigmaBellShape, SetBlendTriangularShap, LinearColors.
- ColorUtils.BlendColor.
- GradientBrush: StartColor, EndColor, OrderedGradientStops.
- GradientBrush: Implement matrix transformations.

---

# 1.1.4 (2026 August 1)

- Graphics: Add Save and Restore which use GraphicsState.
- Inherit PlessGraphics from SkiaGraphics.
- Graphics: Add DrawString overloads.
- Add AbstractControl.Capture property.
- Graphics: RotateTransform, ScaleTransform, TranslateTransform.
- TransformMatrix: Add TransformMatrix(Matrix3x2 matrix) constructor.
- TransformMatrix: Add explicit conversion to/from Matrix3x2.
- TransformMatrix: Add operators ( +, -, * ).
- Add Graphics.DrawArc and GraphicsPath.AddArc overloads.
- Add static MouseButtons property in AbstractControl and Mouse.
- AbstractControl.FontHeight.
- Add TextRenderer.Handler property so text measuring and drawing can be delegated to a full implementation.
- Add ToolTip component.
- Add rect/angle/mode constructors to LinearGradientBrush

---

# 1.1.3 (2026 July 22)

- Center label text vertically in SpeedButton by default.
- Add EscapePressed event to FindReplaceControl.
- SimpleTabControlView: Add SetTabFont overload.
- Create MauiFontInfo.
- Add SetFont method to BaseEntry.
- Improve MauiPopupEntryHandler.
- ToolBarSet: Add FirstToolBar and LastToolBar.
- Adjust XButton default corner radius.
- Auto-scroll popup listbox to selected item.
- PopupWindow.AfterShowPopup.
- Fix PopupWindow movement and buttons.
- Rename Std prefix to X in controls.

---

# 1.1.2 (2026 July 12)

- ToolBar: Add SetItemSizeFromImageSize and GetMaxToolImageSize methods.
- SpeedButton: PictureBoxSpacer to customize image layout and PictureBoxSize to report the displayed image area size.
- Handle Tab/Escape and key args in BaseEntry.
- Add popup font to PopupEntryParams.
- Fix BaseEntry.SelectAll.
- Add IPopupEntryHandler and MauiPopupEntryHandler.
- BaseEntry: Add action callbacks (FocusedAction, UnfocusedAction, SizeChangedAction, CompletedAction, 
TextChangedAction, TabClickedAction, EscapeClickedAction, KeyDownAction) as alternatives to events. 
- BaseEntry: Add TabClicked event and RaiseTabClicked method. Adds ResetEventActions() to clear all action callbacks.
- BaseEntry: Fires KeyDownAction on Windows key press and handles Tab key via RaiseTabClicked.
- MauiControlHandler: add support to handle any View not only ControlView.
- Simplify IControlHandler.
- Fix ScreenToClient related issues.
- Implement IReadOnlyStrings on list popup button.
- Create TextAsValueHelper and move related code from TextBox to this new class.
- Add ValueHelper property to SpeedButton and TextBox.
- Consolidate repetitive initialization code across all numeric ValueEditor classes by introducing dedicated Init methods in TextAsValueHelper. 
Each new method (InitAsDouble, InitAsInt32, etc.) encapsulates character validation setup, error text configuration, 
and optional value initialization, reducing code duplication and improving maintainability.

---

# 1.1.1 (2026 July 4)

- SpeedColorButton: Fix paint when inside status bar.
- Call set focus of container controls on mouse down.
- Toolbar and ToolbarSet are now generic controls.
- Implement round corners for StdIntPicker.
- GenericBorder: Initialize default border corner radius.
- Fix mouse capture to work with generic controls.
- Hide TextPicker editor when PopupWindow is showing.
- Add rounded corner support to PictureBox and ImageDrawable. Adds CornerRadiusX, CornerRadiusY, and UseCornerRadius properties to both 
ImageDrawable and PictureBox. When UseCornerRadius is enabled, image painting uses an SKPath clip to render images 
with rounded corners via SkiaSharp.
- AppUtils.SetSystemAppearance
- Update StdIntPicker default visual styling
- Refactor welcome page to composed controls.
- Add LinkLabel constructor with URL parameter.
- Add fluent helpers to AbstractControl. Introduces a set of chainable `With*` methods on `AbstractControl` to simplify control setup 
and composition. The new API covers adding child controls, setting margin/padding (including side-specific margins), 
horizontal/vertical alignment, font assignment, and parent assignment.
- Add PictureBox constructor with Image parameter.

---

# 1.1.0 (2026 July 2)

- Add controls: StdListView, PopupCalculator, TransparentPanel, KnobSlider, VerticalLinearGauge.
- Calculator: Text, HasError, Value, FormatProvider, AsDouble.
- Calculator: add round borders.
- Calculator is now derived from GenericControl.
- Calculator: Use TextPicker as display control.
- GenericBorder: static DefaultCornerRadius property, RoundCorner method.
- TwoDimensionalBuffer: new members.
- SimpleFormulaEvaluator: Fix incorrect minus char handling.
- Fix painting when TextureBrush or HatchBrush used. Previously these brushes were painted as solid brush on SkiaSharp graphics.
- Fix matrix for gradient brushes.
- HatchBrush: TileModeX, TileModeY.
- HatchBrush: Added setters to all the properties.
- HatchBrush: Added StrokeWidth, TileSize, BackgroundColor.
- ControlSet: MaxPreferredWidth, MaxPreferredHeight, GroupName.
- AbstractControl: GetNamedGroup, MemberOfNamedGroup, GroupName.
- ColorListBox.AddTransparentColor.
- Fix native RadioButton behavior.
- Inherit Color from BaseObject.
- SystemColors.ResetCachedResources.
- MenuItem: Add new constructor with caption and an array of sub-items.
- ToolTipWindow: Fix rich tooltip dissapearing when moving within the same control.
- Fix WebBrowser edge backend bacgkround.
- SpeedButtonWithPopup: ErrorBorderColor, ErrorBorder, DefaultErrorBorderColor,  ShowErrorBorder.
- EditableListPicker.CancelEdit.
- FontNamePicker: SetValue, SetDefaultFontVisible, SetDefaultMonoFontVisible.
- Font: IsDefaultFont, IsDefaultMonoFont, DisplayName, FontOrigin
- PopupToolBar: Fix static border not shown on Linux.
- Simplify font: Do not use native font handler anymore.
- CharUtils: Mapped char replacement functions.
- Fix conversion of image to native image in some situations.
- ListControlItem: Fix AllowAllStatesForUser determination.
- GenericItemControl: process double click for toggle checked.
- Add TiledImageBackground, DrawingUtils.RenderBackgroundImage.

---

# 1.0.21 (2026 June 29)

- New controls: StdIntPicker, EditableListPicker, TextPicker, InnerPopupTextBox.
- New components: GripComponent.
- PopupWindow: Fix popup position if it is outside the screen to fit the screen.
- SpeedButtonWithPopup: Add PopupWindowPosition property.
- PopupCalendar: Fix incorrect default popup size.
- Control: VisibleChanging, OnVisibleChanging.
- SpeedButton.CreateInnerLabel, Label.EmptyTextHint.
- PanelWebBrowser: Fix tool bar layout.
- PanelWebBrowser: Use TextPicker in the toolbar and fix Enter key.
- FindReplaceControl: Use EditableListPicker as find/replace editors.
- AbstractControl.InnerBordersOverride.
- PopupControl: Fix popup position do not cover scrollbars.
- ListBoxItems: Fix exception when used as IList.
- SpeedButtonWithPopup: Fix popup not closed when clicked on the control.
- Window.GetTextBoxHeight.
- FindReplaceControl: Fixed enabled/disabled scope editor when no single scope is allowed.
- VirtualListBox.EditorTextRequested.
- VirtualListBox: Implement item text editing.
- Fix InnerPopupToolBar overlaps scrollbar on maui.
- Printing now uses SkiaSharp.
- Determine SkiaFontScaleFactor on startup to avoid font size difference on Linux.
- Font: SkiaFontScaleFactor,  FontSettingsIteration.
- Create OverflowSafeCounter.
- Move input key conversion to managed code (now keys are processed faster).
- MswPrinterUtils: Fix exception in get printer names.
- SplittedPanel: ShowWebBrowser, EnsureSideBarPanel, ShowFilesAndFolders.
- Maui: Fix cursor for non-platform controls.
- Cursor: Fix exception in ToString.
- ShapeControl: use std border color if pen not specified.

---

# 1.0.20 (2026 June 24)

- Graphics: SkiaEnabled, Canvas.
- Add support for custom pen styles in Skia graphics.
- Improve look of PopupWindow title.
- Redo painting using SkiaSharp. Do not use WxWidgets graphics by default anymore.
- Redo string to wxString conversion to avoid usage of deprecated c++ libraries.
- ShapeControl: CornerRadius, StartAngle, SweepAngle and take into account Padding when drawing shape.
- GridLength: add conversion from float.
- PanelSettings: DefaultTypeConverters, TypeConverters
- PanelSettings: Uses type converters when text to prop value is converted after text edited
- PanelSettings: Improved exception handling during the validation
- PanelSettings: Fixed error picture alignment
- PanelSetitngs instance is passed as a parameter to all registration methods.
- AbstractControl: RaiseProcessException is now public.
- Demo layout improvement.
- TitleWithTwoButtonsView: Add BackButton.
- Maui: Fix double scrollbar issue
- StdButton: change default alignment
- Use std controls in panels and windows included with the library.
- Use std controls instead of native controls in demos.
- StdButton: change default min width
- PanelOKCancelButtons: add new props.
- AbstractControl.RequestPreferredSize event.
- Fix Button background on msw.
- SpeedButton: Fix TabControl theme was ignored previously.
- ListBoxHeader: Use square borders in title buttons.
- Window.MakeWithoutTitleBar.
- Use int coord in Region.
- Turn on net10.0 for linux builds.
- Fix Region exception when SkiaSharp rendering is on.
- ListControlItem: Item measure with options and complex result.
- StdTreeView:  AutoFitTitleWidth, GetTitleControl, GetTitleControlPreferredWidth.
- Improve EmployeeFormSample, use StdTreeView there.
- Add ListControlItem.SetText for cell text.
- Fix round corner border with background painting.
- Create KnownPopupControls, PenInfo.
- AbstractControl.RefreshOnStateChanged.
- ControlView: Fix control position when border is visible.
- Fix DateTimePicker painting.
- Improve pal control double buffered handling.
- Window is now derived from HiddenBorder.
- GripControl.Target is now IGripControlTarget.
- Update to use new SkiaSharp and Maui version.
- ControlView: change save/restore to keep exact count in paint.
- ITreeViewItemNotification: Add more params to item added/removed.
- AbstractControl.IsDarkBackgroundOverride.
- Disable context menu usage as list popup by default.

---

# 1.0.19 (2026 June 18)

- Add InnerPopupTreeView, ResizablePopupControl controls.
- ListBoxHeader: Use GripControl instead of Splitter for column resize. This fixed column resize issues with horizontally scrolled list boxes.
- TreeViewItem: OnItemSourcePropertyChanged, OnItemSourceCollectionChanged.
- IListSource: ItemInserted, ItemRemoved events.
- TreeViewItem: LinearIndex, GetItemCount, EnumVisibleItems, EnumItems, GetRecursiveItemCount, ItemSource.
- Fix StdSlider and StdProgressBar default size.
- ResizablePopupContol: Do not auto-create border and scroll view.
- BaseObject: Fix Post/Invoke to allow run without app handler.
- ListControlSeparatorItem: Add static init and reset methods to configure as separator.
- ListControlItem: Add advanced mnemonic marker support.
- Use ListChangedEventArgs instead of CollectionChangedEventArgs in collections.
- BaseControlItem is now derived from DisposableObject.
- GripControl: Improve painting to look like splitter.
- AbstractControl: WeakReferences, AddWeakReference, RemoveWeakReference.
- Use clipping when painting non-platform controls.
- VirtualListBox: Change horz scroll pos to 0 on resize.
- VirtualListBox: set clip rect when painted.
- VirtualListBox: IsHoverSelectionEnabled, GetContentSize.
- CardPanelHeader: inherit from HiddenGenericBorder.
- TabControl: Fix tab layout to avoid overlapping close button.
- TabControl: ActiveTabTheme, ActiveTabHasBorder, TabHasBorder, FirstPage.
- CardPanelHeader.ApplyKnownTheme, TabControl.ApplyThemeToHeader.
- CardPanelHeader: ActiveTabTheme, ActiveTabHasBorder.
- Maui: Fix clipping when painting child controls.
- Maui: Improve context menu handling.
- Maui: Fix hovered control determination.
- Maui: Improve light/dark theme change handling.
- Maui: Create DisposableContentView.
- Maui: Improve cursor support for non-platform controls.
- Maui: add cursor support on msw.
- Maui: Fix exception in InvalidateSurface call after app exit.
- ToolTipWindow: do not show when any context menus are visible.
- ContextMenu: OpenedContextMenus, HasOpenedContextMenus , ShowToolTipsDuringContextMenu.

---

# 1.0.18 (2026 June 11)

- VirtualListBox: draw vertical grid lines.
- ListBoxHeader: Turn on IsClipped so columns with small width are painted ok.
- VirtualListBox: Fix painting in multi-column mode.
- TreeViewItem: SafeItem, SafeCell.
- VirtualListBox: SafeRow, SafeCell.
- FindReplaceControl: Assign tooltips to buttons.
- Fix internal tooltip behavior.
- VirtualListBox.GetCell overloads.
- Redo VirtualListBox.Items. Now you can assign IListSource{ListControlItem} there.
- Fix StdTreeView exception in Items getter in some situations.
- Receive SystemColorsChanged for controls that have no parent window.
- Add StaticControlEvents.Notification.
- Add Graphics.DrawBitmap with SKBitmap parameter.
- Rewrite some of the Image methods using SKBitmap
- Remove GenericImage. Please use SKBitmap as a generic image.
- SkiaGraphics: Implement DrawPath, FillPath.
- Add support for GraphicsPath on SkiaSharp canvas.
- Implement GraphicsPath handler for SkiaSharp.
- Implement ClipRegion for SkiaGraphics
- Add AbstractControl.ParentBackColorChanged event.
- MswUtils.AccentColor.
- Add ResizableWindowBorder control.
- ColorUtils.GetDimmedColor.
- ToolBar.Addcon.
- SpeedButton and PictureBox: SetIconAsImage.
- ImageDrawable: Add icon painting.
- MswUtils: Get msw cursor related info (path to cursors, base size).
- GripControl.MinPositionDelta.
- Add Window.FrameMetrics.
- SvgImage: Add get/create methods with SvgImageSet result.
- SvgImage: ImageSet get/create methods now return not null values.
- SvgImage: Add CreateSvgImageSet methods.
- WindowFilePreview: add file path label.
- PreviewFile: Fix behaviour on color theme change on msw.

---

# 1.0.17 (2026 June 7)

## Top improvements:

- Add ListControlItem.IsVisible and implement item visibility support in VirtualListBox.
- SvgImage: Loading speed up. Optimized to use SKPicture.
- Improve dpi awareness support on msw.
- VirtualListBox: Implement horz lines painting.
- ListBoxHeader: Fix behavior on sys colors changed.
- ResourceLoader: Add support for data urls through the library. Now it is possible to specify "data:" url prefix.
- Add SvgImageSet, IconStream, KnownMimeTypes, FileFormatDetector classes.

## Other improvements:

- Add Url property to Cursor, IconSet.
- Redo image containers including ImageList, ImageSet, IconSet and other.
- Fix Bitmap.Empty so it is returned immutable
- Reimplement some of the ImageSet methods and props internally in the library.
- Redo svg loading to use SkiaSharp.
- IconSet: Add props to get system icon sizes.
- Use SkiaSharp for image and svg loading.
- ListControlItemWithNotify: Make prop setters thread-safe.
- VirtualListBox: Do not set SuggestedSize in constructor.
- Do not call layout if there are no child controls.
- Fix minor layout issues.
- Add ShapeControl, ResizableBorder, DockedSubPanelContainer controls.
- WebUtils: Add data url handling methods.
- GripControl: Add configure as border methods.
- Add minimize, maximize, restore svg images.
- AbstractControl: Fix BringToFront and SendToBack.
- Graphics: Add FillOrDraw helpers for shapes.
- Improve vsix: Add reopen retry logic and EditorFactory fixes.
- GripControl: Fix layout.
- Add BarPanel.Spring property.
- Fix control layout when all childs are docked.
- PreviewInBrowser: ico and cur file preview.

---

# 1.0.16 (2026 June 1)

- FlagsAndAttributes: fix exception in GetAttribute overload.
- Window: Fix Close method in case when WindowCloseAction.Dispose.
- Complete reimplementation of StatusBar inside the library. Now it is derived from the <see cref="ToolBar"/> class 
and can contain not only simple text panels but also other types of controls, such as speed buttons, images, combo boxes, progress bars, 
and other interactive elements.
- Remove Raised/Sunken from StatusBarPanelStyle.
- PictureBox: Add ImageVertAlign and ImageHorzAlign and guard invalidates.
- GripControl: use ForeColor when grip svg is painted.
- GripControl: Use alignment enums for sizing grip image.
- GripControl: Add TargetProvider property.
- ImageDrawable: Add HorizontalAlignment and VerticalAlignment.
- ToolBar: Add GripControlKind enum and AddSizingGrip.
- ToolBar: Add Panels property.
- ToolBar: Add and Insert methods for different item types.
- Add move grip to PopupWindow toolbar. This enables a move handle on the bottom toolbar (before adding OK/Cancel buttons), 
making the popup easier to reposition on the screen.
- Fix SvgColors to use IsDark property.
- Fix AbstractControl.IndexInParent.
- Fix BaseCollection.SetItemIndex and make it to return bool.
- AbstractControl: Make SetChildIndex return bool and skip layout if not needed.
- AbstractControl: Make IndexInParent property settable.
- AbstractControl: Fix not invalidated when generic child was removed.

---

# 1.0.15 (2026 May 25)

- Add GripControl. It allows the user to resize or move the target control by dragging the grip.
- PictureBox: Reset cached images on system color change.
- Fix mouse events processing when mouse is captured.
- Add RpcStdioClient and RpcStdioWorker sample projects.
- Add bracket-arrow SVGs and export scripts.
- Introduce ContentSizeScale for scroll sizing.
- Grid: Fixed behavior when some control is invisible or ignored layout.
- ScrollViewer: Fix IsScrolledHorizontally, IsScrolledVertically.

---

# 1.0.14 (2026 May 17)

- Added ScrollableCanvasControl.
- Reimplemented ScrollViewer inside the library without using native control.
- RichToolTip is now derived from ScrollableCanvasControl.
- InteriorDrawable: Add LayoutRectanglesParams to GetLayoutRectangles.
- AbstractControl: Add ChildBoundsChanged and ChildSizeChanged events and handlers.
- SizeD.IsAnyNegative, RichToolTip.HasToolTipBorder.
- ScrollableUserControl: Add InteriorScrollableAreaRects and helpers.
- PointD: WithYIncreased, WithXIncreased, Negate.
- AbstractControl.IsValidChild.
- Removed LayoutStyle.Scroll.

---

# 1.0.13 (2026 May 15)

- Make it compilable with new VC++.
- Grid: Improve layout. Now it supports LayoutOffset and uses AllChildrenInLayout in order to get list of control to align.
- Layout speed up of the controls due to assign of font in constructor.
- Default layout now takes into account SuggestedSize.
- Add LayoutFlags.NoParentPerformLayoutCalled flag.
- Add PerformLayoutParams to layout APIs.
- Make ObjectUniqueId thread safe.
- AttributesFactory: Add GenUniqueAttributeName.
- AbstractControl: Add ChildLayoutUpdated event and OnChildLayoutUpdated method.
- ScrollableUserControol: do not refresh if scroll not changed.
- AxisIntervalD: Implement equality and ToString.
- AbstractControl: Fix PerformLayoutAndInvalidate.
- FrameworkElement: Override ToString to include element Name.
- Add MathUtils.EqualOrBothNaN, SizeD.EqualsAllowNaN, Font.AreEqual, AbstractControl.IsParentPerformLayoutCalled(), AbstractControl.OnLayoutUpdated.

---

# 1.0.12 (2026 May 9)

- Improve the installation scripts to better support Windows on Arm64.
- Update the SharpCompress NuGet package used in the RunCmd tools to avoid the vulnerability warning in Visual Studio.

---

# 1.0.11 (2026 May 1)

- Upgrade to wxWidgets 3.3.2.
- Add Windows on Arm64 support.

---

# 1.0.10 (2026 April 5)

- Add `LayoutManager` and `ILayoutManager`, which allow custom layouts for controls and for classes that support `ILayoutItem`.
- `AbstractControl`: add a `LayoutManager` property.
- `Label` and `Graphics.DrawLabel`: add support for `<u>` and `<i>` tags, including in multiline text.
- `SizeD`: add `SumHeights`, `MaxWidth`, and `MaxHeight` overloads that take a `SizeD[]` parameter.
- Add axis helpers and `Contains` methods to `RectD`.

---

# 1.0.9 (2026 April 1)

- Improve exceptions handling.
- Add ControlAndPicture base control.
- Inherit ControlAndButton and ControlAndLabel from ControlAndPicture.
- Inherit SpeedButton from GenericControl.
- Add hovered color support to StdCheckBox.
- ListControlItem: Add checkbox pre-draw hook and color support.
- PanelWebBrowser: Add UrlMapping and virtualize WebBrowser handlers.
- PanelMultilineTextBox: Add tooltips and virtualize click handlers.
- Make WebBrowser search window public and fix Enter/Esc keys processing.
- SpeedButton: IsTransparent, ResetThemes.
- Make BorderCornerRadius a public struct.
- Refactor ControlSet into a generic ControlSet{T}.
- Add UseMarginsWhenDock to LayoutFlags.
- Add UpDownAndLabel composite control implementing an IntPicker with an attached Label.
- DockStyle: Add LeftAutoSize, TopAutoSize and BottomAutoSize.
- Add SVG image helpers with support for default size.
- TabControl: Make HeaderControl property return CardPanelHeader.
- CardPanelHeader.UseTabBold: remove internal modifier from setter.
- Add exception tracking and ExceptionInfo cache.
- Add runtime object key helpers to CommonUtils.
- Add UniqueId support to BaseException.
- Add RuntimeKey and RuntimeKeyAndType to BaseObject.
- Refactor multi-line exception log message formatting.
- ThreadExceptionWindow: Add Throw button if app is executed under debug environment (Visual Studio, etc.).
- ThreadExceptionWindow: Improve uixml reading error reporting.
- ThreadExceptionWindow: Fix Details button behavior.
- ThreadExceptionWindow: Add localization.
- CommandLineArgs: Add command registration and execution methods.
- Add optional color param to for checkbox/radio control part painters.

---

# 1.0.8 (2026 March 24)

- Support multiple exceptions in ThreadExceptionWindow.
- Create GenericPanel, GenericContainerControl.
- Enhance exception logging and details formatting.
- Add TrimCount method to BaseCollection.
- Fix ControlPainter which stopped to work on Windows when we started to use DirectX drawing context.
- FileListBox: Fix column names and ordering.
- VirtualListBox: Notify on horizontal scroll offset change.
- StdTreeView: IsHeaderAllocated and IsHeaderVisible properties to manage header allocation and visibility. 
- StdTreeView: Keep the header's LayoutOffset.X in sync with the internal list box horizontal scroll.
- Support LayoutOffset when Layout=Dock.
- VirtualLIstBox: support complex tooltips for items.
- VirtualBox: Add configurable tooltip provider for items.
- ControlPainter: fix painting for GenericControl.
- Improve exception handling.
- Fix context menu exception when called on GenericControl.

---

# 1.0.7 (2026 March 19)

- Respect MaxWidth for ToolTipWindow.
- RichToolTipParams: Add Reset, Clone and Assign methods to manage and copy state.
- RichToolTipTemplate is now a fully generic control.
- ToolTipFactory: Add overridable tooltip retrieval.
- Add IGetAsToolTip implementation to ImageSet and Image. Not it is possible to assign them to ToolTipObject property of Control.
- Add GetAttributeOrAdd to attributes interfaces and classes.
- Fix console ILogWriter implementation.
- Add support for complex tooltips in ToolBar and SpeedButton.
- Support RichToolTipParams in overlay tooltips.
- Label: Add AfterDrawText event and DrawParameters.
- Add MeasuredBounds to TextAndFontStyle.
- Support element colors in Graphics.DrawTextWithFontStyle.
- Add sample of complex formatting in Label.
- Add ForeColor and BackColor to TextAndFontStyle.
- Label: Introduce DrawLabelFlags and update draw params.
- Add Tag property to TextAndFontStyle.

---

# 1.0.6 (2026 March 16)

- Fit internal tooltips into screen desktop area.
- Window.GetDisplay, AlignUtils.FitToolTipIntoContainer, PictureBox.ClearImage().
- RichToolTip.MaxTextWidth.
- MAUI: Fix empty tooltips in SimpleToolBarView.
- Add two PictureBox.SetImageFrom overloads: one to create an image from RichToolTipParams and another to render a TemplateControl to an image.
- TemplateControl is now derived from HiddenGenericBorder.
- AbstractControl: Add IsWindow and IsGenericControl.
- Add rich tooltip support in AbstractControl.ToolTipObject. Assign RichToolTipParams in order to show complex tooltips with title, image, template and advanced formatting.
- Add SetParams and PostShowToolTip to IRichToolTip
- AbstractControl: Add ResetMouseHoverEvent to clear MouseHover.
- Implement tooltips for GenericControl
- Add Theme property to GenericItemControl.
- Fix StdButton.UseVisualStyleBackColor.

---

# 1.0.5 (2026 March 12)

- Create ToolTipWindow.
- App: ToolTipProvider is now auto-created.
- Improve border painting to allow using of custom pens/brushes.
- BorderSideSettings: Add Brush and Pen props, optimizations.
- BorderSettings: Add BottomLineBorder, constructors and GetPen.
- Add MakeToolWindowWithoutTitleBar to Window.
- RichToolTip: Add tooltip owner and nullable location.
- DrawingUtils: fix empty and transpernt color border paint.
- Color.IsEmptyOrTransparent.
- StdButton: add get effective border color methods.
- StdButton: Improve color theme.
- Add fluent methods Label.SetWordWrap, ControlSet.Padding, AbstractoControl.SetParent, AbstractoControl.SetMargin.
- Inherit StdRadioButton from StdCheckBox.
- ListControlItem.CheckBoxMargin.
- BorderSettings: Add accent, transparent and empty border presets
- Mark BorderSettings.DefaultColor, BorderSettings.DefaultCommonBorderColor as obsolete. You can use DefaultColors.BorderColor for getting default border colors instead of these properties.
- Add SVG image support to ControlStateSettings.
- Add inner border support to BorderSettings.
- ListControlItem: Do not measure text as bold by default. If old behavior is required, specify DrawLabelFlags.MeasureTextAsBold in item flags.
- Fix DrawingUtils.FillBorderRectangle.
- DefaultColors: Change disabled svg color in light theme

---

# 1.0.4 (2026 March 8)

- Add `StdRadioButton`, `StdCheckBox`, `StdButton`, and `GenericItemControl` controls.
- **Graphics:** Add `TextVisible` flag and honor it in `DrawLabel`.
- **Graphics:** Respect `ImageMargin` in `DrawLabel`.
- **Graphics:** Add setter methods for text drawing parameters.
- **Graphics:** Support image-after-text ordering in `DrawLabel`.
- **LightDarkColor:** Add `BlueDarker` and `BlueLighter` colors.
- Add `DefaultColors.AccentColor`.
- Use the `IDialogButtonRoles` interface instead of `Button` in `Window`.
- **StringUtils:** Fix accelerator index bounds and suffix handling. Improve `ParseTextWithIndexAccel`.
- Introduce `MnemonicMarkerHelper` struct to manage mnemonic marker behavior for label-like controls.
- **ListControlItem:** Add image alignment and text visibility support.
- **ListControlItem:** Add `ImageMargin`, `IsImageAfterText`, `IsVerticalOrientation`, `SetContentAlignment`, `BeforeDrawLabel`, and `IndexAccel` properties.
- **ListControlItem:** Fix getting enabled state from the container.
- **ListControlItem:** Support right-aligned checkboxes.
- **ListControlItem:** Add checkbox state helpers and toggle logic.
- Create `ControlStateSvgImages`.
- **DrawingUtils:** Fix border painting in some situations.
- **AbstractControl:** Fix `ClientToScreen` and `ScreenToClient`.
- **PointD:** Add `HalfOfMinValue` and `HalfOfMaxValue`.
- **GenericControl:** Implement overlay painting and handling.
- Move overlay logic from `UserControl` to `AbstractControl`.
- Convert `ListControlItemDefaults` to notify on change.
- **ListItemDrawable:** Fix painting and add more properties.
- **PaperSizes:** Add more known paper sizes.
- Use `PaperKind` instead of `KnownPaperKind`.
- Fix `PaperKind` element names.

---

# 1.0.3 (2026 March 3)

- Create the `StdProgressBar` control. This is similar to the native `ProgressBar`, but is implemented within the library.
- Redo `AnimationPlayer` so it is now implemented internally in the library.
- Add `AnimationPlayer.CustomAnimationScaleFactor`.
- Add HourGlass animated image and loading API.
- Create `AnimatedImageSet`, `KnownAnimatedImages`, and `AnimatedImage`.
- `AnimationPlayer`: Support animation scaling.
- Add DPI scaling and scaled accessors to `NineRects`.
- `GraphicsFactory`: Add rectangle array pixel/DIP conversions.
- `ControlSet`: Add `MaxSize`, `SuggestedSize`, `MaxHeightOnSizeChanged`, and `MaxSizeOnChanged`.
- `StdSlider`: Fix painting and behavior when vertical.
- `Splitter`: Add `IsMouseEnabled` and `SplitEndOnEscape`.
- `ListControlItem`: Add checkbox and tree level image drawing for multi-column items.
- Fix `PaperKind` enum names.
- `PaperSizes`: Add inches conversion, rounding, and helpers.
- Demo: Add 'Create PDF' button in `PrintingSample`.
- Update documentation.
- `Label`: Account for border in sizing and painting. This fixes a regression introduced in the previous version.
- `PathUtils`: Add temporary path utilities and `GenTempFileName`.

---

# 1.0.2 (2026 February 26)

- ListControlItem: Take into account ItemAlignment when cell is painted.
- ListControlItem: Do not offset cells except first for child items of the tree.
- FileListBox: Align size column to the right.
- Change Label to inherit from HiddenGenericBorder to use its border behavior. Now it is possible to draw border around the label.
- FileListBoxItem: Add file metadata columns and formatting.
- Add date/time format properties to FileListBox.
- ListControlItem: Add SetValue, SetSvgImage, SetText, SetHorizontalAlignment and other fluent methods.
- Create AnimatedImageFrameInfo and AnimatedImageExtractor which allow to work with animated gif and webp images.
- Validate printers before opening print dialog.
- Add PrinterUtilities class which exposes HasPrinters, GetDefaultPrinterName and GetPrinterNames.
- Fix several minor issues in demo projects.
- PreviewUixmlSplitted: Add default alignment and source panel width.
- PreviewFileSplitted: Add alignment API for second preview panel.
- WindowFilePreview: add columns to file list.
- Add column definitions and handlers to FileListBox.
- TimePicker: Use DateUtils for AM/PM designator.
- DateUtils: Add AM/PM overrides and helper methods.
- Add System using to Visual Studio templates.

---

Older items can be found [here](Documents/Whatsnew.History/whatsnew-2025.md)