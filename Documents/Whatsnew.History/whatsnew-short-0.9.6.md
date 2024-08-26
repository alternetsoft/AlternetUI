Alternet.UI has undergone significant changes to become a truly cross-platform UI framework. Here's what's new:

* <b>Abstraction Layer</b>: The core framework now separates UI control logic from platform-specific rendering. This allows Alternet.UI controls to work seamlessly across different platforms.
* <b>Multiple Platform Support</b>:
    * <b>Current</b>: WxWidgets (Windows, Linux, macOS) remains the default for a native look and feel.
    * <b>Future</b>: Support for MAUI (Windows, Android, iOS, macOs) and Avalonia.UI (platforms to be confirmed) is coming soon. A special SkiaContainer control bridges the gap, allowing you to use Alternet.UI controls within MAUI applications.

<b>Enhanced UI Features:</b>

* <b>Layout Flexibility:</b> A new Layout property for controls allows you to define layout styles like Dock, Basic, Vertical, or Horizontal.
* <b>Improved StackPanel:</b> More control over child element alignment within StackPanels (centered horizontally, various vertical alignment options).
* <b>New Controls</b>: A range of new controls have been added, including VListBox for efficient large item handling, TabControl, Toolbar, and specialized options like ColorComboBox and FontListBox.

<b>Technical Improvements:</b>

* Updated to WxWidgets 3.2.5
* Integrated SkiaSharp graphics library
* Upgraded to .NET Standard 2.0
* Fixed known Linux issues
* Bug fixes and optimizations

These updates make Alternet.UI a more versatile choice for developers building cross-platform desktop applications.