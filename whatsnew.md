# 0.9.745 (2025 July 22)

- **SpeedButtonWithListPopup**: Auto popup kind with threshold.
  Introduces `PickerPopupKind.Auto` and `MaxItemsUsingContextMenu` to select between context menu and list box based on item count. Default popup kind is now Auto, improving usability for varying list sizes.

- **PanelOkCancelButtons**: Add button text management. 
  Introduced `ResetButtonsText` to reset all button texts to their defaults and `SetButtonText` to set or reset the text of a specific button. 
  Also added `LastClickedAsDialogResult` property to retrieve the dialog result of the last clicked button.

- **Window / wxWidgets**: Restrict adding `GenericControl`. 
  Added a check in `RaiseChildInserted` to prevent adding `GenericControl` instances directly to `Window`, enforcing use of a `Panel` or other container.

- **Control State Settings**: Add `SetCornerRadius` method. 
  Introduced `SetCornerRadius` methods in `ControlColorAndStyle` and `ControlStateBorders` to allow setting the corner radius for all border states of a control.

- **ToolBar**: Add `SpeedButtons` property. 
  Introduces a new `SpeedButtons` property to the `ToolBar` class, providing access to all `SpeedButton` child controls as a `ControlSet`.

- **SpeedButton**: Add `RoundBorder` theme. 
  Introduces the `RoundBorder` theme for `SpeedButton`, including default border radius settings and a new static `RoundBorderTheme` property.

- **MessageBox**: Add `UseInternalDialog` option. 
  Introduces a static `UseInternalDialog` property to control whether the internal dialog is used for `MessageBox` display.

- **WindowMessageBox**: Add dialog implementation. 
  Introduces the `WindowMessageBox` class for displaying MessageBox-like dialogs with customizable text, icon, caption, and buttons. 
  Provides static and instance methods to show message boxes asynchronously and handle user interaction results.

- **Graphics / DrawLabel**: Add image alignment options. 
  Introduced `ImageVerticalAlignment` and `ImageHorizontalAlignment` properties to `DrawLabelParams`, allowing customization of image alignment in label drawing.

- **Label**: Add image alignment properties. 
  Introduced `ImageVerticalAlignment` and `ImageHorizontalAlignment` properties to the `Label` class, allowing customization of image alignment.

- **PanelOkCancelButtons**: Add flexible button management. 
  Refactored `PanelOkCancelButtons` to support dynamic creation, visibility, and event handling for Ok, Cancel, Apply, and custom buttons. 
  Added new constructors, visibility properties, click events, and button management helpers for configuration and exclusivity.

- **AbstractControl / Layout**: Introduce `LayoutFlags` enum. 
  Adds `LayoutFlags` property to `AbstractControl` to customize layout iteration direction. 
  `Splitter` now selects the correct sibling based on layout direction.

- **TabControl / CardPanelHeader**: Add close button. 
  Introduced an optional close button to `CardPanelHeader` with related events and properties. 
  Exposed close button and its visibility through `TabControl`, and added an event for close button clicks.

- **SplittedPanel**: Add panel management helpers. 
  Introduced utility methods for managing child controls, visibility, and dimensions of panels by position: 
  `EnsureChild`, `GetPanel`, `GetPanelVisible`, `GetPanelWidth`, `GetPanelHeight`, 
  `SetPanelWidth`, `SetPanelHeight`, `SetPanelWidthAtLeast`, `SetPanelHeightAtLeast`, 
  `EnsureSideBarChild`, `SetPanelVisible`.

- **FileListBox**: Add file and folder filter predicates.  
  Introduced `FileFilterPredicate` and `FolderFilterPredicate` properties for custom filtering. 
  Added `SelectFolderIfExists` and `SelectInitialFolder` methods to improve folder selection logic. 
  Internal logic updated to apply predicates when listing contents.

# 0.9.744 (2025 July 18)

- **GenericLabel and Label joined into single label control implemented inside the library.** Replaces the GenericLabel class with an 
enhanced Label class, moving and consolidating label functionality. Updates all usages and references from GenericLabel to Label, removes 
the GenericLabel implementation, and adjusts related classes and documentation accordingly. This refactor simplifies the label control 
hierarchy and unifies label-related features.
- **Add radio button support to ListControlItem.** Introduces the IsRadioButton property to ListControlItem, enabling items to be rendered 
and behave as radio buttons. Adds group management methods for mutually exclusive selection within groups. Updates ListBox and VirtualListBox 
to handle ListControlItem instances directly and enforces radio button selection logic. Sample updated to demonstrate 
radio button usage in CheckListBox.
- **Refactor LinkLabel to implement inside the library.** Reworked LinkLabel to inherit from GenericLabel and manage its own state, removing 
the ILinkLabelHandler interface and related handler factory methods. Updated color and event handling logic to be internal to LinkLabel. 
Adjusted PanelSettings and control factory implementations to reflect these changes.
- ScrollViewer: Fix layout related issues.
- PopupWindow: do not focus owner if clicked outside popup.
- **BaseObject.Post** - allows to post action to the message queue.
- PopupWindow: Linux related fixes.
- App.ForceX11OnLinux - forces X11 for the application on Linux.
- Disable scrollbar overlay scrolling on linux.
- **Implement gtk css styling onm Linux.** Added support for automatic loading of GTK CSS styles from a file named {ApplicationFileName}.gtk.css located 
in the application folder. New properties in WxGlobalSettings.Linux class (InjectGtkCss, GtkCss, and LoadGtkCss) allow dynamic theming and 
manual stylesheet injection at startup. Developers can now customize visual appearance - like scrollbars and widget margins - without modifying 
application code. CSS must be configured before application creation to take effect.
- Menu: HasItems, FindItemWithTag, CheckSingleItemWithTag, CheckSingleItem, UncheckItems.
- SpeedButtonWithListPopup: Check selected item if popup kind is menu.
- TabControl and CardPanelHeader: Fixed size of tabs aligned to left or right.
- Fix dpi determination on macOs.
- Graphics.DrawTextWithAngle.
- **Label.IsVerticalText, SpeedButton.IsVerticalText.**
- CardPanelHeader: IsVerticalText, ImageToText.
- TabControl: IsVerticalText, ImageToText, ResetTabImage, SetTabSvg.
- Graphics.DrawLabel now understand vertical text.
- Fix word wrapping related code.
- GenericControl: Add drop down menu support. Introduced the ShowDropDownMenuWhenClicked property and DropDownMenu to GenericControl,
 allowing a context menu to be shown when the control is clicked with the left mouse button. Added ShowDropDownMenu method and updated 
OnMouseLeftButtonDown to handle this behavior.
- Border: Add BorderMargin property. Introduces a BorderMargin property to allow specifying margin around the border. Updates painting and 
layout logic to account for the new margin, ensuring correct rendering and spacing.
- Label: Add mnemonic marker support. Introduces properties and logic to handle mnemonic markers in Label, allowing configuration of 
marker character and enabling/disabling marker processing. Adds utility methods in StringUtils for removing mnemonic markers and 
determining mnemonic character index.

# 0.9.743 (2025 July 13)

