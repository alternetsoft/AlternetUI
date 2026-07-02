## What's New in 1.0.1-1.1.0

Main Improvements:

- New controls: StdListView, StdProgressBar, StdRadioButton, StdCheckBox, StdButton, TransparentPanel, KnobSlider, VerticalLinearGauge, 
GripControl, GripComponent, ResizableWindowBorder, StdIntPicker, EditableListPicker, TextPicker, ShapeControl, ResizableBorder, 
DockedSubPanelContainer, ScrollableCanvasControl.
- Redo painting using SkiaSharp. Do not use WxWidgets graphics by default anymore. Graphics now has Canvas property which is SKCanvas.
- Improved multiple columns support in VirtualListBox and StdTreeView.
- Implemented cell editing in VirtualListBox and StdTreeView.
- Reimplemented ScrollViewer, AnimationPlayer and StatusBar inside the library without using native controls.
- Add ListControlItem.IsVisible and implement item visibility support in VirtualListBox and StdTreeView.
- VirtualListBox: draw vertical and horizontal grid lines painting.
- Add support for Windows on Arm64 and net10.0 on Linux.
- New popup windows: ToolTipWindow, InnerPopupTreeView, ResizablePopupControl, PopupCalculator, InnerPopupTextBox.
- Simplify and speed up Font, Pen, Brush, Region. They do not use native handlers anymore.
- Improved SkiaGraphics and implemented missing features (paths, regions, non-solid pens and brushes).
- Add LayoutManager and ILayoutManager, which allow custom layouts for controls and for classes that support ILayoutItem.
- Continued to improve Maui support in the library. 
- SvgImage: Loading speed up. Optimized to use SKPicture.
- Update to use latest SkiaSharp, Maui And WxWidgets version.
- Improve vsix: Add reopen retry logic and EditorFactory fixes.
- Move input key conversion to managed code (now keys are processed faster).
- ResourceLoader: Add support for data urls through the library. Now it is possible to specify "data:" url prefix.
- Implement optimized string to wxString conversion in managed code to avoid usage of deprecated c++ libraries.
- Calculator new look, fixes and popup.
- Add support for complex tooltips in VirtualListBox, ToolBar and SpeedButton.
- Bug fixes and optimizations.

