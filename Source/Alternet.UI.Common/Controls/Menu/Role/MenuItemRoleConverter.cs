using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Converter class for converting between a string and the Type of a MenuItemRole
    /// </summary>
    public class MenuItemRoleConverter : TypeConverter
    {
        /// <inheritdoc cref="KeyGestureConverter.CanConvertFrom(ITypeDescriptorContext?, Type?)"/>
        public override bool CanConvertFrom(
            ITypeDescriptorContext? context,
            Type? sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <inheritdoc cref="KeyGestureConverter.CanConvertTo(ITypeDescriptorContext?, Type?)"/>
        public override bool CanConvertTo(
            ITypeDescriptorContext? context,
            Type? destinationType)
        {
            // We can convert to an InstanceDescriptor or to a string.
            if (destinationType == typeof(string))
            {
                // When invoked by the serialization engine we can convert
                // to string only for known type
                if (context != null && context.Instance != null)
                {
                    return context.Instance is MenuItemRole;
                }
            }

            return false;
        }

        /// <inheritdoc cref="KeyGestureConverter.ConvertFrom(ITypeDescriptorContext?, CultureInfo?, object?)"/>
        public override object ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? source)
        {
            if (source is not null and string)
                return new MenuItemRole((string)source);

            throw GetConvertFromException(source);
        }

        /// <inheritdoc cref="KeyGestureConverter.ConvertTo(ITypeDescriptorContext?, CultureInfo?, object?, Type?)"/>
        public override object ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo?culture,
            object? value,
            Type? destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    if (value is MenuItemRole role)
                    {
                        return role.Name;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }

            throw GetConvertToException(value, destinationType);
        }
    }
}