- Slider: Bug fixes and new members, improve behavior and default look.
- Button: Show drop down menu on mouse up.
- DateTimePicker: Reimplemented inside the library.
- Fix macOs related painting and layout issues.
- DrawingUtils: Fix WrapTextLineToList.
- SkiaGraphics: Implement Pie, FillPie, DrawPie.
- SpeedEnumButton.IncludeValuePredicate.
- SpeedColorButton.CtrlActionKind.
- Image: Fix conversion from SkBitmap when empty image.
- TemplateUtils: Check SkBitmap size when paint generic child.
- Graphics: Implement Transform internally (needed for better macOs compatibility).
- VirtualTreeControl: Do not scroll to 0 when expand/collapse.
- VirtualListControl: Fix RangeAdditionController.
- Border: new members SetBorderColors, SetLeftBorderColor, SetTopBorderColor, SetRightBorderColor, SetBottomBorderColor.
- AbstractControl: BackgroundPadding, MinimumLocation.
- Implement GetHashCode() using tuples (through the library).
- AbstractControl: SupressInvalidate, EndInvalidateSuppression.
- Slider: thumb corner radius settings.

# 0.9.742 (2025 July 9)

- **Label.WordWrap.**
- **VirtualBox: Fix draw parameters when first item is painted.**
- **BaseObject.Invoke overload with Action param which requires much less resources to execute. Faster Invoke processing.**
- **IntPicker is now derived from TextBoxAndButton and is editable.**
- VirtualListBox: paint unfocused selection in different color.
- VirtualListBox: Fixed SelectedItemIsBold (was ignored)
- Made CardPanelHeader control public.
- VirtualListControl: DefaultUnfocusedSelectedItemTextColor, DefaultUnfocusedSelectedItemBackColor, UnfocusedSelectedItemTextColor, UnfocusedSelectedItemBackColor.
- VirtualListBox: Change unfocused selection color in dark mode.
- TextBoxAndButton: SetErrorText, InitAsNumericEdit.
- CustomTextBox: TextAsInt32, TextAsInt64, SetTextAsNumber.
- NumericUpDown: Derive from IntPicker.
- TreeControlItem: ExpandOnClick, SetItemsExpanded, ExpandItems, CollapseItems, DoInsideUpdate, AutoCollapseSiblings.
- VirtualTreeControl.ApplyVisibilityFilter.
- AbstractControl.OnAfterParentSizeChanged.
- MessageBoxSvg: GetAsImageSet, GetAsBitmap.
- TabControl: Made public interior paint methods.
- VirtualListBox: SelectItemAndScroll overload, FindItemIndex.
- SpeedButton: Add themes PushButton, PushButtonHovered, PushButtonPressed, CheckBorder.
- SpeedButton: StickyChanged event, StickySpreadMode.
- Remove all Idle events.
- AbstractControl: Fix exception in notifications when collection is updated.
- AbstractControl: GetMembersOfGroups, MemberOfAnyGroup.
- ControlSet: ForEach, ForEachVisible.
- ControlPainter: DrawRadioButton, DrawPushButton.
- ControlStateSettings: Add background paint actions.

# 0.9.741 (2025 July 6)

- **Slider: Complete reimplementation on c# inside the library.** This is mainly because WxWidgets slider under MSW on black theme 
is not working properly. Also new Slider will work fine on Maui platform.
- **Display**: Cache Primary display as new WxWidgets sometimes raised exception.
- **ComboBox**: Simplify inheritance chain and started to reimplement ComboBox inside the library (Currently it is GenericComboBox but
in later builds will be used instead of ComboBox).
- **ControlAndButton**: IsSubstituteControlCreated, UseSubstituteControl, SubstituteControl and other related methods.
- **TextBoxAndButton**: IsEditableChanged, IsEditable, DropDownStyle.
- **TextBoxWithListPopup**: Enhance with dropdown events and properties. Added DropDown and DropDownClosed events, DroppedDown property, 
and methods to show/hide the popup. Introduced properties for text selection and empty text hint. Refactored event handling for popup display 
and value changes, and improved substitute control text update logic.
- **TextBoxWithListPopup**: Add lookup and popup enhancements. Introduced properties for lookup behavior (LookupByValue, LookupExactText, 
LookupIgnoreCase) and exposed the ListBox control. Improved popup handling by updating ShowPopup logic, ensuring proper event subscription, 
and refreshing the substitute control after text updates. Also adjusted popup owner assignment and mouse event handling for better consistency.
- **VirtualListBox**: Prevent redundant SetItemsFast calls. Added a check in SetItemsFast to return early if the provided collection is the 
same as the current Items, avoiding unnecessary operations.
- **SpeedButtonWithListPopup**: Add lookup customization options. Introduced the LookupValue event and properties LookupByValue, 
LookupExactText, and LookupIgnoreCase to allow more flexible and configurable item lookup behavior when opening the popup window. 
Updated logic to use these new options and improved item selection in the popup.
- GenericControl: implement Invalidate and Update methods.
- Spacer: now paints border and background if it specified.
- Splitter: Fix to ignore controls with IgnoreLayout = true.
- **Font**: Add Skia font rendering defaults and reset method. Introduces static properties for default Skia font rendering options 
(ScaleX, Subpixel, ForceAutoHinting, Hinting, Edging) to control text appearance when converting to SKFont. 
Adds ResetSkiaFont() method to clear cached SKFont instances, ensuring updated rendering settings are applied. 
- **GraphicsFactory**: Set default Skia font properties. Added initialization of SKFont properties (Subpixel, Hinting, Edging, ScaleX, 
ForceAutoHinting) using default values from the Font class when creating a new SKFont instance. 
This ensures consistent font rendering settings across the application.
- **Add guide for Linux .desktop files.** Created a comprehensive markdown document explaining .desktop file usage on Linux, including 
file locations, categories, icon formats, recommended icon sizes, and best practices for user and system-wide launchers.
- **Add notes on running wxWidgets apps under X11.** Created a documentation file with instructions for launching wxWidgets applications 
using the X11 backend to bypass Wayland, including environment variable usage and .desktop file modification.
- **TabControl**: Refactor event handlers to protected virtual methods. Renamed and refactored several private event handler methods in TabControl 
to protected virtual methods, improving extensibility and enabling derived classes to override event handling. 
- **Thickness**: GetNear, GetFar.

# 0.9.740 (2025 July 2)

