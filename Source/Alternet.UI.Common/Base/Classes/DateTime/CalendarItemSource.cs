using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a source for <see cref="CalendarItem"/> objects.
    /// </summary>
    public partial class CalendarItemSource : ListSource<CalendarItem>, ICalendarItemSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemSource"/> class.
        /// </summary>
        public CalendarItemSource()
        {
        }
    }
}
