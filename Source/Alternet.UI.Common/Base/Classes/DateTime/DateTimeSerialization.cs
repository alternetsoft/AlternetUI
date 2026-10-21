using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IDateTimeSerialization"/> interface
    /// for serializing and deserializing date and time values to and from string representations.
    /// </summary>
    public class DateTimeSerialization : IDateTimeSerialization
    {
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
            DateTime utc = date.ToUniversalTime();
            string xmlDateTime = utc.ToString("yyyy-MM-ddTHH:mm:ss'Z'");
            return xmlDateTime;
        }

        /// <inheritdoc/>
        public virtual bool TryDateTimeFromString(string s, out DateTime result)
        {
            return DateTime.TryParse(s, out result);
        }

        /// <inheritdoc/>
        public string TimeSpanToString(TimeSpan timeSpan)
        {
            var s = timeSpan.ToString("c", CultureInfo.InvariantCulture);
            return s;
        }

        /// <inheritdoc/>
        public bool TryTimeSpanFromString(string s, out TimeSpan result)
        {
            var success = TimeSpan.TryParseExact(
                s,
                "c",
                CultureInfo.InvariantCulture,
                out var ts2);
            result = ts2;
            return success;
        }
    }
}
