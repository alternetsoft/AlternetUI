using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides information about the current system environment.
    /// </summary>
    public static class SystemInformation
    {
        /// <summary>
        /// Gets the maximum number of milliseconds that can elapse between a first click and
        /// a second click for the OS to consider the mouse action a double-click.
        /// </summary>
        /// <returns>
        /// The maximum amount of time, in milliseconds, that can elapse between a first click
        /// and a second click for the OS to consider the mouse action a double-click.
        /// </returns>
        public static int DoubleClickTime => SystemSettings.GetMetric(SystemSettingsMetric.DClickMSec);
    }
}
