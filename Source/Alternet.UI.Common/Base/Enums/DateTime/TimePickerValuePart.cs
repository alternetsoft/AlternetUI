using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the individual components of a time value that
    /// a time picker control can expose or manipulate.
    /// </summary>
    public enum TimePickerValuePart
    {
        /// <summary>
        /// Represents the hour component of the time value.
        /// </summary>
        Hour,

        /// <summary>
        /// Represents the minute component of the time value.
        /// </summary>
        Minute,

        /// <summary>
        /// Represents the second component of the time value.
        /// </summary>
        Second,

        /// <summary>
        /// Represents the AM/PM designator of the time value in 12-hour format.
        /// </summary>
        AmPm,
    }
}
