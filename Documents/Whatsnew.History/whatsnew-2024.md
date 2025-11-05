# 0.9.654 (2024 December 31)

- SpeedButton: DefaultPadding, ImageHorizontalAlignment, ImageVerticalAlignmentm LabelHorizontalAlignment, LabelVerticalAlignment.
- SpeedButton: Fixed IsBold, IsUnderlined not worked.
- MenuItem: SvgImage, SvgImageSize.
- Use DisposingOrDisposed in control's members to make their usage more safe.
- Fixed mono font determination for SkiaSharp fonts.
- Add classes: BaseTypeConverter, ListControlEmptyItem (draws nothing), ListControlSeparatorItem (draws horizontal line).
- ListControlItem: Fixed Font determination.
- ListControlItem: DrawBackgroundAction, DrawForegroundAction (now it's easy to add owner-drawn VirtualListBox items by assigning these actions).
- CustomTextBox: Try to use static Parse method when editing custom types if TypeConverter is absent.
- CustomTextBox: AutoShowError.
- TextBoxAndButton: AutoShowError, AutoBackColor, InnerOuterBorder.
- Demo: add sample of DateTime and other custom types editing in TextBox and TextBoxAndButton controls.
- Add StringConverters.RegisterTypeConverter method for registering custom converters for the types. This register is used in CustomTextBox and PropertyGrid in order to get TypeConverter.
- BaseObjectWithAttr and BaseObjectWithId: made thread safe.
- Simplify Invoke, BeginInvoke, EndInvoke methods and make them static members of BaseObject.
- AbstractControl.Text property is now thread safe (Invoke is called in the setter).
- ConversionUtils: Add parse float array methods (ParseFourFloats, ParseTwoFloats, ParseOneOrTwoOrFourFloats) and use them in RectD, SizeD, PointD and Thickness.
- AbstractControl: AvoidException now returns bool.
- ThreadExceptionWindow: Quit button can be optionally hidden.
- LogUtils: LogException now logs one line with info, but user can double click on it for the details (ThreadExceptionWindow dialog will be shown).
- LogUtils: GenNewId is now thread safe
- LogListBox: Show icon next to ListControlItem log items.
- Fix possible exception when Type.GetProperty is called. This includes all classes that use PropertyInfo including PropertyGrid.
- AssemblyUtils: DefaultBindingFlags, CanResetProp, ResetProperty, GetFieldValue, ToggleBoolMember, GetFirstMember, TrySetMemberValue,  TryGetMemberValue, GetPropertySafe.
- AbstractControl: FontStyleOverride (similar to IsBold).
- Controls: Fixed ParentFont property behavior.
- AbstractControl: Use TabIndex when getting next focused control.
- AbstractControl: Fix BackColor and ForeColor were not updated after parent changed in case when ParentBackColor/ParentForeColor is True.
- SpeedButton: Do not use inner platform control for drawing the picture, so now SpeedButton occupy less memory and works faster.
- PropertyGrid: Use TypeConverter when property is loaded or assigned with new value.
- PropertyGrid: Culture property.

# 0.9.653 (2024 December 27)

- Window: Speedup closing.
- CustomTextBox.TextAsValue - uses TypeConverter and other conversion features and settings in order to convert Text to/from Object.
- CustomTextBox: TextAsValueError, TrimTextRules, ObjectToString, InputTypeArgs, InputType, ResetInputSettings().
- TextBoxAndButton, TextBoxAndLabel: Add InputType, InputTypeArgs.
- Window: Returned OwnersCollection property, made Owner property setter more safe.
- Maui: Initialize application handler as soon as possible.
- MauiUtils: Windows, Pages, Controls, ControlViews, GetChildren.
- Maui: ControlView now responds to app theme change event.
- App.IsNetOrCoreApp and remove some #if NETCOREAPP conditionals.
- Created ControlInitializers, TextBoxInitializers, TextBoxInitializeEventArgs.
- Add SystemSettings.SystemColorsChanged event.
- Maui: Reset system colors on app theme change event.
- Maui: Caret color is selected as opposite of back color.
- LightDarkColor: supports IEquatable, == and != operators, LightOrDark(bool isDark) method, GetHashCode, Equals.
- App: IsMaui, AppThreadId, IsAppThread.
- NativeApi.Generator: Now uses enums from Alternet.UI.Interfaces.
- WebBrowser: IsEdgeBackend.
- Disable Edge backend by default for WebBrowser as it is buggy in current wxWidgets version. Use IsEdgeBackendEnabled to toggle the feature.
- ColorStruct: new constructor and conversion operator from int.
- Color: IsLight, Gray100..Gray950.
- StringUtils: TryParseNumberDelegates, TryParseNumberWithDelegates.
- StringUtils: Different Trim overloads.
- KeyGesture: IsValid, ToString.
- KeyConverter.Default, KeyGestureConverter.Default, ModifierKeysConverter.Default.
- KeyGestureConverter: Fixed to work without context parameter.
- Control: Use DisposingOrDisposed in members.
- Control.cpp: Fixed exceptions on destroy.
- PointD, SizeD, RectD: CoerceCoordFunc, CoerceHeight, Coerce with overloads.
- RectD.DefaultMidpointRounding and use it in Round methods of drawing structs.

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

# 0.9.623 (2024 August 10)

- Improved native library load error logging.
- DialogFactory.ShowRunTerminalCommandDlg, AppUtils.OpenTerminalAndRunCommand, ConsoleUtils: CustomConsole, DummyConsole.

# 0.9.622 (2024 August 9)

- Improved native library load error logging. Shows message if Alternet.UI.Pal.dll is not loaded properly.
- Add classes: TwoDimensionalBuffer, CustomWindowsConsole.
- Control: DPI, FirstChildproperty properties.
- Different fixes in sample projects.
- Developer Tools: Actions are sorted.
- DialogFactory.ShowCriticalMessage allows to show message on critical error before app exit.
- renamed VListBox -> VirtualListBox, VCheckListBox -> VirtualCheckListBox.

# 0.9.621 (2024 August 7)

- 7z supported in the installation, so now it downloads required files much faster.
- Fixed Slider.TickStyle = None on Linux.
- Add classes: PaperSizes, CommandLineArgs, DownloadUtils, HttpClientExtensions, StreamExtensions.
- FileListBox: Drives added to the root folder are sorted.
- FileListBox: DriveItemTemplate = "{0}" by default.
- Improved samples.
- Removed warnings.
- Graphics.DrawImageSliced made internal. This is done because it raises exception on macOs and doesn't work properly on Linux.
- net6 -> net8 for installation and utilities as net6 is not supported on modern linux systems. 

# 0.9.620 (2024 August 4)

- Build pal dll for linux on Ubuntu 23 for better compatibility with modern linux systems.
- GroupBox: Text is now the same as Title.
- Fixed error in MouseInput sample.
- UI.Port.* classes and interfaces made internal.

# 0.9.619 (2024 August 3)

- Return internal HashCode class instead of using from Microsoft.Bcl.HashCode as sometimes this nuget was not copied to bin folder.
- Fixed exception in DragAndDrop sample.
- MouseEventArgs.Source.
- Updated documentation.
- App: Calls Debug.WriteLine for native wxWidgets log messages.
- Control: Do not call drop events if AllowDrop = false.
- New methods in LogUtils, AssemblyUtils, LinuxUtils.
- LogUtils: Renamed LotToFileXX -> LogXXToFile.
- Add NativeApiProvider.DebugImportResolver flag and logging of ImportResolver method.
- Add LogUtils.LogNameValueToFile, DebugUtils.IsDebugDefined.
- Add LinuxUtils.NativeMethods class. 

# 0.9.618 (2024 July 30)

- Different fixes in uixml preview.
- Display: Fixed exception in some cases.
- FileListBox: HiddenSpecialFolders, VisibleSpecialFolders can be set now and are correctly handled.
- FileListBox: Added AdditionalSpecialFolders, AddDrivesToRootFolder, DriveItemTemplate.
- FileListBox: Hide some special folders from the root folder.
- FileListBox: If SelectedFolder is assigned to null, root folder is loaded.
- Added XmlUtils class.
- AssemblyUtils: EnumEvents, EnumProps speed up. As a result - PropertyGrid loads items faster.
- PreviewFileSplitted: 2nd panel can be docked to right or bottom.

# 0.9.617 (2024 July 25)

- Added PreviewSample project.
- Removed exception when resource is loaded from dynamic library.
- Fixed AssemblyUtils.GetAssemblyResPrefix.
- Returned Title property in containers.
- Fixed documentation related projects.

# 0.9.616 (2024 July 24)

- Returned sample projects.
- Clipboard: Fixed exception when image is copied to clipboard.
- XamlCompiler: Fixed references from str to typeof.

# 0.9.615 (2024 July 21)

- Added xml documentation.
- PlessGraphics: Removed exception.
- SkiaUtils: default font related improvements.
- RichTextBox: Use enums instead of int in some methods.
- StatusBar: Window -> AttachedTo.

# 0.9.614 (2024 July 15)

- Label: MaxTextWidth, Wrap.
- <u>**Critical**</u>: Fixed bug in Color == operator.
- <u>**Critical**</u>: Fixed os detection.
- Added NamedColors, RegistryUtils, WebUtils classes.
- Cursor: New constructor: Cursor(Image image, int hotSpotX = 0, int hotSpotY = 0).
- Cursor: New constructor: Cursor(GenericImage image, int hotSpotX = 0, int hotSpotY = 0).
- Fixed ImmutableObject.Immutable set method.
- IconSet.FromUrlOrDefault.
- Color.AsPen now returns immutable pens.
- Speedup in string to color convertion.
- Pens class now use Color.AsPen in order to get pen for the color (speedup and less resources).
- Pens: GetPen, TryGetPen.
- Speedup: Name to brush/color/pen now uses the same names dictionary. Brush names dictionary is removed.
- Now it is possible to register name to color mapping. See NamedColors class.

# 0.9.613 (2024 June 25)

- Used netstandard2.0 in the library.
- Fixed bug in splitter movement, get mouse pos from system.
- Window.Icon can be specified in uixml. Example: "embres:RoslynSyntaxParsing.Sample.ico".
- Application.SetUnhandledExceptionModes.
- BaseException, BaseXmlException, ExceptionCreatedEventArgs classes.
- SplittedPanel: TopBottomVisible, LeftRightVisible.
- LogListBox.ShowDebugWelcomeMessage.
- Alternet.UI.Build.Tasks: Support Alternet.Editor in uixml.
- Cursor.AllowCustomCursors.
- Used ScrollBarOrientation instead of ScrollOrientation beacuse it was a duplicate.
- Clipboard.SetDataObject(IDataObject? data, bool copy).
- new properties in DataFormats as in WinForms.
- DataObject.SetText(string textData, TextDataFormat format).

# 0.9.612 (2024 June 21)

- <u>**Critical**</u>: Fix exception when year > 3000.
- Optimized Keys to/from Key conversions.
- SkiaContainer now handles focus and keyboard input.
- ComboBox: SelectedIndexChanged, DropDownStyle.
- Application.StartuPath, CommonDialog.ShowModal, FileDialog.FilterIndex, Button.UseVisualStyleBackColor, ListControl.SelectedIndexAsInt.

# 0.9.611 (2024 June 18)

- <u>**Critical**</u> Updated to use WxWidgets 3.2.5. Fixes different problems on Linux.
- Display: MinScaleFactor, AllDPI, MinDPI, MaxDPI, HasDifferentDPI, BaseDPI, BaseDPIValue.
- Maui platform related improvements.
- EnumMapping class.

# 0.9.610 (2024 June 16)

- <u>**Critical**</u> StatusBar: Fixed StatusBar was not shown in the window.
- <u>**Critical**</u> SystemSettings: Fixed bug in ResetColors().
- <u>**Critical**</u> Control: Fixed Bounds property set method. Now min width or height is always 0, previously it was possible to set negative values, so it caused Gtk exceptions on Linux.
- Demo: Fixed uixml InputTransparent prop related exceptions.
- ScrollBar: SizeFromMetrics(), ArrowBitmapSizeFromMetrics(), ThumbSizeFromMetrics().
- Hide some properties in controls from PropertyGrid.
- Control: Focus related fixes.
- Image.Url property.
- Demo: Fixed doubling of browsable types in PropGrid demo.
- Demo: Fixed 'Add Panel', 'Set Null' actions for StatusBar demo.

# 0.9.609 (2024 June 15)

- Size and Rect: PixelToDip/PixelFromDip.
- Window: DisplayChanged event.
- LogListBox: Fixed scrolling to end when item added.
- TextBox: Supports INotifyDataErrorInfo interface.
- TextBox: ErrorsChanged event.
- TextBox: HasErrors property.
- TextBox: HasErrorEmptyText(), HasErrorMinLength(), HasErrorMaxLength(), GetErrors().
- TextBox: Added new optional 'errorEnumerator' param in RunDefaultValidation and Report* methods.
- ResourceLoader.StreamFromUrlOrDefault.
- GraphicsFactory: CreateMemoryCanvas(Image image), CreateMemoryDC renamed to CreateMemoryCanvas.
- TextBoxAndLabel: Fixed double Init() in constructor.
- ControlAndLabel: supports INotifyDataErrorInfo.
- Control: e.CurrentTarget is correctly assigned in KeyPress event.
- Control: BubbleMouse renamed to InputTransparent as in Maui.
- Control: Simplified Focus related methods/props.
- Control: AbsolutePosition, AllParents.
- Control: INotifyDataErrorInfo is supported.
- Control: HasErrors - returns whether this control or it's child controls have validation errors.
- Control: GetErrors - gets the validation errors for this control and it's child controls.
- Control: ErrorsChanged event - Occurs when the validation errors have changed for this control or it's child controls.
- Font.SizeInDips.
- ScrollBar: DefaultMetrics, Metrics.
- PointD: +/- operators for (PointD, PointD).
- ImageLockMode is used in LockSurface.
- Improvements related to drawing on Skia and using Control in Maui.

# 0.9.608 (2024 June 12)

- Control: DpiChanged event.
- Control: MeasureCanvas optimized (uses global memory dc and is not created for every control).
- Control: Enabled is returned correctly if parent control is disabled.
- Control: GetPixelScaleFactor() -> ScaleFactor.
- Control: IsMouseOver implemented inside Control and not passed to platform.
- Control: Removed SendSizeEvent as not possible to implement on Maui.
- Control: Removed ScreenToDevice, DeviceToScreen as we have similar PixelToDip/PixelFromDip methods.
- Control: Removed BeginIgnoreRecreate/EndIgnoreRecreate as were buggy and confusing, use BeginInit/EndInit.
- Control: Dpi and ScaleFactor are cached.
- Control: IsParentEnabled, IsThisEnabled.
- LogListBox: BoundToApplicationLog can be set.
- Image.BitsFormat.
- NativeApiGenerator: No switch clause is generated if there is only one event.
- Display: Removed exceptions.
- Display: IsOk, MaxScaleFactor, AllScalefactors, speed optimizations.
- UixmlLoader: new static Func properties LoadFromResName, LoadFromStream, ReportLoadException.
- TabControl: TabSizeChanged event.
- Window: Call PerformLayout on DpiChanged event.
- SystemColors: Updated on SystemColorsChanged.
- PlessMouse: LastMousePosition, LastMousePositionChanged.
- GraphicsFactory: GetOrCreateMemoryDC, CreateMemoryDC.
- Graphicsfactory: ScaleFactor param is now optional in all PixelFromDip/PixelToDip and other methods that have it. If it is not specified, Display.MaxScaleFactor is used.
- PointD: MinValue, MacValue, new constructor.
- ARGBValue, RGBAValue structs and PlessSystemSettingsHandler class.

# 0.9.607 (2024 June 10)

- Added SkiaSharp mega demo in ControlsSample.
- Control: SystemColorsChanged event.
- Control: All event calls and other code move out from On* methods to Raise* methods, so now we do not need to call base method when overriding On* methods.
- Control: Added Bubble* methods.
- Control: GetFocusedControl speed optimization.
- Control: static events FocusedControlChanged, HoveredControlChanged
- GenericImage: HasAlpha, HasMask are now properties and not methods (similar to Image).
- GenericImage: LockSurface() which allows to lock SKCanvas.
- Key and mouse events handling speed optimization.
- Mouse and Keyboard classes are now not abstract and have Default property.
- PictureBox is invalidated when Image prop is set.
- Fixed exception in get mouse pos from system (occured in some cases).
- Platformless caret implemented in Control.

# 0.9.606 (2024 June 8)

- Display.Reset, Image.HasMask.
- Display: AllScreens property checks whether display were added/removed from system.
- Control: ForEachChild(action, recursive), ResetMeasureCanvas(), ResetDisplay().
- Window: DpiChanged event is raised when form is moved to another display and it has other dpi.
- Window: Child's Display and MeasureCanvas properties are updated when form is moved to another display.
- Color: Used ColorStruct inside, AsStruct property, speed optimizations.
- Color: Constructors made public, MinMaxRgb method.
- Image.LockSurface - allows to get SKCanvas from Image.
- ImageBitsFormat class.
- GraphicsFactory: Definable converters from color, pen, font to SKPaint.
- GraphicsFactory: NativeBitsFormat, AlphaBitsFormat, GenericBitsFormat.
- GraphicsFactory: ScaleFactorFromDpi, PixelFromDip, PixelToDip for Coord, Point, Rect, Size.
- Simplify and speed up system color to rgb conversion.
- Graphics: GetDPI now returns SizeI.
- Graphics: HorizontalScaleFactor, VerticalScaleFactor, ScaleFactor.
- Graphics: Removed "I" suffix from DrawRotatedTextI, StretchBlitI, BlitI. Added GraphicsUnit optional parameter instead of it.
- Graphics: Removed optional useMask param in DrawImage.
- Graphics: ToDip for Point, Size, Rect.
- Graphics: FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit).
- Added SkiaSharpSampleDll with SkiaSharp samples.

