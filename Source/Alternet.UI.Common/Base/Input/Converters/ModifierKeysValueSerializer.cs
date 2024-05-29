// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

using Alternet.UI.Port;

namespace Alternet.UI
{
    /// <summary>
    /// Key Converter class for converting between a string and the Type of a Modifiers
    /// </summary>
    public class ModifierKeysValueSerializer : ValueSerializer
    {
        /// <summary>
        /// CanConvertFromString()
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanConvertFromString(string value, IValueSerializerContext context)
        {
            return true;
        }

        /// <summary>
        /// CanConvertToString()
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanConvertToString(object value, IValueSerializerContext context)
        {
            return (value is ModifierKeys keys)
                && ModifierKeysConverter.IsDefinedModifierKeys(keys);
        }

        /// <summary>
        /// ConvertFromString()
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object? ConvertFromString(string value, IValueSerializerContext context)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(ModifierKeys));
            if (converter != null)
                return converter.ConvertFromString(value);
            else
                return base.ConvertFromString(value, context);
        }

        /// <summary>
        /// ConvertToString()
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override string? ConvertToString(object value, IValueSerializerContext context)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(ModifierKeys));
            if (converter != null)
                return converter.ConvertToInvariantString(value);
            else
                return base.ConvertToString(value, context);
        }
    }
}
