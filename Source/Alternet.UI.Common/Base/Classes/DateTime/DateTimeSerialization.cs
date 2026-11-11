using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Xml;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IDateTimeSerialization"/> interface
    /// for serializing and deserializing date and time values to and from string representations.
    /// </summary>
    public class DateTimeSerialization : IDateTimeSerialization
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string TimeFormat = "HH:mm:ss.fffffff";

        /// <summary>
        /// Gets or sets the default instance of <see cref="IDateTimeSerialization"/>
        /// used for date and time serialization and deserialization.
        /// </summary>
        public static IDateTimeSerialization Instance { get; set; } = new DateTimeSerialization();

        static DateTimeSerialization()
        {
        }

        /// <summary>
        /// Tests the serialization and deserialization of date and time values using
        /// the default instance of <see cref="IDateTimeSerialization"/>.
        /// </summary>
        [Conditional("DEBUG")]
        public static void TestDateTimeSerialization()
        {
            var d = DateTime.Now;

            var dateTime = Instance.DateTimeToString(d);
            var successDateTime = Instance.TryDateTimeFromString(dateTime, out var parsedDateTime);

            Debug.WriteLine($"DateTimeSerialization. Local : {d}. UTC : {d.ToUniversalTime()}");
            Debug.WriteLine($"DateTime: {dateTime}, Parsed: {parsedDateTime}, Success: {successDateTime}");
        }

        /// <inheritdoc/>
        public virtual string DateTimeToString(DateTime date)
        {
            return XmlConvert.ToString(date, XmlDateTimeSerializationMode.RoundtripKind);
        }

        /// <inheritdoc/>
        public virtual string TimeToString(TimeOnly time)
        {
            return time.ToString(TimeFormat, CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        public virtual string DateToString(DateOnly date)
        {
            return date.ToString(DateFormat, CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        public string TimeSpanToString(TimeSpan timeSpan)
        {
            return XmlConvert.ToString(timeSpan);
        }

        /// <inheritdoc/>
        public virtual bool TryDateTimeFromString(string s, out DateTime result)
        {
            result = default;
            if (s is null)
                return false;

            try
            {
                result = XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.RoundtripKind);
                return true;
            }
            catch (Exception ex)
            {
                BaseObject.Nop(ex);
                return false;
            }
        }

        /// <inheritdoc/>
        public virtual bool TryTimeFromString(string s, out TimeOnly result)
        {
            return TimeOnly.TryParseExact(
                s, TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        /// <inheritdoc/>
        public virtual bool TryDateFromString(string s, out DateOnly result)
        {
            return DateOnly.TryParseExact(
                s, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        /// <inheritdoc/>
        public bool TryTimeSpanFromString(string? s, out TimeSpan result)
        {
            result = default;
            if (s is null || s.Length == 0)
                return false;

            try
            {
                result = XmlConvert.ToTimeSpan(s);
                return true;
            }
            catch (Exception ex)
            {
                BaseObject.Nop(ex);
                return false;
            }
        }
    }
}
