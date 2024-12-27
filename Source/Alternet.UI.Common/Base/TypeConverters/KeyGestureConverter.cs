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
    public class KeyGestureConverter : BaseTypeConverter
    {
        /// <summary>
        /// Gets delimiter character between key and modifiers.
        /// </summary>
        public const char ModifiersDelimiter = '+';

        /// <summary>
        /// Gets separator character between display string and the key.
        /// </summary>
        public const char DisplayStringSeparator = ',';

        /// <summary>
        /// Gets or sets default type converter for <see cref="KeyGesture"/>.
        /// </summary>
        public static TypeConverter Default = new KeyGestureConverter();

        /// <summary>
        /// Check for Valid enum, as any int can be casted to the enum.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsDefinedKey(Key key)
        {
            return key >= Key.None && key <= Key.Menu;
        }

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
                return new KeyGesture(Key.None);

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

                return new KeyGesture((Key)resultkey, modifiers, displayString);
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
            return sourceType == typeof(string);
        }

        /// <summary>
        /// Gets a value indicating whether this converter can convert an object to the given
        /// destination type using the context.
        /// </summary>
        public override bool CanConvertTo(
            ITypeDescriptorContext? context,
            Type? destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (context?.Instance is KeyGesture keyGesture)
                {
                    return keyGesture.IsValid();
                }
                else
                    return true;
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
                        if (keyGesture.Key == Key.None)
                            return string.Empty;

                        string strBinding = string.Empty;
                        string? strKey = KeyConverter.Default
                            .ConvertTo(context, culture, keyGesture.Key, destinationType) as string;
                        if (!string.IsNullOrEmpty(strKey))
                        {
                            strBinding += ModifierKeysConverter.Default.ConvertTo(
                                context,
                                culture,
                                keyGesture.Modifiers,
                                destinationType) as string;

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
    }
}