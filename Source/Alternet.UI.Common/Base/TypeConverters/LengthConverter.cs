using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Converter class for converting instances of other types to and from coordinate representing length.
    /// </summary>
    public class LengthConverter : BaseTypeConverter
    {
        // This array contains strings for unit types
        // These are effectively "TypeConverter only" units.
        // They are all expressed in terms of the Pixel unit type and a conversion factor.
        private static readonly string[] pixelUnitStrings =
        {
            "px",
            "in",
            "cm",
            "pt",
        };

        private static readonly Coord[] pixelUnitFactors =
        {
            // Pixel itself
            CoordD.One,

            // Pixels per Inch
            CoordD.Coord96,

            // Pixels per Centimeter
            CoordD.Coord96 / CoordD.Coord2And54,

            // Pixels per Point
            CoordD.Coord96 / CoordD.Coord72,
        };

        /// <summary>
        /// Converts the specified coordinate to its string representation using
        /// the specified culture-specific formatting.
        /// </summary>
        /// <param name="l">The coordinate to convert. If the value is not a number (NaN),
        /// the method returns "Auto".</param>
        /// <param name="cultureInfo">The culture-specific formatting information to use for
        /// the conversion.</param>
        /// <returns>A string representation of the coordinate formatted using the specified
        /// culture, or "Auto" if the coordinate
        /// is not a number (NaN).</returns>
        public static string ToString(Coord l, CultureInfo cultureInfo)
        {
            if (MathUtils.IsNaN(l))
                return "Auto";
            return Convert.ToString(l, cultureInfo);
        }

        /// <summary>
        /// Converts a string representation of a coordinate value, including an optional unit,
        /// into a <see cref="Coord"/> object.
        /// </summary>
        /// <remarks>This method is culture-sensitive for parsing the numeric value but uses a fixed,
        /// culture-invariant set of unit strings. Supported unit strings include "px" (pixels)
        /// and other predefined units. If the string does not end with a recognized unit,
        /// it is assumed to represent pixels.</remarks>
        /// <param name="s">The string to parse. The string should be in the format
        /// "[value][unit]", where <c>[value]</c> is a numeric
        /// value and <c>[unit]</c> is an optional unit specifier (e.g., "px", "in").
        /// If no unit is specified, the value
        /// is interpreted as pixels. The special value "auto" is interpreted
        /// as <see cref="Coord.NaN"/>.</param>
        /// <param name="cultureInfo">The <see cref="CultureInfo"/> used to interpret
        /// the numeric value in the string.</param>
        /// <returns>A <see cref="Coord"/> object representing the parsed value.
        /// If the string specifies "auto", <see cref="Coord.NaN"/> is returned.</returns>
        /// <exception cref="FormatException">Thrown if the string cannot be parsed
        /// into a valid coordinate value.</exception>
        public static Coord FromString(string s, CultureInfo cultureInfo)
        {
            /* NOTE - This code is called from FontSizeConverter, so changes will affect both. */

            string valueString = s.Trim();
            string goodString = valueString.ToLowerInvariant();
            int strLen = goodString.Length;
            int strLenUnit = 0;
            Coord unitFactor = CoordD.One;

            // Auto is represented and Coord.NaN
            // properties that do not want Auto and NaN to be in their legit values,
            // should disallow NaN in validation callbacks (same goes for negative values)
            if (goodString == "auto") return Coord.NaN;

            for (int i = 0; i < pixelUnitStrings.Length; i++)
            {
                // NOTE: This is NOT a culture specific comparison.
                // This is by design: we want the same unit string table to work across all cultures.
                if (goodString.EndsWith(pixelUnitStrings[i], StringComparison.Ordinal))
                {
                    strLenUnit = pixelUnitStrings[i].Length;
                    unitFactor = pixelUnitFactors[i];
                    break;
                }
            }

            // important to substring original non-lowered string
            // this allows case sensitive ToDouble/ToSingle below handle
            // "NaN" and "Infinity" correctly.
            // this addresses windows bug 1177408
            valueString = valueString.Substring(0, strLen - strLenUnit);

            // FormatException errors thrown by Convert.ToDouble are pretty uninformative.
            // Throw a more meaningful error in this case that tells that we were attempting
            // to create a Length instance from a string.  This addresses windows bug 968884
            try
            {
                Coord result = Convert.ToSingle(valueString, cultureInfo) * unitFactor;
                return result;
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(
            ITypeDescriptorContext? typeDescriptorContext,
            Type? sourceType)
        {
            // We can only handle strings, integral and floating types
            TypeCode tc = Type.GetTypeCode(sourceType);
            switch (tc)
            {
                case TypeCode.String:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(
            ITypeDescriptorContext? typeDescriptorContext,
            Type? destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) ||
                destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public override object ConvertFrom(
            ITypeDescriptorContext? typeDescriptorContext,
            CultureInfo cultureInfo,
            object source)
        {
            if (source != null)
            {
                if (source is string v)
                {
                    return FromString(v, cultureInfo);
                }
                else
                {
                    return (Coord)Convert.ToSingle(source, cultureInfo);
                }
            }

            throw GetConvertFromException(source);
        }

        /// <inheritdoc/>
        public override object ConvertTo(
            ITypeDescriptorContext? typeDescriptorContext,
            CultureInfo cultureInfo,
            object value,
            Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (value != null && value is Coord v)
            {
                Coord l = v;
                if (destinationType == typeof(string))
                {
                    if (MathUtils.IsNaN(l))
                        return "Auto";
                    else
                        return Convert.ToString(l, cultureInfo);
                }
                else if (destinationType == typeof(InstanceDescriptor))
                {
                    ConstructorInfo ci = typeof(Coord).GetConstructor(new Type[] { typeof(Coord) });
                    return new InstanceDescriptor(ci, new object[] { l });
                }
            }

            throw GetConvertToException(value, destinationType);
        }
    }
}