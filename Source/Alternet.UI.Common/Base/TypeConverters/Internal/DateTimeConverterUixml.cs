using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI.Port
{
    /// <summary>
    /// <see cref="TypeConverter"/> descendant for converting
    /// between a string and <see cref="DateTime"/>.
    /// </summary>
    internal class DateTimeConverterUixml : BaseTypeConverter
    {
        /// <summary>
        /// Converts the given value object to a <see cref="DateTime"></see>.
        /// </summary>
        public virtual DateTime FromString(string? value)
        {
            if (value == null)
            {
                throw GetConvertFromException(value);
            }

            if (value.Length == 0)
            {
                return DateTime.MinValue;
            }

            var dateTimeFormatInfo = (DateTimeFormatInfo?)App.InvariantEnglishUS.GetFormat(
                    typeof(DateTimeFormatInfo));

            DateTimeStyles dateTimeStyles = DateTimeStyles.RoundtripKind
                      | DateTimeStyles.NoCurrentDateDefault
                      | DateTimeStyles.AllowLeadingWhite
                      | DateTimeStyles.AllowTrailingWhite;

            if (dateTimeFormatInfo != null)
            {
                return DateTime.Parse(value, dateTimeFormatInfo, dateTimeStyles);
            }
            else
            {
                return DateTime.Parse(value, App.InvariantEnglishUS, dateTimeStyles);
            }
        }

        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object value)
        {
            if(value is string str)
                return FromString(str);
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to a <see cref="DateTime"></see>.
        /// </summary>
        public virtual string ConvertDateTimeToString(object? value)
        {
            if (value is not DateTime dateTime)
                throw GetConvertToException(value, typeof(string));

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
                App.InvariantEnglishUS);
        }

        public override object? ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType)
        {
            if (destinationType != null && value is DateTime dt)
            {
                return ConvertDateTimeToString(dt);
            }

            return base.ConvertTo(context, culture, value, destinationType!);
        }
    }
}
