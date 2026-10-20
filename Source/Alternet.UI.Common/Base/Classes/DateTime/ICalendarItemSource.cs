using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for a source that manages <see cref="CalendarItem"/> objects.
    /// </summary>
    public interface ICalendarItemSource : IListSource<CalendarItem>
    {
        /// <summary>
        /// Creates a new calendar item and adds it to the source.
        /// The new item is initialized with default values.
        /// </summary>
        /// <returns>A new instance of <see cref="CalendarItem"/>.</returns>
        public CalendarItem CreateItem();
    }
}
