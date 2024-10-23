using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains control activities.
    /// </summary>
    public static class ControlActivities
    {
        private static ZoomControlActivity? keyboardZoomInOut;

        /// <summary>
        /// Gets or sets default <see cref="ZoomControlActivity"/>.
        /// </summary>
        public static ZoomControlActivity KeyboardZoomInOut
        {
            get
            {
                return keyboardZoomInOut ??= new();
            }

            set
            {
                keyboardZoomInOut = value;
            }
        }
    }
}
