using System;
using System.Collections.Generic;
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
        };

        private static IComparer<object>? comparerObjectUsingToString;

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
