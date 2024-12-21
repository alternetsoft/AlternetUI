using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI.Port
{
    /// <summary>
    ///  Wraps the DateTimeValueSerializer, to make it compatible with
    ///  internal code that expects a type converter.
    /// </summary>
    internal class DateTimeConverter2 : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given value object to a <see cref="DateTime"></see>.
        /// </summary>
        public DateTime FromString(string? value)
        {
            // Validate and clean up input.
            if (value == null)
            {
                throw GetConvertFromException(value);
            }

            if (value.Length == 0)
            {
                return DateTime.MinValue;
            }

            // Get a DateTimeFormatInfo
            DateTimeFormatInfo dateTimeFormatInfo;

            dateTimeFormatInfo =
                (DateTimeFormatInfo)TypeConverterHelper.InvariantEnglishUS.GetFormat(
                    typeof(DateTimeFormatInfo));

            // Set the formatting style for round-tripping and to trim the string.
            DateTimeStyles dateTimeStyles = DateTimeStyles.RoundtripKind
                      | DateTimeStyles.NoCurrentDateDefault
                      | DateTimeStyles.AllowLeadingWhite
                      | DateTimeStyles.AllowTrailingWhite;

            // Create the DateTime, using the DateTimeInfo if possible, and the culture otherwise.
            if (dateTimeFormatInfo != null)
            {
                return DateTime.Parse(value, dateTimeFormatInfo, dateTimeStyles);
            }
            else
            {
                // The culture didn't have a DateTimeFormatInfo.
                return DateTime.Parse(value, TypeConverterHelper.InvariantEnglishUS, dateTimeStyles);
            }
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object? value)
        {
            if(value is string str)
                return FromString(str);
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to a <see cref="DateTime"></see>.
        /// </summary>
        public string ConvertDateTimeToString(object? value)
        {
            if (value == null || value is not DateTime)
            {
                throw GetConvertToException(value, typeof(string));
            }

            DateTime dateTime = (DateTime)value;

            // Build up the format string to be used in DateTime.ToString()
            StringBuilder formatString = new("yyyy-MM-dd");

            if (dateTime.TimeOfDay == TimeSpan.Zero)
            {
                // The time portion of this DateTime is exactly at midnight.
                // We don't include the time component if the Kind is unspecified.
                // Otherwise, we're going to be including the time zone info, so'll
                // we'll have to include the time.
                if (dateTime.Kind != DateTimeKind.Unspecified)
                {
                    formatString.Append("'T'HH':'mm");
                }
            }
            else
            {
                long digitsAfterSecond = dateTime.Ticks % 10000000;
                int second = dateTime.Second;

                // We're going to write out at least the hours/minutes
                formatString.Append("'T'HH':'mm");
                if (second != 0 || digitsAfterSecond != 0)
                {
                    // need to write out seconds
                    formatString.Append("':'ss");
                    if (digitsAfterSecond != 0)
                    {
                        // need to write out digits after seconds
                        formatString.Append("'.'FFFFFFF");
                    }
                }
            }

            // Add the format specifier that indicates we want the DateTimeKind to be
            // included in the output formulation -- UTC gets written out with a "Z",
            // and Local gets written out with e.g. "-08:00" for Pacific Standard Time.
            formatString.Append('K');

            // We've finally got our format string built, we can create the string.
            return dateTime.ToString(
                formatString.ToString(),
                TypeConverterHelper.InvariantEnglishUS);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (destinationType != null && value is DateTime dt)
            {
                return ConvertDateTimeToString(dt);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
