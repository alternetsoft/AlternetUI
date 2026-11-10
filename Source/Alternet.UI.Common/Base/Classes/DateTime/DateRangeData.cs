using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a data structure that holds a range of dates
    /// and associated data of type <typeparamref name="TData"/>.
    /// </summary>
    /// <typeparam name="TData">The type of data associated with each date in the range.</typeparam>
    public class DateRangeData<TData>
    {
        private readonly TData[] items;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeData{TData}"/> class
        /// with the specified minimum and maximum dates.
        /// </summary>
        /// <param name="minDate">The minimum date of the range.</param>
        /// <param name="maxDate">The maximum date of the range.</param>
        public DateRangeData(DateOnly minDate, DateOnly maxDate)
            : this(minDate, maxDate, new TData[DateUtils.DayCount(minDate, maxDate)])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeData{TData}"/> class
        /// with the specified minimum and maximum dates and an array of associated data.
        /// </summary>
        /// <param name="minDate">The minimum date of the range.</param>
        /// <param name="maxDate">The maximum date of the range.</param>
        /// <param name="items">An array of data associated with each date in the range.</param>
        /// <exception cref="ArgumentException">Thrown when the length of the items array
        /// does not match the number of days in the range.</exception>
        public DateRangeData(DateOnly minDate, DateOnly maxDate, TData[] items)
        {
            if (maxDate < minDate)
                (minDate, maxDate) = (maxDate, minDate);

            MinDate = minDate;
            MaxDate = maxDate;

            if (items.Length != DateUtils.DayCount(MinDate, MaxDate))
                throw new ArgumentException(
                    "Items array length must equal the day count.", nameof(items));

            this.items = items;
        }

        /// <summary>
        /// Gets the minimum date of the range.
        /// </summary>
        public DateOnly MinDate { get; }

        /// <summary>
        /// Gets the maximum date of the range.
        /// </summary>
        public DateOnly MaxDate { get; }

        /// <summary>
        /// Gets the number of days in the range.
        /// </summary>
        public int Count => items.Length;

        /// <summary>
        /// Gets the array of data associated with each date in the range.
        /// </summary>
        public TData[] Items => items;

        /// <summary>
        /// Gets or sets the data associated with the specified date in the range.
        /// </summary>
        /// <param name="date">The date for which to get or set the associated data.</param>
        /// <returns>The data associated with the specified date.</returns>
        public TData? this[DateOnly date]
        {
            get
            {
                var index = GetIndex(date);
                if (index == null)
                    return default;
                return items[index.Value];
            }

            set
            {
                if (value == null)
                    return;
                var index = GetIndex(date);
                if (index == null)
                    return;
                items[index.Value] = value;
            }
        }

        /// <summary>
        /// Gets whether the specified date is within the range.
        /// </summary>
        /// <param name="date">The date to check.</param>
        /// <returns>True if the date is within the range; otherwise, false.</returns>
        public bool ContainsDate(DateOnly date)
        {
            return date >= MinDate && date <= MaxDate;
        }

        /// <summary>
        /// Gets the index of the specified date in the range.
        /// </summary>
        /// <param name="date">The date for which to get the index.</param>
        /// <returns>The index of the specified date in the range, or null if the date is outside the range.</returns>
        public int? GetIndex(DateOnly date)
        {
            if (date < MinDate || date > MaxDate)
                return null;

            return DateUtils.DayDifference(MinDate, date);
        }
    }
}
