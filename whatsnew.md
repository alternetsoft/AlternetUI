# 0.9.652 (2024 December 24)

- AbstractControl: ProcessTab, TabIndex, FirstVisibleChild, ChildrenRecursive,  VisibleChildCount, HasVisibleChildren, HasFocusableChildren, GetFocusableChildren, IsKeyBindingValid, ValidateKeyBinding.
- Keyboard.ProcessTabInternally. Default is True. Specifies whether to process TAB key internally without using the platform.
- New classes: IndexedValues, NamedCommand, NamedCommands, GlobalFocusNextEventArg, CommandConverter, FormUtils.
- New structs: DelayedEvent, CommandSourceStruct.
- New events: CustomListBox.DelayedSelectionChanged, AbstractControl.GlobalFocusNextControl.
- New methods: DisposableObject.RunWhenIdle, BaseObject.TryCatchSilent.
- New properties: PanelWebBrowser.FindParams, InputBinding.IsActive.
- New enums: WindowCloseAction.
- ButtonBase: Command, CommandParameter, CommandTarget. As a result Button, CheckBox, RadioButton controls now have these properties. Command is called when control is clicked.
- New feature: Old/New focused control can be retrieved in the event handlers.
- ControlSet: First, IsEmpty, WhenCheckedChanged.
- Fixed: ListControl.RecreateItems.
- Fixed: Activated/Deactivated were raised only for top-most windows (now raised for all controls).
- Fixed: Backspace key were ignored in some cases.
- CustomListBox: IsSelectionModeSingle, IsSelectionModeMultiple.
- CustomListBox: SelectedItems element type changed from object to TItem.
- VirtualListBox: Handle keys internally in single select mode.
- SplittedTreeAndCards: SelectedItemTag property, use delayed event when selected item is changed.
- FindReplaceControl: Use delayed events when text is changed.
- MauiControlHandler: Fix control resize events not fired in some cases.
- ActionsListBox: Add action methods now return created item.
- ActionsListBox: Added AddActionSpacer() (creates non selectable item).
- Window: LastActivateTime, CloseAction, Initialize, IsOwnerVisible, CanClose, OwnedWindowsVisible.
- Window: Fix usage of ActiveControl on Show.
- Window: Default, HasInputBindings, ProcessKeyBindings.
- Window: CloseIdle method, redo CloseAction behavior.
- Redo: Process input bindings in controls without C++ code.
- Demo: Add ControlsTestWindow to Samples page.
- WindowWebBrowserSearch: Fixed exception when WebBrowser was disposed.
- WindowWebBrowserSearch: Use last activated window in order to get WebBrowser control where search is performed.
- WindowWebBrowserSearch: Remember last window position and search options.
- App: LastActivatedWindows, FindVisibleWindow, HasVisibleForm.
- MenuItem: IsShortcutEnabled.
- MenuItem: Fixed Enabled state handling if Command is specified.
- SpeedButton: ShortcutInfo, Command, CommandParameter, CommandTarget.
- ToolBar: GetToolControlAt, SetToolCommand, new SetToolShortcut overload, redo DeleteAll.
- Move InputBindings property from Window to AbstractControl and also add InputBindingsRecursive property.
- AbstractControl: Better TabStop and CanSelect handling.
- Command: CanExecuteOverride, CanExecuteFunc, ExecuteAction.
- Optimize ClickAction in different controls not to use events add/remove.
- InputUtils: IsDigit, IsNumPadDigit.
- DisposableObject: EmptyDisposable, DisposingOrDisposed.
- Remove Xaml\Port\Common\Logging as not used.
- FrameworkElement: made some props public (ContentElements, LogicalParent, LogicalChildrenCollection).
- FrameworkElement: made much more simple and efficient.
- AdvDictionary is now derived from ConcurrentDictionary.
- Optimize FlagsAndAttributes.SetAttribute.
- Use try/catch to supress exception in log to file and other log methods.
- Fixed: Dispose on child controls was not called.
- IObjectToString: new ToString overload.
- ObjectToStringFactory: GetTypeConverter, CreateAdapter, CreateAdapterForTypeConverter, Converters, HandleTypeDescriptorRefreshed.
- ObjectToStringFactory: Made public IObjectToString providers.
- IObjectToStringOptions: Culture, Context, UseInvariantCulture.
- New enum members: ValueValidatorKnownError.None, TextBoxOptions.UseTypeConverter, TextBoxOptions.UseInvariantCulture.
- CustomTextBox new events: GlobalStringToValue, StringToValue, GlobalValueToString, ValueToString.
- CustomTextBox new properties: TypeConverter, Context, Culture, IsNumber, IsSignedInt, IsUnsignedInt, IsFloat, IsSignedNumber, IsMinValueNegativeOrNull, IsHexNumber.
- CustomTextBox new methods: SetErrorTextFromDataType, SetValidator, SetValueAndValidator.
- CustomTextBox new feature: Use events and/or type converter for text to/from value conversion.
- Display: Fixed incorrect behavior under Maui in some cases.
- Maui: Fixed incorrect scrollbar metrics usage in some cases.
- Maui: Fixed incorrect ScaleFactor usage in some cases.
- Maui: Added DisposableContentPage, WaitContentPage.
- Fixed Window.CreateAs.
- Maui: Fixed to use app theme in order to get scrollbar colors.
- WeakReferenceValue: Changed event, RaiseValueChanged().
- AsbtractControl: do not raise events if disposed.

