## What's New in 1.0.1-1.1.0

**Main Improvements:**

- New controls: StdListView, StdProgressBar, StdRadioButton, StdCheckBox, StdButton, TransparentPanel, KnobSlider, 
VerticalLinearGauge, GripControl, GripComponent, ResizableWindowBorder, StdIntPicker, EditableListPicker, TextPicker, 
ShapeControl, ResizableBorder, DockedSubPanelContainer, and ScrollableCanvasControl.
- Reworked painting using SkiaSharp. WxWidgets graphics are no longer used by default. Graphics now expose a `Canvas` property of type `SKCanvas`.
- Improved multi-column support in VirtualListBox and StdTreeView.
- Added cell editing in VirtualListBox and StdTreeView.
- Reimplemented ScrollViewer, AnimationPlayer, and StatusBar within the library, without native controls.
- Added `ListControlItem.IsVisible` and implemented item visibility in VirtualListBox and StdTreeView.
- VirtualListBox: added vertical and horizontal grid line rendering.
- Added support for Windows on Arm64 and .NET 10.0 on Linux.
- New popup windows: ToolTipWindow, InnerPopupTreeView, ResizablePopupControl, PopupCalculator, and InnerPopupTextBox.
- Simplified and accelerated Font, Pen, Brush, and Region. They no longer use native handlers.
- Improved SkiaGraphics and implemented missing features (paths, regions, non-solid pens and brushes).
- Added LayoutManager and ILayoutManager, enabling custom layouts for controls and classes that support ILayoutItem.
- Continued improvements to MAUI support in the library.
- SvgImage: improved loading speed, optimized to use SKPicture.
- Updated to the latest versions of SkiaSharp, MAUI, and WxWidgets.
- VSIX improvements: added reopen retry logic and EditorFactory fixes.
- Moved input key conversion to managed code (keys are now processed faster).
- ResourceLoader: added support for `data:` URLs. You can now specify a `"data:"` prefix.
- Implemented optimized string-to-wxString conversion in managed code to avoid deprecated C++ libraries.
- Calculator: new look, fixes, and popup support.
- Added support for complex tooltips in VirtualListBox, ToolBar, and SpeedButton.
- Various bug fixes and optimizations.
