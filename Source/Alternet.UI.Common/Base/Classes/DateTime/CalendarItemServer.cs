using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a server for <see cref="CalendarItem"/> objects.
    /// </summary>
    public partial class CalendarItemServer : ListSource<CalendarItem>, ICalendarItemServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemServer"/> class.
        /// </summary>
        public CalendarItemServer()
        {
        }
    }
}