# 0.9.651 (2024 December 17)

- AbstractControl: DelayedTextChanged event.
- Window.ActiveControl property.
- Simplify and speedup native keyboard, mouse and application events.
- TabControl: ContentsVisible, TabPaintAlignment.
- AbstractControl: OnAfterParentScroll, RaiseForegroundColorChanged, OnAfterParentMouseWheel, SetFocusIdle, ForEachVisibleChild.
- AbstractControl: Optimize scroll notification.
- AbstractControl: MinimumSize, MaximumSize constraints are handled correctly (previously were handled in Control).
- AbstractControl: IsUnderline.
- SpeedButton: StickyToggleOnClick, ImageLabelDistance, HasImage.
- SpeedButton: Fixed Font and IsBold behavior.
- SpeedButton: Fixed incorrect painting in hover state in some cases.
- ComboBox: Fixed auto size bugs.
- ToolBar: LastTool.
- MathUtils: ApplyMinMaxCoord, ClampCoord.
- Fixed SizeD.ApplyMin.
- FindReplaceControl: ScopeItemSelectionOnly.
- Timer: StartOnce, StartRepeated.
- ControlAndButton, ControlAndLabel: Fixed horizontal alignment. Fixed SetFocus.
- PanelOkCancelButtons: ApplyButton is not created when no needed. Add HandleCancelButtonClick, HandleOkButtonClick.
- ValidationUtils: Redo IsValidMailAddress without using exception.
- DialogWindow.ShowDialogAsync, WeakReferenceValue.Reset.
- TextBoxAndLabel: use delayed event for validation.
- Add WindowTextInput window and use it instead of platform dialogs.
- DialogWindow: ActiveControl is focused on ShowModal.
- TextBoxAndButton.InitErrorAndBorder, ControlUtils.FindVisibleControl.
- ValueFromUserParams.OnSetup.
- PlatformDefaults: IncFontSizeHighDpi, IncFontSize.

# 0.9.650 (2024 December 10)

- VirtualListBox: Fixed RecreateItems method.
- TextBoxAndButton: InitPasswordEdit.
- ToolBar: SetToolSvg, SetToolImage(KnownButton), SetToolTag, GetToolTag, GetToolsAs, GetTools, GetStickyTools.
- KnownButton: TextBoxShowPassword, TextBoxHidePassword.
- AbstractControl: OnAfterParentKeyPress, SetChildrenVisible.
- AbstractControl: Fixed exception in ForEachVisibleChild.
- AbstractControl: Do not call Invalidate if BeginUpdate is called.
- New constructors for Collection, VirtualListBoxItems, ListControlItems{T}.
- CustomKeyEventArgs.ShiftOrNone.
- TimerUtils: LogActionRunTime, GetActionRunTime.
- Redo Control.CenterOnParent without platform handler.
- HVAlignment: VerticalOrNull, HorizontalOrNull.
- SvgImage: ImgEyeOn, ImgEyeOff.
- ControlAndButton: BtnComboBoxKnownImage, DefaultBtnComboBoxImage, DefaultBtnEllipsisImage, DefaultBtnPlusImage, DefaultBtnMinusImage, BtnComboBoxSvg.

# 0.9.649 (2024 December 8)

- LayoutStyle.Scroll - new control layout style as in ScrollViewer.
- ScrollViewer: Fix bad layout in some cases.
- SpeedButton: DefaultClickRepeatDelay, ClickRepeatDelay.
- VirtualListBox: VisibleCount, ScrollToRow(int), ScrollToFirstRow(), ScrollToLastRow(), TopIndex (set method).
- VirtualListBox: SelectItemOnNextPage(), SelectItemOnPreviousPage(), SelectNextItem(), SelectPreviousItem().
- PlessVariant struct made public. It implements variant structure which can contain data with different types.
- FindReplaceControl: Scope, IsScopeAllOpenDocuments, IsScopeCurrentProject, IsScopeSelectionOnly, IsScopeCurrentDocument, OptionFindTextAtCursor, OptionPromptOnReplace.
- AbstractControl.OnBeforeParentKeyPress.
- TimerUtils: Change ScrollBar click interval 20->50.
- ControlAndButton: Fix IsBtnClickRepeated behavior.
- AbstractControl: DoInsideInit, GetChildrenMaxPreferredSizePadded, SetScrollBarInfo.
- KeyEventArgs.IsHandledOrSupressed.
- Fix Display.AllDPI behavior in some cases.
- PropertyGridSample: Add test actions for methods with result.