# 0.9.605 (2024 June 5)

- Color: GetRgbValues, WithRed, WithGreen, WithBlue, WithAlpha.
- Color: Optimization of the internal structure (occupy less memory and speed up).
- Color: SkiaColor, State properties.
- Color: Optimization of AsPen and GetAsPen.
- Color: Optimization of Color to SKColor conversion.
- Image and GenericImage: static Create(int width, int height, SKColor[] pixels).
- Image and GenericImage: Correct pixel conversion in case when image has alpha or mask color
- Image: static Create(int width, int height, Color color).
- Image and GenericImage: ConvertToDisabled implemented internally
- Image: ChangeLightness(int ialpha).
- Image: Load methods now understand urls.
- Image: Save to file now uses file system.
- Image: Exceptions are not raised during load/save, only false is returned in case of error.
- GenericImage: Pixels, RgbData, AlphaData properties.
- GenericImage: FillPixels, FillAlphaData, FillRgbData, CreatePixels, CreateRgbData, CreateAlphaData, SetRgbValuesFromPtr, SetAlphaValuesFromPtr.
- GenericImage: Removed GetNativeData, SetNativeData, GetNativeAlphaData, SetNativeAlphaData. Use Pixels, AlphaData, RgbData instead of them.
- GraphicsFactory: Many new create SkPaint methods.
- GraphicsFactory: DefaultScaleQuality, DefaultAntialias
- SkiaUtils: IsFamilySkia, FontFamilies, ResetFonts.
- Font.SkiaMetrics property.
- By default only Skia compatible fonts are available in library.
- FontFamily: SkiaTypeface, IsFixedPitch, IsFixedPitchFontFamily
- New classes: FontListBox, FontNameAndSize, SampleFonts.
- Correct implementation of default fixed pitch font search for maui/skia.
- Included assets required by SkiaSharp in library csproj.
- DialogFactory: AskByte and other methods.
- Impoved installation scripts. Now TargetPlatfroms override can be specified using bool flags (See Source/Version/SampleFrameworksOverride.props).

