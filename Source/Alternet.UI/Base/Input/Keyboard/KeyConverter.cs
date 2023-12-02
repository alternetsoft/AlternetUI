#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//
// Description:
//
//      KeyConverter : Converts a key string to the *Type* that the string represents and vice-versa
//
// Features:
//
//
//
// 

using System;
using System.ComponentModel;    // for TypeConverter
using System.Globalization;     // for CultureInfo

namespace Alternet.UI
{
    /// <summary>
    /// Key Converter class for converting between a string and the Type of a Key
    /// </summary>
    /// <ExternalAPI/> 
    public class KeyConverter : TypeConverter
    {
        /// <summary>
        /// CanConvertFrom()
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        /// <ExternalAPI/> 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
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
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            // We can convert to a string.
            // We can convert to an InstanceDescriptor or to a string.
            if (destinationType == typeof(string))
            {
                // When invoked by the serialization engine we can convert to string only for known type
                if (context != null && context.Instance != null)
                {
                    Keys key = (Keys)context.Instance;
                    return ((int)key >= (int)Keys.None/* && (int)key <= (int)Key.DeadCharProcessed*/);
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
        /// <ExternalAPI/> 
        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo culture, object source)
        {
            if (source is string)
                return FromString((string)source);
            throw GetConvertFromException(source);
        }

        /// <summary>
        /// Converts string representation of the key to the <see cref="Keys"/>
        /// enumeration.
        /// </summary>
        /// <param name="source">String representation of the key.</param>
        /// <returns>Key value parsed from string.</returns>
        /// <exception cref="NotSupportedException">
        /// Raised when string representation of the key is unknown.
        /// </exception>
        public static Keys FromString(string source)
        {
            string fullName = ((string)source).Trim();
            object key = GetKey(fullName, CultureInfo.InvariantCulture);
            if (key != null)
            {
                return ((Keys)key);
            }
            else
            {
                throw new NotSupportedException(
                    SR.Get(SRID.Unsupported_Key, fullName));
            }
        }

        /// <summary>
        /// ConvertTo()
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        /// <ExternalAPI/> 
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if (destinationType == typeof(string) && value != null)
            {
                Keys key = (Keys)value;
                if (key == Keys.None)
                {
                    return String.Empty;
                }

                if (key >= Keys.D0 && key <= Keys.D9)
                {
                    return Char.ToString((char)(int)(key - Keys.D0 + '0'));
                }

                if (key >= Keys.A && key <= Keys.Z)
                {
                    return Char.ToString((char)(int)(key - Keys.A + 'A'));
                }

                String strKey = MatchKey(key, culture);
                if (strKey != null)
                {
                    return strKey;
                }
            }
            throw GetConvertToException(value, destinationType);
        }

        private static object GetKey(string keyToken, CultureInfo culture)
        {
            if (keyToken.Length == 0)
            {
                return Keys.None;
            }
            else
            {
                keyToken = keyToken.ToUpper(culture);
                if (keyToken.Length == 1 && Char.IsLetterOrDigit(keyToken[0]))
                {
                    if (Char.IsDigit(keyToken[0]) && (keyToken[0] >= '0' && keyToken[0] <= '9'))
                    {
                        return ((int)(Keys)(Keys.D0 + keyToken[0] - '0'));
                    }
                    else if (Char.IsLetter(keyToken[0]) && (keyToken[0] >= 'A' && keyToken[0] <= 'Z'))
                    {
                        return ((int)(Keys)(Keys.A + keyToken[0] - 'A'));
                    }
                    else
                    {
                        throw new ArgumentException(SR.Get(SRID.CannotConvertStringToType, keyToken, typeof(Keys)));
                    }
                }
                else
                {
                    Keys keyFound = (Keys)(-1);
                    switch (keyToken)
                    {
                        case "ENTER": keyFound = Keys.Enter; break;
                        case "ESC": keyFound = Keys.Escape; break;
                        case "PGUP": keyFound = Keys.PageUp; break;
                        case "PGDN": keyFound = Keys.PageDown; break;
                        case "PRTSC": keyFound = Keys.PrintScreen; break;
                        case "INS": keyFound = Keys.Insert; break;
                        case "DEL": keyFound = Keys.Delete; break;
                        case "WINDOWS": keyFound = Keys.Windows; break;
                        case "WIN": keyFound = Keys.Windows; break;
                        case "BACKSPACE": keyFound = Keys.Backspace; break;
                        case "BKSP": keyFound = Keys.Backspace; break;
                        case "BS": keyFound = Keys.Backspace; break;
                        case "SHIFT": keyFound = Keys.Shift; break;
                        case "CONTROL": keyFound = Keys.Control; break;
                        case "CTRL": keyFound = Keys.Control; break;
                        case "ALT": keyFound = Keys.Alt; break;
                        case "SEMICOLON": keyFound = Keys.Semicolon; break;
                        case "COMMA": keyFound = Keys.Comma; break;
                        case "MINUS": keyFound = Keys.Minus; break;
                        case "PERIOD": keyFound = Keys.Period; break;
                        case "OPENBRACKETS": keyFound = Keys.OpenBracket; break;
                        case "CLOSEBRACKETS": keyFound = Keys.CloseBracket; break;
                        case "QUOTES": keyFound = Keys.Quote; break;
                        case "BACKSLASH": keyFound = Keys.Backslash; break;
                        case "PLAY": keyFound = Keys.MediaPlayPause; break;
                        default: keyFound = (Keys)Enum.Parse(typeof(Keys), keyToken, true); break;
                    }

                    if ((int)keyFound != -1)
                    {
                        return keyFound;
                    }
                    return null;
                }
            }
        }

        private static string MatchKey(Keys key, CultureInfo culture)
        {
            if (key == Keys.None)
                return String.Empty;
            else
            {
                switch (key)
                {
                    case Keys.Backspace: return "Backspace";
                    case Keys.Escape: return "Esc";
                }
            }
            if ((int)key >= (int)Keys.None/* && (int)key <= (int)Key.DeadCharProcessed*/)
                return key.ToString();
            else
                return null;
        }
    }
}

