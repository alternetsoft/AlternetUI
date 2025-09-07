using System;
using System.ComponentModel;
using System.Globalization;
using Alternet.UI.Port;

namespace Alternet.UI
{
    /// <summary>
    /// Converter class for serializing a <see cref="KeyGesture"/>.
    /// </summary>
    public class KeyGestureValueSerializer : ValueSerializer
    {
        /// <inheritdoc/>
        public override bool CanConvertFromString(string value, IValueSerializerContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        public override bool CanConvertToString(object value, IValueSerializerContext context)
        {
#pragma warning disable
            return (value is KeyGesture keyGesture)
                && ModifierKeysConverter.IsDefinedModifierKeys(keyGesture.Modifiers)
                && KeyGestureConverter.IsDefinedKey(keyGesture.Key);
#pragma warning restore
        }

        /// <inheritdoc/>
        public override object? ConvertFromString(string value, IValueSerializerContext context)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(KeyGesture));
            if (converter != null)
                return converter.ConvertFromString(value);
            else
                return base.ConvertFromString(value, context);
        }

        /// <inheritdoc/>
        public override string? ConvertToString(object value, IValueSerializerContext context)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(KeyGesture));
            if (converter != null)
                return converter.ConvertToInvariantString(value);
            else
                return base.ConvertToString(value, context);
        }
    }
}