# 0.9.647 (2024 December 2)

- Colors are loaded faster to the color editors.
- Color: Fixed ToString and IsNamedColor.
- Maui: Add ColorPickerView.
- Maui: Fixed ControlView behavior on macOs.
- Changed default mono fonts for macos.

# 0.9.646 (2024 November 27)

- Fixed scrollbar interior metrics and painting.
- Updated installation to reflect referenced libraries changes.

# 0.9.645 (2024 November 25)

- Fixed installation on macOs Sonoma and newer versions.
- Use WxWidgets 3.2.6.
- Use new Edge library (microsoft.web.webview2.1.0.2903.40).
- ControlAndButton.IsMinusButtonFirst property.
- RichTextBox: remove xml open/save as not supported on macOs.
- Alternet.UI.RunCmd: Added ReplaceInFiles command.

# 0.9.644 (2024 November 23)

- Added Net 9 support.
- ToolBar: IsToolClickRepeated, GetToolIsClickRepeated.
- ControlAndButton: HasBtnEllipsis, HasBtnPlusMinus, IdButtonEllipsis, IdButtonPlus, IdButtonMinus, IsBtnClickRepeated.
- ControlAndButton: HasButton, GetBtnName, SetHasButton.
- Fixed: Default monospaced font size is updated like font size.
- Used MAUI 9 in the library.
- Fixed: VisualStateChanged events not fired if state not changed.
- Fixed: AbstractControl.SetSizeToContent.
- Fixed: Template controls layout.
- Removed SkiaSharp related warnings.
- Fixed: Caret behavior on Linux and macOs.

# 0.9.643 (2024 November 15)

- VirtualListBox: MeasureItem event and related methods.
- VirtualListBox: Owner draw as in WinForms, DrawItem event and related methods, DrawMode property.
- Fixed Idle events behavior. Added App.WakeUpIdleWithTimer property and WakeUpIdle() method.
- TabControl: Internal CardPanel now uses parent's font and colors.
- VirtualListBox: Fixed bad repaint on macos.
- RichToolTip: Fixed right border paint.
- AbstractControl: AlignInParent, AlignInRect, ContainsMouseCursor, DockInRect, DockInParent.
- PopupControl: FocusParentOnShow.
- AbstractControl: MinMargin, MinPadding made protected (were internal).
- SplittedPanel: All sub panels now have AbstractControl type.
- Graphics is not allocated if no painting is done.
- Border.AutoPadding.
- Updated SkiaSharp references to the new version.
- HasInnerBorder property in TextBoxAndButton, TextBoxAndLabel, ComboBoxAndLabel controls.
- ToolBar: SetToolIsClickRepeated, FindTool.
- ControlAndButton: HasBtnComboBox.
- ControlAndButton: ButtonClick event now has ControlAndButtonClickEventArgs as event argument.
- Graphics: DrawImage and FillRectangle overloads as in WinForms.
- CustomListBox: SelectedIndexChanged event as in WinForms.
- Renamed ControlPainter -> ControlPaint, added DrawBorder method.
- Renamed VirtualListBox: Renamed HScrollBarVisible -> HorizontalScrollbar as in WinForms.
- VirtualListBox.TopIndex as in WinForms.
- Fixed native scrollbars behavior.

# 0.9.642 (2024 November 7)

- ContainerControl: now derived from HiddenBorder so it's possible to specify custom background and border for all the containers (like panel, stack panel, etc.).
- Created ControlAndButton, TextBoxAndButton controls.
- Button: new constructors with text and click action parameters.
- Label: new constructor with text parameter.
- SpeedButton: now has no round corners by default.
- UserControl: Fixed background selector for different states. Now it returns default background and not null like it was previously.
- ControlSet: ParentForeColor, ParentBackColor methods.
- ControlAndLabel: Error picture now uses parent's background color by default.
- ToolBar: MakeBottomAligned, MakeTopAligned methods.
- Demo: Updated ToolBar demo with new features.
- Demo: Added setting of the custom background color for the panel on TextNumbersPage.
- Border: Fixed GetPreferredSize if no border.
- ControlStateBorders: Fixed Clone method.
- Created ToolStripSeparator, ControlSubscriber.
- PopupControl: AcceptOnTab, AcceptOnSpace, HideOnClickParent, FocusContainerOnClose, CancelOnLostFocus, 
AcceptOnLostFocus, CloseWhenIdle, HideOnMouseLeave.
- Control: Do not call invalidate if not visible on screen.
- AbstractControl: Contains, IsParentWindowVisible, VisibleOnScreen.
- AbstractControl: DoubleClick event as in WinForms.
- AbstractControl.Focused implemented (previously was empty and was overriden only in Control).
- AbstractControl: Optimization. Do not call paint if rect is empty.
- AbstractControl: minor layout improvements.
- Font: static GetWidth, GetHeight.
- AbstractControl: OnChildLostFocus.
- Mouse.Moved static event.
- Changed AbstractControl.IsMouseOver to use Mouse.GetPosition.
- ImageList.Draw.
- Fixed Color -> System.Drawing.Color conversion.