- **Create SpeedButtonWithPopup, SpeedDateButton, DatePicker, TimePicker, TextBoxWithListPopup.**
- SpeedButton: LabelImage, ShowComboBoxImageAtRight().
- Fixed cursor for generic controls.
- Button: ShowDropDownMenuWhenClicked, DropDownMenu, ShowDropDownMenu().
- ColorPicker: Show combo button at right.
- SpeedButtonWithListPopup: IndexOfValue, PopupKind.
- SpeedButtonWithListPopup: Unbind Items from popup window items, so popup window is not created when you access items.
- SpeedEnumButton: show context menu as popup by default.
- FindReplaceControl: Use ListPicker as scope editor. Use TextBoxWithListPopup instead of ComboBox.
- ControlAndButton: BtnPlusImage, BtnPlusImageSvg, BtnMinusImage, BtnMinusImageSvg, SetMinusImage, SetPlusImage, SetPlusMinusImages.
- SpeedDateButton: Set default title for the popup.
- Improved focus related code.
- ToolBar: AddSpacerCore, AddCustomBtn.
- Use default control back and fore colors in pickers.
- SpeedButtonWithListPopup: ValueToDisplayString, fix GetValueAsString.
- LinkLabel: DefaultNormalColor, DefaultVisitedColor, DefaultHoverColor and fix color on MSW dark theme.
- SpeedColorButton.AddColor, ListControlItem.AddRangeOfValues, UserControl.ShowDropDownMenuWhenClicked, ContextMenu.ShowAsDropDown.
- ListControlItem: Add IComparable support.
- ListControl: Add FormatProvider and redo get item text.
- Redo mouse enter and mouse leave event handling to support generic controls.
- ScrollViewer: up/down scroll delta is used when ScrollBar is clicked.
- ScrollViewer: Do not multiply size in GetSuggesstedSize.
- ControlAndButton: BtnEllipsisImage, BtnEllipsisImageSvg, GetBtnComboType(), GetBtnPlusMinusType(), GetBtnEllipsisType().
- TextBoxAndButton: Fix vertical align of the inner TextBox.
- ToolBar.OverrideButtonType.
- SpeedButtonWithPopup: Close popup if it is opened when control clicked.
- AbstractControl: Fixed SetFocusIfPossible. Now it will focus child control if needed.
- WindowTextInput: Fix editor layout.
- Window: Fix show modal, add set focus of active control.
- TreeControlItem: Add support for IComparable.
- Splitter: new members which allow to customize behavior and look.

# 0.9.739 (2025 June 27)

- **WebBrowser: Fix background painting for Edge backend.**
- **Create SimpleFormulaEvaluator, LabelAndButton, IntPicker, generic ControlAndButton.**
- FormulaEngine: Do not use Microsoft.CodeAnalysis.CSharp.Scripting anymore, now use SimpleFormulaEvaluator.
- Label: Add DrawDefaultText override.
- AnimationPlayer: Fix LoadFile when animation type is not specified.
- SvgImage.AsNormalImage override.
- SpeedButtonWithListPopup: Fixed behavior if child of form used as control.
- SpeedButtonWithListPopup: Fixed to be at least one line in height.
- SpeedButtonWithListPopup: Items, ValueAs.
- ImageList: AddResized, AddImageStrip and 5 new constructors.
- AssemblyUtils: FindNonGenericMethod, fix InvokeMethodWithResult.
- ControlAndButton: ButtonCombo, ButtonEllipsis, ButtonPlus, ButtonMinus.
- Demo: Replace Slider, ComboBox with new picker controls (as not working properly when dark theme is used on MSW).
- WindowTextInput: hide system menu and close btn in title.
- MenuItem: Commented out disabled images assign as WxWidgets implementation was buggy on Windows 11 when dark theme is selected.
- New members: SpeedEnumButton.ExcludeValues, ControlAndButton.ReportError, DialogFactory.AskIntAsync, Keyboard.IsControlPressed, 
PaintEventArgs.WithRect, ToolbarUtils.GetNormalAndDisabledSvg.
- Toolbar: AddDefaultComboBoxBtn, DoubleClickAsClick, AddSpeedBtn override.
- AbstractControl: Do not create dc if paint is not needed.
- AbstractControl.DrawDefaultbackground now has flags parameter.
- All Picker controls: draw combo btn on the right.
- TemplateUtils.RaisePaintForGenericChildren: do not create dc if no painting is performed.
- Control and Panel: do not paint extra background (overall speed up in controls painting as previously default background was always painted).
- GenericLabel: new members ShowDebugCorners, BeforeDrawText event, DrawLabelParams.
- SpeedButton: Clip long text so it will not paint over the image (if image is aligned to the right of the text).
- SpeedButton: Label is now GenericLabel (and has more features  than GenericTextControl used previously).
- SpeedButton: ResetImages, SetImages, SetImageSets, SetDefaultComboBoxImage, SetSvgImage, SetContentHorizontalAlignment override.

# 0.9.738 (2025 June 24)

- Add controls: ColorPickerAndButton, EnumPicker, SpeedEnumButton, EnumPickerAndButton, FontNamePicker, FontSizePicker, ListPicker, SpeedButtonWithListPopup.
- **Add auto paint of child generic controls**.
- GenericLabel is now generic control and doesn't use native control handle.
- Maui: ControlView now supports AbstractControl.
- SpeedButton: Fixed border in disabled state in StaticBorder theme.
- App.SetAppearance - allows to set color theme (dark/light/system).
- TextBox.AutoUrl made dummy as WxWidgets implementation is buggy.
- Update wxAlternetColourProperty implementation to wxWidgets 3.3.0.
- Slider: Improve behavior and look in dark theme on Windows.
- Calendar: Fix NoMonthChange property setter.
- ListView: improve look in dark mode on MSW.
- SpeedColorButton: new members ListBox, FindOrAddColor, EmptyColorTitle, Select.
- ColorListBox: GetItemValue, FindOrAddColor.
- ColorPicker is now derived from SpeedColorButton.
- Change base class of HorizontalLine to GenericControl.
- SpeedButton: new methods SetContentHorizontalAlignment, SetContentVerticalAlignment.
- PanelSettings: Change used color editor to ColorSelector.
- PanelSettings: Use EnumPicker as enums editor.
- VirtualListBox: SelectItemAndScroll, FindItemIndexWithValue.
- Demo: Use EnumPicker, ColorPicker and other picker controls instead of ComboBox.
- App.IsWindows11AtLeast, PropertyGrid.IsFlagsOrEnum.
- Add default localized titles to popup windows.

# 0.9.737 (2025 June 20)

- WebBrowser: Fix Edge backend background painting.
- Update to use wxWidgets 3.3.0.
- Update to use microsoft.web.webview2.1.0.3296.44.
- RichToolTip: Fix image size and allow multiline title.
- RichToolTip: static CreateToolTipImage (allows to create bitmap with aligned title, text and icon).
- Update documentation to reflect latest API changes.

# 0.9.736 (2025 June 13)

- VirtualListBox: Fixed SetColorThemeToDefault().
- SvgImage: Use Image.ToGrayScaleCached in AsDisabledImage for color svg.
- TemplateUtils: Remove isClipped param from RaisePaintRecursive and other methods.
- Maui: Move event handlers from private to protected in ControlView.
- SystemSettings: Do not increment default font size on Maui.
- SkiaUtils: Changed default font size to 10.
- Maui: Add font name and size to IToolBarItem.
- Maui: Add SimpleTabControlView.DefaultTabStyle, DefaultTabIsBoldWhenSelected.
- Maui: SimpleToolBarView.IsBoldWhenSticky.

# 0.9.735 (2025 June 6)

- Maui: Fixed Item's IsVisible behavior in SimpleToolBarView.
- Maui: SimpleToolBarView buttons are now rectangular.
- Maui: SimpleToolBarView.InsertButton method.
- Maui: It is possible to specify button paddings in SimpleToolBarView.
- SimpleDialogTitleView: Optional Gear button (add method, on click event)
- SimpleDialogTitleView: Can be initialized without Close button.
- Maui: Different improvements in SimplePopupDialog and it's descendants.
- Control: Fix SetFocus, add check for minimized window.
- Fix LogUtils.ShowTestActionsDialog, add Disposed event handler.
- Window: Fix ShowAndFocus for minimized window.
- ThreadExceptionWindow: add Copy button and fixed border style for the memo.
- Add WindowWithMemoAndButton form.
- Window: Fix incorrect modal dialog behavior in some cases.
- Maui: Add settings panel to SimpleSearchDialog.
- Maui: SvgImage property in the item of SimpleToolBarView.

