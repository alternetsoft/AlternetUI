// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Converter class for converting between a string and the
    /// <see cref="ModifierKeys"/>.
    /// </summary>
    public class ModifierKeysConverter : TypeConverter
    {
        private const char ModifierDelimiter = '+';

        private static readonly ModifierKeys ModifierKeysFlag =
            ModifierKeys.Windows | ModifierKeys.Shift |
            ModifierKeys.Alt | ModifierKeys.Control;

        /// <summary>
        ///     Check for Valid enum, as any int can be casted to the enum.
        /// </summary>
        public static bool IsDefinedModifierKeys(ModifierKeys modifierKeys)
        {
            return modifierKeys == ModifierKeys.None ||
                (((int)modifierKeys & ~((int)ModifierKeysFlag)) == 0);
        }

        /// <summary>
        /// Parses string representation of the key modifiers.
        /// </summary>
        /// <param name="source">String value to parse.</param>
        /// <returns>Enumeration with key modifiers.</returns>
        public static ModifierKeys FromString(string source)
        {
            string modifiersToken = ((string)source).Trim();
            ModifierKeys modifiers =
                GetModifierKeys(modifiersToken, CultureInfo.InvariantCulture);
            return modifiers;
        }

        /// <summary>
        /// Converts <see cref="ModifierKeys"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="modifiers">Key modifiers.</param>
        /// <param name="forUser">Result string is for user or not.</param>
        /// <returns>A <see cref="string"/> representation
        /// of <see cref="ModifierKeys"/> value.</returns>
        public static string ToString(ModifierKeys modifiers, bool forUser = false)
        {
            string strModifiers = string.Empty;

            Add(ModifierKeys.Control);
            Add(ModifierKeys.Alt);
            Add(ModifierKeys.Windows);
            Add(ModifierKeys.Shift);

            return strModifiers;

            void Add(ModifierKeys key)
            {
                if ((modifiers & key) == key)
                {
                    if (strModifiers.Length > 0)
                        strModifiers += ModifierDelimiter;

                    strModifiers += MatchModifiers(key, forUser);
                }
            }
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(
            ITypeDescriptorContext? context,
            Type sourceType)
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

        /// <inheritdoc/>
        public override bool CanConvertTo(
            ITypeDescriptorContext? context,
            Type? destinationType)
        {
            // We can convert to a string.
            if (destinationType == typeof(string))
            {
                // When invoked by the serialization engine we can convert to
                // string only for known type
                if (context?.Instance is ModifierKeys keys)
                {
                    return IsDefinedModifierKeys(keys);
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override object ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object source)
        {
            if (source is string v)
                return FromString(v);
            throw GetConvertFromException(source);
        }

        /// <inheritdoc/>
        public override object ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (value is null)
                return string.Empty;

            if (destinationType == typeof(string))
            {
                ModifierKeys modifiers = (ModifierKeys)value;

                if (!IsDefinedModifierKeys(modifiers))
                {
                    throw new InvalidEnumArgumentException(
                        nameof(value), (int)modifiers, typeof(ModifierKeys));
                }
                else
                    return ToString(modifiers);
            }

            throw GetConvertToException(value, destinationType);
        }

        internal static string MatchModifiers(ModifierKeys modifierKeys, bool forUser = false)
        {
            string modifiers = string.Empty;
            switch (modifierKeys)
            {
                case ModifierKeys.Control:
                    if (forUser && BaseApplication.IsMacOS)
                        modifiers = StringUtils.MacCommandKeyTitle;
                    else
                        modifiers = "Ctrl";
                    break;
                case ModifierKeys.Shift:
                    modifiers = "Shift";
                    break;
                case ModifierKeys.Alt:
                    modifiers = "Alt";
                    break;
                case ModifierKeys.Windows:
                    modifiers = "Windows";
                    break;
            }

            return modifiers;
        }

        private static ModifierKeys GetModifierKeys(
            string modifiersToken,
            CultureInfo culture)
        {
            ModifierKeys modifiers = ModifierKeys.None;
            if (modifiersToken.Length != 0)
            {
                int offset;
                do
                {
                    offset = modifiersToken.IndexOf(ModifierDelimiter);
                    string token = (offset < 0)
                        ? modifiersToken : modifiersToken.Substring(0, offset);
                    token = token.Trim();
                    token = token.ToUpper(culture);

                    if (token == string.Empty)
                        break;

                    switch (token)
                    {
                        case "CONTROL":
                        case "CTRL":
                            modifiers |= ModifierKeys.Control;
                            break;

                        case "SHIFT":
                            modifiers |= ModifierKeys.Shift;
                            break;

                        case "ALT":
                            modifiers |= ModifierKeys.Alt;
                            break;

                        case "WINDOWS":
                        case "WIN":
                            modifiers |= ModifierKeys.Windows;
                            break;

                        default:
                            throw new NotSupportedException(
                                string.Format("Unsupported modifier {0}.", token));
                    }

                    modifiersToken = modifiersToken.Substring(offset + 1);
                }
                while (offset != -1);
            }

            return modifiers;
        }
    }
}
