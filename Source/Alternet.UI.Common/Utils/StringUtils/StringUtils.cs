using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines delegate type for the methods that convert the string representation of
    /// a number to its number equivalent.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values
    /// that indicates
    /// the permitted format of <paramref name="s"/>.</param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information
    /// about <paramref name="s"/>.
    /// </param>
    /// <param name="result">
    /// When this method returns and if the conversion succeeded, contains a number equivalent
    /// of the numeric value or symbol contained in <paramref name="s"/>.
    /// Contains default value for the type if the conversion failed.
    /// </param>
    /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
    public delegate bool TryParseNumberDelegate(
        string? s,
        NumberStyles style,
        IFormatProvider? provider,
        out object? result);

    /// <summary>
    /// Contains <see cref="string"/> related static methods.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Represents the symbol "Wg".
        /// </summary>
        /// <remarks>This field is a constant string value and is
        /// intended to be used as a predefined symbol.</remarks>
        public static readonly string SymbolWg = "Wg";

        /// <summary>
        /// Represents the lowercase 'x' symbol used for layout, diagnostics, or interop.
        /// </summary>
        public static readonly string SymbolLowercaseX = "x";

        /// <summary>
        /// Gets the prefix string for CDATA sections.
        /// </summary>
        public static readonly string PrefixCDATA = "<![CDATA[";

        /// <summary>
        /// Gets the suffix string for CDATA sections.
        /// </summary>
        public static readonly string SuffixCDATA = "]]>";

        /// <summary>
        /// Gets html start bold tag string constant.
        /// </summary>
        public static readonly string BoldTagStart = "<b>";

        /// <summary>
        /// Gets html end bold tag string constant.
        /// </summary>
        public static readonly string BoldTagEnd = "</b>";

        /// <summary>
        /// Gets initialized string with one space character.
        /// </summary>
        public static readonly string OneSpace = " ";

        /// <summary>
        /// Gets initialized string with four space characters.
        /// </summary>
        public static readonly string FourSpaces = "    ";

        /// <summary>
        /// Gets initialized string with one double quote character (").
        /// </summary>
        public static readonly string OneDoubleQuote = "\"";

        /// <summary>
        /// Gets initialized string with one carriage return.
        /// </summary>
        public static readonly string OneCarriageReturn = "\r";

        /// <summary>
        /// Gets initialized string with one new line character.
        /// </summary>
        public static readonly string OneNewLine = "\n";

        /// <summary>
        /// Gets initialized string with three dots.
        /// </summary>
        public static readonly string ThreeDots = "...";

        /// <summary>
        /// Gets title of the command key on MacOs (0x2318 character).
        /// </summary>
        public static readonly string MacCommandKeyTitle = "\u2318";

        /// <summary>
        /// Gets brackets characters used in <see cref="TrimWhitespaceAndBrackets"/>
        /// and some other places.
        /// </summary>
        public static char[] BracketsChars = ['(', ')', '[', ']', '{', '}', '<', '>',];

        /// <summary>
        /// Gets or sets array of delegates used for the text to number conversion.
        /// </summary>
        public static TryParseNumberDelegate[] TryParseNumberDelegates =
        [
            StringUtils.TryParseInt32,
            StringUtils.TryParseUInt32,
            StringUtils.TryParseInt64,
            StringUtils.TryParseUInt64,
            StringUtils.TryParseDouble,
            StringUtils.TryParseDecimal,
        ];

        /// <summary>
        /// Gets or sets values that split one string to many.
        /// </summary>
        /// <remarks>
        /// This field must contain at least CR, LF, CRLF, LFCR strings
        /// and <see cref="Environment.NewLine"/>.
        /// </remarks>
        public static string[] StringSplitToArrayChars =
        {
            Environment.NewLine,
            "\r\n",
            "\n\r",
            "\n",
            "\r",
        };

        private static IComparer<object>? comparerObjectUsingToString;

        static StringUtils()
        {
            Test();
        }

        /// <summary>
        /// Returns <see cref="IComparer{T}"/> which converts objects to strings using
        /// <see cref="object.ToString"/> and uses
        /// <see cref="string.Compare(string?, string?, StringComparison)"/> for comparing.
        /// </summary>
        public static IComparer<object> ComparerObjectUsingToString
        {
            get
            {
                comparerObjectUsingToString ??= new ComparerUsingToString<object>();
                return comparerObjectUsingToString;
            }
        }

        /// <summary>
        /// Determines whether the specified string contains CR (\r) or LF (\n) characters.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <returns><c>true</c> if the string contains CR or LF
        /// characters; otherwise, <c>false</c>.</returns>
        public static bool ContainsNewLineChars(string input)
        {
            if (input is null || input.Length == 0)
                return false;
            return input.AsSpan().IndexOfAny('\r', '\n') >= 0;
        }

        /// <summary>
        /// Modifies specified string by adding prefix and suffix.
        /// </summary>
        /// <param name="s">String to modify.</param>
        /// <param name="prefixAndSuffix">Prefix and suffix string.</param>
        /// <returns>Modified string with prefix and suffix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string PrefixSuffix(string s, string prefixAndSuffix)
        {
            return $"{prefixAndSuffix}{s}{prefixAndSuffix}";
        }

        /// <summary>
        /// Gets whether the specified string has only valid chars.
        /// </summary>
        /// <param name="s">String to check.</param>
        /// <param name="isOk">Function used to check whether character is valid.</param>
        /// <returns></returns>
        public static bool HasOnlyValidChars(string? s, Func<char, bool> isOk)
        {
            if (s is null)
                return true;
            var length = s.Length;
            for (int i = 0; i < length; i++)
            {
                if (!isOk(s[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets whether the specified character is English char or dot.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsEnglishCharOrDot(char c)
        {
            return c == '.' || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        /// <summary>
        /// Changes <see cref="StringComparison"/> value to use <paramref name="ignoreCase"/>.
        /// </summary>
        /// <param name="comparisonType">Value to change.</param>
        /// <param name="ignoreCase">Whether to ignore case.</param>
        /// <returns></returns>
        public static StringComparison OverrideIgnoreCase(
            StringComparison comparisonType,
            bool ignoreCase)
        {
            if (ignoreCase)
            {
                switch (comparisonType)
                {
                    case StringComparison.CurrentCulture:
                        return StringComparison.CurrentCultureIgnoreCase;
                    case StringComparison.InvariantCulture:
                        return StringComparison.InvariantCultureIgnoreCase;
                    case StringComparison.Ordinal:
                        return StringComparison.OrdinalIgnoreCase;
                }
            }
            else
            {
                switch (comparisonType)
                {
                    case StringComparison.CurrentCultureIgnoreCase:
                        return StringComparison.CurrentCulture;
                    case StringComparison.InvariantCultureIgnoreCase:
                        return StringComparison.InvariantCulture;
                    case StringComparison.OrdinalIgnoreCase:
                        return StringComparison.Ordinal;
                }
            }

            return comparisonType;
        }

        /// <summary>
        /// Gets whether string <paramref name="s"/> starts with one of the strings
        /// specified in <paramref name="items"/> collection.
        /// </summary>
        /// <typeparam name="T">Type of the item in the collection.</typeparam>
        /// <param name="s">String to test.</param>
        /// <param name="items">Collection.</param>
        /// <returns></returns>
        public static bool StartsWith<T>(string s, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                var value = item?.ToString();
                if (value is null)
                    continue;
                if (s.StartsWith(value))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Limits the length of text to a defined length.
        /// </summary>
        /// <param name="text">The source text.</param>
        /// <param name="maxLength">The maximum limit of the string to return.</param>
        public static string? LimitLength(string? text, int maxLength)
        {
            if (text is null || text.Length <= maxLength)
                return text;
            return text.Substring(0, maxLength);
        }

        /// <summary>
        /// Generates unique string in the format A00000000000000000000000000000000 where
        /// 0 represents a digit.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetUniqueString()
        {
            return $"A{Guid.NewGuid():N}";
        }

        /// <summary>
        /// Gets whether <paramref name="text"/> is a hex number.
        /// </summary>
        /// <param name="text">Text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHexNumber(string? text)
        {
            if (text is null)
                return false;
            return uint.TryParse(text, NumberStyles.HexNumber, null, out _);
        }

        /// <summary>
        /// Combines array elements to string without any separators.
        /// </summary>
        public static string ToStringSimple<T>(IEnumerable<T> items)
        {
            StringBuilder builder = new();

            foreach (var item in items)
            {
                if (item is not null)
                    builder.Append(item.ToString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Combines array elements to string inside <paramref name="prefix"/>
        /// and <paramref name="suffix"/>. Array elements are separated by the
        /// string specified in the <paramref name="separator"/> parameter.
        /// </summary>
        /// <typeparam name="T">Type of the array item.</typeparam>
        /// <param name="items">Array of items.</param>
        /// <param name="prefix">Prefix string to add before items text. Optional.
        /// Uses '(' if not specified.</param>
        /// <param name="suffix">Suffix string to add after items text. Optional.
        /// Uses '(' if not specified.</param>
        /// <param name="separator">Separator string to add between each item text. Optional.
        /// Uses ', ' if not specified.</param>
        public static string ToString<T>(
            IEnumerable<T> items,
            string? prefix = default,
            string? suffix = default,
            string? separator = default)
        {
            prefix ??= "(";
            suffix ??= ")";
            separator ??= ", ";

            StringBuilder builder = new();

            foreach (var item in items)
            {
                if (builder.Length == 0)
                    builder.Append($"{prefix}{item}");
                else
                    builder.Append($"{separator}{item}");
            }

            if (builder.Length == 0)
                return $"{prefix}{suffix}";

            builder.Append(suffix);

            return builder.ToString();
        }

        /// <summary>
        /// Combines name and value pairs to string.
        /// </summary>
        /// <param name="names">Names.</param>
        /// <param name="values">Values.</param>
        /// <typeparam name="T">Type of values array.</typeparam>
        /// <returns></returns>
        public static string ToString<T>(string[]? names, T[] values)
        {
            var result = new StringBuilder();
            result.Append('{');

            for (int i = 0; i < values.Length; i++)
            {
                var name = names?[i];
                var value = values[i]?.ToString();
                if (i > 0)
                    result.Append(", ");
                if (name is null)
                {
                    if (value is not null)
                        result.Append(value);
                }
                else
                    result.Append($"{name}={value}");
            }

            result.Append('}');
            return result.ToString();
        }

        /// <summary>
        /// Combines name and value pairs to string. If <see cref="BaseObject.UseNamesInToString"/>
        /// is <c>false</c>, names are ignored and only values are returned.
        /// </summary>
        /// <typeparam name="T">Type of values array.</typeparam>
        /// <param name="names">Names.</param>
        /// <param name="values">Values.</param>
        /// <returns></returns>
        public static string ToStringWithOrWithoutNames<T>(string[] names, T[] values)
        {
            if (BaseObject.UseNamesInToString)
                return ToString<T>(names, values);
            return ToString<T>(null, values);
        }

        /// <summary>
        /// Converts the string representation of a number to its number equivalent.
        /// A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="typeCode">Number type identifier.</param>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/>
        /// values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method is equivalent of "TryParse" methods implemented in number types.
        /// </remarks>
        public static bool TryParseNumber(
            TypeCode typeCode,
            string? s,
            NumberStyles? style,
            IFormatProvider? provider,
            out object? result)
        {
            var tryParse = GetTryParseDelegate(typeCode);
            if (tryParse is null)
            {
                result = AssemblyUtils.GetDefaultValue(typeCode);
                return false;
            }

            style ??= AssemblyUtils.GetDefaultNumberStyles(typeCode);

            var isOk = tryParse(s, style.Value, provider, out result);
            return isOk;
        }

        /// <summary>
        /// Parses the string representation of an object using <see cref="TypeConverter"/>
        /// registered for the <typeparamref name="T"/> type.
        /// </summary>
        /// <param name="text">The string to parse.</param>
        /// <typeparam name="T">Type of the object to parse from the string.</typeparam>
        /// <param name="context"><see cref="ITypeDescriptorContext"/> value which is used
        /// when text is converted.</param>
        /// <param name="culture"><see cref="CultureInfo"/> value which is used
        /// when text is converted.</param>
        /// <param name="useInvariantCulture">
        /// Whether to use <see cref="TypeConverter"/> with invariant
        /// string to value conversion.
        /// </param>
        /// <returns>Object parsed from the string or Null.</returns>
        public static T? ParseWithTypeConverter<T>(
            string? text,
            ITypeDescriptorContext? context = null,
            CultureInfo? culture = null,
            bool useInvariantCulture = true)
        {
            var typeConverter = StringConverters.Default.GetTypeConverter(typeof(T));

            var result = ParseWithTypeConverter(
                        text,
                        typeConverter,
                        context,
                        culture,
                        useInvariantCulture);
            return (T?)result;
        }

        /// <summary>
        /// Parses the string representation of an object using the specified type converter.
        /// </summary>
        /// <param name="text">The string to parse.</param>
        /// <param name="context"><see cref="ITypeDescriptorContext"/> value which is used
        /// when text is converted.</param>
        /// <param name="culture"><see cref="CultureInfo"/> value which is used
        /// when text is converted.</param>
        /// <param name="useInvariantCulture">
        /// Whether to use <see cref="TypeConverter"/> with invariant
        /// string to value conversion.
        /// </param>
        /// <returns>Object parsed from the string or Null.</returns>
        /// <param name="typeConverter"></param>
        public static object? ParseWithTypeConverter(
            string? text,
            TypeConverter? typeConverter = null,
            ITypeDescriptorContext? context = null,
            CultureInfo? culture = null,
            bool useInvariantCulture = true)
        {
            if (typeConverter is null)
                return null;

            object? result;

            if (useInvariantCulture)
            {
                if (context is null)
                    result = typeConverter.ConvertFromInvariantString(text);
                else
                    result = typeConverter.ConvertFromInvariantString(context, text);
            }
            else
            {
                if (context is null)
                {
                    if (culture is null)
                        result = typeConverter.ConvertFromString(text);
                    else
                        result = typeConverter.ConvertFromString(context, culture, text);
                }
                else
                {
                    if (culture is null)
                        result = typeConverter.ConvertFromString(context, culture, text);
                    else
                        result = typeConverter.ConvertFromString(context, culture, text);
                }
            }

            return result;
        }

        /// <summary>
        /// Calls <see cref="sbyte.TryParse(string, NumberStyles, IFormatProvider, out sbyte)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseSByte(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = sbyte.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Calls <see cref="byte.TryParse(string, NumberStyles, IFormatProvider, out byte)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseByte(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = byte.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Calls <see cref="short.TryParse(string, NumberStyles, IFormatProvider, out short)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseInt16(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = short.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Calls <see cref="ushort.TryParse(string, NumberStyles, IFormatProvider, out ushort)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseUInt16(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = ushort.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Calls <see cref="int.TryParse(string, NumberStyles, IFormatProvider, out int)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/>
        /// values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseInt32(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = int.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Uses collection of <see cref="TryParseNumberDelegate"/> delegates
        /// in order to convert string to a number.
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/>
        /// values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="delegates">Collection of delegates used for the conversion.</param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParseNumberWithDelegates(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result,
            IEnumerable<TryParseNumberDelegate> delegates)
        {
            foreach (var proc in delegates)
            {
                var converted = proc(
                    s,
                    style,
                    provider,
                    out result);

                if (converted)
                    return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Calls <see cref="uint.TryParse(string, NumberStyles, IFormatProvider, out uint)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/>
        /// values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseUInt32(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = uint.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Calls <see cref="long.TryParse(string, NumberStyles, IFormatProvider, out long)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseInt64(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = long.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Calls <see cref="ulong.TryParse(string, NumberStyles, IFormatProvider, out ulong)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseUInt64(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = ulong.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Calls <see cref="float.TryParse(string, NumberStyles, IFormatProvider, out float)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseSingle(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = float.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of characters specified
        /// in an array from the string. <paramref name="trimStart"/> and <paramref name="trimEnd"/>
        /// parameters specify whether to remove leading and trailing occurrences.
        /// </summary>
        /// <param name="s">String to process.</param>
        /// <param name="chars">An array of Unicode characters to remove, or null.</param>
        /// <param name="trimStart">Whether to trim leading characters.</param>
        /// <param name="trimEnd">Whether to trim trailing characters.</param>
        /// <returns></returns>
        public static string? Trim(
            string? s,
            char[] chars,
            bool trimStart = true,
            bool trimEnd = true)
        {
            if (s is null)
                return null;

            if (trimStart)
            {
                if (trimEnd)
                {
                    return s.Trim(chars);
                }
                else
                {
                    return s.TrimStart(chars);
                }
            }
            else
            {
                if (trimEnd)
                {
                    return s.TrimEnd(chars);
                }
                else
                {
                    return s;
                }
            }
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the string.
        /// <paramref name="trimStart"/> and <paramref name="trimEnd"/>
        /// parameters specify whether to remove leading and trailing occurrences.
        /// </summary>
        /// <param name="s">String to process.</param>
        /// <param name="trimStart">Whether to trim leading characters.</param>
        /// <param name="trimEnd">Whether to trim trailing characters.</param>
        /// <returns></returns>
        public static string? Trim(
            string? s,
            bool trimStart = true,
            bool trimEnd = true)
        {
            if (s is null)
                return null;

            if (trimStart)
            {
                if (trimEnd)
                {
                    return s.Trim();
                }
                else
                {
                    return s.TrimStart();
                }
            }
            else
            {
                if (trimEnd)
                {
                    return s.TrimEnd();
                }
                else
                {
                    return s;
                }
            }
        }

        /// <summary>
        /// Trims the string using the specified text trimming rules.
        /// Result is always not Null.
        /// </summary>
        /// <param name="s">String to trim.</param>
        /// <param name="rules">Text trimming rules.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string SafeTrim(string? s, TrimTextRules rules)
        {
            return Trim(s, rules) ?? string.Empty;
        }

        /// <summary>
        /// Removes mnemonic markers from a string, such as a specified character used
        /// to indicate shortcut keys.
        /// </summary>
        /// <param name="input">The input string potentially containing mnemonic markers.</param>
        /// <param name="mnemonicMarker">
        /// The character used to mark mnemonic access keys.
        /// Single instances indicate a mnemonic; double instances represent a literal marker.
        /// </param>
        /// <returns>
        /// A cleaned string with mnemonic markers removed and escaped markers collapsed
        /// into a single literal.
        /// Returns the original string if <paramref name="input"/> is null or empty.
        /// </returns>
        /// <remarks>
        /// Commonly used for stripping mnemonic markup from UI labels,
        /// preserving readability while removing access key symbols.
        /// </remarks>
        public static string RemoveMnemonicMarkers(string input, char mnemonicMarker = '&')
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var builder = new System.Text.StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == mnemonicMarker)
                {
                    if (i + 1 < input.Length && input[i + 1] == mnemonicMarker)
                    {
                        builder.Append(mnemonicMarker);
                        i++;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    builder.Append(input[i]);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Removes mnemonic markers from a string and returns the index of the
        /// mnemonic character, if present.
        /// </summary>
        /// <param name="s">The input string containing mnemonic markers.</param>
        /// <param name="mnemonicCharIndex">
        /// The output index of the mnemonic character in the returned string.
        /// Returns -1 if no mnemonic marker is found.
        /// </param>
        /// <param name="mnemonicMarker">The character used to mark mnemonics.</param>
        /// <returns>
        /// The input string with mnemonic markers removed.
        /// Double markers are converted to a single literal character.
        /// </returns>
        public static string GetWithoutMnemonicMarkers(
            string s,
            out int mnemonicCharIndex,
            char mnemonicMarker)
        {
            mnemonicCharIndex = -1;

            if (string.IsNullOrEmpty(s))
                return s;

            var builder = new System.Text.StringBuilder(s.Length);
            int logicalIndex = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == mnemonicMarker)
                {
                    if (i + 1 < s.Length)
                    {
                        if (s[i + 1] == mnemonicMarker)
                        {
                            builder.Append(mnemonicMarker);
                            i++;
                            logicalIndex++;
                        }
                        else
                        {
                            mnemonicCharIndex = logicalIndex;
                            builder.Append(s[i + 1]);
                            i++;
                            logicalIndex++;
                        }
                    }
                }
                else
                {
                    builder.Append(s[i]);
                    logicalIndex++;
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the index of the mnemonic character in the specified string.
        /// </summary>
        /// <param name="input">The string to search for the mnemonic character.</param>
        /// <param name="mnemonicMark">The mnemonic character used to
        /// identify the intended character.</param>
        /// <returns>The index of the mnemonic character, or -1 if not found.</returns>
        public static int GetMnemonicCharIndex(string input, char mnemonicMark = '&')
        {
            if (string.IsNullOrEmpty(input))
                return -1;

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == mnemonicMark)
                {
                    if (input[i + 1] == mnemonicMark)
                    {
                        i++;
                        continue;
                    }

                    return i + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Trims the string using the specified text trimming rules.
        /// </summary>
        /// <param name="s">String to trim.</param>
        /// <param name="rules">Text trimming rules.</param>
        /// <returns></returns>
        public static string? Trim(string? s, TrimTextRules rules)
        {
            if (s is null)
                return null;

            bool trimStart = !rules.HasFlag(TrimTextRules.NoLeading);
            bool trimEnd = !rules.HasFlag(TrimTextRules.NoTrailing);

            string? result;

            if (rules.HasFlag(TrimTextRules.TrimWhiteChars))
                result = Trim(s, trimStart, trimEnd);
            else
                result = s;

            s = Trim(result, GetTrimTextRulesChars(rules), trimStart, trimEnd);

            return s;
        }

        /// <summary>
        /// Removes all leading and trailing brackets and white-space characters
        /// from the specified string. Uses bracket characters from <see cref="BracketsChars"/>.
        /// </summary>
        /// <param name="text">Text to trim.</param>
        /// <returns></returns>
        public static string TrimWhitespaceAndBrackets(string? text)
        {
            if (text is null)
                return string.Empty;
            var s = text.Trim().Trim(BracketsChars);
            return s;
        }

        /// <summary>
        /// Gets an array of characters that can be passed to <see cref="string.Trim(char[])"/>
        /// function from the specified <see cref="TrimTextRules"/> parameter.
        /// </summary>
        /// <param name="rules">Rules for text trimming.</param>
        /// <returns></returns>
        public static char[] GetTrimTextRulesChars(TrimTextRules rules)
        {
            List<char> chars = new();

            if (rules.HasFlag(TrimTextRules.TrimSpaces))
                chars.Add(' ');

            if (rules.HasFlag(TrimTextRules.TrimRoundBrackets))
            {
                chars.Add('(');
                chars.Add(')');
            }

            if (rules.HasFlag(TrimTextRules.TrimSquareBrackets))
            {
                chars.Add('[');
                chars.Add(']');
            }

            if (rules.HasFlag(TrimTextRules.TrimFigureBrackets))
            {
                chars.Add('{');
                chars.Add('}');
            }

            if (rules.HasFlag(TrimTextRules.TrimAngleBrackets))
            {
                chars.Add('<');
                chars.Add('>');
            }

            var charsArray = chars.ToArray();
            return charsArray;
        }

        /// <summary>
        /// Removes the specified prefix from the beginning of the input string, if present.
        /// Supports case-sensitive or case-insensitive comparisons
        /// using <see cref="StringComparison"/>.
        /// </summary>
        /// <param name="input">The input string to evaluate.</param>
        /// <param name="prefix">The prefix to remove from <paramref name="input"/>.</param>
        /// <param name="comparison">The string comparison mode used for prefix matching.</param>
        /// <returns>
        /// The input string without the prefix if it matched; otherwise, the original input.
        /// Returns <c>null</c> if <paramref name="input"/> is <c>null</c>.
        /// </returns>
        public static string? RemovePrefix(
            string? input,
            string? prefix,
            StringComparison comparison = StringComparison.CurrentCulture)
        {
            if (input == null || string.IsNullOrEmpty(prefix))
                return input;

            return input.StartsWith(prefix, comparison)
                ? input.Substring(prefix!.Length)
                : input;
        }

        /// <summary>
        /// Removes the "http://", "https://" and "www." prefixes
        /// </summary>
        /// <param name="input">The input string to process.</param>
        /// <returns>The input string without the prefixes if any matched;
        /// otherwise, the original input.</returns>
        public static string? RemoveHttpAndWwwPrefixes(string? input)
        {
            if (input == null)
                return null;
            var prefixes = new[] { @"http://", @"https://", @"www." };
            return RemovePrefixes(input, prefixes, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Removes the specified prefixes from the beginning of the input string, if present.
        /// </summary>
        /// <param name="input">The input string to process.</param>
        /// <param name="prefixes">The prefixes to remove from <paramref name="input"/>.</param>
        /// <param name="comparison">The string comparison mode used for prefix matching.</param>
        /// <returns>The input string without the prefixes if any matched;
        /// otherwise, the original input.
        /// Returns <c>null</c> if <paramref name="input"/> is <c>null</c>.
        /// </returns>
        public static string? RemovePrefixes(
            string? input,
            IEnumerable<string> prefixes,
            StringComparison comparison = StringComparison.CurrentCulture)
        {
            if (input == null || prefixes is null)
                return input;
            foreach (var prefix in prefixes)
            {
                if (string.IsNullOrEmpty(prefix))
                    continue;
                if (input.StartsWith(prefix, comparison))
                    input = input.Substring(prefix.Length);
            }

            return input;
        }

        /// <summary>
        /// Gets whether the specified string equals CR, LF or their combinations.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool EqualsNewLine(string str)
        {
            if (str is null || str.Length < 1 || str.Length > 2)
                return false;

            foreach(var s in StringSplitToArrayChars)
            {
                if (str == s)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Splits one string to many using <see cref="StringSplitToArrayChars"/> as separators.
        /// </summary>
        /// <param name="str">String to split.</param>
        /// <param name="removeEmptyLines">Whether to remove empty lines from the result.</param>
        /// <returns></returns>
        public static string[] Split(string str, bool removeEmptyLines = false)
        {
            return str.Split(
                StringSplitToArrayChars,
                removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// Calls <see cref="double.TryParse(string, NumberStyles, IFormatProvider, out double)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseDouble(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = double.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Calls <see cref="decimal.TryParse(string, NumberStyles, IFormatProvider, out decimal)"/>
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <paramref name="s"/>.</param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information
        /// about <paramref name="s"/>.
        /// </param>
        /// <param name="result">
        /// When this method returns and if the conversion succeeded, contains a number equivalent
        /// of the numeric value or symbol contained in <paramref name="s"/>.
        /// Contains default value for the type if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseDecimal(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = decimal.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        /// <summary>
        /// Splits text into three parts using the specified position of the accelerator
        /// character. Returns an array of three <see cref="TextAndFontStyle"/> elements.
        /// </summary>
        /// <param name="text">String to split into three parts.</param>
        /// <param name="accelStyle">Style of the char on the
        /// <paramref name="indexAccel"/> position.</param>
        /// <param name="indexAccel">Position of the accelerator character.</param>
        /// <returns></returns>
        public static TextAndFontStyle[] ParseTextWithIndexAccel(
            string text,
            int indexAccel,
            FontStyle accelStyle = FontStyle.Underline)
        {
            if (indexAccel < 0)
                return [text];

            TextAndFontStyle prefixElement = new(text.Substring(0, indexAccel));
            TextAndFontStyle accelElement = new(text.Substring(indexAccel, 1), accelStyle);
            TextAndFontStyle suffixElement = new(text.Substring(indexAccel + 1));

            TextAndFontStyle[] result = [prefixElement, accelElement, suffixElement];
            return result;
        }

        /// <summary>
        /// Inserts html bold tags before and after substring.
        /// </summary>
        /// <param name="s">The text which will be processed.</param>
        /// <param name="textToFind">The text around which html bold tags will be inserted.</param>
        /// <param name="comparison"><see cref="StringComparison"/> used to specify how text
        /// will be compared.</param>
        /// <returns>Changed text with html bold tags around found substring.</returns>
        public static string InsertBoldTags(
            string s,
            string textToFind,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return InsertTags(
                        s,
                        textToFind,
                        BoldTagStart,
                        BoldTagEnd,
                        comparison);
        }

        /// <summary>
        /// Inserts tags before and after substring.
        /// </summary>
        /// <param name="s">The text which will be processed.</param>
        /// <param name="textToFind">The text around which tags will be inserted.</param>
        /// <param name="comparison"><see cref="StringComparison"/> used to specify how text
        /// will be compared.</param>
        /// <returns>Changed text with tags around found substring.</returns>
        /// <param name="beforeTag">The tag string which will be inserted before
        /// the found substring.</param>
        /// <param name="afterTag">The tag string which will be inserted after
        /// the found substring.</param>
        /// <returns></returns>
        public static string InsertTags(
            string s,
            string textToFind,
            string beforeTag,
            string afterTag,
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            var pos = s.IndexOf(textToFind, comparison);

            if (pos < 0)
                return s;

            string result;

            var afterTagPos = pos + textToFind.Length;

            if (afterTagPos >= s.Length)
                result = s + afterTag;
            else
                result = s.Insert(afterTagPos, afterTag);

            result = result.Insert(pos, beforeTag);
            return result;
        }

        /// <summary>
        /// Wraps the specified string in a CDATA section.
        /// </summary>
        /// <param name="s">The string to wrap in a CDATA section.</param>
        /// <returns>A string wrapped in a CDATA section.</returns>
        public static string WithCDATA(string? s)
        {
            return $"{PrefixCDATA}{s}{SuffixCDATA}";
        }

        /// <summary>
        /// Calls <see cref="string.Format(string, object)"/>
        /// in a safe way allowing to use null parameters.
        /// </summary>
        public static string? SafeStringFormat(string? value, string? format)
        {
            if (value is null || format is null)
                return value;

            var result = string.Format(format, value);
            return result;
        }

        internal static TryParseNumberDelegate? GetTryParseDelegate(TypeCode typeCode)
        {
            return typeCode switch
            {
                TypeCode.SByte => TryParseSByte,
                TypeCode.Byte => TryParseByte,
                TypeCode.Int16 => TryParseInt16,
                TypeCode.UInt16 => TryParseUInt16,
                TypeCode.Int32 => TryParseInt32,
                TypeCode.UInt32 => TryParseUInt32,
                TypeCode.Int64 => TryParseInt64,
                TypeCode.UInt64 => TryParseUInt64,
                TypeCode.Single => TryParseSingle,
                TypeCode.Double => TryParseDouble,
                TypeCode.Decimal => TryParseDecimal,
                _ => null,
            };
        }

        [Conditional("DEBUG")]
        private static void Test()
        {
        }

        /// <summary>
        /// Implementation of <see cref="IComparer{T}"/> interface that compares
        /// two objects using <see cref="object.ToString"/> methods.
        /// </summary>
        /// <typeparam name="T">Type of objects to compare.</typeparam>
        public class ComparerUsingToString<T> : IComparer<T>
        {
            /// <inheritdoc/>
            public int Compare(T? x, T? y)
            {
                var s1 = x?.ToString();
                var s2 = y?.ToString();
                var result = string.Compare(s1, s2, StringComparison.CurrentCulture);
                return result;
            }
        }
    }
}