# 0.9.604 (2024 June 3)

- Fixed exception when maximized window is closed.
- RadioButton: CheckedChanged event is now fired when IsChecked property changed from code.
- Control: LocationChanged event is now fired.
- Window: StateChanged event is now fired.
- Image: ExtensionsForLoad, ExtensionsForSave props.
- Color and RGBValue to/from SKColor conversions.
- GenericImage and Image to/from SKBitmap conversions.
- Font, Brush, Pen to/from Skia converters.
- Brush: Renamed BrushColor property to AsColor.
- Window: IsMaximized, IsMinimized.
- Enum renamed GenericControlState -> VisualControlState.
- Control.CurrentState renamed to VisualState as Window has State property and it was confusing.
- Font.Default and Font.DefaultMono set methods.
- FontFamily: Families, FamiliesNames and FamiliesNamesAscending speedup, types changed to IEnumerable.
- FontFamily: Reset, IsOk.
- FontFamily.IsFamilyValid speedup and implemented on c# internally.
- SystemFonts: All properties are now can be set.
- SystemFonts: Default, DefaultMono, Serif, SansSerif, GetFont, SetFont.
- FontFamily.GetName property.
- Renamed BaseApplication to App.
- SolidBrush: made all constructors public.
- Color.AsBrush now returns immutable brush.
- Brushes: Now uses Color.AsBrush. As a result: speedup and less resources are used.
- GenericImage: Load and constructor now understand urls (previously only filename).
- LogListBox: Ctrl+C (selected items to clipboard).
- Control: Calls to events moved to Raise* methods from On* methods, so now overriding doesn't require calling base.On* method.
- Display.Default.
- Font and Color: AsFillPaint, AsStrokePaint props.
- SkiaGraphics: better DrawText, GetTextExtent.
- GenericImage: Contstructors without IntPtr.
- GenericImage: Added constrcutor with SKColor[] parameter.
- GenericImage: Static methods GetRGBValues, GetAlphaValues, SeparateAlphaData.
- GenericImage: All load file methods now understand urls.
- GenericImage: Save methods now use FileSystem.
- ResourceLoader now uses FileSystem, so it can be redirected by the developer.
- RectI and RectD: static CreateRect(Width, Height).
- ImageSet, ImageList, IconSet improved: Common ancestor, support IImageContainer interface.
- ImageList.ImageSize is now SizeI as in WinForms.

