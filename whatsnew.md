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

# 1.0.1 (2026 February 23)

- Color: Use ToDisplayString() instead of NameLocalized in list controls, speed button, list utilities, and other places.
- Color: ToDisplayString() now returns a non-nullable string.
- Use ElementContentAlign in Button.
- StdTreeView: Restore scroll position after removing selected items.
- Demo: Fix WebBrowser sample.
- Coerce color before assigning in SpeedColorButton so it will be synced with popup list box colors.
- CommandLineArgs: Add nullable bool parsing.
- Copy DoubleClickAction in ListControlItem Assign.
- Fixed exception in menu sample.

---

# 1.0.0 (2026 February 20)

- Remove openGL usage as it is not supported on macOs anymore.
- DisposableObject: Add static InstanceDisposed event.
- FrameworkElement: Add two static events (InstanceNameChanged and InstanceLogicalParentChanged).
- The generator now embeds the UI XML content directly into generated C# by emitting a private static readonly string. This speeds up form loading.
- UixmlLoader: Add LoadExistingFromString and LoadExistingEx.
- ControlSet: Add MaxWidthOnSizeChanged and SizeChanged.

---

# 0.9.813 (2026 February 11)

- Image: Add dark-mode image recoloring support (RecolorForDarkModeIfRequired and RecolorForDarkMode methods).
- Derive PictureBox from HiddenGenericBorder. This allows PictureBox to paint optional border around the image if needed.

---

# 0.9.812 (2026 February 6)

- Image: Add ScaleImages and Scale methods.
- Fix print dialog was not shown on Windows 11.
- Make ILogWriter methods return ILogWriter for chaining.
- Introduces IsCurrentPrinting and CurrentDrawingContext properties to expose print operation status and context.
- Refactor PaperSizes to use Create methods and add docs. Enhance PaperSize with custom size support and ToString
- Add Log methods to different print related objects for diagnostics.
- Add extension methods and fluent API to LogWriter (WriteSeparator, WriteLines, WriteKeyValue, BeginSection, EndSection, and WriteException).

---

# 0.9.811 (2026 January 30)

- Fix exeption in PrintPreview dialog.
- Add double-click settings and utility methods to SystemInformation.
- Add MouseEventArgsSnapshot struct for mouse event data.
- SpeedButton: Refactored the Assign method to set a checkmark SVG image when the menu item is checked.
- SpeedButton: Improved image assignment logic to handle SVG and bitmap images more consistently.
- Add ImgCheck property in KnownSvgImages for a check mark icon and UrlImageCheck in KnownSvgUrls.

---

# 0.9.810 (2026 January 19)

- Fix measure text issue in Graphics.
- Add GenericBorder, HiddenGenericBorder controls. They are similar to Border and HiddenBorder but are inherited from GenericControl.
- Add automatic version resource generation to vcxproj.
- Add Windows native version info utility class (MswNativeVersionInfo).
- Control: Add setter for IsMouseCaptured property.
- Add mouse capture override logic to AbstractControl.
- AbstractControl: The IsMouseCaptured property now reflects and controls the override state, ensuring consistent mouse capture behavior.
- Updated constructors in different controls to accept AbstractControl instead of Control as the parent parameter.
- Add Changed event to StdTreeView.
- StdTreeView: Add methods to retrieve columns by name, get multiple columns by names, and set the order of columns by column objects or names.
- Add Name property to ListControlColumn

---

# 0.9.809 (2026 January 15)

- Support multiple columns in StdTreeView and VirtualListBox.
- Add column visibility support to ListBoxHeader.
- Add ColumnDeleted, ColumnInserted, and ColumnSizeChanged events to ListBoxHeader.
- ListBox: Update default tooltip colors for dark mode support.
- Refactor and enhance overlay tooltip logic in VirtualListBox.
- ListControlItem: Introduced ToolTip and IsToolTipVisible properties, allowing items to display tooltips and track their visibility state.
- ListControlItem: Add GetItemText and DefaultGetItemText for flexible item text retrieval with culture support.
- ListControlItem: SafeCell for safe cell access in multi-column items.
- Add MeasureRows method to VirtualListBox.
- VirtualListBox: Extracted and refactored row and empty text painting methods for improved readability and maintainability.
- VirtualListBox: Added properties for empty text foreground color.
- Indent log messages based on opened log sections.
- Maui: Improve context menu handling.
- InnerPopupToolBar: Add option to toggle default min popup width constraint.

---

# 0.9.808 (2026 January 6)

- UserControl: Improve overlay tooltip positioning and fitting logic.
- ToolBar: Updates SpeedButton initialization to inherit parent background and foreground colors.
- ToolBar: Introduced AddItems and SetItems methods to allow adding or replacing ToolBar items using IMenuProperties.
- SpeedButton: Refactor label text color retrieval in SpeedButton. Introduced GetLabelTextColor.
- InnerPopupToolBar: Improved back and fore color handling by setting parent and default colors
- MauiUtils: Add ContextMenuUnderlayColorDark and ContextMenuUnderlayColorLight for overlay fill colors.
- MauiUtils: Refactors ShowContextMenu to support overlay and alignment, and improves context menu display logic.
- MauiUtils: Adds methods for converting client coordinates to parent-relative coordinates.
- MauiUtils: Add HideContextMenus and HideContextMenusInControlView methods to centralize hiding of context menus.
- Add ContainerSizeOverride property to PopupControl.
- Add DefaultMinPopupWidth to InnerPopupToolBar.
- Refactor context menu alignment and host control logic.
- Add NineRectsParts enum and utility methods to NineRects.
- RectI: Setters for SkiaLocation, SkiaSize, Left, Top, Right, Bottom, TopLeft, TopRight, BottomLeft, and BottomRight properties.
- RectD: Add ellipse bounding box and top-right offset methods.
- OverlayToolTipParams: Refactor tooltip location handling with offset support. Add LocationWithOffset and LocationWithoutOffset properties.
- ControlOverlayWithImage: Add image size and bounds properties.
- OverlayToolTipParams: Add ToolTip and AssociatedControl.
- Add CustomAttr property to IMenuItemProperties.
- Add HasMargin property to ToolBarSeparatorItem.
- Menu: Add PrependSeparator and PrependDisabledText methods, allowing insertion of a separator or a disabled text item at the beginning.
- Add BottomRight alignment to HVDropDownAlignment.
- Add Prepend method to ItemContainerElement.

---

Older items can be found [here](Documents/Whatsnew.History/whatsnew-2025.md)