# 0.9.641 (2024 November 1)

- VirtualListBox.SelectionUnderImage property.
- Rewrite Graphics.DrawLabel on c#.
- Graphics: DrawElements and new DrawLabel overload.
- GenericLabel: Use HVAlignment instead of GenericAlignment for TextAlignment property.
- GenericLabel: TextAlignmentVertical, TextAlignmentHorizontal properties.
- GenericLabel: Take into account border size when paint label text.
- GenericLabel: Do not add dummy space before Text when image is visible, use default image/text distance.
- AbstractControl.Invalidated event and related methods.
- AbstractControl.MouseCaptureChanged event and related methods.
- ComboBox.DrawMode.
- Fixed Color to System.Drawing.Color conversion.
- Use HVAlignment instead of GenericAlignment in different places.
- Remove debug assert on DrawText with wrong color. Now drawing is not performed with wrong colors and no asserts are shown.
- Demo: VirtualListBox, show bold tags in item's text.
- Demo: In ListBoxBigDataWindow show results with search text in bold and images.
- ListControlItem.LabelFlags.
- ListView: Fixed different exceptions when working with columns and items.
- ListView: Changed HasBorder behavior. This property currently does nothing and ListView is always without the border. 
This is done to prevent bad behavior of platform control on Windows in some situations. In order to have border place ListView 
inside Border control.
- PopupControl: PopupResult, HideOnEnter, HideOnDeactivate.
- AbstractControl: ForEachParent, ForEachVisibleChild, OnBeforeChildKeyDown, OnAfterChildKeyDown, OnBeforeParentKeyDown, 
OnAfterParentKeyDown, OnBeforeParentMouseDown, OnAfterParentMouseDown, OnBeforeParentMouseUp, OnAfterParentMouseUp.
- MenuItem: Fixed Opened/Closed events not raised in some situations.

# 0.9.640 (2024 October 29)

- ToolBarSet is now derived from Border.
- Fixed exceptions on Control.Dispose in some cases.
- Fixed wrong set focus to child when click in control.
- Fixed mouse cursor look when over child controls of UserControl.
- Fixed input char events not raised in some cases. This happened when child control was added and removed from UserControl.
- Fixed NativeObject.SetNativePointer to work more correctly.
- Created HiddenBorder control. Some controls are now derived from HiddenBorder: 
ControlAndLabel, ToolBar, ToolBarSet, PaintActionsControl, PaintActionsControl. So now is possible to show custom border for them.
- Border: Some properties were renamed as many controls will be derived from Border. So we need to have property names in Border 
to be simple to understand in the derived controls.
- Created PopupControl - Allows to implement popups inside client area of the parent control.
- Graphics: DrawTextWithBoldTags, DrawTextWithFontStyle.
- LogListBox: can show custom draw log items.
- Added sample: 'VirtualListBox with BigData'.
- ComboBox: Shadow copy of items is no more stored on recreate in pal control.
- Fixed EnumerableUtils.ConvertItems (last portion of data was not added to result).
- VirtualListBox.SetItemsFast(VirtualListBoxItems value, SetItemsKind kind)
- RectD: static Ceiling, Truncate, Round.
- WindowsUtils.SetWindowRoundCorners.

# 0.9.639 (2024 October 24)

