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
        /// The last occurrence of the specified weekday in a month.
        /// </summary>
        Last
    }
}