# 0.9.734 (2025 June 5)

- Maui: Create SimpleTwoFieldInputDialog and SimpleSearchDialog.

# 0.9.733 (2025 June 4)

- Update used Microsoft.CodeAnalysis.CSharp nugets.

# 0.9.732 (2025 June 3)

- Maui: Do not show ScrollBars if not needed.
- VirtualListBox.IsPartialRowVisible.
- SkiaGraphics: Implement some of the clipping methods.
- Maui: implemented align origin for internal dialogs.

# 0.9.731 (2025 June 1)

- Maui: Fixed mouse position for events of child controls.
- Maui: Process mouse double-click events.
- Maui: SimpleInputDialog.DisplayPromptAsync.
- SimpleToolBarView: Disabled tab stop on buttons.
- Maui: Fixed BaseEntry.EscapeClicked event not called in some cases.
- BaseObservableCollection: List property and other new members.

# 0.9.730 (2025 May 30)

- Maui: Implement App.AddIdleTask (this fixed different issues in the library as many methods use it).
- Add BaseObservableCollection and inherit BaseCollection from it.
- BrushAndPen: implement AsPaint.
- SkiaGraphics: Implement FillPolygon, Polygon and redo RoundedRectangle, Rectangle, Ellipse, Circle.
- BaseObservableCollection: Add different Sort methods.

# 0.9.729 (2025 May 29)

- AbstractControl: Do not call realign when child control is added/removed and it's IgnoreLayout is true.
- DrawingUtils: Fix wrap text methods.
- Label: Fix WrapToParent and MaxTextWidth.
- Fix SystemSettings.Reset stack overflow in some situations.
- Maui: Implemented control SetFocus in handler.
- Maui: ControlView.TrySetFocusWithTimeout.
- Graphics: Allow to pass an empty rect to DrawElements.
- GenericLabel: Fixed GetPreferredSize.

# 0.9.728 (2025 May 24)

- **Downgraded to an older version of SkiaSharp** due to compatibility issues with ARM64 Linux.
- **Graphics:** `DrawLabel` can render multiline text when the corresponding flag is specified.
- **Graphics:** Added the `DrawStrings` method, which draws a collection of strings with adjustable line spacing and text alignment.
- **VirtualTreeControl:** Added `Invoke` calls to certain methods.
- **VirtualListBox:** Item text can now be multiline (requires `DrawLabelFlags.TextHasNewLineChars` to be specified).
- **VirtualTreeControl:** Fixed the `ContextMenuStrip` property setter.

# 0.9.727 (2025 May 22)

- **Added AssemblyMetaData**: Provides methods for loading assembly metadata.
- Added `VirtualListBoxView` and `VirtualTreeControlView`: fast list box and tree view controls for the MAUI platform with customizable item painting features.
- **Window**: Fixed the `MinimumSize` and `MaximumSize` setters.

# 0.9.726 (2025 May 16)

- DataObject: Fixed DeserializeDataObject.
- PlessSystemColors: Fixed ControlText initialize.
- AbstractControl: Fix ForeColor, BackColor setters to support Color.Empty as in WinForms.
- Image: Fix FromSkia under Maui.
- VirtualTreeContrrol: Fixed sync TreeItems to ListItems.
- Added consts for system colors when dark theme is used.
- ImageDrawable: Stretch is now False by default.
- SkiaGraphics: Fixed DrawImage with rect param.
- SpeedButton.ImageStretch.
- Maui: Fixed ImageSet and ImageList behavior.
- Changed Highlight and HighlightText default colors.

# 0.9.725 (2025 May 13)

- Maui: Fixed scroll bar painting on HighDpi displays.
- Maui: Implemented mouse handling for child controls.
- Maui: Update colors on theme change.
- Command: new constructors with action params.
- SkiaUtils: GrayscaleColorFilter, ImageToBitmap, ConvertToGrayscale.
- Image: ToGrayScale() now uses SkiaSharp.

# 0.9.724 (2025 May 9)

- TabControl: HeaderButtons, HeaderButtonsCount, ContentsControl, HeaderControl, GetHeaderButton, GetHeaderButtonAt.
- TreeControlItem: HasCollapsedParents, ParentItems, NextOrPrevSibling.
- TreeControlItem: Items are now IReadOnlyList{TreeControlItem}.
- VirtualListBox and VirtualTreeControl: ScrollIntoView.
- WindowListEditor: Use VirtualTreeControl instead of TreeView.
- VirtualListBox.Items is now BaseCollection{ListControlItem}.
- DataObject: Support IDictionary save/load to/from xml.
- AbstractControl.OnContextMenuCreated.
- VirtualTreeControl: inherit from Border and use it's border.
- VirtualTreeControl: SelectItem now expands all parents of the selected item.
- VirtualTreeControl: Expand/Collapse don't clear selection.
- VirtualListControl: Redo RangeAdditionController to work better.
- VirtualListBox.RefreshLastRow.
- VirtualListBox: Fixed EnsureVisible for partially visible last item
- VirtualListBox:SelectedIndexes and SelectedItems property setters
- VirtualListBox:Fixed vertical align of the item text.
- Fixed AbstractControl.HoveredControl related issues.
- LogListBox: Fix last item paint when changed by App.LogReplace.
- Demo: Add context menu for header button of TabControl.
- Update csproj to use new SkiaSharp nuget version.
- FindReplaceControl: Find, Replace and Scope editors now have border so they will look properly on Linux and macOs.
- LogUtils: Log exception with sub-items for inner exceptions.
- Image: FromBase64String, ToBase64String.
- Fix possible Assembly.Location exception.
- BrushOrColor: conversion operators from Brush/Color.
- ImageDrawable: Fixed not used SvgColor.

# 0.9.723 (2025 April 29)

- MessageBox: We changed the declaration of all members to return void and include an onClose parameter. This change was made to enable 
the implementation of MessageBox within the library and to allow its use on the MAUI platform.
- Fix modal dialogs behavior.
- Add WindowFilePreview, TreeControlSeparatorItem.
- VirtualListBox: Fixed scroll bars behavior.
- VirtualListBox: Use SuggestedVisiblity setting for scrollbars.
- VirtualListBox: add mouse wheel support.
- VirtualListBox: Fix check box paint when scrolled horz.
- VirtualListBox: Fix some painting issues.
- VirtualListBox: repaint on lost/got focus.
- VirtualListBox: Changed DefaultCurrentItemBorderColor and improved SetColorThemeToDark().
- TreeControlItem: new members CreateItem, AddWithText, PrependWithText, Prepend, Insert.
- Fixed Window.Close (not asked CanClose properly).
- FileListBox: SelectedFolderChanged, SelectFolderByFileName.
- PreviewTextFile: Use background thread for loading the file.
- UserControl: BorderStyle now works properly.
- LogListBox is now derived from VirtualTreeControl.
- VirtualTreeControl, VirtualListBox: Fixed HasBorder and BorderStyle.
- AbstractControl.ContextMenuCreated event.
- Control: Fixed ClientSize (sometimes returned invalid value).
- Control: Fixed InteriorWidth, InteriorHeight.
- Fixed absent border in different controls (issue introduced in the previous build).
- ListControlItem.ContainerRelatedData: Add custom attributes and flags.
- Add svg for check box and radio button painting (will use in Maui port).
- Fixed Color.Gray100 and other Gray* fields.
- Fixed some Linux related issues in Window and ListBox.
- ScrollViewer: Improved mouse wheel scrolling.
- Control: Fix wrong Visible property value after native control is recreated.
- PropertyGrid: Fixed focus exception in some cases.
- AsbtractControl: ReportedBounds, fix DefaultForeColor, DefaultBackColor.
- Maui: Fix ControlView painting if bounds are empty.
- MauiDialogFactoryHandler: Implemented ShowMessageBox for one and two buttons dialogs.
- Maui: ControlView.IsDark, InteriorDrawable.HideScrollBars.
- Add WindowSearchForMembers to developer tools (added to the context menu of LogListBox).
- EnumImages: do not raise exception if loaded svg not found.

