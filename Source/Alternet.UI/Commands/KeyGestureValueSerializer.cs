// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// Description: KeyGestureValueSerializer - Serializes a KeyGesture to and from a string

using System;
using System.ComponentModel;
using System.Globalization;
using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    /// KeyGestureValueSerializer - Converter class for serializing a KeyGesture
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
#pragma warning disable 6506
            return (value is KeyGesture keyGesture)
                && ModifierKeysConverter.IsDefinedModifierKeys(keyGesture.Modifiers)
                && KeyGestureConverter.IsDefinedKey(keyGesture.Key);
#pragma warning restore 6506
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