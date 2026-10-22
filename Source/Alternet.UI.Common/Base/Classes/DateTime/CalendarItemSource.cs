using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a source for <see cref="CalendarItem"/> objects.
    /// </summary>
    public partial class CalendarItemSource : DisposableObject
    {
        private readonly ListSource<CalendarItem> items = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemSource"/> class.
        /// </summary>
        public CalendarItemSource()
        {
        }

        /// <summary>
        /// Gets a collection of <see cref="CalendarItem"/> objects.
        /// </summary>
        public ListSource<CalendarItem> Items => items;

        /// <inheritdoc/>
        public virtual CalendarItem CreateItem()
        {
            var item = new CalendarItem();
            return item;
        }

        /// <summary>
        /// Adds a <see cref="CalendarItem"/> to the source.
        /// </summary>
        /// <param name="item">The <see cref="CalendarItem"/> to add.</param>
        public virtual void Add(CalendarItem item)
        {
            items.Add(item);
        }
    }
}
