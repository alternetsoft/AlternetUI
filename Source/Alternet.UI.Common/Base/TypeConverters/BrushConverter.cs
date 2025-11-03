using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>Converts brushes from one data type to another. Access this class through the
    /// <see cref="TypeDescriptor" />.</summary>
    public class BrushConverter : BaseTypeConverter
    {
        /// <summary>Determines if this converter can convert an object in the given source type
        /// to the native type of the converter.</summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext" />
        /// that provides a format context. You can use this object to get additional information
        /// about the environment from which this converter is being invoked. </param>
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

        /// <summary>Converts the given object to the converter's native type.</summary>
        /// <param name="context">A <see cref="TypeDescriptor" /> that
        /// provides a format context. You can use this object to get additional information about
        /// the environment from which this converter is being invoked. </param>
        /// <param name="culture">A <see cref="CultureInfo" /> that specifies
        /// the culture to represent the brush. </param>
        /// <param name="value">The object to convert. </param>
        /// <returns>An <see cref="object" /> representing the converted value.</returns>
        /// <exception cref="System.ArgumentException">The conversion cannot be performed.</exception>
        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object value)
        {
            if (value is not string text)
                return base.ConvertFrom(context, culture, value);

            string text2 = text.Trim();

            void ThrowError()
            {
                throw new ArgumentException("Invalid Brush:" + text2);
            }

            object? obj;
            if (text2.Length == 0)
            {
                obj = null;
            }
            else
            {
                obj = Brushes.TryGetBrush(text2);
                if (obj == null)
                {
                    culture ??= CultureInfo.CurrentCulture;

                    char listSeparator = culture.TextInfo.ListSeparator[0];
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
                    if (text2.IndexOf(listSeparator) == -1)
                    {
                        if (text2.Length >= 2
                            && (text2[0] == '\'' || text2[0] == '"')
                            && text2[0] == text2[text2.Length - 1])
                        {
                            string name = text2.Substring(1, text2.Length - 2);
                            obj = Brushes.TryGetBrush(name);
                        }
                        else
                        if ((text2.Length == 7 && text2[0] == '#')
                            || (text2.Length == 8 && (text2.StartsWith("0x")
                            || text2.StartsWith("0X")))
                            || (text2.Length == 8 && (text2.StartsWith("&h")
                            || text2.StartsWith("&H"))))
                        {
                            var convertedValue =
                                converter.ConvertFromString(context, culture, text2);
                            if (convertedValue is null)
                                ThrowError();
                            obj = new SolidBrush(Color.FromArgb(-16777216 | (int)convertedValue!));
                        }
                    }

                    if (obj == null)
                    {
                        string[] array = text2.Split(listSeparator);
                        int[] array2 = new int[array.Length];
                        for (int i = 0; i < array2.Length; i++)
                        {
                            var convertedValue = converter.ConvertFromString(context, culture, array[i]);
                            if (convertedValue is null)
                                ThrowError();
                            array2[i] = (int)convertedValue!;
                        }

                        switch (array2.Length)
                        {
                            case 1:
                                obj = new SolidBrush(Color.FromArgb(array2[0]));
                                break;
                            case 3:
                                obj = new SolidBrush(Color.FromArgb(
                                    array2[0],
                                    array2[1],
                                    array2[2]));
                                break;
                            case 4:
                                obj = new SolidBrush(Color.FromArgb(
                                    array2[0],
                                    array2[1],
                                    array2[2],
                                    array2[3]));
                                break;
                        }
                    }
                }

                if (obj == null)
                {
                    ThrowError();
                }
            }

            return obj;
        }
    }
}
