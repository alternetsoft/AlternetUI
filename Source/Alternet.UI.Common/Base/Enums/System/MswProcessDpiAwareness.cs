using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI;

/// <summary>
/// Specifies the different high DPI modes.
/// </summary>
/// <remarks>
/// <para>
/// Specifying the high DPI mode is dependent on the OS version.
/// Setting the high DPI mode will work on Windows 10 version 1703 or later.
/// </para>
/// <para>
/// Changing the DPI mode while the application is running doesn't impact
/// scaling if you're using the `PerMonitor` value.
/// If there is more than one monitor attached and their DPI settings are different,
/// the DPI may change when the window is moved from one monitor to the other.
/// In this case, the application rescales according to the new monitor's DPI settings.
/// Alternatively, the DPI can be changed when the OS scaling setting
/// is changed for the monitor the window is on.
/// </para>
/// </remarks>
public enum MswProcessDpiAwareness : int
{
    /// <summary>
    /// DPI unaware. This process does not scale for DPI changes and is always assumed to have a scale factor
    /// of 100% (96 DPI). It will be automatically scaled by the system on any other DPI setting.
    /// </summary>
    Unaware = 0,

    /// <summary>
    /// System DPI aware. This process does not scale for DPI changes. It will query for the DPI once and use that value
    /// for the lifetime of the process.If the DPI changes, the process will not adjust to the new DPI value.
    /// It will be automatically scaled up or down by the system when the DPI changes from the system value.
    /// </summary>
    SystemAware = 1,

    /// <summary>
    /// Per monitor DPI aware. This process checks for the DPI when it is created and adjusts the scale factor whenever the DPI changes.
    /// These processes are not automatically scaled by the system.
    /// </summary>
    PerMonitorAware = 2,

}