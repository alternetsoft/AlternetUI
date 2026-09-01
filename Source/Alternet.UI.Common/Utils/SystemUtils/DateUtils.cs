using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to <see cref="DateTime"/>.
    /// </summary>
    public static class DateUtils
    {
        /// <summary>Specifies the maximum date value of the
        /// <see cref="DateTimePicker"/> and other date editors.
        /// This field is read-only.</summary>
        public static readonly DateTime MaxDateTime = new(9998, 12, 31);

        /// <summary>Gets the minimum date value of the
        /// <see cref="DateTimePicker"/> and other date editors.</summary>
        public static readonly DateTime MinDateTime = new(1753, 1, 1);

        /// <summary>
        /// Represents 12:00 AM (midnight) on the minimum date.
        /// </summary>
        public static readonly DateTime MinimumDateMidnight = DateTime.MinValue.Date.AddHours(0);

        /// <summary>
        /// Represents 12:00 PM (noon) on the minimum date.
        /// </summary>
        public static readonly DateTime MinimumDateNoon = DateTime.MinValue.Date.AddHours(12);

        /// <summary>
        /// Gets or sets the designator override for AM/PM formatting in time representations.
        /// </summary>
        /// <remarks>This property allows customization of the AM designator used in date and time
        /// formatting. If set to null (default value), the default designator
        /// will be used based on the current culture
        /// settings.</remarks>
        public static string? AmDesignatorOverride;

        /// <summary>
        /// Gets or sets the designator override for PM formatting in time representations.
        /// </summary>
        /// <remarks>This property allows customization of the PM designator used in date and time
        /// formatting. If set to null (default value), the default designator
        /// will be used based on the current culture
        /// settings.</remarks>
        public static string? PmDesignatorOverride;
        
        private static DayOfWeek? systemFirstDayOfWeek;

        /// <summary>Gets the maximum date value allowed for the control.</summary>
        /// <returns>A <see cref="System.DateTime" /> representing the
        /// maximum date value for the control.</returns>
        public static DateTime MaximumDateTime
        {
            get
            {
                DateTime maxSupportedDateTime =
                    CultureInfo.CurrentCulture.Calendar.MaxSupportedDateTime;
                if (maxSupportedDateTime.Year > MaxDateTime.Year)
                {
                    return MaxDateTime;
                }

                return maxSupportedDateTime;
            }
        }

        /// <summary>Gets the minimum date value allowed for the control.</summary>
        /// <returns>A <see cref="System.DateTime"/> representing the
        /// minimum date value for the control.</returns>
        public static DateTime MinimumDateTime
        {
            get
            {
                DateTime minSupportedDateTime =
                    CultureInfo.CurrentCulture.Calendar.MinSupportedDateTime;
                if (minSupportedDateTime.Year < MinDateTime.Year)
                {
                    return MinDateTime;
                }

                return minSupportedDateTime;
            }
        }

        /// <summary>
        /// Gets <see cref="DateTime"/> format used in JavaScript
        /// or in other situations.
        /// </summary>
        public static string DateFormatJs { get; set; } = "yyyy-MM-ddTHH:mm:ss.fffK";

        /// <summary>
        /// Gets the first day of the week according to the current culture's settings.
        /// You can set this property to override the default behavior and specify a custom first day of the week.
        /// </summary>
        public static DayOfWeek SystemFirstDayOfWeek
        {
            get
            {
                return systemFirstDayOfWeek ?? System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
            }

            set
            {
                systemFirstDayOfWeek = value;
            }
        }

        /// <summary>
        /// Returns the culture-specific AM or PM designator for the specified date and time value.
        /// </summary>
        /// <remarks>If a user-defined override for the AM or PM designator is set, it is used in
        /// preference to the culture-specific value. If the resulting designator is null,
        /// empty, or consists only of
        /// white space, the method defaults to "AM" or "PM" based on the time of day. The returned designator is
        /// trimmed of any leading or trailing white space.</remarks>
        /// <param name="dt">The date and time value for which to determine the AM or PM designator.</param>
        /// <param name="formatProvider">An optional object that supplies culture-specific formatting information.
        /// If null, the current culture is used.</param>
        /// <returns>A string containing the AM or PM designator appropriate for the specified
        /// date and time, using any
        /// user-defined overrides if present.</returns>
        public static string GetAmOrPmDesignator(DateTime dt, IFormatProvider? formatProvider = null)
        {
            string result;

            if (IsAM(dt))
            {
                result = AmDesignatorOverride ?? dt.ToString("tt") ?? GetAmPmDesignators(formatProvider).AM;
            }
            else
            {
                result = PmDesignatorOverride ?? dt.ToString("tt") ?? GetAmPmDesignators(formatProvider).PM;
            }

            result = result.Trim();

            if (string.IsNullOrEmpty(result))
            {
                result = IsAM(dt) ? "AM" : "PM";
            }

            return result;
        }

        /// <summary>
        /// Gets the names of the months in the Gregorian calendar, based on the specified kind and format provider.
        /// </summary>
        /// <param name="kind">The kind of month names to retrieve (full or abbreviated).</param>
        /// <param name="formatProvider">An optional object that supplies culture-specific formatting information.
        /// If null, the current culture is used.</param>
        /// <returns>An array of month names corresponding to the specified kind and format provider.</returns>
        public static string[] GetMonthNames(MonthNamesKind kind = MonthNamesKind.Full, IFormatProvider? formatProvider = null)
        {
            var info = GetFormatInfo(formatProvider);
            return kind switch
            {
                MonthNamesKind.Full => info.MonthNames,
                MonthNamesKind.Abbreviated => info.AbbreviatedMonthNames,
                _ => info.MonthNames,
            };
        }

        /// <summary>
        /// Gets the names of the days of the week in the Gregorian calendar, based on the specified kind and format provider. 
        /// </summary>
        /// <param name="kind">The kind of day names to retrieve (full, abbreviated, or shortest).</param>
        /// <param name="formatProvider">An optional object that supplies culture-specific formatting information.
        /// If null, the current culture is used.</param>
        /// <returns>An array of day names corresponding to the specified kind and format provider.</returns>
        public static string[] GetDayNames(DayNamesKind kind = DayNamesKind.Full, IFormatProvider? formatProvider = null)
        {
            var info = GetFormatInfo(formatProvider);
            return kind switch
            {
                DayNamesKind.Full => info.DayNames,
                DayNamesKind.Abbreviated => info.AbbreviatedDayNames,
                DayNamesKind.Shortest => info.ShortestDayNames,
                _ => info.DayNames,
            };
        }

        /// <summary>
        /// Gets the current timestamp in ticks.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetCurrentTimestamp()
        {
            return DateTime.Now.Ticks;
        }

        /// <summary>
        /// Gets the <see cref="DateTimeFormatInfo"/> from the specified format provider,
        /// or uses the current culture if the provider is <c>null</c>.
        /// </summary>
        /// <param name="formatProvider">The format provider to retrieve the
        /// culture-specific information from. If <c>null</c>, the current
        /// culture will be used.</param>
        /// <returns>The <see cref="DateTimeFormatInfo"/> associated with
        /// the specified format provider.</returns>
        public static DateTimeFormatInfo GetFormatInfo(IFormatProvider? formatProvider = null)
        {
            return DateTimeFormatInfo.GetInstance(formatProvider ?? CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns <c>true</c> if the specified format provider has AM or PM designators
        /// and 12 hour time format can be used.
        /// </summary>
        public static bool HasAmPmDesignators(IFormatProvider? formatProvider = null)
        {
            var info = GetFormatInfo(formatProvider);
            return !string.IsNullOrEmpty(info.AMDesignator)
                || !string.IsNullOrEmpty(info.PMDesignator);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DaysOfWeek"/> contains the specified <see cref="DayOfWeek"/>.
        /// </summary>
        /// <param name="days">The days of the week to check.</param>
        /// <param name="day">The day of the week to look for.</param>
        /// <returns><c>true</c> if the specified <see cref="DaysOfWeek"/> contains the specified <see cref="DayOfWeek"/>;
        /// otherwise, <c>false</c>.</returns>
        public static bool HasDay(this DaysOfWeek days, DayOfWeek day)
        {
            return (days & (DaysOfWeek)(1 << (int)day)) != 0;
        }

        /// <summary>
        /// Gets the localized time separator from the specified format provider,
        /// or uses the current culture if the provider is <c>null</c>.
        /// </summary>
        /// <param name="formatProvider">The format provider to retrieve the
        /// culture-specific information from. If <c>null</c>, the current
        /// culture will be used.</param>
        /// <returns>The time separator character as a string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetTimeSeparator(IFormatProvider? formatProvider = null)
        {
            var info = GetFormatInfo(formatProvider);
            return info.TimeSeparator;
        }

        /// <summary>
        /// Determines whether the specified time is in the morning (ante meridiem, AM).
        /// </summary>
        /// <remarks>This method checks the hour component of the provided DateTime to determine if it
        /// falls within the AM period, which is from 12:00 midnight to 11:59 AM.</remarks>
        /// <param name="dt">The date and time value to evaluate.</param>
        /// <returns>true if the hour component of the specified date and time is less than 12;
        /// otherwise, false.</returns>
        public static bool IsAM(DateTime dt)
        {
            if (dt.Hour < 12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the localized AM and PM designators from the specified format provider,
        /// or uses the current culture if the provider is <c>null</c>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (string AM, string PM) GetAmPmDesignators(IFormatProvider? formatProvider = null)
        {
            var info = GetFormatInfo(formatProvider);
            return (info.AMDesignator, info.PMDesignator);
        }

        /// <summary>
        /// Subtracts ticks of <see cref="DateTime.Now"/>
        /// with the specified timestamp and gets absolute value of the result.
        /// </summary>
        /// <param name="timestamp">Value to subtract from the <see cref="DateTime.Now"/>.
        /// Specified in ticks.</param>
        /// <returns>
        /// The absolute difference in ticks between the current time and the specified timestamp.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetAbsDistanceWithNow(long timestamp)
        {
            return Math.Abs(DateUtils.GetCurrentTimestamp() - timestamp);
        }

        /// <summary>
        /// Converts a <see cref="TimeOnly"/> to a <see cref="DateTime"/> using the specified date.
        /// </summary>
        /// <param name="time">The time of day.</param>
        /// <param name="date">The date to combine with the time.</param>
        /// <returns>A <see cref="DateTime"/> representing the combined date and time.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTime(TimeOnly time, DateOnly date)
        {
            return date.ToDateTime(time);
        }

        /// <summary>
        /// Converts a <see cref="TimeOnly"/> to a <see cref="DateTime"/> using today's date.
        /// </summary>
        /// <param name="time">The time of day.</param>
        /// <returns>A <see cref="DateTime"/> representing today at the specified time.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTimeToday(TimeOnly time)
        {
            return DateTime.Today.Add(time.ToTimeSpan());
        }

        /// <summary>
        /// Gets the effective maximum date value, ensuring it does not exceed the defined maximum date limit.
        /// </summary>
        /// <param name="maxDate">The maximum date to evaluate.</param>
        /// <returns>The effective maximum date.</returns>
        public static DateTime EffectiveMaxDate(DateTime maxDate)
        {
            DateTime maximumDateTime = MaximumDateTime;
            if (maxDate > maximumDateTime)
            {
                return maximumDateTime;
            }

            return maxDate;
        }

        /// <summary>
        /// Gets the effective minimum date value, ensuring it does not fall below the defined minimum date limit.
        /// </summary>
        /// <param name="minDate">The minimum date to evaluate.</param>
        /// <returns>The effective minimum date.</returns>
        public static DateTime EffectiveMinDate(DateTime minDate)
        {
            DateTime minimumDateTime = MinimumDateTime;
            if (minDate < minimumDateTime)
            {
                return minimumDateTime;
            }

            return minDate;
        }

        /// <summary>
        /// Converts milliseconds to ticks.
        /// </summary>
        /// <param name="msec">Value to convert.</param>
        /// <returns>The equivalent value in ticks.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TicksFromMilliseconds(long msec)
        {
            return msec * TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Gets the start of the week for the specified date, based on the system's first day of the week.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>The start of the week for the specified date.</returns>
        public static DateOnly GetStartOfWeek(DateOnly date)
        {
            return GetStartOfWeek(date, SystemFirstDayOfWeek);
        }

        /// <summary>
        /// Gets the start of the week for the specified date, based on the system's first day of the week.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>The start of the week for the specified date.</returns>
        public static DateTime GetStartOfWeek(DateTime date)
        {
            return GetStartOfWeek(date, SystemFirstDayOfWeek);
        }

        /// <summary>
        /// Gets the start of the week for the specified date, based on the provided first day of the week.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <param name="firstDayOfWeek">The first day of the week.</param>
        /// <returns>The start of the week for the specified date.</returns>
        public static DateTime GetStartOfWeek(DateTime date, DayOfWeek firstDayOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - firstDayOfWeek)) % 7;
            return date.AddDays(-diff).Date;
        }

        /// <summary>
        /// Gets the start of the week for the specified date, based on the provided first day of the week.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <param name="firstDayOfWeek">The first day of the week.</param>
        /// <returns>The start of the week for the specified date.</returns>
        public static DateOnly GetStartOfWeek(DateOnly date, DayOfWeek firstDayOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - firstDayOfWeek)) % 7;
            return date.AddDays(-diff);
        }

        /// <summary>
        /// Gets the first date of the month for the specified <see cref="DateOnly"/>.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>The first date of the month.</returns>
        public static DateOnly GetFirstDateOfMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Gets the number of days in the month for the specified <see cref="DateOnly"/>.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>The number of days in the month.</returns>
        public static int GetDaysInMonth(DateOnly date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        /// <summary>
        /// Gets the earlier of two <see cref="DateOnly"/> values.
        /// </summary>
        /// <param name="a">The first date to compare.</param>
        /// <param name="b">The second date to compare.</param>
        /// <returns>The earlier of the two dates.</returns>
        public static DateOnly Min(DateOnly a, DateOnly b)
        {
            return a < b ? a : b;
        }

        /// <summary>
        /// Gets the later of two <see cref="DateOnly"/> values.
        /// </summary>
        /// <param name="a">The first date to compare.</param>
        /// <param name="b">The second date to compare.</param>
        /// <returns>The later of the two dates.</returns>
        public static DateOnly Max(DateOnly a, DateOnly b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// Determines whether the specified <see cref="DateOnly"/> falls on a weekend (Saturday or Sunday).
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns><c>true</c> if the date falls on a weekend; otherwise, <c>false</c>.</returns>
        public static bool IsWeekend(DateOnly date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Gets the dates of the weekends (Saturdays and Sundays) for the specified <see cref="DateOnly"/> month.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>An enumerable of dates that fall on weekends.</returns>
        public static IEnumerable<DateOnly> GetWeekendsOfMonth(DateOnly date)
        {
            return GetDatesOfMonth(date, IsWeekend);
        }

        /// <summary>
        /// Gets the dates of the month for the specified <see cref="DateOnly"/> that satisfy the given predicate.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <param name="predicate">A predicate to filter the dates. If null, all dates are returned.</param>
        /// <returns>An enumerable of dates that satisfy the predicate.</returns>
        public static IEnumerable<DateOnly> GetDatesOfMonth(DateOnly date, Predicate<DateOnly>? predicate = null)
        {
            int daysInMonth = GetDaysInMonth(date);
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateOnly currentDate = new (date.Year, date.Month, day);
                if (predicate == null || predicate(currentDate))
                {
                    yield return currentDate;
                }
            }
        }

        /// <summary>
        /// Tries to get the date of the month for the specified <see cref="DateOnly"/> and day.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <param name="day">The day of the month. Must be between 1 and the number of days in the month.</param>
        /// <param name="result">When this method returns, contains the date of the month if the day is valid;
        /// otherwise, the default value.</param>
        /// <returns><c>true</c> if the day is valid for the specified month; otherwise, <c>false</c>.</returns>
        public static bool TryGetDateOfMonth(DateOnly date, int day, out DateOnly result)
        {
            if (day < 1 || day > GetDaysInMonth(date))
            {
                result = default;
                return false;
            }

            result = new DateOnly(date.Year, date.Month, day);
            return true;
        }

        /// <summary>
        /// Gets the date of the month for the specified <see cref="DateOnly"/> and day.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <param name="day">The day of the month. Must be between 1 and the number of days in the month.</param>
        /// <returns>The date of the month.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the day is not valid for the specified month.</exception>
        public static DateOnly GetDateOfMonth(DateOnly date, int day)
        {
            if (day < 1 || day > GetDaysInMonth(date))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(day),
                    $"Day must be between 1 and {GetDaysInMonth(date)} for the specified month.");
            }

            return new DateOnly(date.Year, date.Month, day);
        }

        /// <summary>
        /// Gets the last date of the month for the specified <see cref="DateOnly"/>.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>The last date of the month.</returns>
        public static DateOnly GetLastDateOfMonth(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        /// <summary>
        /// Converts ticks to milliseconds.
        /// </summary>
        /// <param name="ticks">Value to convert.</param>
        /// <returns>The equivalent value in milliseconds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TicksToMilliseconds(long ticks)
        {
            return ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Gets current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetNowInMilliseconds()
            => DateUtils.GetCurrentTimestamp() / TimeSpan.TicksPerMillisecond;

        /// <summary>
        /// Gets the time in milliseconds for the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dt">The date and time value to convert.</param>
        /// <returns>The equivalent time in milliseconds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetInMilliseconds(DateTime dt)
            => dt.Ticks / TimeSpan.TicksPerMillisecond;

        /// <summary>
        /// Gets the time in milliseconds which is the
        /// difference between the specified <see cref="DateTime"/> and the current time.
        /// </summary>
        /// <param name="time">The date and time value to compare with the current time.</param>
        /// <returns>The elapsed time in milliseconds. Returned value is always non-negative.</returns>
        public static long GetAbsElapsedMilliseconds(DateTime time)
        {
            return Math.Abs(GetNowInMilliseconds() - GetInMilliseconds(time));
        }

        /// <summary>
        /// Gets current time in milliseconds using
        /// <see cref="DateTimeOffset.ToUnixTimeMilliseconds()"/>.
        /// </summary>
        /// <returns>The current time in Unix milliseconds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetNowInUnixMilliseconds() => DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}

/*
https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings

============
"d" The day of the month, from 1 to 31.
============
"dd" The day of the month, from 01 to 31.
============
"ddd" The abbreviated name of the day of the week.
2009-06-15T13:45:30 -> Mon (en-US)
2009-06-15T13:45:30 -> Пн (ru-RU)
============
"dddd" The full name of the day of the week.
2009-06-15T13:45:30 -> Monday (en-US)
2009-06-15T13:45:30 -> понедельник (ru-RU)
============
"f" The tenths of a second.
2009-06-15T13:45:30.6170000 -> 6
2009-06-15T13:45:30.05 -> 0
============
"ff" The hundredths of a second.
2009-06-15T13:45:30.6170000 -> 61
2009-06-15T13:45:30.0050000 -> 00
============
"fff" The milliseconds.
6/15/2009 13:45:30.617 -> 617
6/15/2009 13:45:30.0005 -> 000
============
"ffff" The ten thousandths of a second.
2009-06-15T13:45:30.6175000 -> 6175
2009-06-15T13:45:30.0000500 -> 0000
============
"fffff" The hundred thousandths of a second.
2009-06-15T13:45:30.6175400 -> 61754
6/15/2009 13:45:30.000005 -> 00000
============
"ffffff" The millionths of a second.
2009-06-15T13:45:30.6175420 -> 617542
2009-06-15T13:45:30.0000005 -> 000000
============
"fffffff" The ten millionths of a second.
2009-06-15T13:45:30.6175425 -> 6175425
2009-06-15T13:45:30.0001150 -> 0001150
============
"F" If non-zero, the tenths of a second.
2009-06-15T13:45:30.6170000 -> 6
2009-06-15T13:45:30.0500000 -> (no output)
============
"FF" If non-zero, the hundredths of a second.
2009-06-15T13:45:30.6170000 -> 61
2009-06-15T13:45:30.0050000 -> (no output)
============
"FFF" If non-zero, the milliseconds.
The "FFF" Custom Format Specifier. 2009-06-15T13:45:30.6170000 -> 617
2009-06-15T13:45:30.0005000 -> (no output)
============
"FFFF" If non-zero, the ten thousandths of a second.
The "FFFF" Custom Format Specifier. 2009-06-15T13:45:30.5275000 -> 5275
2009-06-15T13:45:30.0000500 -> (no output)
============
"FFFFF" If non-zero, the hundred thousandths of a second.
The "FFFFF" Custom Format Specifier. 2009-06-15T13:45:30.6175400 -> 61754
2009-06-15T13:45:30.0000050 -> (no output)
============
"FFFFFF" If non-zero, the millionths of a second.
2009-06-15T13:45:30.6175420 -> 617542
2009-06-15T13:45:30.0000005 -> (no output)
============
"FFFFFFF" If non-zero, the ten millionths of a second.
2009-06-15T13:45:30.6175425 -> 6175425
2009-06-15T13:45:30.0001150 -> 000115
============
"g", "gg" The period or era.
2009-06-15T13:45:30.6170000 -> A.D.
============
"h" The hour, using a 12-hour clock from 1 to 12.
2009-06-15T01:45:30 -> 1
2009-06-15T13:45:30 -> 1
============
"hh" The hour, using a 12-hour clock from 01 to 12.
2009-06-15T01:45:30 -> 01
2009-06-15T13:45:30 -> 01
============
"H" The hour, using a 24-hour clock from 0 to 23.
2009-06-15T01:45:30 -> 1
2009-06-15T13:45:30 -> 13
============
"HH" The hour, using a 24-hour clock from 00 to 23.
2009-06-15T01:45:30 -> 01
2009-06-15T13:45:30 -> 13
============
"K" Time zone information.
The "K" Custom Format Specifier.

With DateTime values:
2009-06-15T13:45:30, Kind Unspecified ->
2009-06-15T13:45:30, Kind Utc -> Z
2009-06-15T13:45:30, Kind Local -> -07:00 (depends on local computer settings)

With DateTimeOffset values:
2009-06-15T01:45:30-07:00 --> -07:00
2009-06-15T08:45:30+00:00 --> +00:00
============
"m" The minute, from 0 to 59.
2009-06-15T01:09:30 -> 9
2009-06-15T13:29:30 -> 29
============
"mm" The minute, from 00 to 59.
2009-06-15T01:09:30 -> 09
2009-06-15T01:45:30 -> 45
============
"M" The month, from 1 to 12.
2009-06-15T13:45:30 -> 6
============
"MM" The month, from 01 to 12.
2009-06-15T13:45:30 -> 06
============
"MMM" The abbreviated name of the month.
2009-06-15T13:45:30 -> Jun (en-US)
2009-06-15T13:45:30 -> juin (fr-FR)
2009-06-15T13:45:30 -> Jun (zu-ZA)
============
"MMMM" The full name of the month.
2009-06-15T13:45:30 -> June (en-US)
2009-06-15T13:45:30 -> juni (da-DK)
2009-06-15T13:45:30 -> uJuni (zu-ZA)
============
"s" The second, from 0 to 59.
2009-06-15T13:45:09 -> 9
============
"ss" The second, from 00 to 59.
2009-06-15T13:45:09 -> 09
============
"t" The first character of the AM/PM designator.
2009-06-15T13:45:30 -> P (en-US)
2009-06-15T13:45:30 -> 午 (ja-JP)
2009-06-15T13:45:30 -> (fr-FR)
============
"tt" The AM/PM designator.
2009-06-15T13:45:30 -> PM (en-US)
2009-06-15T13:45:30 -> 午後 (ja-JP)
2009-06-15T13:45:30 -> (fr-FR)
============
"y" The year, from 0 to 99.
0001-01-01T00:00:00 -> 1
0900-01-01T00:00:00 -> 0
1900-01-01T00:00:00 -> 0
2009-06-15T13:45:30 -> 9
2019-06-15T13:45:30 -> 19
============
"yy" The year, from 00 to 99.
0001-01-01T00:00:00 -> 01
0900-01-01T00:00:00 -> 00
1900-01-01T00:00:00 -> 00
2019-06-15T13:45:30 -> 19
============
"yyy" The year, with a minimum of three digits.
0001-01-01T00:00:00 -> 001
0900-01-01T00:00:00 -> 900
1900-01-01T00:00:00 -> 1900
2009-06-15T13:45:30 -> 2009
============
"yyyy" The year as a four-digit number.
0001-01-01T00:00:00 -> 0001
0900-01-01T00:00:00 -> 0900
1900-01-01T00:00:00 -> 1900
2009-06-15T13:45:30 -> 2009
============
"yyyyy" The year as a five-digit number.
0001-01-01T00:00:00 -> 00001
2009-06-15T13:45:30 -> 02009
============
"z" Hours offset from UTC, with no leading zeros.
2009-06-15T13:45:30-07:00 -> -7
============
"zz" Hours offset from UTC, with a leading zero for a single-digit value.
2009-06-15T13:45:30-07:00 -> -07
============
"zzz" Hours and minutes offset from UTC.
2009-06-15T13:45:30-07:00 -> -07:00
============
":" The time separator.
2009-06-15T13:45:30 -> : (en-US)
2009-06-15T13:45:30 -> . (it-IT)
2009-06-15T13:45:30 -> : (ja-JP)
============
"/" The date separator.
2009-06-15T13:45:30 -> / (en-US)
2009-06-15T13:45:30 -> - (ar-DZ)
2009-06-15T13:45:30 -> . (tr-TR)
============
"string"
'string' Literal string delimiter.
2009-06-15T13:45:30 ("arr:" h:m t) -> arr: 1:45 P
2009-06-15T13:45:30 ('arr:' h:m t) -> arr: 1:45 P
============
% Defines the following character as a custom format specifier.
Using Single Custom Format Specifiers. 2009-06-15T13:45:30 (%h) -> 1
============
\ The escape character.
Character literals and Using the Escape Character.
2009-06-15T13:45:30 (h \h) -> 1 h
============
Any other character The character is copied to the result string unchanged.
Character literals. 2009-06-15T01:45:30 (arr hh:mm t) -> arr 01:45 A
*/