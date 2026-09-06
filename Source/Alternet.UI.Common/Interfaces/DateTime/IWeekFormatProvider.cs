using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines an interface for providing week format information,
    /// including the first day of the week and the calendar week rule.
    /// </summary>
    public interface IWeekFormatProvider
    {
        /// <summary>
        /// Gets or sets the format provider to use for formatting dates, if applicable.
        /// </summary>
        public IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets the calendar week rule to use for determining week boundaries, if applicable.
        /// </summary>
        public CalendarWeekRule? WeekRule { get; set; }

        /// <summary>
        /// Gets or sets the first day of the week to use for determining week numbers.
        /// </summary>
        public DayOfWeek? FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets the effective first day of the week based on the provided
        /// format provider or defaults to the system's culture settings.
        /// </summary>
        /// <param name="weekFormat">The week format provider. If null, the system's culture settings are used.</param>
        /// <returns>The effective first day of the week.</returns>
        public static DayOfWeek EffectiveFirstDayOfWeek(IWeekFormatProvider? weekFormat = null)
        {
            return weekFormat?.FirstDayOfWeek ?? DateUtils.GetFirstDayOfWeek(weekFormat?.FormatProvider);
        }

        /// <summary>
        /// Gets the effective calendar week rule based on the provided
        /// format provider or defaults to the system's culture settings.
        /// </summary>
        /// <param name="weekFormat">The week format provider. If null, the system's culture settings are used.</param>
        /// <returns>The effective calendar week rule.</returns>
        public static CalendarWeekRule EffectiveWeekRule(IWeekFormatProvider? weekFormat = null)
        {
            return weekFormat?.WeekRule ?? DateUtils.GetCalendarWeekRule(weekFormat?.FormatProvider);
        }

        /// <summary>
        /// Gets the effective format info based on the provided format provider or defaults to the system's culture settings.
        /// </summary>
        /// <param name="weekFormat">The week format provider. If null, the system's culture settings are used.</param>
        /// <returns>The effective format info.</returns>
        public static DateTimeFormatInfo EffectiveFormatInfo(IWeekFormatProvider? weekFormat = null)
        {
            return DateUtils.GetFormatInfo(weekFormat?.FormatProvider);
        }

        /// <summary>
        /// Gets the effective week of the year for the specified date,
        /// considering the calendar's week rule and first day of the week.
        /// </summary>
        /// <param name="weekFormat">The week format provider. If null, the system's culture settings are used.</param>
        /// <param name="date">The date for which to get the week of the year.</param>
        /// <returns>The effective week of the year.</returns>
        public static int EffectiveWeekOfYear(DateOnly date, IWeekFormatProvider? weekFormat = null)
        {
            return DateUtils.GetWeekOfYear(
                date,
                EffectiveFormatInfo(weekFormat),
                EffectiveWeekRule(weekFormat),
                EffectiveFirstDayOfWeek(weekFormat));
        }

        /// <summary>
        /// Gets the start date of the specified week number in the given year,
        /// considering the calendar's week rule and first day of the week.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="weekNumber">The week number.</param>
        /// <param name="weekFormat">The week format provider. If null, the system's culture settings are used.</param>
        /// <returns>The start date of the specified week.</returns>
        public static DateOnly GetStartOfWeek(int year, int weekNumber, IWeekFormatProvider? weekFormat = null)
        {
            return DateUtils.GetStartOfWeek(
                year,
                weekNumber,
                EffectiveWeekRule(weekFormat),
                EffectiveFirstDayOfWeek(weekFormat),
                weekFormat?.FormatProvider);
        }
    }
}
