// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;


namespace Alternet.UI
{
    /// <summary>
    public struct DateTime : IEquatable<DateTime>
    {
        internal DateTime(System.DateTime nativeDateTime)
        {
            NativeDateTime = nativeDateTime;
        }

        public bool Equals(DateTime other)
        {
            return NativeDateTime.Equals(other.NativeDateTime);
        }

        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            NativeDateTime = new System.DateTime(year, month, day, hour, minute, second, millisecond);
        }

        public int Year => NativeDateTime.Year;

        public int Month => NativeDateTime.Month;

        public int Day => NativeDateTime.Day;

        public int Hour => NativeDateTime.Hour;

        public int Minute => NativeDateTime.Minute;

        public int Second => NativeDateTime.Second;

        public int Millisecond => NativeDateTime.Millisecond;

        internal System.DateTime NativeDateTime;

        public static DateTime Now
        {
            get
            {
                return new DateTime(System.DateTime.Now);
            }
        }

        public static DateTime MinValue
        {
            get
            {
                return new DateTime(System.DateTime.MinValue);
            }
        }

        public long Ticks
        {
            get
            {
                return NativeDateTime.Ticks;
            }
        }

        public DateTimeKind Kind
        {
            get
            {
                return NativeDateTime.Kind;
            }
        }

        public TimeSpan TimeOfDay
        {
            get
            {
                return NativeDateTime.TimeOfDay;
            }
        }

        public static DateTime Parse(string value, IFormatProvider provider, DateTimeStyles styles)
        {
            return new DateTime(System.DateTime.Parse(value, provider, styles));
        }

        public override string ToString()
        {
            return NativeDateTime.ToString();
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return NativeDateTime.ToString(format, provider);
        }

        public string ToLongTimeString()
        {
            return NativeDateTime.ToLongTimeString();
        }

    }
}