- ListViewColumn: MinAutoWidth, MaxAutoWidth, TitleSize, IsAttached, WidthInPercentToDips.
- ToolBar is now derived from Border so it can be setup to have complex borders.
- ComboBox: New events DropDownClosed, DropDown and related methods.
- ComboBox: New property DroppedDown.
- ComboBox: Fixed wrong painting of owner-drawn items in some configurations.
- ListControlItem.DisplayText - allows to specify display text when painted in VirtualListBox or in ComboBox when OwnerDraw is on.
- ListViewColumnWidthMode.FixedInPercent.
- Used SkiaSharp 3 in the library as previous version had MAUI related issues introduced by the latest Visual Studio update..
- Graphics.RequireMeasure - fast methods to get text measure canvas.
- ToolBar.AddSpeedBtn with KnownButton parameter now uses KnownButtons to get title, image and other information.
- RichToolTip complete reimplementation on c# inside the library.
- VirtualListBox: Fixed used wrong svg color when color theme was changed.
- VirtualListBox: SetColorThemeToDefault(), SetColorThemeToDark().
- Documentation samples added to main demo (see it in Samples tab under "Documentation Samples" item).
- Updated documentation to reflect api changes.
- VisualControlState.Selected - new control state.
- HVAlignment: added new methods.
- SpeedButton: When Sticky is True, uses UseThemeForSticky property to get theme for painting.
- Display: SuggestedLargeImages.
- AbstractControl: HasParent, IsRoot, Root.
- Added base classes: BaseObjectWithAttr and BaseObjectWithId. Most of other classes are derived from them.
- Added classes: ListItemDrawable, EnumImages, KnownButtons, LightDarkImage, CachedSvgImage, PanelFormSelector.
- AbstractControl now indirectly derived from BaseObjectWithAttr.
- DebugUtils.RegisterExceptionsLogger - helps to debug MAUI applications.
- Added ControlActivities.KeyboardZoomInOut.
- Made default control font 1 point larger on low dpi. Use Control.DefaultFont instead of Font.Default in most places.
- Stack layout bug fixes. Also added support for HorizontalLayout.Fill when Layout = Horizontal.
- Window: GlobalWindowNotifications, AddGlobalWindowNotification, RemoveGlobalWindowNotification.
- ControlsSample: Zoom in/out. Ctrl+Shift+Plus and Ctrl+Shift+Minus do zoom in/out of the font in the each form.
- Added new svg images to KnownSvgImages.
- DeveloperToolsPanel: Added toolbars.
- Supressed WxWidgets debug message about gesture events.
- WebBrowserSearchWindow now is not modal.
- Fixed application did not exit in some cases when all forms were closed.

# 0.9.638 (2024 October 19)

- Improved ComboBox owner draw methods. So now it can paint ListContolItem in the popup. Added ComboBox.DefaultItemPainter.
- ObjectUniqueId: Optimized. Now it uses UInt64 counter for the ids. And only after UInt64.MaxValue is reached starts to use guid.
- Updated used nuget packages due to security warnings from Microsoft.
- LightDarkColor is now derived from Color and Color now has virtual method which allows to implement complex color theme strategies in the applications.
- IFontAndColor: WithFont, WithForeColor, WithBackColor.
- GenericLabel: Fixed painting on SkiaGraphics.
- New classes: AlignUtils, ObjectWithRecord, MessageBoxSvgs.
- App.SafeWindow, Display.SafeDisplay, ToolBar.DefaultMinItemSize, Color.AsImageSet, Image.FromUrlOrNull, Grid.RowColumnCount, PictureBox.SetIcon.
- Window: SetLocationInRectI, SetLocationInWindow, SetLocationOnDisplay.
- WindowStartLocation: new enum members CenterMainWindow, CenterActiveWindow, ScreenTopRight, ScreenBottomRight.
- Added Spacer generic control. It can be used to occupy space. This is only for layout. Library doesn't create platform control for Spacer so it is much faster to use it comparing to Panel or other platform controls.
- ImmutableObject: Did not raise property change events.
- PaperSizes: A6InDips, A5InDips, A4InDips.
- SystemSettings: IsUsingDarkBackgroundOverride, AppearanceIsDarkOverride, DoInsideTempAppearanceIsDark.
- ImageList: AddFromAssemblyUrl, AddFromUrl.

## Control templates

- Added control templates painting. Related classes: TemplateControl, TemplateUtils, TemplateControls.
- ToolBar.AddPicture with TemplateControl param.

## Point, rectangle, size and thickness

- RectD: InflateVert, InflateHorz, GetCenter, SetCenter, GetFarLocation, SetFarLocation, HasEmptyHeight, HasEmptyWidth, WithLocation, WithEmptySize.
- RectI: WithSize, WithLocation.
- PointD: AsRect, IncLocation.
- SizeD: Shrink, InchesToDips, MillimetersToDips.
- Thickness: WithTop, WithBottom, WithLeft, WithRight.
 
## TransformMatrix 

- Properties are now fields.
- Speed optimizations.
- TransformMatrix is now struct.
- Operators: == and !=.
- New constructor and methods: Equals, Assign, Clone, TransformRect.

## Graphics

- Graphics: Returned DrawText with TextFormat parameter. It is completely rewritten and now is not specific to WxWidgets as implemented inside Alternet.UI using C#.
- Graphics: speed up of Transform, HasTransform
- Fixed Graphics.DrawText bug on Linux when transform is set.
- GraphicsFactory: MeasureCanvasOverride.
- Graphics: DoInsideClipped, PushAndTranslate, DrawFormattedText, PopClip, PushClip and more DrawText overloads.
- DrawingUtils: new methods MeasureText, WrapTextLineToList, WrapTextToList.
- Graphics: FillRectangleAtCenter, DrawPointAtCenter. 
- Font: WithBold, Unscaled, UniqueId.
- Font: Larger now uses SmallerLargerSizeScaleFactor.
- TextFormat: DefaultRecord, Default(), Alignment(value), SuggestWidth(value). Added chained calls for methods.

