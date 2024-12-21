using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI.Port
{
    /// <summary>
    ///  Wraps the DateTimeValueSerializer, to make it compatible with
    ///  internal code that expects a type converter.
    /// </summary>
    internal class DateTimeConverter2 : TypeConverter
    {
        private readonly DateTimeValueSerializer dateTimeValueSerializer = new();
        private readonly IValueSerializerContext valueSerializerContext
            = new DateTimeValueSerializerContext();

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

        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value)
        {
            return dateTimeValueSerializer.ConvertFromString(
                value as string,
                valueSerializerContext);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (destinationType != null && value is DateTime)
            {
                dateTimeValueSerializer.ConvertToString(value as string, valueSerializerContext);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
