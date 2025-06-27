using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI;

/// <summary>
///  Specifies the different high DPI modes.
/// </summary>
/// <remarks>
///  <para>
///  Specifying the high DPI mode is dependent on the OS version.
///  Setting the high DPI mode will work on Windows 10 version 1703 or later.
///  </para>
///  <para>
///  Changing the DPI mode while the application is running doesn't impact
///  scaling if you're using the `PerMonitor` value.
///  If there is more than one monitor attached and their DPI settings are different,
///  the DPI may change when the window is moved from one monitor to the other.
///  In this case, the application rescales according to the new monitor's DPI settings.
///  Alternatively, the DPI can be changed when the OS scaling setting
///  is changed for the monitor the window is on.
///  </para>
/// </remarks>
public enum HighDpiMode
{
    /// <summary>
    /// Does not scale for DPI changes and always assumes a scale factor of 100%.
    /// </summary>
    DpiUnaware,

    /// <summary>
    /// Queries for the DPI of the primary monitor once and uses this for the
    /// application on all monitors.
    /// </summary>
    SystemAware,

    /// <summary>
    /// Checks for DPI when it's created and adjusts scale factor when the DPI changes.
    /// </summary>
    PerMonitor,

    /// <summary>
    /// Similar to <see cref="PerMonitor"/>, but enables child window DPI change
    /// notification and improved scaling.
    /// </summary>
    PerMonitorV2,

    /// <summary>
    ///  Similar to <see cref="DpiUnaware"/>, but improves the quality of graphic content.
    /// </summary>
    DpiUnawareGdiScaled,
}