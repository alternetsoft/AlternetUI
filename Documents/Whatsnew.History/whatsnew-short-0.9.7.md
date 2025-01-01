# Whatsnew 0.9.700

Summary:

- AlterNET UI now supports .NET 9, MAUI 9, and SkiaSharp 3.
- Improved MAUI support across the library alongside with WxWidgets.
- New controls: TextBoxAndButton (TextBox with buttons and image on the right side), RichToolTip (reimplemented inside the library), PopupControl (popups inside the parent control), and others.
- Generic controls (can be used as parts of native controls and are handled internally in the library without operating system resources allocation) and Template controls that can be rendered to Graphics or Bitmap.
- VirtualListView: Improved painting in owner-draw mode, with different methods for thread-safe operations that allow fast loading of a large number of items.

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

