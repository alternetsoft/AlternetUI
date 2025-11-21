using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>Converts colors from one data type to another. Access this class through the
    /// <see cref="TypeDescriptor" />.</summary>
    public class ColorConverter : BaseTypeConverter
    {
        private static readonly string ColorConstantsLock = "colorConstants";
        private static readonly string SystemColorConstantsLock = "systemColorConstants";
        private static readonly string ValuesLock = "values";

        private static Hashtable? colorConstants;
        private static Hashtable? systemColorConstants;
        private static TypeConverter.StandardValuesCollection? values;

        private static Hashtable Colors
        {
            get
            {
                if (ColorConverter.colorConstants == null)
                {
                    string colorConstantsLock = ColorConverter.ColorConstantsLock;
                    lock (colorConstantsLock)
                    {
                        if (ColorConverter.colorConstants == null)
                        {
                            Hashtable hash = new(StringComparer.OrdinalIgnoreCase);
                            ColorConverter.FillConstants(hash, typeof(Color));
                            ColorConverter.colorConstants = hash;
                        }
                    }
                }

                return ColorConverter.colorConstants;
            }
        }

        private static Hashtable SystemColors
        {
            get
            {
                if (ColorConverter.systemColorConstants == null)
                {
                    string systemColorConstantsLock =
                        ColorConverter.SystemColorConstantsLock;
                    lock (systemColorConstantsLock)
                    {
                        if (ColorConverter.systemColorConstants == null)
                        {
                            Hashtable hash = new(StringComparer.OrdinalIgnoreCase);
                            ColorConverter.FillConstants(hash, typeof(SystemColors));
                            ColorConverter.systemColorConstants = hash;
                        }
                    }
                }

                return ColorConverter.systemColorConstants;
            }
        }

        /// <summary>
        /// Converts the specified color to its string representation, using the provided context and culture
        /// information if available.
        /// </summary>
        /// <remarks>For colors with an alpha value less than 255, the string includes the alpha
        /// component. The separator used between components is determined by the specified culture. This method is
        /// useful for serializing colors for display or storage in a culture-sensitive format.</remarks>
        /// <param name="color">The color to convert to a string. If the color is empty, an empty string is returned.</param>
        /// <param name="context">An optional type descriptor context that can provide additional information about the conversion.
        /// May be null.</param>
        /// <param name="culture">An optional culture information object used to format the string output. If null,
        /// the invariant culture is used.</param>
        /// <returns>A string representation of the color. Returns the color's name for known or named colors, or a
        /// comma-separated list of ARGB components for other colors. Returns an empty string if the color is empty.
        /// Returns null if the color is null.</returns>
        public static string? ConvertToString(
            Color? color,
            ITypeDescriptorContext? context = null,
            CultureInfo? culture = null)
        {
            if (color == null)
            {
                return null;
            }

            if (color == Color.Empty)
            {
                return string.Empty;
            }

            if (color.IsKnownColor)
            {
                return color.Name;
            }

            if (color.IsNamedColor)
            {
                return "'" + color.Name + "'";
            }

            culture ??= CultureInfo.InvariantCulture;

            string separator = culture.TextInfo.ListSeparator + " ";
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
            int num = 0;
            string[] array;

            SKColor v = color;

            if (v.Alpha < 255)
            {
                array = new string[4];
                array[num++] =
                    converter.ConvertToString(context, culture, v.Alpha)!;
            }
            else
            {
                array = new string[3];
            }

            array[num++] =
                converter.ConvertToString(context, culture, v.Red)!;
            array[num++] =
                converter.ConvertToString(context, culture, v.Green)!;
            array[num++] =
                converter.ConvertToString(context, culture, v.Blue)!;
            return string.Join(separator, array);
        }

        /// <summary>Determines if this converter can convert an object in the given source type
        /// to the native type of the converter.</summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext" /> that provides a
        /// format context. You can use this object to get additional information about
        /// the environment from which this converter is being invoked. </param>
        /// <param name="sourceType">The type from which you want to convert. </param>
        /// <returns>
        ///     <see langword="true" /> if this object can perform the conversion; otherwise,
        ///     <see langword="false" />.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
        {
            if (sourceType == null)
                return false;
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>Returns a value indicating whether this converter can convert an object to
        /// the given destination type using the context.</summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext" /> that provides a
        /// format context. </param>
        /// <param name="destinationType">A <see cref="System.Type" /> that represents the
        /// type to which you want to convert. </param>
        /// <returns>
        ///     <see langword="true" /> if this converter can perform the operation; otherwise,
        ///     <see langword="false" />.</returns>
        public override bool CanConvertTo(
            ITypeDescriptorContext? context,
            Type? destinationType)
        {
            return destinationType == typeof(InstanceDescriptor) ||
                base.CanConvertTo(context, destinationType);
        }

        /// <summary>Converts the given object to the converter's native type.</summary>
        /// <param name="context">A <see cref="TypeDescriptor" /> that provides a format context.
        /// You can use this object to get additional information about the environment from
        /// which this converter is being invoked. </param>
        /// <param name="culture">A <see cref="System.Globalization.CultureInfo" /> that
        /// specifies the culture to represent the color. </param>
        /// <param name="value">The object to convert. </param>
        /// <returns>An <see cref="object" /> representing the converted
        /// value.</returns>
        /// <exception cref="System.ArgumentException">The conversion cannot be
        /// performed.</exception>
        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object value)
        {
            if (value is string text)
                return ColorFromString(context, culture, text);
            else
                return base.ConvertFrom(context, culture, value);
        }

        /// <summary>Converts the specified object to another type. </summary>
        /// <param name="context">A formatter context. Use this object to extract
        /// additional information about the environment from which this converter is being invoked.
        /// Always check whether this value is <see langword="null" />. Also, properties on
        /// the context object may return <see langword="null" />. </param>
        /// <param name="culture">A <see cref="System.Globalization.CultureInfo" /> that
        /// specifies the culture to represent the color. </param>
        /// <param name="value">The object to convert. </param>
        /// <param name="destinationType">The type to convert the object to. </param>
        /// <returns>An <see cref="object" /> representing the converted value.</returns>
        /// <exception cref="System.ArgumentNullException">
        ///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
        /// <exception cref="System.NotSupportedException">The conversion cannot be
        /// performed.</exception>
        public override object? ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType)
        {
            if (value is Color color1)
            {
                if (destinationType == typeof(string))
                {
                    return ConvertToString(color1, context, culture) ?? string.Empty;
                }
                else if (destinationType == typeof(InstanceDescriptor))
                {
                    object[]? arguments = null;
                    Color color = color1;
                    MemberInfo? memberInfo;
                    if (color.IsEmpty)
                    {
                        memberInfo = typeof(Color).GetField("Empty");
                    }
                    else if (color.IsSystemColor)
                    {
                        memberInfo = AssemblyUtils.GetFirstMember(typeof(SystemColors), color.Name);
                    }
                    else if (color.IsKnownColor)
                    {
                        memberInfo = AssemblyUtils.GetFirstMember(typeof(Color), color.Name);
                    }
                    else if (color.A != 255)
                    {
                        Type[] memberInfoTypes = new Type[]
                        {
                                typeof(int),
                                typeof(int),
                                typeof(int),
                                typeof(int),
                        };
                        memberInfo = typeof(Color).GetMethod(
                            "FromArgb",
                            types: memberInfoTypes);
                        arguments = new object[]
                        {
                            color.A,
                            color.R,
                            color.G,
                            color.B,
                        };
                    }
                    else if (color.IsNamedColor)
                    {
                        memberInfo = typeof(Color).GetMethod(
                            "FromName",
                            new Type[] { typeof(string) });
                        arguments = new object[] { color.Name };
                    }
                    else
                    {
                        Type[] memberInfoTypes2 = new Type[]
                            {
                                typeof(int),
                                typeof(int),
                                typeof(int),
                            };
                        memberInfo = typeof(Color).GetMethod(
                            "FromArgb",
                            memberInfoTypes2);
                        arguments = new object[]
                        {
                            color.R,
                            color.G,
                            color.B,
                        };
                    }

                    if (memberInfo != null)
                    {
                        return new InstanceDescriptor(memberInfo, arguments);
                    }

                    return null;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>Retrieves a collection containing a set of standard values for the data
        /// type for which this validator is designed. This will return <see langword="null" />
        /// if the data type does not support a standard set of values.</summary>
        /// <param name="context">A formatter context. Use this object to extract
        /// additional information about the environment from which this converter is being invoked.
        /// Always check whether this value is <see langword="null" />. Also, properties
        /// on the context object may return <see langword="null" />. </param>
        /// <returns>A collection containing <see langword="null" /> or a standard set
        /// of valid values. The default implementation always returns
        /// <see langword="null" />.</returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(
            ITypeDescriptorContext? context)
        {
            if (ColorConverter.values == null)
            {
                string valuesLock = ColorConverter.ValuesLock;
                lock (valuesLock)
                {
                    if (ColorConverter.values == null)
                    {
                        ArrayList arrayList = new();
                        arrayList.AddRange(ColorConverter.Colors.Values);
                        arrayList.AddRange(ColorConverter.SystemColors.Values);
                        int num = arrayList.Count;
                        for (int i = 0; i < num - 1; i++)
                        {
                            for (int j = i + 1; j < num; j++)
                            {
                                if (arrayList[i]!.Equals(arrayList[j]))
                                {
                                    arrayList.RemoveAt(j);
                                    num--;
                                    j--;
                                }
                            }
                        }

                        arrayList.Sort(0, arrayList.Count, new ColorConverter.ColorComparer());
                        ColorConverter.values =
                            new TypeConverter.StandardValuesCollection(arrayList.ToArray());
                    }
                }
            }

            return ColorConverter.values;
        }

        /// <summary>Determines if this object supports a standard set of values that can be
        /// chosen from a list.</summary>
        /// <param name="context">A <see cref="TypeDescriptor" /> through which additional
        /// context can be provided. </param>
        /// <returns>
        ///     <see langword="true" /> if
        ///     <see cref="ColorConverter.GetStandardValues" /> must be called to find
        ///     a common set of values the object supports; otherwise,
        ///     <see langword="false" />.</returns>
        public override bool GetStandardValuesSupported(
            ITypeDescriptorContext? context)
        {
            return true;
        }

        /// <summary>
        /// Converts <see cref="string"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext" /> that provides a
        /// format context. You can use this object to get additional information about
        /// the environment from which this converter is being invoked. </param>
        /// <param name="culture">A <see cref="System.Globalization.CultureInfo" /> that
        /// specifies the culture to represent the color. </param>
        /// <param name="text">The string to convert.</param>
        /// <returns></returns>
        internal static Color? ColorFromString(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            string text)
        {
            object? obj;
            string text2 = text.Trim();
            if (text2.Length == 0)
            {
                obj = Color.Empty;
            }
            else
            {
                obj = ColorConverter.GetNamedColor(text2);
                if (obj == null)
                {
                    culture ??= CultureInfo.CurrentCulture;

                    char c = culture.TextInfo.ListSeparator[0];
                    bool flag = true;
                    TypeConverter converter =
                        TypeDescriptor.GetConverter(typeof(int));
#pragma warning disable
                    if (text2.IndexOf(c) == -1)
#pragma warning restore
                    {
#pragma warning disable
                        if (text2.Length >= 2 && (text2[0] == '\'' ||
                            text2[0] == '"') && text2[0] == text2[text2.Length - 1])
#pragma warning restore
                        {
#pragma warning disable
                            string name = text2.Substring(1, text2.Length - 2);
#pragma warning restore
                            obj = Color.FromName(name);
                            flag = false;
                        }
                        else if ((text2.Length == 7 && text2[0] == '#') ||
                            (text2.Length == 8 && (text2.StartsWith("0x") ||
                            text2.StartsWith("0X"))) ||
                            (text2.Length == 8 && (text2.StartsWith("&h") ||
                            text2.StartsWith("&H"))))
                        {
                            obj = Color.FromArgb(-16777216 |
                                (int)converter.ConvertFromString(
                                    context,
                                    culture,
                                    text2)!);
                        }
                    }

                    if (obj == null)
                    {
                        string[] array = text2.Split(c);
                        int[] array2 = new int[array.Length];
                        for (int i = 0; i < array2.Length; i++)
                        {
                            array2[i] = (int)converter.ConvertFromString(
                                context,
                                culture,
                                array[i])!;
                        }

                        switch (array2.Length)
                        {
                            case 1:
                                obj = Color.FromArgb(array2[0]);
                                break;
                            case 3:
                                obj = Color.FromArgb(
                                    array2[0],
                                    array2[1],
                                    array2[2]);
                                break;
                            case 4:
                                obj = Color.FromArgb(
                                    array2[0],
                                    array2[1],
                                    array2[2],
                                    array2[3]);
                                break;
                        }

                        flag = true;
                    }

                    if (obj != null && flag)
                    {
                        int num = ((Color)obj).ToArgb();
                        foreach (object? obj2 in ColorConverter.Colors.Values)
                        {
                            if (obj2 == null)
                                throw new InvalidOperationException();

                            Color color = (Color)obj2;
                            if (color.ToArgb() == num)
                            {
                                obj = color;
                                break;
                            }
                        }
                    }
                }

                if (obj == null)
                {
                    throw new ArgumentException("Invalid Color:" + text2);
                }
            }

            return obj as Color;
        }

        internal static object? GetNamedColor(string name)
        {
            object? obj = ColorConverter.Colors[name];
            if (obj != null)
            {
                return obj;
            }

            return ColorConverter.SystemColors[name];
        }

        private static void FillConstants(Hashtable hash, Type enumType)
        {
            foreach (var propertyInfo in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (propertyInfo.FieldType == typeof(Color))
                {
                    hash[propertyInfo.Name] = propertyInfo.GetValue(null);
                }
            }
        }

        private class ColorComparer : IComparer
        {
            public int Compare(object? left, object? right)
            {
                if (left == null)
                    throw new ArgumentNullException(nameof(left));
                if (right == null)
                    throw new ArgumentNullException(nameof(right));

                Color color = (Color)left;
                Color color2 = (Color)right;

                return string.Compare(color.Name, color2.Name, false, CultureInfo.InvariantCulture);
            }
        }
    }
}