# 0.9.722 (2025 April 23)

- **VirtualListBox**: Fully implemented inside the library using C#.
- **ListView**: Fixed scrollbar painting on Windows.
- **ListView**: `HasBorder` property now works correctly.
- Added **ProcessRunnerWithNotification**.
- **TreeControlItem**: New constructor with `Text` parameter.
- **ListControlItem**: Added `GetCheckBoxInfo`, `GetContainerRelated`.
- **VirtualTreeControl**: Implemented `DefaultTreeButtons`, `ToggleExpanded`, `EnsureVisible`, `SelectFirstItem`, `RefreshTree`, `CreateListBox`.
- **VirtualTreeControl**: Fixed `BackColor` and `ForeColor`.
- **VirtualTreeControl**: Implemented `SelectLastItem()`, `SelectFirstItemAndScroll()`, `SelectLastItemAndScroll()`.
- **VirtualTreeControl**: Fixed synchronization issues between tree items and internal listbox items occurring in some cases.
- **VirtualTreeControl**: Fixed incorrect text offset for items without children.
- **AbstractControl**: Fixed `IsDarkBackground`.
- **Color**: Fixed `IsDark`.
- **DefaultColors**: Added `ControlForeColor`, `ControlBackColor`.
- **DefaultColors**: Fixed `WindowBackColor` and `WindowForeColor`.
- **MAUI**: Level margin in `SimpleTreeView` is now calculated using tree button size.
- **MAUI**: Fixed `SimpleToolBarView` not working properly on macOS.
- **MAUI**: Implemented `ControlView.SetFocusIfPossible`.
- **PanelFormSelector**: Uses a busy cursor.
- **PanelFormSelector**: Now integrates `VirtualTreeControl`.
- **Control**: Added `AllowDefaultContextMenu` property.
- **AbstractControl**: `Enabled` is now thread-safe.
- **Demo**: Added header items to `CheckListBox` sample page.
- **ResourceLoader**: Added `ExtractResourceSafe`, `ExtractResourcesSafe`.

# 0.9.721 (2025 April 17)

- PreviewFile: Fixed default text and bk colors.
- FileListBox: new members and draw folder using color svg.
- FileListBox: Inherit from VirtualTreeControl.
- FileListBox: 'Go to up folder' action is implemented more properly.
- FileListBox: NavigateToRootFolder(), NavigateToParentFolder().
- BackgroundWorkManager: Catch and report exceptions.
- Maui: SimpleInputDialog.
- HVAlignment: BottomLeft, BottomRight, TopRight.
- Control.cpp: Fixed problems with BeginUpdate/EndUpdate.
- VirtualTreeControl: ListBox is now created as child control and VirtualTreeControl is derived from Control.
- VirtualTreeControl: Improved tree items and listbox items synchronization.
- VirtualTreeControl: Fixed some issues in demo.
- VirtualTreeControl: Improved TreeControlItem to update base item properties automatically depending on it's state.
- VirtualTreeControl: RootItem setter.
- AnimationPlayer.LoadFromUrl works without exception if no such file.
- GenericImage: Load method return false instead of exception if no image resource is found.
- IconSet: Do not raise exceptions if no icon is found, also support nullable params in constructors.
- ResourceLoader: StreamFromUrlOr, GetAsset.
- ResourceLoader: Most of the load methods now return null instead of raising an exception if no such resource is found.
- ListControlItem: IsBold, paint image from ImageList if ImageIndex is specified.
- TreeControlItem: ItemCount, GetItem(index).
- Create ProgressDialog.
- Image: ToGrayScaleCached, ResetGrayScaleCached.

# 0.9.720 (2025 April 8)

- Added VirtualTreeControl. It represents a tree control that inherits from VirtualListBox. 
See sample in ```Source\Samples\ControlsSampleDll\Pages\ListControls\ListBoxAsTreeWindow.cs```.
- VirtualListBox: Fixed svg checkboxes color in selected state.
- VirtualListBox: Fixed svg check size to fit into item.
- AbstractControl.UseSmallImages, Menu.AddSeparator().
- TreeControlItem: FirstChild, LastChild.
- App: BeginBusyCusros/EndBusyCursor are now thread safe.
- Caret: Fix internal cpp exception in some cases.

# 0.9.719 (2025 March 30)

- Added support for Linux ARM64 platform.
- Maui: SimpleTreeView.
- ListControlItem.ForegroundMargin, IsChecked, ForegroundMarginLeft.
- ListControl: Add properties which allow to specify svg for use instead of native check images.
- Integrated UI.Interfaces library into UI.Common library.
- ComboBox.AllowMouseWheel, PanelSettings.DefaultAllowComboBoxMouseWheel.
- Change used licenses to MIT.
- Update used nugets to the versions.
- Timer: Fixed Interval property setter.
- Improve exception handling in the library.
- Create TreeControlItem, BaseObjectWithNotify, ListControlItemWithNotify.
- Sample: How to setup ListBox as TreeView.
- KnownSvgImages: new members.
- AbstractControl.ContextMenuStrip is now auto created and is always not null.
- DefaultColors: WindowBackColor, WindowForeColor.
- Fix default fore/back colors in ContainerControl, CardPanel, Splitter, AnimationPlayer. 
- Demo: Use Panel instead of Control in all demo pages.
- ToolBar: Fixed behavior related to parent's back/fore color.
- VirtualListBox: Fix painting when checkboxes are big.
- ToolBar: Fixed text item sizing.

# 0.9.718 (2025 March 24)

- Update library to use WxWidgets 3.2.7 and microsoft.web.webview2.1.0.3124.44.
- LogListBox: draw horizontal line as log separator.
- ListControlSeparatorItem: Change default color.

# 0.9.717 (2025 March 22)

