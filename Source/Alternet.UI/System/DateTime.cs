// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;


namespace Alternet.UI
{
    /// <summary>
    /// Represents an instant in time, typically expressed as a date and time of day.
    /// </summary>
    public struct DateTime : IEquatable<DateTime>
    {
        internal DateTime(System.DateTime nativeDateTime)
        {
            NativeDateTime = nativeDateTime;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(DateTime other)
        {
            return NativeDateTime.Equals(other.NativeDateTime);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTime"/> structure to the specified year, month, day, hour, minute, second and millisecond.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            NativeDateTime = new System.DateTime(year, month, day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// Gets the year component of the date represented by this instance.
        /// </summary>
        public int Year => NativeDateTime.Year;

        /// <summary>
        /// Gets the month component of the date represented by this instance.
        /// </summary>
        public int Month => NativeDateTime.Month;

        /// <summary>
        /// Gets the day component of the date represented by this instance.
        /// </summary>
        public int Day => NativeDateTime.Day;

        /// <summary>
        /// Gets the hour component of the date represented by this instance.
        /// </summary>
        public int Hour => NativeDateTime.Hour;

        /// <summary>
        /// Gets the minute component of the date represented by this instance.
        /// </summary>
        public int Minute => NativeDateTime.Minute;

        /// <summary>
        /// Gets the seconds component of the date represented by this instance.
        /// </summary>
        public int Second => NativeDateTime.Second;

        /// <summary>
        /// Gets the milliseconds component of the date represented by this instance.
        /// </summary>
        public int Millisecond => NativeDateTime.Millisecond;

        internal System.DateTime NativeDateTime;

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time on this computer, expressed as the local time.
        /// </summary>
        public static DateTime Now
        {
            get
            {
                return new DateTime(System.DateTime.Now);
            }
        }

        /// <summary>
        /// Represents the smallest possible value of <see cref="DateTime"/>. This field is read-only.
        /// </summary>
        public static DateTime MinValue
        {
            get
            {
                return new DateTime(System.DateTime.MinValue);
            }
        }

        /// <summary>
        /// Gets the number of ticks that represent the date and time of this instance.
        /// </summary>
        public long Ticks
        {
            get
            {
                return NativeDateTime.Ticks;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the time represented by this instance is based on local time, Coordinated Universal Time (UTC), or neither.
        /// </summary>
        public DateTimeKind Kind
        {
            get
            {
                return NativeDateTime.Kind;
            }
        }

        /// <summary>
        /// Gets the time of day for this instance.
        /// </summary>
        public TimeSpan TimeOfDay
        {
            get
            {
                return NativeDateTime.TimeOfDay;
            }
        }

        /// <summary>
        /// Converts the string representation of a date and time to its <see cref="DateTime"/> equivalent by using culture-specific format information and a formatting style.
        /// </summary>
        /// <param name="value">A string that contains a date and time to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about value.</param>
        /// <param name="styles">A bitwise combination of the enumeration values that indicates the style elements that can be present in value for the parse operation to succeed, and that defines how to interpret the parsed date in relation to the current time zone or the current date.</param>
        /// <returns>Returns a <see cref="DateTime"/> from the specified string.</returns>
        public static DateTime Parse(string value, IFormatProvider provider, DateTimeStyles styles)
        {
            return new DateTime(System.DateTime.Parse(value, provider, styles));
        }

        /// <summary>
        /// Converts the string representation of a date and time to its <see cref="DateTime"/> equivalent by using culture-specific format information and a formatting style.
        /// </summary>
        /// <param name="value">A string that contains a date and time to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about value.</param>
        /// <returns>Returns a <see cref="DateTime"/> from the specified string.</returns>
        public static DateTime Parse(string value, IFormatProvider provider)
        {
            return new DateTime(System.DateTime.Parse(value, provider, DateTimeStyles.None));
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent string representation
        /// </summary>
        /// <returns>A string representation of value of the current <see cref="DateTime"/> object.</returns>
        public override string ToString()
        {
            return NativeDateTime.ToString();
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent string representation
        /// </summary>
        /// <param name="format">A standard or custom date and time format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A string representation of value of the current <see cref="DateTime"/> object as specified as format and provider..</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return NativeDateTime.ToString(format, provider);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent long time string representation.
        /// </summary>
        /// <returns>A string that contains the long time string representation of the current <see cref="DateTime"/> object.</returns>
        public string ToLongTimeString()
        {
            return NativeDateTime.ToLongTimeString();
        }
    }
}
