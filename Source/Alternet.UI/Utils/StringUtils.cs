using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="string"/> related static methods.
    /// </summary>
    public static class StringUtils
    {
        private static IComparer<object>? comparerObjectUsingToString;

        /// <summary>
        /// Gets or sets values that split one string to many when
        /// log operations are performed (or in other situations).
        /// </summary>
        /// <remarks>
        /// An example of such split values is <see cref="Environment.NewLine"/>
        /// which is assigned to <see cref="StringSplitToArrayChars"/> by default.
        /// </remarks>
        public static string[] StringSplitToArrayChars { get; set; } = new string[]
        {
            Environment.NewLine,
            "\r\n",
            "\n\r",
            "\n",
        };

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