- ToolBar: IsVertical, SetMargins, SetToolContentAlignment, SetToolSpacerAlignment, SetToolImageAlignment, SetToolTextAlignment.
- ToolBar: Fix wrong layout in some cases.
- Critical: Application.cpp: Use wxAlternetLog in all cases. This supresses strange modal dialogs appeared in some cases.
- Use SkiaSharp for image loading #174. This fixes problems with png image loading in some cases.
- ScrollViewer: mouse wheel optionally scrolls children.
- SpeedButton: SpacerHorizontalAlignment, SpacerVerticalAlignment.
- SpeedButton and ToolBar: Fix ImageToText behavior.
- PictureBox: Clears the state of Images when ImageSets are assigned, and vice versa.
- App: Simplify and speed up log related code.
- Maui: Fix layout and use weak references in SimpleTabControlView.
- Maui: Created views: BaseLogView, CollectionLogView.
- GridLength: optimize Value.
- AbstractControl: OnBeforeChildMouseWheel, OnAfterChildMouseWheel, IncHorizontalLayoutOffset, IncVerticalLayoutOffset.
- Keyboard.IsShiftPressed, HVAlignment.CenterLeft.

# 0.9.716 (2025 March 16)

- Maui: Fixes and improvements in SimpleToolBarView, ColorPickerView.
- Maui: Implemented SimpleTabControlView, SimpleDialogTitleView. 
- SvgImage: WasLoaded, Xml, LoadingError, GetXmlForSvgLoadedWithError, XmlForSvgLoadedWithError.
- SvgImage: New parameter in constructor which allows to specify whether to throw exceptions when image is loaded.
- New controls: HorizontalLine, VerticalLine.
- New class: DefaultColors.
- PanelSettings.AddHorizontalLine.
- SvgImageDataKind.PreloadUrl - allows to preload svg image in constructor.
- Fix App.DeviceType determination in non-maui application.

# 0.9.715 (2025 March 11)

- Maui: SimpleToolBarView - simple toolbar with speed buttons.
- Timer: Fixed exception when Start called from non UI thread.
- #185 Made DataFormats.FileDrop to be the same as DataFormats.Files.
- IBaseObjectWithAttr.
- Use 4.13 CodeAnalysis nuget in the library.
- Optimize unhandled exception logging.
- App.ProcessLogQueue, ResourceLoader.StringFromUrlOrNull, StreamUtils.StringFromStreamOrNull.
- LogUtils: GetExceptionMessageText, GetExceptionDetailsText.
- Maui: Add CollectionViewExamplePage which shows how to work with CollectionView.

# 0.9.714 (2025 March 4)

- Maui: Do not use own SynchronizationContext.
- AbstractControl.Cursor: Fixed to update on screen immediately when set (previously updated when mouse was moved).
- Cursor: KnownCursorType, ToString.
- Maui: TitleWithTwoButtonsView.
- Maui: Add run debug action button to LogContentPage.
- Do not stop release build on sign tool error.
- PopupControl: AllowNegativeLocation, FitIntoParent, MinLocation.
- AbstractControl: InteriorWidth, InteriorHeight, InteriorSize, WidthAndMargin, HeightAndMargin, SizeAndMargin.
- WebBrowser.ScrollToBottomAsyncJs.
- Return 'Controls Test Window' in demo with WebBrowser page.
- App: AddBakgroundTask, AddBackgroundAction.
- KnownAssemblies: AllReferenced, PreloadReferenced().
- DebugUtils: Faster logged exceptions processing.
- Demo: Fix ThreadingMainWindow.
- Add PythonUtils, BackgroundTaskQueue, BackgroundWorkManager.

# 0.9.713 (2025 February 25)

- AbstractControl: Fix exception in PaintCaret occured in some cases.
- Maui: LogContentPage, EnumPickerView.
- LogUtils: CreateDeveloperTools, ShowDeveloperTools.

# 0.9.712 (2025 February 20)

- Fixed bugs introduced with change Alternet.UI.csproj target to netstandard2.0.
- Updated install scripts.
- Fixed Calendar foreground color on dark theme.

# 0.9.711 (2025 February 19) 

