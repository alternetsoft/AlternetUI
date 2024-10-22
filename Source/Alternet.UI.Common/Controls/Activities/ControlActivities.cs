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
        private static ControlActivityZoom? keyboardZoomInOut;

        /// <summary>
        /// Gets or sets default <see cref="ControlActivityZoom"/>.
        /// </summary>
        public static ControlActivityZoom KeyboardZoomInOut
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
