using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for a server that manages <see cref="CalendarItem"/> objects.
    /// </summary>
    public interface ICalendarItemServer : IListSource<CalendarItem>
    {
    }
}
