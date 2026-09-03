using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a combination of a relative weekday, a day of the week, and a month.
    /// </summary>
    public readonly struct RelativeWeekdayOfMonth
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeWeekdayOfMonth"/>
        /// struct with the specified relative weekday, day of the week, and month.
        /// </summary>
        /// <param name="relativeWeekday">The relative weekday occurrence (e.g., First, Second, Last).</param>
        /// <param name="dayOfWeek">The day of the week (e.g., Monday, Tuesday).</param>
        /// <param name="month">The month (e.g., January, February).</param>
        public RelativeWeekdayOfMonth(RelativeWeekday relativeWeekday, ExtendedDayOfWeek dayOfWeek, CalendarMonth month)
        {
            RelativeWeekday = relativeWeekday;
            DayOfWeek = dayOfWeek;
            Month = month;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeWeekdayOfMonth"/> struct with default values.
        /// </summary>
        public RelativeWeekdayOfMonth()
        {
            RelativeWeekday = RelativeWeekday.First;
            DayOfWeek = ExtendedDayOfWeek.Day;
            Month = CalendarMonth.January;
        }

        /// <summary>
        /// Gets the relative weekday occurrence (e.g., First, Second, Last).
        /// </summary>
        public RelativeWeekday RelativeWeekday { get; }

        /// <summary>
        /// Gets the day of the week (e.g., Monday, Tuesday).
        /// </summary>
        public ExtendedDayOfWeek DayOfWeek { get; }

        /// <summary>
        /// Gets the month (e.g., January, February).
        /// </summary>
        public CalendarMonth Month { get; }

        /// <summary>
        /// Determines whether two <see cref="RelativeWeekdayOfMonth"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="RelativeWeekdayOfMonth"/> instance to compare.</param>
        /// <param name="right">The second <see cref="RelativeWeekdayOfMonth"/> instance to compare.</param>
        /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(RelativeWeekdayOfMonth left, RelativeWeekdayOfMonth right)
        {
            return left.RelativeWeekday == right.RelativeWeekday &&
                   left.DayOfWeek == right.DayOfWeek &&
                   left.Month == right.Month;
        }

        /// <summary>
        /// Determines whether two <see cref="RelativeWeekdayOfMonth"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="RelativeWeekdayOfMonth"/> instance to compare.</param>
        /// <param name="right">The second <see cref="RelativeWeekdayOfMonth"/> instance to compare.</param>
        /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(RelativeWeekdayOfMonth left, RelativeWeekdayOfMonth right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a tuple representation of the <see cref="RelativeWeekdayOfMonth"/> instance,
        /// containing the relative weekday, day of the week, and month.
        /// </summary>
        /// <returns>A tuple containing the relative weekday, day of the week, and month.</returns>
        public (RelativeWeekday RelativeWeekday, ExtendedDayOfWeek DayOfWeek, CalendarMonth Month) AsTuple()
        {
            return (RelativeWeekday, DayOfWeek, Month);
        }

        /// <summary>
        /// Creates a new <see cref="RelativeWeekdayOfMonth"/> instance with the specified month,
        /// using the current instance's relative weekday and day of the week.
        /// </summary>
        /// <param name="month">The month to use.</param>
        /// <returns>A new <see cref="RelativeWeekdayOfMonth"/> instance with the specified month.</returns>
        public RelativeWeekdayOfMonth WithMonth(CalendarMonth month)
        {
            return new RelativeWeekdayOfMonth(RelativeWeekday, DayOfWeek, month);
        }

        /// <summary>
        /// Creates a new <see cref="RelativeWeekdayOfMonth"/> instance with the specified day of the week,
        /// using the current instance's relative weekday and month.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week to use.</param>
        /// <returns>A new <see cref="RelativeWeekdayOfMonth"/> instance with the specified day of the week.</returns>
        public RelativeWeekdayOfMonth WithDayofWeek(ExtendedDayOfWeek dayOfWeek)
        {
            return new RelativeWeekdayOfMonth(RelativeWeekday, dayOfWeek, Month);
        }

        /// <summary>
        /// Creates a new <see cref="RelativeWeekdayOfMonth"/> instance with the specified relative weekday,
        /// using the current instance's day of the week and month.
        /// </summary>
        /// <param name="relativeWeekday">The relative weekday to use.</param>
        /// <returns>A new <see cref="RelativeWeekdayOfMonth"/> instance with the specified relative weekday.</returns>
        public RelativeWeekdayOfMonth WithRelativeWeekday(RelativeWeekday relativeWeekday)
        {
            return new RelativeWeekdayOfMonth(relativeWeekday, DayOfWeek, Month);
        }

        /// <summary>
        /// Gets the date corresponding to the specified year based on the relative weekday, day of the week, and month.
        /// </summary>
        /// <param name="year">The year for which to get the date.</param>
        /// <returns>The date corresponding to the specified year, or <c>null</c> if not found.</returns>
        public DateOnly? GetDate(int year)
        {
            var start = DateUtils.GetFirstDateOfMonth(year, Month);
            var end = DateUtils.GetLastDateOfMonth(year, Month);

            var days = DateUtils.DaysWhere(start, end, DayOfWeek);

            return DateUtils.GetRelativeDay(days, RelativeWeekday);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (RelativeWeekday, DayOfWeek, Month).GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is RelativeWeekdayOfMonth other)
            {
                return this == other;
            }

            return false;
        }
    }
}
