#pragma warning disable
#nullable disable

using System;
using System.Diagnostics;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    ///     XamlGridLengthSerializer is used to persist a GridLength structure in Baml files
    /// </summary>
    internal class XamlGridLengthSerializer
    {
        // Used to emit Definitions namespace prefix
        internal const string DefNamespacePrefix = "x";

        // Used to emit Definitions namespace
        internal const string DefNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

        // Used to emit the x:Array tag
        internal const string ArrayTag = "Array";

        // Used to emit the x:Type attribute for Array
        internal const string ArrayTagTypeAttribute = "Type";

        #region Construction

        /// <summary>
        ///     Constructor for XamlGridLengthSerializer
        /// </summary>
        /// <remarks>
        ///     This constructor will be used under
        ///     the following two scenarios
        ///     1. Convert a string to a custom binary representation stored in BAML
        ///     2. Convert a custom binary representation back into a GridLength
        /// </remarks>
        private XamlGridLengthSerializer()
        {
        }


        #endregion Construction

        #region Conversions


        // Parse a GridLength from a string given the CultureInfo.
        static internal void FromString(
                string s,
                CultureInfo cultureInfo,
            out Coord value,
            out GridUnitType unit)
        {
            string goodString = s.Trim().ToLowerInvariant();

            value = 0.0f;
            unit = GridUnitType.Pixel;

            int i;
            int strLen = goodString.Length;
            int strLenUnit = 0;
            Coord unitFactor = 1.0f;

            //  this is where we would handle trailing whitespace on the input string.
            //  peel [unit] off the end of the string
            i = 0;

            if (goodString == UnitStrings[i])
            {
                strLenUnit = UnitStrings[i].Length;
                unit = (GridUnitType)i;
            }
            else
            {
                for (i = 1; i < UnitStrings.Length; ++i)
                {
                    //  Note: this is NOT a culture specific comparison.
                    //  this is by design: we want the same unit string table to work across all cultures.
                    if (goodString.EndsWith(UnitStrings[i], StringComparison.Ordinal))
                    {
                        strLenUnit = UnitStrings[i].Length;
                        unit = (GridUnitType)i;
                        break;
                    }
                }
            }

            //  we couldn't match a real unit from GridUnitTypes.
            //  try again with a converter-only unit (a pixel equivalent).
            if (i >= UnitStrings.Length)
            {
                for (i = 0; i < PixelUnitStrings.Length; ++i)
                {
                    //  Note: this is NOT a culture specific comparison.
                    //  this is by design: we want the same unit string table to work across all cultures.
                    if (goodString.EndsWith(PixelUnitStrings[i], StringComparison.Ordinal))
                    {
                        strLenUnit = PixelUnitStrings[i].Length;
                        unitFactor = PixelUnitFactors[i];
                        break;
                    }
                }
            }

            //  this is where we would handle leading whitespace on the input string.
            //  this is also where we would handle whitespace between [value] and [unit].
            //  check if we don't have a [value].  This is acceptable for certain UnitTypes.
            if (strLen == strLenUnit
                && (unit == GridUnitType.Auto
                    || unit == GridUnitType.Star))
            {
                value = 1;
            }
            //  we have a value to parse.
            else
            {
                Debug.Assert(unit == GridUnitType.Pixel
                            || MathUtils.AreClose(unitFactor, 1.0));

//                ReadOnlySpan<char> valueString = goodString.AsSpan(0, strLen - strLenUnit);
                var valueString = goodString.Substring(0, strLen - strLenUnit);
                value = Coord.Parse(valueString, provider: cultureInfo) * unitFactor;
            }
        }


        #endregion Conversions

        #region Fields

        //  Note: keep this array in sync with the GridUnitType enum
        static private string[] UnitStrings = { "auto", "px", "*" };

        //  this array contains strings for unit types that are not present in the GridUnitType enum
        static private string[] PixelUnitStrings = { "in", "cm", "pt" };
        static private Coord[] PixelUnitFactors =
        {
            96.0f,             // Pixels per Inch
            96.0f / 2.54f,      // Pixels per Centimeter
            96.0f / 72.0f,      // Pixels per Point
        };

        #endregion Fields
    }
}