using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains helper properties and methods for the value conversion.
    /// </summary>
    public static partial class ConversionUtils
    {
        /// <summary>
        /// Converts <see cref="InnerOuterSelector"/> to the tuple with two boolean values.
        /// </summary>
        public static (bool Outer, bool Inner) FromInnerOuterSelector(InnerOuterSelector selector)
        {
            (bool Outer, bool Inner) result;

            switch (selector)
            {
                default:
                case InnerOuterSelector.None:
                    result.Outer = false;
                    result.Inner = false;
                    break;
                case InnerOuterSelector.Inner:
                    result.Outer = false;
                    result.Inner = true;
                    break;
                case InnerOuterSelector.Outer:
                    result.Outer = true;
                    result.Inner = false;
                    break;
                case InnerOuterSelector.Both:
                    result.Outer = true;
                    result.Inner = true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Converts <paramref name="hasOuter"/> and <paramref name="hasInner"/> boolean
        /// parameters to <see cref="InnerOuterSelector"/>.
        /// </summary>
        /// <param name="hasOuter">Whether outer is selected.</param>
        /// <param name="hasInner">Whether inner is selected.</param>
        /// <returns></returns>
        public static InnerOuterSelector ToInnerOuterSelector(bool hasOuter, bool hasInner)
        {
            if (hasOuter)
            {
                if (hasInner)
                {
                    return InnerOuterSelector.Both;
                }
                else
                {
                    return InnerOuterSelector.Outer;
                }
            }
            else
            {
                if (hasInner)
                {
                    return InnerOuterSelector.Inner;
                }
                else
                {
                    return InnerOuterSelector.None;
                }
            }
        }

        /// <summary>
        /// Parses string with 1, 2 or 4 float values separated by the
        /// comma character or other separator.
        /// All whitespaces and brackets are trimmed before parsing is done.
        /// Null is returned if error occurs during parsing or number of parsed floats
        /// is different from 1, 2 or 4.
        /// </summary>
        /// <param name="s">String to parse.</param>
        /// <param name="formatProvider">Optional <see cref="IFormatProvider"/> with
        /// string format settings. If not specified, <see cref="App.InvariantEnglishUS"/>
        /// is used.</param>
        /// <returns></returns>
        public static float[]? ParseOneOrTwoOrFourFloats(
            string? s,
            IFormatProvider? formatProvider = null)
        {
            s = StringUtils.TrimWhitespaceAndBrackets(s);

            using var tokenizer = new StringTokenizer(s, App.InvariantEnglishUS);

            if (tokenizer.TryReadSingle(out var a))
            {
                if (tokenizer.TryReadSingle(out var b))
                {
                    if (tokenizer.TryReadSingle(out var c))
                    {
                        if (tokenizer.TryReadSingle(out var d))
                            return [a, b, c, d];

                        return null;
                    }

                    return [a, b];
                }

                return [a];
            }

            return null;
        }

        /// <summary>
        /// Parses string with two float values separated by the comma character or other separator.
        /// All whitespaces and brackets are trimmed before parsing is done.
        /// </summary>
        /// <param name="source">String to parse.</param>
        /// <param name="formatProvider">Optional <see cref="IFormatProvider"/> with
        /// string format settings. If not specified, <see cref="App.InvariantEnglishUS"/>
        /// is used.</param>
        /// <returns></returns>
        public static (float, float) ParseTwoFloats(
            string? source,
            IFormatProvider? formatProvider = null)
        {
            source = StringUtils.TrimWhitespaceAndBrackets(source);

            formatProvider ??= App.InvariantEnglishUS;

            TokenizerHelper th = new(source, formatProvider);

            (float, float) value;

            string firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty" || string.IsNullOrEmpty(firstToken))
            {
                value = (0, 0);
            }
            else
            {
                value = (
                    Convert.ToSingle(firstToken, formatProvider),
                    Convert.ToSingle(th.NextTokenRequired(), formatProvider));
            }

            // There should be no more tokens in this string.
            th.LastTokenRequired();

            return value;
        }

        /// <summary>
        /// Parses string with four float values separated by the comma character or other separator.
        /// All whitespaces and brackets are trimmed before parsing is done.
        /// </summary>
        /// <param name="source">String to parse.</param>
        /// <param name="formatProvider">Optional <see cref="IFormatProvider"/> with
        /// string format settings. If not specified, <see cref="App.InvariantEnglishUS"/>
        /// is used.</param>
        /// <returns></returns>
        public static (float, float, float, float) ParseFourFloats(
            string? source,
            IFormatProvider? formatProvider = null)
        {
            source = StringUtils.TrimWhitespaceAndBrackets(source);

            formatProvider ??= App.InvariantEnglishUS;

            TokenizerHelper th = new(source, formatProvider);

            (float, float, float, float) value;

            string firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty" || string.IsNullOrEmpty(firstToken))
            {
                value = (0, 0, 0, 0);
            }
            else
            {
                value = (
                    Convert.ToSingle(firstToken, formatProvider),
                    Convert.ToSingle(th.NextTokenRequired(), formatProvider),
                    Convert.ToSingle(th.NextTokenRequired(), formatProvider),
                    Convert.ToSingle(th.NextTokenRequired(), formatProvider));
            }

            // There should be no more tokens in this string.
            th.LastTokenRequired();

            return value;
        }

        /// <summary>
        /// Calls the specified conversion action.
        /// </summary>
        /// <typeparam name="TConvertFrom">Type of the value to convert from.</typeparam>
        /// <typeparam name="TConvertTo">Type of the value to convert to.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="convert">Conversion action.</param>
        /// <param name="result">Result of the conversion.</param>
        /// <returns>True if value was converted using the conversion event;
        /// False otherwise.</returns>
        public static bool ConvertWithAction<TConvertFrom, TConvertTo>(
            TConvertFrom? value,
            Action<ValueConvertEventArgs<TConvertFrom?, TConvertTo?>>? convert,
            out TConvertTo? result)
        {
            if (convert is not null)
            {
                var e = new ValueConvertEventArgs<TConvertFrom?, TConvertTo?>(value);
                convert(e);
                if (e.Handled)
                {
                    result = e.Result;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
