#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// ModifierKeysConverter : Converts a Modifier string to the *Type* that
// the string represents and vice-versa.

using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Converter class for converting between a string and the
    /// <see cref="ModifierKeys"/>.
    /// </summary>
    /// <ExternalAPI/> 
    public class ModifierKeysConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(
            ITypeDescriptorContext context,
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
            ITypeDescriptorContext context,
            Type destinationType)
        {
            // We can convert to a string.
            if (destinationType == typeof(string))
            {
                // When invoked by the serialization engine we can convert to
                // string only for known type
                if (context != null && context.Instance != null &&
                    context.Instance is ModifierKeys)
                {
                    return (IsDefinedModifierKeys((ModifierKeys)context.Instance));
                }
            }
            return false;
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
        {
            if (source is string)
                return FromString((string)source);
            throw GetConvertFromException(source);
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
        /// <returns>A <see cref="string"/> representation of <see cref="ModifierKeys"/> value.</returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static string ToString(ModifierKeys modifiers, bool forUser = false)
        {
            string strModifiers = "";

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
                        strModifiers += Modifier_Delimiter;

                    strModifiers += MatchModifiers(key, forUser);
                }
            }
        }

        /// <inheritdoc/>
        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if (destinationType == typeof(string))
            {
                ModifierKeys modifiers = (ModifierKeys)value;

                if (!IsDefinedModifierKeys(modifiers))
                {
                    throw new InvalidEnumArgumentException(
                        "value", (int)modifiers, typeof(ModifierKeys));
                }
                else
                    return ToString(modifiers);
            }
            throw GetConvertToException(value, destinationType);
        }

        private static ModifierKeys GetModifierKeys(
            string modifiersToken,
            CultureInfo culture)
        {
            ModifierKeys modifiers = ModifierKeys.None;
            if (modifiersToken.Length != 0)
            {
                int offset = 0;
                do
                {
                    offset = modifiersToken.IndexOf(Modifier_Delimiter);
                    string token = (offset < 0) ? modifiersToken : modifiersToken.Substring(0, offset);
                    token = token.Trim();
                    token = token.ToUpper(culture);

                    if (token == String.Empty)
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
                            throw new NotSupportedException(SR.Get(SRID.Unsupported_Modifier, token));
                    }

                    modifiersToken = modifiersToken.Substring(offset + 1);
                } while (offset != -1);
            }
            return modifiers;
        }

        /// <summary>
        ///     Check for Valid enum, as any int can be casted to the enum.
        /// </summary>
        public static bool IsDefinedModifierKeys(ModifierKeys modifierKeys)
        {
            return (modifierKeys == ModifierKeys.None ||
                (((int)modifierKeys & ~((int)ModifierKeysFlag)) == 0));
        }

        private const char Modifier_Delimiter = '+';

        private static ModifierKeys ModifierKeysFlag =
            ModifierKeys.Windows | ModifierKeys.Shift |
            ModifierKeys.Alt | ModifierKeys.Control;

        internal static string MatchModifiers(ModifierKeys modifierKeys, bool forUser = false)
        {
            string modifiers = String.Empty;
            switch (modifierKeys)
            {
                case ModifierKeys.Control:
                    modifiers = "Ctrl";
                    break;
                case ModifierKeys.Shift:
                    modifiers = "Shift";
                    break;
                case ModifierKeys.Alt:
                    modifiers = "Alt";
                    break;
                case ModifierKeys.Windows:
                    if (forUser && Application.IsMacOs)
                        modifiers = StringUtils.MacWindowsKeyTitle;
                    else
                        modifiers = "Windows";
                    break;
            }
            return modifiers;
        }
    }
}