# 0.9.603 (2024 May 30)

- Control: TextChanged event.
- LogListBox: Fixed item repaint if LogReplace.
- Image: Save methods now have optional quality parameter (but currently they are used only in maui port).
- Added some SkiaSharp related features. 
- Alternet.UI.Port namespace is used in all cs files in Port sub-folder.

# 0.9.602 (2024 May 26)

- TransformMatrix reimplemented on c#.
- Graphics: Push and Pop reimplemented on c#.
- Resource url embres protocol now works without assembly name in url.
- WebBrowser: moved xml comments to cs file.
- Fixed csproj files so FrameworksOverride.props now works.
- Add Changed event to IconSet and ImageList.

# 0.9.601 (2024 May 23)

- All controls are now separated from WxWidgets and were moved to Alternet.UI.Common.
- Font: GetNumericWeightOf, CoerceFontParams, GetWeightClosestToNumericValue.
- Font.SizeInPixels is now int.
- Font: different static ToUserString methods.
- Font.Encoding is now FontEncoding (previously was int).
- FontFamily.GetName(GenericFontFamily family).
- Application.Idle is now static event.
- Use KnownSystemColor instead of SystemSettingsColor in all places.
- Speed optimization: CheckBox, ProgressBar, MenuItem, KeyBinding, InputBinding.
- Add touch related enums and event.
- Implemented Default property in all common dialogs.
- Enums moved to UI.Interfaces dll.

# 0.9.600 (2024 May 16)

- Alternet.UI is now fully crossplatform. It was separated from WxWidgets. 
We also started to port Alternet.UI to MAUI and Avalonia.UI. 
It will be possible to use any Alternet.UI.Control descendant with these libraries.
We plan to support at least three platforms: WxWidgets (Window, Linux, MacOs), MAUI (Windows, Android, iOS), Avalonia.UI.
- Added ContainerControl - base class for all container controls.
- Renamed GenericToolbar to ToolBar. Old toolbar worked only with WxWidgets, the new one is crossplatform.
- Speedup of property handling in controls: ComboBox, NumericUpDown, Slider, DateTimePicker, TextBox. 

---

# 0.9.527 (2024 April 4)

- Added FileSystem, DefaultFileSystem classes and IFileSystem interface.
- Control: FileSystem, GetFileSystem().
- Used IFileSystem in FileListBox, Preview* controls.

# 0.9.526 (2024 March 26)

- FileListBox.Sorted.
- PathUtils: PushDirectory, PopDirectory, GetFullPath.
- Resourceloader: Fix relative to full path conversion.
- AppUtils.SegmentCommandLine.
- ListControlItem.CanRemove.
- VListBox: Invalidated when item added or removed.
- VListBox: SelectAll, UnselectAll, SetAllSelected.
- Menu.ForEachItem, MenuItem.EnabledFunc.
- ListBox: HasItems(), HasSelectedItems(), CanRemoveSelectedItem(), RemoveSelectedItem().

# 0.9.525 (2024 March 25)

- Added controls: PreviewFile, PreviewFileSplitted, PreviewInBrowser, PreviewTextFile, PreviewUixml, PreviewUixmlSplitted.
- Added FileListBox control.
- Control.LastDoubleClickTimestamp.

# 0.9.524 (2024 March 22)

- ListBox now calls ListControlItem.DoubleClickAction if it's specified.
- Regress C++ from/to C# NativeObject conversion as did not work properly on Linux.
- Different default Border control colors for dark/light themes.

# 0.9.523 (2024 March 21)

