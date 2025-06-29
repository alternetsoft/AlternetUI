using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the hour format for the <c>TimePicker</c> and other controls.
    /// </summary>
    public enum TimePickerHourFormat
    {
        /// <summary>
        /// Use operating system settings.
        /// </summary>
        System,

        /// <summary>
        /// 12-hour format (AM/PM).
        /// </summary>
        Hour12,

        /// <summary>
        /// 24-hour format.
        /// </summary>
        Hour24,
    }
}