- Target Alternet.UI.csproj to netstandard2.0 (#173).
- Image: FromUrlCached, ClearCachedImages.
- Window: Fix exception in ModalResult setter which was raised in some situations.
- FilesListBox: Show drives list after known folders as it looks better on Linux and macOs.
- Keyboard.IsAltShiftPressed, DelayedEvent.RaiseWithoutDelay, Calendar.SetColorThemeToAuto, VirtualListBox.DefaultSetItemsKind.
- VirtualListBox.DelayedSelectionChanged now works similar to SelectionChanged as had some problems on macOs. Will be reimplemented in future builds.
- ScrollBar: calls Designer.RaisePropertyChanged on value change.
- Demo: Bug fixes and minor improvements.
- Updated documentation in order to reflect latest api changes.
- AssemblyUtils: GetOrLoadAssemblyByName.
- KnownAssemblies.LibraryInteropServices, KnownTypes: InteropServicesNativeLibrary, InteropServicesDllImportResolver.

# 0.9.710 (2025 February 14) Nugets Released

- Fixes related to arm64 support.
- Fixes related to nuget packages build.
- SplittedTreeAndCards: Do not use DelayedSelectionChanged.

# 0.9.709 (2025 February 12)

- ColorDialog: macOs related fixes.
- Improve installation scripts.
- CommandLineArgs: AsBool, Reset, Parse().
- Color: Darker, DarkerDarker, Lighter, LighterLighter.
- Demo: MacOs and Linux dark theme related improvements.
- Demo: Improve layout and fix minor bugs.

# 0.9.708 (2025 February 8)

- Window: set default BackColor and ForeColor in contstructor as it will improve look on macOs.
- RichTextBox.WriteLineText - Writes text at the current position and moves caret to the beginning of the next line.
- Do not log wxWidgets asserts if same msg was just reported.
- ToolBar: Fixed AddTextCore.
- AppUtils: Fixed ExecuteTerminalCommand.
- TemplateUtils: Fixed macOs related bug.
- Fixed exception when ThreadExceptionWindow was closed in some cases.
- ColorListBox, ColorComboBox: Add optional param to AddColors.
- ThreadExceptionWindow: Use MultilineTextBox as it looks better on macOs.
- PanelWithToolBar: underline toolbar in the constructor as it looks better on dark themes.
- App: Fix error when exception-dialog is closed in some cases.
- ToolBar: Fix SetToolSvg to work properly on dark theme.
- ThreadExceptionWindow: improve layout.
- LightDarkColors.Yellow.
- EnumImages: add svg support when images are loaded.
- Demo: Use svg images in ListBoxBigDataWindow instead of png (so now it looks good on dark theme).
- ControlPaint: add Light, Dark, LightLight, DarkDark, IsDark, IsDarker.
- Add new members to Image and ImageList: WithLightLightColors, WithLightColors, WithConvertedColors.
- GenericImage: ConvertColors, LightLightColors(), LightColors().
- ColorDialog: now works properly on macOs.
- CommonDialog: Remove old style ShowDialog method. Use ShowAsync instead of it.

# 0.9.707 (2025 February 4)

- ToolBar: default separator color made the same as border color.
- Maui: Improved bitmap handler to support ScaleFactor and canvas on bitmap creation.
- Maui: Fix exception in ControlView when disposed PlatformView was invalidated.
- Toolbar.SetBorderAndMargin - initializes default toolbar border, padding and margin.
- Demo: Improved layout and fixed minor bugs.
- Install scripts: Do not use Net 9 on old maxOs x64.
- Remove SkiaSharp alpha nuget source as no longer needed.
- AbstractControl.GetLabelFont - fixed to support all font style overrides.
- GenericLabel.MakeAsLinkLabel() - initialized control to look like LinkLabel.
- PanelSettings: Use GenericLabel instead of LinkLabel because LinkLabel shows strange empty tooltip on Linux when mouse is over the control.
- ThreadExceptionWindow: Fixed Details dialog show.
- App: Fixed Error dialog show.
- Window: Fixed issues in modal dialogs behavior.
- RichTextBox.ScrollToCaret().
- App: IsArmOS, IsArmProcess.

# 0.9.706 (2025 January 30)

- Implement modal dialogs internally so under macOs and Linux problems with modal dialogs are fixed.
- Moved ShowDialogAsync, ModalResult and other members related to modal dialogs handling from DialogWindow to Window.
- From now the only method for modal dialog showing is ShowDialogAsync as legacy methods are not compatible with Maui and not worked properly on macOs and Linux.
- Add AbstractControl.FindChild oveload.
- FileDialog.Reset and static props with default values for the dialog properties.
- App: TopModalDialog, ModalDialogs.

# 0.9.705 (2025 January 26)

- ImageSet: Fixed FromSvgStream.
- PropertyGrid.AddInitializer.
- Clipboard: Implement support for DataFormats.Serializable.
- UixmlLoader: FindType, Initialize.
- Clipboard: Fixed exception on get data occured in some cases.

# 0.9.704 (2025 January 22)

- Added Calculator control.
- PanelSettings: overloads for AddLinkLabel, AddButton.
- LinkLabel: Fixed preferred size.
- Grid: Fixed layout to use child's min and max size.
- Window.Activate now works on macOs.
- SpeedButton: Changed static border theme color.
- SpeedColorButton: dispose used popup and dialog.
- SpeedColorButton: Draw border around color image.
- PopupWindow: inherit from Window.
- Color.AsImageWithBorder.
- ToolBar: DefaultItemPadding, AddTextCore, AddTextBtnCore.
- ToolBar: MinimumSize is updated when ItemSize is changed.
- ToolBar: Simple text item is now added with settings similar to SpeedButton item. As a result height of toolbars with only text items will be equal to the toolbars with speedbuttons.

# 0.9.703 (2025 January 17)

- PanelSettings control is finished.
- ControlAndLabel: LabelToControl property which allow to specify horizontal or vertical layout between label and control.
- Return ICommandSource.CommandTarget and add Command.CurrentTarget property.
- Create FontComboBox, PanelListBoxAndCards.
- Maui: Add CheckBoxWithLabelView.
- Maui: Update to the new nuget versions.
- Maui: Update used WinRT version.
- Clipboard: Flush, HasFormat.
- IDataObject.HasFormat.
- ListControlItem: TextHasBold, SetLabelFlag.
- Fixed VirtualListBox.FindFirstVisibleFromLast for 0 item.
- PictureBox: IsImageCentered, GetImagePreferredSize().
- ColorComboBox and ColorListBox: Value property setter is fixed. Find(Color? value) method is fixed.
- Fixed some issues in scroll layout.
- PanelSettings: Color value editors.
- PanelSettings: Fixed text and selector editor creation.
- Set HorizontalAlignment = Left in constructor for some controls.
- Clipboard formats handling improvement.
- Clipboard: Fixed exception macos.
- Calendar: Added update theme after handle created.
- PopupCalendar: more correct initial size.
- PopupWindow: By default title bar made visible as allows to move popup.
- TabControl: Fixed not to create invisible pages on dispose.
- Border.ResetBorders(), ScrollViewer.CreateWithChild.
- Control: Fixed MinimumSize, MaximumSize setters.
- Redo native controls events handling.
- List editor now is non-modal.
- BaseComponent merged into FrameworkElement.

# 0.9.702 (2025 January 12)

- VirtualListBox: Fixed not repatined after EndUpdate in some cases.
- VirtualListBox: Fixed some selection related bugs.
- VirtualListBox: Added RemoveSelectedAndUpdateSelection, SelectItemsAndScroll, SelectLastItemAndScroll.
- VirtualListBox: Redo EnsureVisible internally without using native control.
- Add SystemColorsLight and used them in Calendar and PlessSystemColors. SystemColorsLight contains predefined RGB values for system colors when light theme is selected.
- ObjectUniqueId: precalculate HashCode.
- IndexedValues.GetLockedItemCached.
- ListControlItem: Selected state is saved per container.
- Exclude invisible on screen controls from TAB traversal.
- Set CanSelect and TabStop in constructor for some controls.
- TextBox: Fixed WantTab behavior.
- Fixed Tab traversal behavior.
- Fixed: Invisible controls don't receive key down anymore.
- ColorComboBox: Fixed colors were not filled by default.
- RichToolTip: redesigned to work without creating internal controls.
- Demo: Return design corners to PropertyGrid sample.
- RichToolTip: ToolTipAlignment, ToolTipHorizontalAlignment,  ToolTipVerticalAlignment.
- RichToolTip: ToolTipVisibleChanged event.
- RichToolTip is now derived from ScrollViewer and can be scrollable.
- Improved exception handling. Stack frame is now correctly shown for the exceptions raised in the library.
- UnhandledExceptionMode: CatchWithDialog, CatchWithDialogAndThrow, CatchWithThrow.
- Create focus event arguments and pass them to GorFocus and LostFocus events.
- Maui: Fixed AbstractControl.FocusedControl behavior.
- Maui: OnHandleCreated and OnHandleDestroyed are called by ControlView.
- Maui: Maui: Clipboard now keeps non text data.
- Set ParentBackColor/ParentForeColor = true in constructor for different panel like controls.
- Create ErrorPictureBox, ComboBoxAndButton, BaseEventArgsWithAttr.
- Fixed ParentBackColor and ParentForeColor behavior.
- Fixed layout when LayoutStyle.Scroll.
- ListControlItem.ImageIndex, AbstractControl.GetErrorsCollection.
- Clipboard: Fix to allow SetData for multiple formats in the same DataObject.
- UnmanagedDataObjectAdapter: Do not raise exception on unknown data formats.
- Add PanelSettings control (unfinished).
- TextBox.TextAlign type changed to TextHorizontalAlignment.
- ControlAndButton: TextChanged, DelayedTextChanged events.
- ComboBoxAndLabel.SelectedIndexChanged event.
- ControlAndLabel: TextChanged, DelayedTextChanged events.
- BaseException: Tag, IntFlags, IntFlagsAndAttributes, FlagsAndAttributes, CustomFlags, CustomAttr.
- App.LogError: Fixed behavior in case when parameters is null.

# 0.9.701 (2025 January 8)

- ListBox and CheckListBox are now derived from VirtualListBox. As a result they work much faster with large number of items and has less platform specific problems.
- VirtualListBox: Optimized operations inside BeginUpdate/EndUpdate. As a result Items.AddRange and other operations works much faster.
- VirtualListBox: Fixed incorrect measure item in some cases. Implement more features internally without using platform control. Add FindAndSelect method. Ctrl+A select all items in the multuselect mode.
- VirtualListBox: Implement selection internally without using platform control.
- VirtualListBox: Fixed SelectedIndex prop setter.
- RichToolTip: Message can be multiline and is word wrapped now.
- ListControlItem: SvgImageWidth, SvgImageHeight, HorizontalAlignment, VerticalAlignment.
- CharValidator: Fix didn't allow Delete key on Linux.
- KnownSvgImages: ImgSearch, Search.
- Add classes: EmptyTextHints.
- PictureBox: Fixed GetPreferredSize.
- ControlAndButton.IsButtonLeft.
- TextBoxAndButton: InitFilterEdit, InitSearchEdit, SetSingleButton.
- TextBoxAndButton: Removed InitErrorAndBorder as we have InnerOuterBorder.
- ControlAndLabel: Do not create error picture if not requested.
- TextBoxAndLabel.AutoShowError.
- CustomTextBox: use idle event for errorPicture show.
- Use DisposingOrDisposed in some controls for more safety.
- CharValidator.AlwaysValidControlChars, AbstractControl.IgnoreCharValidator.
- Value editors: set UseCharValidator = false by default.
- Collection editor: Fixed look on Linux. Updated to work with new ListBox and CheckListBox. Allows to specify item attributes (like color, font, etc.) for list controls. Added reload of all properties after value is edited in PropertyGrid.
- ThreadExceptionWindow: Do not show label about 'Quit' button if it is hidden. Show BaseException.AdditionalInformation if it is specified.
- Calendar: MarkAll, ResetAttrAll, DefaultUseGeneric, SetColorThemeToLight, SetThemeInConstructor, DefaultShowWeekNumbers, DefaultSequentalMonthSelect, DefaultShowHolidays.
- WindowSizeToContentMode: new enum members.
- WindowWebBrowserSearch: Fixed search behavior.
- ToolbarSet: Fixed layout when HasBorder = true.
- Toolbar: AddRightSpeedBtn, AddSpeedBtnCore (many overloads with diff params).
- FindReplaceControl: Fix layout, add OptionsVisible, ToggleReplaceVisible.
- ImmutableObject: IsPropertyChangedSuspended, SuspendPropertyChanged, ResumePropertyChanged, DoInsideSuspendedPropertyChanged.
- ScrollBar: setter for PosInfo property.
- ScrollBar.PositionInfo: Assign, Record.
- CardPanelItem: SupressException, DefaultSupressException.
- UIXmlLoader: better error handling when uixml is loaded.
- Fixed Control.ClientSize setter so it uses minimum and maximum sizes.
- AbstractControl: Do not layout if added/removed control is hidden or ignored in layout.
- LogUtils: Better exception logging.
- Fixed AbstractControl.ChildrenLayoutBounds.
- Fix exception on app exit in some cases.

# 0.9.700 (2025 January 1)

Summary:

- AlterNET UI now supports .NET 9, MAUI 9, and SkiaSharp 3.
- Improved MAUI support across the library alongside with WxWidgets.
- New controls: TextBoxAndButton (TextBox with buttons and image on the right side), RichToolTip (reimplemented inside the library), PopupControl (popups inside the parent control), and others.
- Generic controls (can be used as parts of native controls and are handled internally in the library without operating system resources allocation) and Template controls that can be rendered to Graphics or Bitmap.
- VirtualListBox: Improved painting in owner-draw mode, with different methods for thread-safe operations that allow fast loading of a large number of items.

More details:

## CONTROLS AND WINDOWS

- Window and its descendants can be created and used as controls. We have added additional constructors and static CreateAs method with WindowKind parameter, which can override window kind. For example, this technology allows inserting a window as a child of another window or control. 
- Separated platform control from abstract control. So now we have AbstractControl and GenericControl, which are not bound to the OS control.
- Control now notifies subscribers (GlobalNotifications and Notifications properties) about its events. You can use AddGlobalNotification, RemoveGlobalNotification, AddNotification, and RemoveNotification methods to register/unregister for notifications.
- Moved InputBindings property and related methods from Window to AbstractControl to specify input bindings for any control.
- Improved FrameworkElement (the parent class of the control) made it much simpler and more efficient.
- Dispose is now called for all child controls.
- DelayedTextChanged and DelayedSelectionChanged events allow for the implementation of more user-friendly applications.
- Simplify and speed up native keyboard, mouse, and application events. Optimize scroll notifications.

## NEW FEATURES IN CONTROLS

- Many controls are now derived from Border, so it is possible to specify custom borders with any color and width.
- Command and CommandParameter properties are added to different controls, so now it’s possible to specify ICommand (or its name) in the UIXML, and the command will be executed. We have added a NamedCommand class, which handles command execution.
- The new CharValidator property is implementing character validation on the input.
- TextBox new feature: Use events and/or type converter for text to/from value conversion.
- Improved ComboBox and VirtualListView painting in owner-draw mode.
- VirtuaListBox now has methods that allow loading items to the control very fast. Additionally, there are methods that can load items from another thread.
- All controls have many new properties and methods.

## LAYOUT

- Stack layout bug fixes. Also added support for HorizontalLayout.Fill when Layout = Horizontal.
- LayoutStyle.Scroll - The new control layout style is as in ScrollViewer.
- PerformLayout method speed optimization.
- ScrollViewer: Fixed incorrect layout in some cases.

## NEW CONTROLS

- New ControlAndButton and TextBoxAndButton controls. They have unlimited buttons and an optional error image on the right side of the control.
- RichToolTip completely reimplemented. Now, it is derived from Control and shows tooltips inside itself. It supports showing tooltips with images only (without title and message text).
- Added control templates painting. Related classes: TemplateControl, TemplateUtils, TemplateControls.
- Created PopupControl - Allows popups inside the client area of the parent control.

## NEW LIBRARIES

- .Net 9 support added
- AlterNET UI updated to use the latest WxWidgets 3.2.6.
- WebBrowser control now uses the newest Edge library.
- MAUI 9 is now used.
- SkiaSharp 3 is now used, as the previous version had MAUI-related issues introduced by the latest Visual Studio update.
- NuGet packages used in the library are updated to the latest versions.

## MAUI

- Optimized control painting and caret movement.
- Scrollbars are now painted in the ControlView.
- SkiaGraphics: Implemented some of the drawing methods that were not previously implemented.
- SVG images are now supported.

## CLASSES

- AdvDictionary is now derived from ConcurrentDictionary.
- PlessVariant struct made public. It implements a variant structure that can contain data of different types.
- IndexedValues - a fast and simple container for the index/value pairs.
- Optimized FlagsAndAttributes.
- New base classes: BaseObjectWithAttr and BaseObjectWithId. Most of the other classes are derived from them.

## GRAPHICS

- The DrawLabel method has been completely rewritten to support additional features.
- New DrawElements, DrawTextWithBoldTags, and DrawTextWithFontStyle methods.
- RequireMeasure - fast methods to get text measure canvas.
- Color now has a virtual method to get ARGB, which allows the implementation of color themes in the application.
- DrawText method now includes the TextFormat parameter. It has been completely rewritten and is now not specific to WxWidgets as it was implemented inside the library.
- Transform and HasTransform properties have been optimized

## CROSS-PLATFORM

- Added iOS, Android, and MacCatalyst to TargetFrameworks of MAUI-related libraries and projects.
- Fixed installation on macOS Sonoma and newer versions.
- Fixed caret behavior on Linux and macOS.

---

Older items can be found [here](Documents/Whatsnew.History/whatsnew-2024.md)