- Speedup C++ from/to C# NativeObject conversion. This increases overall application performance.
- Uixml preview sample. Discussion and screenshot is here: https://github.com/alternetsoft/AlternetUI/discussions/130
- Updated documentation.
- ListControltem: SvgImage, SvgImageSize, DoubleClickAction.
- SvgImage: ImageWithColor, ImageSetWithColor, LoadImage.
- FileListBox, PreviewUixml, HiddenWindow controls.
- Application: new static Invoke* methods similar to Control.Invoke* methods.

# 0.9.522 (2024 March 20)

- VCheckListBox.
- LightDarkColor and LightDarkColors.
- Improved exception handling. Added exceptions catching on non-Windows machines.
- Fixed ignored mouse events on Linux.
- TabControl: changed default border color as it was bad on Macos.
- TabControl: Increase default tab padding. This is done because on normal dpi display distance was too small.

# 0.9.521 (2024 March 16)

- Splitter: improved painting when resize is done.
- Improved log handling. Now it works faster and is more thread safe.
- Changed default color for disabled svg images for light theme.
- Added SvgImage, MonoSvgImage, ColorSvgImage, TwoColorSvgImage.
- TextBox changed default error state image/color. Now error back/fore color is not used by default.
- Fixed uixml and cs in documentation samples.
- Added Clone and NormalBorderAsHovered to all theme related classes.
- SpeedButton: DefaultCustomTheme, CustomTheme, StaticBorderTheme
- SpeedButton.KnownTheme.StaticBorder (new SpeedButton theme with border in the normal state).
- LogUtils: Add optional Kind param to log methods.
- ThreadExceptionWindow made public.
- Application.Run now shows exception dialog when unhandled exception occurs where user can select 
whether to continue or exit the app.
- Application: ThreadExceptionExitCode, LogUnhandledThreadException, UnhandledExceptionMode, UnhandledExceptionModeIfDebugger, 
SetUnhandledExceptionModeIfDebugger, ExitAndTerminate(int exitCode = 0).
- Graphics.DrawText(string text, PointD origin). Uses Control.DefaultFont and Brush.Default for drawing the text.
- Graphics.DrawCheckBox, DrawingUtils.GetCheckBoxSize.
- CustomControlPainter: GetCheckBoxSize, DrawCheckBox, Current.
- VListBox: implemented checkboxes.
- VListBox: CheckOnClick, CheckedIndicesDescending, CheckedIndices, CheckedCount, RemoveCheckedItems, ClearChecked, CheckItems, 
SetItemChecked, SetItemCheckState, CheckedChanged event.
- Control.MeasureCanvas.
- Improved Window resize behavior.
- SplittedTreeAndCards: Now possible to specify kind of the left control (TreeView or VListBox). Added many new props and methods.
- Control.BindScrollEvents.
- VListBox.HScrollBarVisible (not finished).
- Added FromSvgString to Image and ImageSet. These methods are faster than FromSvgStream.

# 0.9.520 (2024 March 13)

- VListBox: SelectionVisible, CurrentItemBorderVisible, TextVisible.
- Added VListBox/ColorListBox samples to ListBoxes tabs in ControlsSample demo.
- BorderSettings: ToGrayScale, ToColor(Color).
- Border side is not painted if color is empty or transparent or not ok.
- ListControlItem.Border.
- VListBox: DefaultCurrentItemBorder, CurrentItemBorder, SelectionBorder.
- DrawingUtils.FillBorderRectangle.
- GenericLabel: Improved painting and layout.
- PopupWindow{T}, PopupListBox{T}, PopupColorListBox.
- SpeedColorButton: PopupWindow, ShowPopupWindow.
- Improved Application.Log related code.

# 0.9.519 (2024 March 11)

- StringSearch.UseContains. So now it's possible to specify whether to use partial text compare during search operations 
in any ListControl descendant (ListControl.Search.UseContains).
- ColorListBox.
- Control.GetUpdateClientRect.
- Many bug fixes, new properties and methods in VListBox. This control is ready to use.
- KnownColorSvgUrls and KnownColorSvgImages.
- Meny new properties in ListControlItem. Now it allows to specify style of the item (font, color, height, etc.). This is used in VListBox.
- Application.Log: Added LogItemKind param.
- LogListBox: shows image near log item.

# 0.9.518 (2024 March 9)

- Image.FromScreen.
- StreamFromUrl now uses Path.GetFullPath #124.
- ResourceLoader.CustomStreamFromUrl event.
- ImageSet constructor with url parameter.
- Fixed Control.HideToolTip.
- UIActionSimulator: Added SendKey which is SendKeyUp+SendKeyDown.
- Added NativeKeyCode from 33 to 126, so, for example, NativeKeyCode.V can be used.
- UIActionSimulator: Added Send__If methods, so it is possible to have send command in the single line without if checks.

# 0.9.517 (2024 March 8)

- ColorComboBox.Value.
- Fixed MenuItem raised multiple Click events. Fixed two problems: If menu item was not on the main level it raised multiple 
Click events. Clicks were logged twice in the MenuSample.
- ListControl.CustomItemText event.
- VListBox: DefaultItemMargin, ItemMargin, DefaultSelectedItemTextColor, SelectedItemTextColor, DefaultItemTextColor, 
ItemTextColor, SelectedItemBackColor, DefaultSelectedItemBackColor, SelectedItemIsBold.
- VListBox: Added virtual methods to get font and color settings for the items.
- VListBox: Added virtual DrawItem and MeasureItemSize.
- NinePatchDrawingWindow: Draw on all screens.

# 0.9.516 (2024 March 5)

- Started VListBox control. This is ListBox descendant which is capable to contain huge number of items.
- Fixed Alternet.UI.Pal library loader.
- Graphics: FromScreen, DrawRotatedTextI, BlitI, StretchBlitI.
- Renamed DrawSlicedImage to DrawImageSliced and defined as Graphics extension #115.
- AppUtils.ExecuteTerminalCommand.
- Add ConsoleUtils class.

# 0.9.515 (2024 March 4)

- TextureBrush.
- RectD: Inflate(), Deflate().
- DrawingUtils: DrawDoubleBorder, DrawSlicedImage.
- Control: AddVerticalGroupBox, AddHorizontalGroupBox.
- ComboBox: DefaultImageVerticalOffset, DefaultImageTextDistance, DefaultImageBorderColor, GetItemImageRect.
- Improved demo layout.
- New Bitmap contructor with url param: Bitmap(string url).
- Graphics: DrawImageI, FillRectangleI.
- Brush.Transparent.
- Image: AsBrush, Bounds.
- Control: AddTabControl, new Group method override.
- Implemented and used converter for GraphicsUnit.
- Improved ColorComboBox painting.

