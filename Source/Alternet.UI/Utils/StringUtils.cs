using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines delegate type for the methods that convert the string representation of
    /// a number to its number equivalent.
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
        /// Gets title of the windows key on MacOs (0x2318 character).
        /// </summary>
        public const string MacCommandKeyTitle = "\u2318";

        private static IComparer<object>? comparerObjectUsingToString;

        /// <summary>
        /// Gets or sets values that split one string to many when
        /// log operations are performed (or in other situations).
        /// </summary>
        /// <remarks>
        /// An example of such split values is <see cref="Environment.NewLine"/>
        /// which is assigned to <see cref="StringSplitToArrayChars"/> by default.
        /// </remarks>
        public static string[] StringSplitToArrayChars { get; set; } =
        [
            Environment.NewLine,
            "\r\n",
            "\n\r",
            "\n",
        ];

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
        public static string GetUniqueString()
        {
            return $"A{Guid.NewGuid():N}";
        }

        /// <summary>
        /// Gets whether <paramref name="text"/> is a hex number.
        /// </summary>
        /// <param name="text">Text.</param>
        public static bool IsHexNumber(string? text)
        {
            if (text is null)
                return false;
            return uint.TryParse(text, NumberStyles.HexNumber, null, out _);
        }

        /// <summary>
        /// Combines array elements to comma separated string inside round brackets.
        /// </summary>
        /// <typeparam name="T">Type of the array item.</typeparam>
        /// <param name="items">Array of items.</param>
        /// <param name="prefix">Prefix string to add before items text.</param>
        /// <param name="suffix">Suffix string to add after items text.</param>
        /// <param name="separator">Separator string to add between each item text.</param>
        public static string ToString<T>(
            IEnumerable<T> items,
            string? prefix = default,
            string? suffix = default,
            string? separator = default)
        {
            prefix ??= "(";
            suffix ??= ")";
            separator ??= ", ";
            string result = string.Empty;
            foreach(var item in items)
            {
                if (result == string.Empty)
                    result = $"{prefix}{item}";
                else
                    result += $"{separator}{item}";
            }

            if (result == string.Empty)
                return $"{prefix}{suffix}";

            result += suffix;

            return result;
        }

        /// <summary>
        /// Combines name and value pairs to string.
        /// </summary>
        /// <param name="names">Names.</param>
        /// <param name="values">Values.</param>
        /// <returns></returns>
        public static string ToString<T>(string[] names, T[] values)
        {
            var result = new StringBuilder();
            result.Append('{');

            for (int i = 0; i < names.Length; i++)
            {
                var name = names[i];
                var value = values[i]?.ToString();
                var item = $"{name}={value}";

                if (i > 0)
                    result.Append(", ");
                result.Append(item);
            }

            result.Append('}');
            return result.ToString();
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

        internal static bool TryParseSByte(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = sbyte.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseByte(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = byte.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseInt16(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = short.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseUInt16(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = ushort.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseInt32(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = int.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseUInt32(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = uint.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseInt64(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = long.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseUInt64(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = ulong.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseSingle(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = float.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseDouble(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = double.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
        }

        internal static bool TryParseDecimal(
            string? s,
            NumberStyles style,
            IFormatProvider? provider,
            out object? result)
        {
            var isOk = decimal.TryParse(s, style, provider, out var value);
            result = value;
            return isOk;
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

        private class ComparerUsingToString<T> : IComparer<T>
            where T : class
        {
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