## Control

- Separated platform control from abstract control. So now we have AbstractControl, GenericControl which are not binded to the os control.
- New properties: MarginTop, MarginBottom, MarginRight, MarginLeft, PaddingTop, PaddingBottom, PaddingLeft, PaddingRight, BoundsInPixels.
- UserControl: BackgroundColor is now used in background painting (previously only brush).
- Control: SetChildrenUseParentBackColor, SetChildrenUseParentForeColor, SetChildrenUseParentFont.
- Fixed: Control.IsBold was not taken into account if UseParentFont = true
- Control.PreviewKeyDown event (like in WinForms) and related methods.
- New properties: IsPlatformControl, LocationInPixels, SizeInPixels, BoundsInPixels, TitleAsObject, ToolTipProvider, IgnoreSuggestedWidth, IgnoreSuggestedSize, HasScaleFactor.
- Control.PerformLayout speed optimization.
- ControlSet: Margin* and Padding* methods.

## RichToolTip

- Now it is derived from Control and shows tooltips inside itself.
- Now it is possible to show tooltip with image without title and message text.
- Improved RichToolTip demo.
- Chained calls now possible for it's methods (they return RichToolTip instance).
- Fixed exception occured when custom colors were specified.
- Specified default tooltip colors as in Windows 11 for light and dark themes.

## Samples

- DrawingSample: Text and Transform pages improved.
- Added control templates usage sample in CustomDrawTestPage.
- Used new DrawText with TextFormat in GraphicsPath demo page.
- Added sample: png loading from resource to ImageList.

---

# 0.9.637 (2024 October 8)

- Window: Fix exception on close if tooltip is shown.
- Control.CaretInfo. Allows to customize internally painted caret.
- Control.LongTap event and related methods.
- MAUI: Fixed scrollbar movement on Android.
- MAUI: Fixed scrollbar touch events handling.
- MAUI: pan and double tap gestures support in ControlView.
- Timer.TickAction, RichToolTip.HideDefault.
- Control: VisualStatesOverride, VisualStates.
- Control: RaiseScroll overload, RaiseScrollPageDown, RaiseScrollPageUp, RaiseScrollPageLeft, RaiseScrollPageRight.
- BaseObject: StaticOptions, UseNamesInToString.
- InteriorDrawable: ElementClick, Scroll events.
- InteriorNotification.SendScrollToControl.
- MAUI: RequireSwipeGesture, OnSwipeRight, OnSwipeLeft, OnSwipeUp, OnSwipeDown, OnSwipeGesture in ControlView.
- DateUtils: Ticks to/from milliseconds.
- KnownSvgImages: ImgCircleFilled, ImgDiamondFilled.
- RectD, RectI: TopLineCenter, BottomLineCenter, set method for Center property.
- SpeedColorButton: ShowColorSelector, ShowColorPopup, ShowColorDialog.

# 0.9.636 (2024 October 4)

- MenuItem: Opened, Closed, Highlighted events.
- TreeView: HitTestLocation, GetNodeAt, GetNodeAtMouseCursor.
- Control: RunKnownAction, RunAfterGotFocus, RunAfterLostFocus, TouchEventsAsMouse.
- Control: Fixed visual state determination in order to make it more compatible with mobile devices.
- Keyboard: IsKeyboardPresent, ToggleKeyboardVisibility, HideKeyboard, ShowKeyboard, IsSoftKeyboardShowing.
- MAUI: Add support for multiple key events on Android.
- MAUI: Fixed default mono font detection.
- ControlView: Added keyboard show/hide methods.
- ImageList.AddSvg, GenericImage.AsImage.
- SizeI, SizeD: MinWidthHeight, MaxWidthHeight.
- ColorSvgImage, MonoSvgImage: FromFile.
- SizeI.SameWidthHeight, Mouse.IsMousePresent, InteriorDrawable.CornerClick, Font.SmallerLargerSizeScaleFactor.

# 0.9.635 (2024 September 29)

- Returned Title property in some controls as they can be used in TabControl instead of TabPage.
- TextBox: SelectionStart, SelectedText, SelectionLength, TextLength.
- Removed TextBox.ValueValidator. Use Control.CharValidator instead of this property. Removed because it was only WxWidgets specific and had very limited functionality.
- App: DeviceType, IsDesktopDevice, IsPhoneDevice, IsTabletDevice.
- MAUI: Renamed SKCanvasViewAdv -> PlatformView.
- MAUI: Added ios and Android to TargetFrameworks of MAUI related libraries and projects.
- MAUI: Fixed MacCatalyst platform detection.
- MAUI: Keyboard events raised on Android.
- MAUI: Fixed key handling on macOs.
- MAUI: Raise scroll thumb events on Android.
- MAUI: Fix border paint in ControlView.
- Control: IsVisible, IsEnabled (aliases to Visible and Enabled).
- Created KnownAssemblies, KnownTypes, LazyStruct{T}, ExceptionUtils.

