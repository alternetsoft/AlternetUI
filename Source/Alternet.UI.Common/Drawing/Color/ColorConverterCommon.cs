// This code is based on the .NET implementation.
// We thank the .NET team for making it available under the MIT license.
// We have modified this code to fit our needs.

#nullable enable

using System;
using System.Globalization;

namespace Alternet.Drawing
{
    // Minimal color conversion functionality, without a dependency on TypeConverter itself.
    internal static class ColorConverterCommon
    {
        public static Color ConvertFromString(string strValue, CultureInfo culture)
        {
            culture ??= CultureInfo.CurrentCulture;
            string text = strValue.Trim();
            if (text.Length == 0)
                return Color.Empty;

            var c = NamedColors.GetColorOrNull(text);

            // First, check to see if this is a standard name.
            if (c is not null)
                return c;

            char sep = culture.TextInfo.ListSeparator[0];

            // If the value is a 6 digit hex number only, then
            // we want to treat the Alpha as 255, not 0
            if (!text.Contains(new string(sep, 1)))
            {
                // text can be '' (empty quoted string)
#pragma warning disable
                if (text.Length >= 2 && (text[0] == '\'' || text[0] == '"')
                    && text[0] == text[text.Length - 1])
#pragma warning restore
                {
                    // In quotes means a named value
#pragma warning disable
                    string colorName = text.Substring(1, text.Length - 2);
#pragma warning restore
                    return Color.FromName(colorName);
                }
                else if ((text.Length == 7 && text[0] == '#') ||
                         (text.Length == 8 && (text.StartsWith("0x") || text.StartsWith("0X"))) ||
                         (text.Length == 8 && (text.StartsWith("&h") || text.StartsWith("&H"))))
                {
                    // Note: int.Parse will raise exception if value cannot be converted.
                    return Color.FromArgb(
                        unchecked((int)(0xFF000000 | (uint)IntFromString(text, culture))));
                }
            }

            // Nope. Parse the RGBA from the text.
            string[] tokens = text.Split(sep);
            int[] values = new int[tokens.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = unchecked(IntFromString(tokens[i], culture));
            }

            // We should now have a number of parsed integer values.
            // We support 1, 3, or 4 arguments:
            //
            // 1 -- full ARGB encoded
            // 3 -- RGB
            // 4 -- ARGB
            return values.Length switch
            {
                1 => Color.FromArgb(values[0]),
                3 => Color.FromArgb(values[0], values[1], values[2]),
                4 => Color.FromArgb(values[0], values[1], values[2], values[3]),
                _ => throw new ArgumentException("Invalid color: " + text),
            };
        }

        private static int IntFromString(string text, CultureInfo culture)
        {
            if (culture is null)
                throw new ArgumentNullException(nameof(culture));

            text = text.Trim();

            try
            {
                if (text[0] == '#')
                {
#pragma warning disable
                    return IntFromString(text.Substring(1), 16);
#pragma warning restore
                }
                else if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                         || text.StartsWith("&h", StringComparison.OrdinalIgnoreCase))
                {
#pragma warning disable
                    return IntFromString(text.Substring(2), 16);
#pragma warning restore
                }
                else
                {
                    var formatInfo = (NumberFormatInfo?)culture.GetFormat(typeof(NumberFormatInfo));
                    return IntFromString(text, formatInfo);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    string.Format(
                        "ConvertInvalidPrimitive={0} is not a valid value for {1}.",
                        text,
                        nameof(Int32)),
                    e);
            }
        }

        private static int IntFromString(string value, int radix)
        {
            return Convert.ToInt32(value, radix);
        }

        private static int IntFromString(string value, NumberFormatInfo? formatInfo)
        {
            return int.Parse(value, NumberStyles.Integer, formatInfo);
        }
    }
}
