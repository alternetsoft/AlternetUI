using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the relative occurrence of a weekday within a month.
    /// Used to represent rules like "first Monday" or "last Friday".
    /// </summary>
    public enum RelativeWeekday
    {
        /// <summary>
        /// The first occurrence of the specified weekday in a month.
        /// </summary>
        First,

        /// <summary>
        /// The second occurrence of the specified weekday in a month.
        /// </summary>
        Second,

        /// <summary>
        /// The third occurrence of the specified weekday in a month.
        /// </summary>
        Third,

        /// <summary>
        /// The fourth occurrence of the specified weekday in a month.
        /// </summary>
        Fourth,

        /// <summary>
        /// The fifth occurrence of the specified weekday in a month.
        /// </summary>
        Fifth,

        /// <summary>
        /// The last occurrence of the specified weekday in a month.
        /// </summary>
        Last,

        /// <summary>
        /// Represents every occurrence of the specified weekday in a month.
        /// </summary>
        Every,

        /// <summary>
        /// Represents only the odd occurrences of the specified weekday in a month.
        /// </summary>
        Odd,

        /// <summary>
        /// Represents only the even occurrences of the specified weekday in a month.
        /// </summary>
        Even,

        /// <summary>
        /// Represents the penultimate (second to last) occurrence of the specified weekday in a month.
        /// </summary>
        Penultimate,

        /// <summary>
        /// Represents the middle occurrence of the specified weekday in a month (e.g., 2nd or 3rd depending on month length).
        /// </summary>
        Middle,
    }
}