# 0.9.634 (2024 September 23)

- MAUI: Renamed SkiaContainer -> ControlView.
- MAUI: ControlView.Interior is now created only on demand.
- MAUI: macOs keyboard support added for ControlView.
- Control.CharValidator property and CharValidator class.
- Graphics: HasTransform, HasClip.
- EnumMapping: many new properties and methods.
- Added BitArray64, IntUtils, ULongUtlis.

# 0.9.633 (2024 September 19)

- Added Window.Load event.
- MAUI: Added MacCatalyst to the TargetFrameworks to all MAUI related csproj.
- Added PaintActionsControl.
- Fix exception in ControlSet.Parent method.
- Added async methods to DialogFactory.
- RichTextBox: Ctrl+G opens GoToLine dialog.
- ComboBox: DefaultDisabledTextColor, GetDisabledTextColor()
- ColorComboBox, ColorListBox, SpeedColorButton: DefaultDisabledImageColor, DisabledImageColor, UseDisabledImageColor.
- Control: GrowMinSize method, MinSizeGrowMode property.
- ListControlItem: HideSelection, HideFocusRect. Allow to hide selection and focus rect for the item in VirtualListBox.
- SpeedColorButton: ActionKind, ColorDialog, ColorImageSize -> ColorImageSizeDips, DefaultColorImageSize -> DefaultColorImageSizeDips.

# 0.9.632 (2024 September 16)

- Clipboard: AsyncRequired, OnlyText, GetDataObjectAsync, SetDataObjectAsync.
- Added RowColumnIndex struct.
- Control.RowColumn, Color.NameLocalized, Label.WrapToParent().
- ControlSet: SuspendLayout, ResumeLayout, DoInsideLayout methods.
- ControlSet: SuspendLayout is called when multiple layout related operations are performed.
- ColorListBox, ColorComboBox: Default colors are sorted by localized name.
- ColorListBox, ColorComboBox: Use localized color names if they are specified.
- ColorListBox, ColorComboBox: AddColor, CreateItem, AddColors, Find, FindOrAdd.

# 0.9.631 (2024 September 12)

