using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Alternet.Drawing
{
    /// <summary>Converts brushes from one data type to another. Access this class through the <see cref="T:System.ComponentModel.TypeDescriptor" />.</summary>
    public class BrushConverter : TypeConverter
    {
        /// <summary>Determines if this converter can convert an object in the given source type to the native type of the converter.</summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. You can use this object to get additional information about the environment from which this converter is being invoked. </param>
        /// <param name="sourceType">The type from which you want to convert. </param>
        /// <returns>
        ///     <see langword="true" /> if this object can perform the conversion; otherwise, <see langword="false" />.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>Converts the given object to the converter's native type.</summary>
        /// <param name="context">A <see cref="T:System.ComponentModel.TypeDescriptor" /> that provides a format context. You can use this object to get additional information about the environment from which this converter is being invoked. </param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that specifies the culture to represent the brush. </param>
        /// <param name="value">The object to convert. </param>
        /// <returns>An <see cref="T:System.Object" /> representing the converted value.</returns>
        /// <exception cref="T:System.ArgumentException">The conversion cannot be performed.</exception>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string text)
            {
                string text2 = text.Trim();
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

                        char c = culture.TextInfo.ListSeparator[0];
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
                        if (text2.IndexOf(c) == -1)
                        {
                            if (text2.Length >= 2 && (text2[0] == '\'' || text2[0] == '"') && text2[0] == text2[text2.Length - 1])
                            {
                                string name = text2.Substring(1, text2.Length - 2);
                                obj = Brushes.TryGetBrush(name);
                            }
                            else if ((text2.Length == 7 && text2[0] == '#') || (text2.Length == 8 && (text2.StartsWith("0x") || text2.StartsWith("0X"))) || (text2.Length == 8 && (text2.StartsWith("&h") || text2.StartsWith("&H"))))
                            {
                                obj = new SolidBrush(Color.FromArgb(-16777216 |
                                    (int)converter.ConvertFromString(
                                        context,
                                        culture,
                                        text2)));
                            }
                        }

                        if (obj == null)
                        {
                            string[] array = text2.Split(new char[]
                            {
                                c,
                            });
                            int[] array2 = new int[array.Length];
                            for (int i = 0; i < array2.Length; i++)
                            {
                                array2[i] = (int)converter.ConvertFromString(
                                    context,
                                    culture,
                                    array[i]);
                            }

                            switch (array2.Length)
                            {
                                case 1:
                                    obj = new SolidBrush(Color.FromArgb(array2[0]));
                                    break;
                                case 3:
                                    obj = new SolidBrush(Color.FromArgb(array2[0], array2[1], array2[2]));
                                    break;
                                case 4:
                                    obj = new SolidBrush(Color.FromArgb(array2[0], array2[1], array2[2], array2[3]));
                                    break;
                            }
                        }
                    }

                    if (obj == null)
                    {
                        throw new ArgumentException("Invalid Brush:" + text2);
                    }
                }

                return obj;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
