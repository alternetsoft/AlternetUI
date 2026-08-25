using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the days of the week, including additional values for day, weekday, and weekend.
    /// </summary>
    public enum ExtendedDayOfWeek
    {
        /// <summary>
        /// Represents Sunday, the first day of the week (0).
        /// </summary>
        Sunday = 0,
     
        /// <summary>
        /// Represents Monday, the second day of the week (1).
        /// </summary>
        Monday = 1,
        
        /// <summary>
        /// Represents Tuesday, the third day of the week (2).
        /// </summary>
        Tuesday = 2,
        
        /// <summary>
        /// Represents Wednesday, the fourth day of the week (3).
        /// </summary>
        Wednesday = 3,
        
        /// <summary>
        /// Represents Thursday, the fifth day of the week (4).
        /// </summary>
        Thursday = 4,
        
        /// <summary>
        /// Represents Friday, the sixth day of the week (5).
        /// </summary>
        Friday = 5,
        
        /// <summary>
        /// Represents Saturday, the seventh day of the week (6).
        /// </summary>
        Saturday = 6,

        /// <summary>
        /// Represents any day of the week (7).
        /// </summary>
        Day = 7,
        
        /// <summary>
        /// Represents a weekday (Monday to Friday) (8).
        /// </summary>
        Weekday = 8,

        /// <summary>
        /// Represents a weekend day (Saturday and Sunday) (9).
        /// </summary>
        Weekend = 9,
    }
}
