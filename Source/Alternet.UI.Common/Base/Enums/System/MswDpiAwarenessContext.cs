using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the possible DPI awareness contexts for a thread or process.
    /// </summary>
    public enum MswDpiAwareness
    {
        /// <summary>
        /// Invalid DPI awareness context. This value is used to indicate an error or uninitialized state.
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// DPI unaware. The app is always assumed to have a scale factor of 100% (96 DPI).
        /// Windows scales the app automatically when DPI changes.
        /// </summary>
        Unaware = 0,

        /// <summary>
        /// System DPI aware. The app queries DPI once at startup and uses that value for its lifetime.
        /// </summary>
        SystemAware = 1,

        /// <summary>
        /// Per-monitor DPI aware. The app adjusts its scale factor when moved between monitors with different DPI settings.
        /// </summary>
        PerMonitorAware = 2,
    }
}