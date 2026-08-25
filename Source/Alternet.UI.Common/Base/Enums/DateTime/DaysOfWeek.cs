using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies days of the week that can be combined using bitwise operations.
    /// </summary>
    [Flags]
    public enum DaysOfWeek
    {
        /// <summary>
        /// Represents no days of the week.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents Sunday.
        /// </summary>
        Sunday = 1,

        /// <summary>
        /// Represents Monday.
        /// </summary>
        Monday = 2,

        /// <summary>
        /// Represents Tuesday.
        /// </summary>
        Tuesday = 4,

        /// <summary>
        /// Represents Wednesday.
        /// </summary>
        Wednesday = 8,

        /// <summary>
        /// Represents Thursday.
        /// </summary>
        Thursday = 16,

        /// <summary>
        /// Represents Friday.
        /// </summary>
        Friday = 32,

        /// <summary>
        /// Represents Saturday.
        /// </summary>
        Saturday = 64,

        /// <summary>
        /// Represents all days of the week (Sunday through Saturday).
        /// </summary>
        All = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday,

        /// <summary>
        /// Represents the weekdays (Monday through Friday).
        /// </summary>
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,

        /// <summary>
        /// Represents the weekend days (Sunday and Saturday).
        /// </summary>
        Weekend = Sunday | Saturday,
    }
}
