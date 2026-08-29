using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a daily repeat pattern rule.
    /// </summary>
    public partial class DailyRepeatPatternRule : DateRepeatPatternRule
    {
        private int intervalDays = 2;
        private RepeatKind kind = RepeatKind.EveryDay;

        /// <summary>
        /// Represents the kind of daily repeat pattern.
        /// </summary>
        public enum RepeatKind
        {
            /// <summary>
            /// Repeat every day (default behavior).
            /// </summary>
            EveryDay,

            /// <summary>
            /// Repeat only on even days of the month (2, 4, 6, ...).
            /// </summary>
            EvenDays,

            /// <summary>
            /// Repeat only on odd days of the month (1, 3, 5, ...).
            /// </summary>
            OddDays,

            /// <summary>
            /// Repeat every N days (uses <see cref="IntervalDays"/>).
            /// </summary>
            IntervalDays,

            /// <summary>
            /// Repeat only on weekdays (Mon–Fri).
            /// </summary>
            Weekdays,

            /// <summary>
            /// Repeat only on weekends (Sat–Sun).
            /// </summary>
            Weekends,
        }

        /// <summary>
        /// Gets or sets the kind of daily repeat pattern.
        /// </summary>
        public virtual RepeatKind Kind
        {
            get => kind;
            set
            {
                kind = GetNewFieldValue(kind, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of days between occurrences.
        /// Example: 1 means "every day", 2 means "every 2 days".
        /// </summary>
        public virtual int IntervalDays
        {
            get => intervalDays;
            set
            {
                if (value < 1)
                    value = 1;
                intervalDays = GetNewFieldValue(intervalDays, value);
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<DateOnly> GetDates(DateOnly minDate, DateOnly maxDate)
        {
            CoerceMinMaxDate(ref minDate, ref maxDate);

            if (minDate > maxDate)
                return Array.Empty<DateOnly>();

            var currentDate = minDate;

            switch (Kind)
            {
                case RepeatKind.EveryDay:
                    return GetEveryDay();
                case RepeatKind.EvenDays:
                    return GetDays((d) => d.Day % 2 == 0);
                case RepeatKind.OddDays:
                    return GetDays((d) => d.Day % 2 != 0);
                case RepeatKind.IntervalDays:
                    return GetIntervalDays();
                case RepeatKind.Weekends:
                    return GetWeekDays(DaysOfWeek.Weekend);
                case RepeatKind.Weekdays:
                    return GetWeekDays(DaysOfWeek.Weekdays);
                default:
                    return Array.Empty<DateOnly>();
            }

            IEnumerable<DateOnly> GetIntervalDays()
            {
                var returnedCount = 0;
                var occ = OccurrenceCount;

                while (currentDate <= maxDate)
                {
                    yield return currentDate;
                    currentDate = currentDate.AddDays(intervalDays);
                    returnedCount++;
                    if (occ > 0 && returnedCount >= occ)
                        yield break;
                }
            }

            IEnumerable<DateOnly> GetEveryDay()
            {
                while (currentDate <= maxDate)
                {
                    yield return currentDate;
                    currentDate = currentDate.AddDays(1);
                }
            }

            IEnumerable<DateOnly> GetDays(Func<DateOnly, bool> predicate)
            {
                while (currentDate <= maxDate)
                {
                    if (predicate(currentDate))
                        yield return currentDate;
                    currentDate = currentDate.AddDays(1);
                }
            }

            IEnumerable<DateOnly> GetWeekDays(DaysOfWeek days)
            {
                return GetDays((d) => days.HasDay(d.DayOfWeek));
            }
        }
    }
}
