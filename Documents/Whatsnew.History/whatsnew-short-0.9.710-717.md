# What's New in versions 0.9.711–0.9.717

## MAUI:

- Created the following views: `SimpleTabControlView`, `SimpleDialogTitleView`, `LogContentPage`, `EnumPickerView`, `TitleWithTwoButtonsView`, `BaseLogView`, `CollectionLogView`.
- Added `SimpleToolBarView`—a simple toolbar with speed buttons.
- Removed the use of custom `SynchronizationContext`.

## FIXES:

- **Timer:** Fixed an exception when `Start` was called from a non-UI thread.
- **Critical:** Updated `Application.cpp` to use `wxAlternetLog` in all cases. This change suppresses unusual modal dialogs that appeared under certain conditions.
- Improved image loading by using SkiaSharp (#174). This resolves issues with loading PNG images in specific scenarios.
- **SpeedButton and ToolBar:** Resolved `ImageToText` behavior inconsistencies.
- **AbstractControl:** Corrected the cursor update mechanism to refresh on the screen immediately when set (previously, it refreshed only after mouse movement).
- **AbstractControl:** Fixed an exception in `PaintCaret` that occurred in certain cases.
- Corrected `App.DeviceType` determination in non-MAUI applications.
- Fixed the calendar's foreground color in dark themes.
- **Window:** Fixed an exception in the `ModalResult` setter, which was raised in specific situations.

## NEW FEATURES IN CONTROLS:

- Added new controls: `HorizontalLine`, `VerticalLine`.
- **ToolBar:**
  - Added members including `IsVertical`, `SetMargins`, `SetToolContentAlignment`, `SetToolSpacerAlignment`, `SetToolImageAlignment`, and `SetToolTextAlignment`.
  - Fixed layout issues in certain cases.
- **ScrollViewer:** Introduced an optional feature for the mouse wheel to scroll child elements.
- **SpeedButton:** Added `SpacerHorizontalAlignment` and `SpacerVerticalAlignment`.
- **AbstractControl:**
  - Added `OnBeforeChildMouseWheel` and `OnAfterChildMouseWheel`.
  - Introduced methods like `IncHorizontalLayoutOffset` and `IncVerticalLayoutOffset`.
  - Added properties for handling dimensions, such as `InteriorWidth`, `InteriorHeight`, `InteriorSize`, `WidthAndMargin`, `HeightAndMargin`, and `SizeAndMargin`.
- **PanelSettings:** Added method `AddHorizontalLine`.

## OTHER

- **SvgImageDataKind:** Introduced `PreloadUrl` for preloading SVG images in the constructor.
- **App:**
  - Added `AddBackgroundTask` and `AddBackgroundAction`.
  - Implemented `BackgroundTaskQueue` and `BackgroundWorkManager`.
- Updated `Alternet.UI.csproj` to target `netstandard2.0` (#173).
