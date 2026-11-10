using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a collection of date ranges, where each range is associated with a collection of items
    /// of type <typeparamref name="TItem"/>.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class DateRangeCollection<TItem> : DateRangeData<List<TItem>?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeCollection{TItem}"/> class
        /// </summary>
        /// <param name="minDate">The minimum date of the range.</param>
        /// <param name="maxDate">The maximum date of the range.</param>
        public DateRangeCollection(DateOnly minDate, DateOnly maxDate)
            : base(minDate, maxDate)
        {
        }

        /// <summary>
        /// Adds an item to the list associated with the specified date.
        /// </summary>
        /// <param name="date">The date to associate the item with.</param>
        /// <param name="item">The item to add.</param>
        /// <returns>True if the item was added successfully; otherwise, false.</returns>
        public bool AddItem(DateOnly date, TItem item)
        {
            if (date < MinDate || date > MaxDate)
                return false;
            var index = DateUtils.DayCount(MinDate, date);
            var list = Items[index];
            
            if (list == null)
            {
                list = new List<TItem>();
                Items[index] = list;
            }

            list.Add(item);

            return true;
        }
    }

    /// <summary>
    /// Represents a list of date ranges, where each range is associated
    /// with a list of <see cref="CalendarItem"/> objects.
    /// </summary>
    public class CalendarItemsByDate : DateRangeCollection<CalendarItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemsByDate"/> class
        /// with the specified minimum and maximum dates.
        /// </summary>
        /// <param name="minDate">The minimum date of the range.</param>
        /// <param name="maxDate">The maximum date of the range.</param>
        public CalendarItemsByDate(DateOnly minDate, DateOnly maxDate)
            : base(minDate, maxDate)
        {
        }
    }
}
