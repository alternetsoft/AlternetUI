using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Gets html start bold tag string constant.
        /// </summary>
        public const string BoldTagStart = "<b>";

        /// <summary>
        /// Gets html end bold tag string constant.
        /// </summary>
        public const string BoldTagEnd = "</b>";

        /// <summary>
        /// Gets initialized string with one space character.
        /// </summary>
        public const string OneSpace = " ";

        /// <summary>
        /// Gets initialized string with four space characters.
        /// </summary>
        public const string FourSpaces = "    ";

        /// <summary>
        /// Gets initialized string with one double quote character (").
        /// </summary>
        public const string OneDoubleQuote = "\"";

        /// <summary>
        /// Gets initialized string with one carriage return.
        /// </summary>
        public const string OneCarriageReturn = "\r";

        /// <summary>
        /// Gets initialized string with one new line character.
        /// </summary>
        public const string OneNewLine = "\n";

        /// <summary>
        /// Gets title of the windows key on MacOs (0x2318 character).
        /// </summary>
        public const string MacCommandKeyTitle = "\u2318";

        /// <summary>
        /// Gets or sets values that split one string to many.
        /// </summary>
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
        /// Modfies specified string by adding prefix and suffix.
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
        /// Gets whether the specified character is english char or dot.
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
        /// Calls <see cref="uint.TryParse(string, NumberStyles, IFormatProvider, out uint)"/>
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
        private static void TestParseTextWithIndexAccel()
        {
            var s1 = "Text1 with acell [2]";
            var s2 = "Text2 with acell [-5]";

            var splitted1 = ParseTextWithIndexAccel(s1, 2, FontStyle.Bold);
            var splitted2 = ParseTextWithIndexAccel(s2, -5, FontStyle.Bold);

            LogUtils.LogTextAndFontStyle(splitted1);
            LogUtils.LogTextAndFontStyle(splitted2);
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
