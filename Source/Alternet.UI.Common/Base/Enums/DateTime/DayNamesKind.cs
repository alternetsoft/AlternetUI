using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the kind of day names to be used in date-related controls.
    /// </summary>
    public enum DayNamesKind
    {
        /// <summary>
        /// Represents the full names of the days of the week (e.g., "Monday", "Tuesday").
        /// </summary>
        Full,

        /// <summary>
        /// Represents the abbreviated names of the days of the week (e.g., "Mon", "Tue").
        /// </summary>
        Abbreviated,

        /// <summary>
        /// Represents the shortest unique names of the days of the week (e.g., "M", "T").
        /// </summary>
        Shortest,
    }
}
