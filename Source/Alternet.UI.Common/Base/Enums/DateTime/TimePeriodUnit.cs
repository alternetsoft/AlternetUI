using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the unit of measurement for a time period.
    /// Used together with an integer value to represent durations
    /// such as "3 Months" or "2 Hours".
    /// </summary>
    public enum TimePeriodUnit
    {
        /// <summary>
        /// Represents a period measured in years.
        /// </summary>
        Years,

        /// <summary>
        /// Represents a period measured in months.
        /// </summary>
        Months,

        /// <summary>
        /// Represents a period measured in weeks.
        /// </summary>
        Weeks,

        /// <summary>
        /// Represents a period measured in days.
        /// </summary>
        Days,

        /// <summary>
        /// Represents a period measured in hours.
        /// </summary>
        Hours,

        /// <summary>
        /// Represents a period measured in minutes.
        /// </summary>
        Minutes,

        /// <summary>
        /// Represents a period measured in seconds.
        /// </summary>
        Seconds,

        /// <summary>
        /// Represents a period measured in milliseconds.
        /// </summary>
        Milliseconds,
    }
}
