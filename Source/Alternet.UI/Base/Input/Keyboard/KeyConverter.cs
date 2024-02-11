// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Key Converter class for converting between a string and the Type of a Key
    /// </summary>
    public class KeyConverter : TypeConverter
    {
        /// <summary>
        /// Converts string representation of the key to the <see cref="Key"/>
        /// enumeration.
        /// </summary>
        /// <param name="source">String representation of the key.</param>
        /// <returns>Key value parsed from string.</returns>
        /// <exception cref="NotSupportedException">
        /// Raised when string representation of the key is unknown.
        /// </exception>
        public static Key FromString(string source)
        {
            string fullName = ((string)source).Trim();
            object? key = GetKey(fullName, CultureInfo.InvariantCulture);
            if (key != null)
            {
                return (Key)key;
            }
            else
            {
                throw new NotSupportedException(
                    SR.Get(SRID.Unsupported_Key, fullName));
            }
        }

        /// <summary>
        /// CanConvertFrom()
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
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
        /// TypeConverter method override.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="destinationType">Type to convert to</param>
        /// <returns>true if conversion is possible</returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            // We can convert to a string.
            // We can convert to an InstanceDescriptor or to a string.
            if (destinationType == typeof(string))
            {
                // When invoked by the serialization engine we can convert to string
                // only for known type
                if (context != null && context.Instance != null)
                {
                    Key key = (Key)context.Instance;
                    return (int)key >= (int)Key.None/* && (int)key <= (int)Key.DeadCharProcessed*/;
                }
            }

            return false;
        }

        /// <summary>
        /// ConvertFrom()
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public override object ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object source)
        {
            if (source is string v)
                return FromString(v);
            throw GetConvertFromException(source);
        }

        /// <summary>
        /// ConvertTo()
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object? ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (destinationType == typeof(string) && value != null)
            {
                Key key = (Key)value;
                if (key == Key.None)
                {
                    return string.Empty;
                }

                if (key >= Key.D0 && key <= Key.D9)
                {
                    return char.ToString((char)(int)(key - Key.D0 + '0'));
                }

                if (key >= Key.A && key <= Key.Z)
                {
                    return char.ToString((char)(int)(key - Key.A + 'A'));
                }

                string? strKey = MatchKey(key, culture);
                if (strKey != null)
                {
                    return strKey;
                }
            }

            throw GetConvertToException(value, destinationType);
        }

        private static object? GetKey(string keyToken, CultureInfo culture)
        {
            if (keyToken.Length == 0)
            {
                return Key.None;
            }
            else
            {
                keyToken = keyToken.ToUpper(culture);
                if (keyToken.Length == 1 && char.IsLetterOrDigit(keyToken[0]))
                {
                    if (char.IsDigit(keyToken[0]) && (keyToken[0] >= '0' && keyToken[0] <= '9'))
                    {
                        return (int)(Key)(Key.D0 + keyToken[0] - '0');
                    }
                    else if (char.IsLetter(keyToken[0])
                        && (keyToken[0] >= 'A' && keyToken[0] <= 'Z'))
                    {
                        return (int)(Key)(Key.A + keyToken[0] - 'A');
                    }
                    else
                    {
                        throw new ArgumentException(
                            SR.Get(SRID.CannotConvertStringToType, keyToken, typeof(Key)));
                    }
                }
                else
                {
                    Key keyFound;
                    switch (keyToken)
                    {
                        case "ENTER": keyFound = Key.Enter; break;
                        case "ESC": keyFound = Key.Escape; break;
                        case "PGUP": keyFound = Key.PageUp; break;
                        case "PGDN": keyFound = Key.PageDown; break;
                        case "PRTSC": keyFound = Key.PrintScreen; break;
                        case "INS": keyFound = Key.Insert; break;
                        case "DEL": keyFound = Key.Delete; break;
                        case "WINDOWS": keyFound = Key.Windows; break;
                        case "WIN": keyFound = Key.Windows; break;
                        case "BACKSPACE": keyFound = Key.Backspace; break;
                        case "BKSP": keyFound = Key.Backspace; break;
                        case "BS": keyFound = Key.Backspace; break;
                        case "SHIFT": keyFound = Key.Shift; break;
                        case "CONTROL": keyFound = Key.Control; break;
                        case "CTRL": keyFound = Key.Control; break;
                        case "ALT": keyFound = Key.Alt; break;
                        case "SEMICOLON": keyFound = Key.Semicolon; break;
                        case "COMMA": keyFound = Key.Comma; break;
                        case "MINUS": keyFound = Key.Minus; break;
                        case "PERIOD": keyFound = Key.Period; break;
                        case "OPENBRACKETS": keyFound = Key.OpenBracket; break;
                        case "CLOSEBRACKETS": keyFound = Key.CloseBracket; break;
                        case "QUOTES": keyFound = Key.Quote; break;
                        case "BACKSLASH": keyFound = Key.Backslash; break;
                        case "PLAY": keyFound = Key.MediaPlayPause; break;
                        default: keyFound = (Key)Enum.Parse(typeof(Key), keyToken, true); break;
                    }

                    if ((int)keyFound != -1)
                    {
                        return keyFound;
                    }

                    return null;
                }
            }
        }

        private static string? MatchKey(Key key, CultureInfo? culture)
        {
            if (key == Key.None)
                return string.Empty;
            else
            {
                switch (key)
                {
                    case Key.Backspace: return "Backspace";
                    case Key.Escape: return "Esc";
                }
            }

            if ((int)key >= (int)Key.None/* && (int)key <= (int)Key.DeadCharProcessed*/)
                return key.ToString();
            else
                return null;
        }
    }
}