# 0.9.514 (2024 March 2)

- Added ColorComboBox. You can check how it looks in Button page of ControlSample demo.
- SpeedButton.HideToolTipOnClick.
- TabControl.TabsVisible.
- Button.SetImageMargins parameters are now in dips.
- FindReplaceControl: DefaultFindEditBorderColorLight, DefaultFindEditBorderColorDark, DefaultNotFoundBorderLight, 
DefaultNotFoundBorderDark, FindEditBorder, ReplaceEditBorder, FindEditBorderColor.
- Control.GetPreferredSize().
- Image.IsDisposed.
- Graphics: DrawImage now asserts image size #114.
- Demo layout improved.
- Control.MinElementSize and it's used in popup window Ok and Cancel buttons.
- ComboBox owner draw improved.
- ComboBox: OwnerDrawItemBackground, OwnerDrawItem properties. 

# 0.9.513 (2024 February 29)

- SpeedButton shortcuts are now handled in KeyDown.
- ThreadExceptionWindow: used svg image.
- ShortcutInfo class.
- Added 3 color svg images in Resources\ColorSvg: circle-exclamation-blue, circle-xmark-red, triangle-exclamation-yellow.
- ControlSet new methods: Padding, MinSize, MinWidth, MinHeight.
- SpeedButton.LoadSvg(string url, SizeI imageSize)

# 0.9.512 (2024 February 27)

- Implemented owner drawn ComboBox.
- Added IComboBoxItemPainter and ComboBox.ItemPainter.
- Added more ImageSet.GetNormalAndDisabledSvg overrides.

# 0.9.511 (2024 February 26)

- Added UIActionSimulator. This class is used to simulate user 
interface actions such as a mouse click or a key press. Common usage for this class would be to provide 
playback and record (aka macro recording) functionality for users, or to drive unit tests by simulating user sessions. 
This class currently doesn't work when using Wayland with Linux.
- Control: MinChildMargin, AddLabels.
- Fixed bugs with colors on control recreate. This is a big difference in default look of the controls. Previous behavior 
set wrong default colors to some of the controls. Compare, for example, Button look on Windows in this build with old builds.
- ComboBox: HasBorder, EmptyTextHint.
- FindReplaceControl: Used EmptyTextHint in editors.
- ControlList.Items are IListControlItems now.

# 0.9.509 (2024 February 25)

- SimpleSoundPlayer. On Linux requires package osspd: sudo apt-get -y install osspd.
- Add SystemSound and SystemSounds.
- Fixed PropertyGrid.SuggestedInitDefaults() (mostly Linux related)
- ImageSet: New FromSvgStream override capable of loading svg with two different colors.
- ImageSet: Original FromSvgStream doesn't dispose Stream anymore.
- ImageSet: GetNormalAndDisabledSvg.
- Uixml code generator: improve error output.
- Control.Title, so now possible to add any control as TabControl page.
- TabControl: Returned Pages, SelectedPageChanged #107.
- Fixed layout: SuggestedHeight was previously ignored in vertical layout.
- Returned TabControl.SelectedPage.
- #108 MinMasterTemplate.
- Fixed mouse wheel event fired multimple times #103.
- TabControl is repainted when tab page Title changed.
- TabConrol.DefaultMinTabSize.
- SpeedButton: DefaultUseTheme, CustomTheme, UseTheme properties.
- Application.Log fixed exception in some situations.
- TabControl.TabTheme.
- GenericToolBarSet -> ToolBarSet.
- Fixed Display.Name.
- Removed #if FEATURE_WINDOWS_SYSTEM_COLORS.
- Control.AddButtons.
- SupressBell, StopSound.

# 0.9.507 (2024 February 23)

## 2024 February 23

- Hidden AuiManager and other Aui* controls. The reason is that they do not work properly on Linux and MacOs. Also
they are very limited. We suggest using TabControl, SideBarPanel, ToolBar, Splitter, SplittedPanel instead of Aui* controls.
- Hidden Sizer* objects and interfaces. As we introduced Control.Layout property, all Sizer* functionality can be 
implemented through this or using StackPanel, Grid and other layout related containers.
- TabControl: Draw vertical lines between tabs.
- TabControl.SetTabImage.
- TabControl.DisplayRectangle.
- SpeedButton.Text is now displayed if needed.
- Color: Added static events StringToColor, ColorToString, ColorToDisplayString.
- Color: AHex, RGBWeb, ARGBWeb properties.
- Color from tuple (byte, byte, byte) implicit conversion (Calls Color.FromRgb).
- Color from tuple (byte, byte, byte, byte) implicit conversion (Calls Color.FromArgb).
- SpeedColorButton: Now can show color text near color image.
- SpeedColorButton: Events ValueChanged, StringToColor, ColorToString.
- SpeedColorButton: Text property is synced with Value.
- SpeedColorButton: Value is now nullable.
- Fixed SpeedColorButton/SpeedButton behavior if Ebabled=false.
- FindReplaceControl: Fixed incorrect align.
- Control: ParentFont, ParentForeColor, ParentBackColor.
- GenericToolbar.ImageToText.
- SpeedButton: Improved default colors.
- Hidden CardPanelHeader control. The reason it is limited clone of GenericToolBar. You can use TabControl or GenericToolBar instead.
- <u>**Uixml to Cs generator change.**</u>. Now it is not allowed to specify event handlers in the Uixml 
if control has no Name property specified. Previously it was possible but raised an exception in some cases. 
- Display: AllScreens, Bounds, BoundsDip, DeviceName.
- Application.IsRunning.
- ThreadExceptionWindow fixed and improved: Removed exception when its used. Now it is possible to specify whether to
 show Continue button. Layout is improved. Keys Esc and Enter work now. Added localization for buttons.
- UiXml reading improved: Dialog with error information is shown when there are uixml reading errors. 
UixmlLoader.ShowExceptionDialog property.
- Demo improved.

## 2024 February 20

- GenericTabControl -> TabControl.
- Now TabControl works fine under Linux.
- UserPaintControl -> UserControl as in WinForms.
- WinForms compatibility related improvements.
- Fixed: PictureBox/SpeedButton layout.
- PopupPictureBox made public.
- PopupWindow: ShowOkButton, ShowCancelButton.

### AnimationPlayer improved:

- Now it is possible to connect custom animation provider using CreatePlayerDriver function.
- Demo improved (show info, show frame buttons).
- New props: FrameCount, AnimationSize, IsOk.
- New methods: GetDelay, GetFrame.
- IAnimationPlayer interface.

### CardPanelHeader improved:

- event EventHandler? ButtonSizeChanged.
- CardPanelHeaderItem? GetTab(int? index).
- bool RemoveAt(int? index).
- int Insert(int? index, string text, ObjectUniqueId cardId).

## 2024 February 19

- <u>**GenericTabControl.HasInteriorBorder.**</u>.
- Color implicit operator to Brush and Pen.
- PaintEventArgs: Graphics, ClipRectangle.
- Rect.FromLTRB(PointD leftTop, PointD rightBottom).
- DrawingUtils: FillRectangleBorder, FillRectanglesBorder.
- <u>**WebBrowser sample impoved**</u> (Samples\ControlsSample\Pages\Other\WebBrowserPage.uixml.cs). 
Added three sample pages to combobox url: Animated GIF player, MP3 player, WAV player. 

## 2024 February 18

- <u>**Integrated all other samples to ControlsSample**</u>.
- Application: DefaultIcon, IdleLog.
- Control: LayoutData, LayoutProps.
- Threading sample (ControlsSample/InternalSamples/ThreadingSample) improved. Added correct thread cancellation when form is closed.
- Hidden AuiManager sample. Currently we can't suggest to use AuiManager and other Aui* controls. 
After testing it's features and functionality, we found that it works badly under Linux and MacOs. 
Also there are some limitations in the Aui* controls which are critical. Currently we suggest to use:
SplittedPanel as a container for sidebars, SideBarPanel as a simple tabcontrol for sidebars, GenericToolBar as a toolbar.

## 2024 February 16

- IFlagsAndAttributes: this[string name], Flags, Attr.
- #82 PropertyGridSample: sort components (A to Z).
- Add GraphicControl. This is UserPaintControl descendant. By default GraphicControl is not focusable.
- GenericLabel: TextFormat, bug fixes.
- Improved Samples.
- CardPanelHeader.DefaultMinTabSize.
- Control: CustomLayout event, CustomFlags, CustomAttr.
- Control: <u>Dock property is used in all layouts.</u> For example you can if some child of the StackPanel
has Dock=DockStyle.Right, it will be aligned right. All other align rules are applied after docking child controls.
An example is added to LayoutSample/StackPanel.
- Moved sample controls back to demo (FancySlider, FancyProgressBar, TikTacToe). 
- BaseControlItem: Tag, FlagsAndAttributes, FlagsAndAttributes, CustomAttr.

## 2024 February 15

### GenericLabel improvement:

- Can optionally draw Image next to text.
- Can align text.
- Can be painted in different fore and back colors when hovered or has other states.
- Background and border is optionally painted.
- Color? TextBackColor.
- bool ImageVisible.
- int MnemonicCharIndex.
- GenericAlignment TextAlignment.
- string? TextPrefix.
- string? TextSuffix.
- event ImageChanged.
- Image and DisabledImage properties.
- Made not focusable by default.

### Other:

