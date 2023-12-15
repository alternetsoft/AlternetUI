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
        private static int? doubleClickTime;

        /// <summary>
        /// Gets the maximum number of milliseconds that can elapse between a first click and
        /// a second click for the OS to consider the mouse action a double-click.
        /// </summary>
        /// <returns>
        /// The maximum amount of time, in milliseconds, that can elapse between a first click
        /// and a second click for the OS to consider the mouse action a double-click.
        /// </returns>
        public static int DoubleClickTime
        {
            get => doubleClickTime ?? SystemSettings.GetMetric(SystemSettingsMetric.DClickMSec);

            set => doubleClickTime = value;
        }

        /// <summary>
        /// Gets the number of lines to scroll when the mouse wheel is rotated.
        /// </summary>
        /// <returns>
        /// The number of lines to scroll on a mouse wheel rotation, or -1 if the
        /// "One screen at a time" mouse option is selected.</returns>
        public static int MouseWheelScrollLines { get; set; } = 3;
    }
}
