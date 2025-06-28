using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Gets <see cref="DateTime"/> format used in Javascript
        /// or in other situations.
        /// </summary>
        public static string DateFormatJs { get; set; } = "yyyy-MM-ddTHH:mm:ss.fffK";

        /// <summary>
        /// Subtracts <see cref="DateTime.Now"/>
        /// with the specified timestamp and gets absolute value of the result.
        /// </summary>
        /// <param name="timestamp">Value to subtract from the <see cref="DateTime.Now"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetAbsDistanceWithNow(long timestamp)
        {
            return Math.Abs(DateTime.Now.Ticks - timestamp);
        }

        /// <summary>
        /// Converts milliseconds to ticks.
        /// </summary>
        /// <param name="msec">Value to convert.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TicksFromMilliseconds(long msec)
        {
            return msec * TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Converts ticks to milliseconds.
        /// </summary>
        /// <param name="ticks">Value to convert.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TicksToMilliseconds(long ticks)
        {
            return ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Gets current time in milliseconds.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetNowInMilliseconds()
            => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        /// <summary>
        /// Gets current time in milliseconds using
        /// <see cref="DateTimeOffset.ToUnixTimeMilliseconds()"/>.
        /// </summary>
        /// <returns></returns>
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