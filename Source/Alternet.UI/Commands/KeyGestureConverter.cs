// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Alternet.UI
{
    /// <summary>
    /// Converter class for converting between a string
    /// and the <see cref="KeyGesture"/>.
    /// </summary>
    public class KeyGestureConverter : TypeConverter
    {
        internal const char DisplayStringSeparator = ',';

        private const char ModifiersDelimiter = '+';

        private static readonly KeyConverter KeyConverter = new();
        private static readonly ModifierKeysConverter ModifierKeysConverter = new();

        /// <summary>
        /// Parses string representation of the key with modifiers to the
        /// <see cref="KeyGesture"/> instance.
        /// </summary>
        /// <param name="source">String value containing key name
        /// and key modifiers.</param>
        /// <returns></returns>
        public static KeyGesture? FromString(string source)
        {
            string fullName = ((string)source).Trim();
            if (fullName == string.Empty)
                return new KeyGesture(Keys.None);

            string keyToken;
            string modifiersToken;
            string displayString;

            // break apart display string
            int index = fullName.IndexOf(DisplayStringSeparator);
            if (index >= 0)
            {
                displayString = fullName.Substring(index + 1).Trim();
                fullName = fullName.Substring(0, index).Trim();
            }
            else
            {
                displayString = string.Empty;
            }

            // break apart key and modifiers
            index = fullName.LastIndexOf(ModifiersDelimiter);
            if (index >= 0)
            {
                // modifiers exists
                modifiersToken = fullName.Substring(0, index);
                keyToken = fullName.Substring(index + 1);
            }
            else
            {
                modifiersToken = string.Empty;
                keyToken = fullName;
            }

            ModifierKeys modifiers = ModifierKeys.None;
            object resultkey = KeyConverter.FromString(keyToken);
            if (resultkey != null)
            {
                object temp = ModifierKeysConverter.FromString(modifiersToken);
                if (temp != null)
                {
                    modifiers = (ModifierKeys)temp;
                }

                return new KeyGesture((Keys)resultkey, modifiers, displayString);
            }

            return null;
        }

        /// <summary>
        /// Gets a value indicating whether this converter can convert an object in the given
        /// source type to the native type of the converter using the context.
        /// </summary>
        public override bool CanConvertFrom(
            ITypeDescriptorContext? context,
            Type? sourceType)
        {
            // We can only handle string.
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this converter can convert an object to the given
        /// destination type using the context.
        /// </summary>
        public override bool CanConvertTo(
            ITypeDescriptorContext? context,
            Type? destinationType)
        {
            // We can convert to an InstanceDescriptor or to a string.
            if (destinationType == typeof(string))
            {
                // When invoked by the serialization engine we can convert to
                // string only for known type
                if (context != null && context.Instance != null)
                {
                    if (context.Instance is KeyGesture keyGesture)
                    {
                        return ModifierKeysConverter.IsDefinedModifierKeys(
                            keyGesture.Modifiers)
                                && IsDefinedKey(keyGesture.Key);
                    }
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override object ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? source)
        {
            if (source is not null and string)
            {
                object? result = FromString((string)source);
                if (result != null)
                    return result;
            }

            throw GetConvertFromException(source);
        }

        /// <summary>
        /// Converts the given object to the converter's native type.
        /// </summary>
        public override object ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type? destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    if (value is KeyGesture keyGesture)
                    {
                        if (keyGesture.Key == Keys.None)
                            return string.Empty;

                        string strBinding = string.Empty;
                        string strKey = (string)KeyConverter.ConvertTo(context, culture, keyGesture.Key, destinationType) as string;
                        if (strKey != string.Empty)
                        {
                            strBinding += ModifierKeysConverter.ConvertTo(context, culture, keyGesture.Modifiers, destinationType) as string;
                            if (strBinding != string.Empty)
                            {
                                strBinding += ModifiersDelimiter;
                            }

                            strBinding += strKey;

                            if (!string.IsNullOrEmpty(keyGesture.DisplayString))
                            {
                                strBinding += DisplayStringSeparator + keyGesture.DisplayString;
                            }
                        }

                        return strBinding;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }

            throw GetConvertToException(value, destinationType);
        }

        // Check for Valid enum, as any int can be casted to the enum.
        internal static bool IsDefinedKey(Keys key)
        {
            return key >= Keys.None && key <= Keys.Menu;
        }
    }
}