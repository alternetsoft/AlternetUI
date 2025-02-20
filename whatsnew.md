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

# 0.9.710 (2025 February 14)

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