- PictureBox: Padding now is used to specify offsets for the image.
- PictureBox: CenterHorz and CenterVert used when Text is painted.
- Thickness: LeftTop, RightBottom.
- CardPanelHeader: DefaultTabPadding, TabMargin, TabPadding.
- GenericToolBar.SetToolAlignCenter.
- GenericToolBar: ItemSize, DefaultToolBarDistance, ToolBarDistance.
- StackPanel: Horizontally aligned StackPanel can have centered items (item's HorizontalAlignment = Center). See demo in PropertyGridDemo for GenericToolBarSet control.
- SpeedColorButton.ShowDialog.
- PropertyGridSample: Add SpeedColorButton demo.
- PointD, RectD: GetLocation(bool).
- RectD, SizeD: GetSize(bool).
- Control: DefaultOnLayout, DefaultGetPreferredSize.
- Control: ColumnSpan, RowSpan.
- Control: GlobalGetPreferredSize, GlobalOnLayout static events.
- MinMasterTemplate: Add uixml support.
- Add Tests\CustomControlInUixml sample project.
- FindReplaceControl: Use ComboBox instead TextBox as find/replace controls.
- FindReplaceControl: Add find scope editor.
- GenericTabControl.TabAlignment, SelectTab.
- SideBarPanel is now derived from GenericTabControl.
- Font: Fixed AsBold and AsUnderlined. Previosly original font style was completely ignored. Now Bold/Underlined style 
is added to the original font style. For example if font is italic, AsBold property will return both italic and bold font.
- Graphics.DrawLabel.

---

# 0.9.506 (2024 February 14)

## 2024 February 14

### Layout improvement:

Added Control.Layout property. Now it is possible to specify layout method for any control. 
Old way:

```xml
<GroupBox Padding="5" Margin="0,5,0,0">
	<VerticalStackPanel>
		<Label Text="Last Item" Margin="0,0,0,10"/>
		<Button Text="To View" Click="ScrollLastItemIntoViewButton_Click"/>
	</VerticalStackPanel>
</GroupBox>
```

New way:

```xml
<GroupBox Layout="Vertical" Padding="5" Margin="0,5,0,0">
	<Label Text="Last Item" Margin="0,0,0,10"/>
	<Button Text="To View" Click="ScrollLastItemIntoViewButton_Click"/>
</GroupBox>
```

So we do not need to add extra StackPanel or other containers, we can just specify 
Layout="Vertical" in the parent control in order to change default layout style.

Currently these layouts are implemented: None, Dock, Basic, Vertical, Horizontal.

- None: No layout. Use Bounds and Size properties to move/resize child controls.
- Basic: Default layout for the Control. VerticalAlignment, HorizontalAlignment and SuggestedSize properties 
are used to layout the child controls.
- Vertical, Horizontal: StackPanel like alignment. Also opposite bound can be aligned via
VerticalAlignment and HorizontalAlignment properties.
- Dock: Control.Dock property is used to align the child controls.

Control.GetDefaultLayout() virtual method returns default layout in the control. 
For example, Control returns LayoutStyle.Basic, VerticalStackPanel returns LayoutStyle.Vertical.
It allows to specify used layout in case when Layout property is null (by default).

Element 'Fill' was added to VerticalAlignment and HorizontalAlignment enums. It allows to
layout the child control so it will occupy the remaining empty space of the container.

### Other improvements and changes:

- Renamed Toolbar -> ToolBar, Window.Toolbar -> Window.ToolBar.
- Control: ChildRemoved, ChildInserted, ChildVisibleChanged events.
- Window: ToolBar property is now Control.
- Window: StatusBar property is now FrameworkElement.
- LayoutDirection enum and property were renamed to LangDirection.
- Splitter: Now works faster.

## 2024 February 12

- Control: AllChildren, AllChildrenInLayout.
- SideBarPanel.TabAlignment.
- StackPanel: Improved aligment. Now child's VerticalAlignment property (Stretched, Top and Bottom) 
can be used to specify vertical alignment. For the example see SideBarPanel control.
Previously all childs were aligned to top.
- StackPanel: IsVertical, IsHorizontal.
- Improved popup windows behavior on MacOs.
- Control: MouseEnter, MouseLeave now use BubbleMouse.
- PropertyGridSample: Add SideBarPanel demo.
- Updated used Nuget packages #85.

## 2024 February 11

- PopupWindow: Improved behavior, fixed bugs.
- Created samples for PopupCalendar, PopupPropertyGrid, PopupTreeView.
- #82 Hide Text prop in some controls.
- Calendar.HitTest, Application.BeforeNativeLogMessage.

## 2024 February 10

- Improved layout on form resize.
- PopupWindow: Add Ok and Cancel buttons on bottom.
- On Linux popup now uses ShowModal as in this case form is positioned in the center of the screen. 
On some Linux systems window manager doesn't allow to position the form, so all popups will be centered. 
Previously popup forms were shown in strange positions on some Linux systems.
- PopupWindow.ModalPopups.
- PopupWindow: System window border is used instead of Border control.
- PopupWindow: By default popup is now resizable.
- Improved PopupCheckListBox and added it's demo in ControlsSample.
- MouseEventArgs: Fixed Location, X, Y values.
- Documentation: search only when Enter key pressed (so now search in docs is much faster).
- Add Source\Tests\UseLocalPackages project. This is an example on how to have nugets in local folder.
- Control: Right, Bottom props set methods.
- Window.SupressEsc.

---

# 0.9.505 (2024 February 6)

## 2024 February 6

- Controls: Speed up of create and destroy.
- Controls: Speed up of event handling.
- Fixed not found Pal library under some Linux versions.
- Fixed compilation problem under Linux introduced Feb 5.

## 2024 February 5

- PropertyGrid: CanResetProp, ResetProp.
- ColorDialog.Default.
- Control: ProcessException event, AvoidException
- Color.Parse improved: Now conversion is faster and doesn't create ColorConverter instance. Color surrounded with ( ) is 
converted correctly. New Parse method with context and culture params.
- PropertyGrid: Fixed color property paint. Now color image is square.
- Updated documentation.

## 2024 February 2

- Control.BubbleMouse.
- NativeControlPainter class which allows to paint different control parts in the native OS style.
- Fixed installation scripts errors introduced Jan 31.
- Improved documentation web site look.

## 2024 January 31

- Window.Closed event as in WinForms.
- Control: ContainsFocus, HelpRequested, OnHelpRequested.
- Window.KeyPreview.
- Window: AlwaysOnTop -> TopMost (as in WinForms).
- Mouse events like in WinForms.
- LangVersion = 10.0 for some projects.
- Now it is possible to setup Alternet.UI without having Net 8.
- Lowered LangVersion in csproj from 'latest' to 11.0.

## 2024 January 27

- Documentation is now built with the latest DocFx version.

## 2024 January 25

- Added SpeedColorButton, FancyProgressBar, FancySlider, TicTacToeControl controls.
- Added PropertyGrid.PropertyCustomCreate event.
- Border control painting improved.
- Fixed: Use StartLocation only once.
- Fixed #79 (PropertyGridSample resize problem).
- Fixed TabControl resize problem on Linux.
- Fixed Maximize Window problem on Linux. Returned window maximize button for Linux.
- Control: HandleCreated, HandleDestroyed events.
- ComboBoxAndLabel: SelectedItem, SelectedIndex, IsEditable, Items.
- Added documentation samples for some of the controls.
- ListControl, ComboBoxAndLabel: Items set method.

## 2024 January 23

- Added SideBarPanel control. This control can be used in <see cref="SplittedPanel"/> side bars.
- DialogWindow: EscModalResult, EnterModalResult.
- Border: optimized usage of graphics resources.
- Improved documentation (added control pictures and fixed documentation sample projects).
- PropertyGridSample: Added samples for controls (ComboBoxAndLabel, TextBoxAndLabel, SpeedTextButton, GenericToolBarSet).
- Color.IsDark(), TabControl.PageAdded.
- ControlsSample: Autodetect image color in button sample.
- Window: StartLocation is used, fixed #16.
- Fixed UserPaintControl.HasBorder behavior. Now it affects border paint in all cases.
- FindReplaceControl improved.

## 2024 January 20

- RichToolTip improved: Fixed align when title is empty. Now it is possible to show simple tooltip like is done in ToolTip.
 Added ShowSimple static method.
- Modal windows are shown more correctly.
- Added MiniFrameWindow, DialogWindow.
- Activation of the application when modal form is visible is now OK.
- When modal form is closed, previous window is activated OK.
- ShowModal moved to DialogWindow.
- Control: CenterOnParent, SetSizeToContent.
- PanelOkCancelButtons.UseModalResult, SizeD.PositiveInfinity.

## 2024 January 18

- Fixed exception when some keys pressed on Linux.
- FontStyle made compatible with WinForms.
- Fixed uixml preview in VS.
- SystemInformation.WorkingArea.

---

# 0.9.500 (2024 January 15)

This beta focuses on supporting the development of the cross-platform AlterNET Studio Code Editor,
 adding new controls, adding missing properties to the existing controls, fixing known bugs, and improvements.

This is a short list of changes from 0.9.404 to 0.9.500.

- Added new controls: Calendar, ScrollBar, Splitter, AnimationPlayer, MultilineTextBox, RichTextBox,
 GenericLabel, LogListBox, GenericToolBar, ComboBoxAndLabel, SplittedPanel, TextBoxAndLabel,
 RichToolTip, GenericTabControl, FindReplaceControl, SpeedButton and SpeedTextButton.
- TextBox control now supports validating user input.
- Added new classes: ToolTip, Display, SystemSettings, Caret, Cursor, Cursors, and SystemFonts.
- Added HighDpi display support.
- Improved SVG image handling.
- Fixed control flickering on Windows.
- Improved control look and behavior on Linux and macOS.
- Added missing properties and methods to the existing controls.
- Bug fixes and optimizations.

---

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

---

Older items can be found [here](whatsnew-2023.md)