- MAUI platform: Fixed SkiaContainer was focused on mouse over and not on mouse down.
- MAUI platform: Clipboard handler.
- MAUI platform: Mouse clicks on system scrollbars raise scroll events.
- Add HVAlignment struct and Control.Alignment property.
- Temporary disabled custom mouse cursors on high dpi as they are not properly supported by wxWidgets.
- Marked modal dialog methods as obsolete (MAUI platform doesn't support modal dialogs).
- Add BoolBoxes, NullBoolBoxes, App.IsDesktopOs.

# 0.9.630 (2024 September 9)

- MAUI platform: System scrollbars are now painted for the SkiaContainer.
- MAUI platform: Fixed size changed events not fired.
- Added 'Using SkiaSharp' article in the documentation.
- PropertyGrid: Fixed localized prop names sorting.
- Control is now indirectly derived from BaseComponent.
- MAUI platform: fix exception in DlgGoto in Alternet.Editor.
- All Control descendants: Add new constructor with parameter which allows to specify parent of the control.
- CommonDialog is now derived from BaseComponent.
- ArrayUtils.AreEqual, FileUtils.StringToFileIfChanged, FileUtils.StringToFile, StreamUtils.AreEqual, Font.WithSize.

# 0.9.629 (2024 September 3)

- <u>**SpeedButton.IsClickRepeated.**</u>
- <u>**App.MainWindow.**</u>
- Control: LastClickedTimestamp, ParentDisposed(), UpdateToolTip().
- BaseComponent is now derived from DisposableObject and has Site and Tag properties.
- Timer is now derived from BaseComponent.
- Window: Owner and OwnedWindows properties type changed from IWindow to Window.
- Added classes: ApplicationContext, BrushAndPen, TimerUtils, IntPtrUtils.
- RectD: NotEmptyAndContains, WithHeight, WithWidth, DeflatedWithPadding, InflatedWithPadding, WithX, WithY.
- RectI: WithHeight, WithWidth, WithX, WithY.
- EnumArray: Assign, Clone, SetValues.
- SkiaGraphics: Implemented some of the drawing methods which previosly throwed NotImplementedException.
- Continued implementation of the generic scrollbar control.
- SkiaGraphics: Fix image paint for Alternet.Editor.
- Control: Simplified methods related to scrolling. Now implementing custom scrolling behavior is much easier.
- Added attribute classes: BaseAtrtribute, BaseValueAttribute, BaseBoolAttribute, IsTextLocalizedAttribute, 
IsTitleLocalizedAttribute, IsToolTipLocalizedAttribute.
- Continued preparations for ControlsSample localization.

# 0.9.628 (2024 August 29)

- Control: Added subscription to notifications. You can use AddGlobalNotification, RemoveGlobalNotification, 
AddNotification, RemoveNotification methods for this.
- Control: Notifies subscribers (in GlobalNotifications and Notifications) about it's events.
- Control: Added new scrollbars related methods and properties: HorzScrollBarPosition, VertScrollBarPosition, 
GetScrollBarPosition, SetScrollBarPosition. Control now sends scrollbar related notifications to subscribers.
- Fixed Control.RaiseMouseMove behavior (not always called OnMouseMove).
- Maui platform: Added svg images support.
- Fixed overflow exception in ImageSet to Image conversion on Maui platform.
- Fixed exception in SvgImage.ImageSetWithColor.
- SkiaGraphics: Implemented many different drawing methods which previosly throwed NotImplementedException.
- Added BrushOrColor, BorderDrawable, InteriorDrawable, IControlNotification, ControlNotification, EnumArray, 
SkiaColorFilter classes and interfaces.
- Continued implementation of the generic scrollbar control painting.
- Improved immutable support for Image and ImageSet. SvgImage now returns immutable images.
- SkiaGraphics: InterpolationMode property is now implemented and used in DrawImage methods.
- Graphics: GetPixel removed as not supported in SkiaSharp, use pixel access implemented in the Image and GenericImage.
- SkiaRegionHandler: Fixed exception in one of the constructors.
- Added Bitmap.Empty, SizeD.Abs, SizeI.Abs, RectD.Scale, ImmutableObject.SetImmutable, Rect.GetCircleBoundingBox.

# 0.9.627 (2024 August 26)

- Created portable exe demos for Windows, Linux and MacOs. 
Demos can be downloaded from [here](https://github.com/alternetsoft/AlternetUI/releases/tag/0.9.623-beta).
- Window and it's descendants can be created and used as controls. We have added additional constructors with WindowKind parameter 
and static method ```Window.CreateAs<T>(WindowKind)``` which can override window kind. For example this technology can be used
to allow inserting window as a child of other window or control. 
- Maui: Fix exception when Ctrl+F pressed in [Alternet.Editor](https://www.alternetsoft.com/products/editor).
- Added Drawable classes: BaseDrawable, ImageDrawable, RectangleDrawable.
- Image: SizeDip, BoundsDip methods with scaleFactor param.
- Improvements in Alternet.UI.RunCmd app.
- BoderSettings and BorderSideSettings were derived from ImmutableObject.
- Added classes: SvgImageInfo, EventArgsUtils.
- Made some preparations in the code for the generic scrollbar control painting implementation.

# 0.9.626 (2024 August 22)

- Speed up of the control painting and caret movement on Maui platform.
- Fixed Alternet.Editor related double-scale of the images on Maui.
- Removed old style Control.MousePosition, Mouse.GetPosition() which were without parameters. Now you can use 
Mouse.GetPosition with parameters (control or scale factor). This change is done in order to implement better support for
multiple monitors with different dpi.
- Intergated SkiaSharpSample into ControlsSampleDll.
- Fixed CustomWindowsConsole behavior on Win 7.
- Control.RefreshRectsUnion, SkiaUtils.GetBounds, SkiaGraphics.OriginalScaleFactor.
- SkiaGraphics and SkiaContainer: UseUnscaledDrawImage.
- Alternet.UI.RunCmd project improved (new commands, etc.).

# 0.9.625 (2024 August 18)

- Control: IntAttr, IntFlags, IntFlagsAndAttributes, IsTextLocalized, IsTitleLocalized, IsToolTipLocalized.
- Added 'Download PDF' link to the bottom of the documentation page.
- Updated build scripts so all dlls in the nugets are signed by signtool.
- TwoWayEnumMapping: SourceToDest, DestToSource.
- Added attributes and flags interfaces with id and value type params.
- Factory class splitted into FlagsFactory, AttributesFactory, FlagsAndAttributesFactory. 
Added methods which allow to create flags and attributes with int identifiers.
- FlagsAndAttributes class made public.
- Fixed api info generation.

# 0.9.624 (2024 August 13)

- RichTextBox exception fixed.
- Generated documentation pdf.
- UIActionSimulator: use global MouseButton enum.
- UIActionSimulator: moved to common library and separated from WxWidgets.
- UIActionSimulator: Use ModifierKeys instead of local KeyModifier enum.
- UIActionSimulator: Use Key instead of WxWidgetsKeyCode.

---

Older items can be found [here](Documents/Whatsnew.History/whatsnew-2024.md)