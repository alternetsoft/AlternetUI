using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for comparing objects.
    /// </summary>
    public static class CompareUtils
    {
        /// <summary>
        /// Compares two objects using a sequence of comparison delegates,
        /// returning the first non-zero result.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparisons">
        /// An array of <see cref="Comparison{T}"/> delegates to use for comparison,
        /// in order of priority.
        /// </param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/>
        /// and <paramref name="y"/>:
        /// less than zero if <paramref name="x"/> is less than <paramref name="y"/>;
        /// zero if they are equal;
        /// greater than zero if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public static int CompareByPriority<T>(
            T x,
            T y,
            params Comparison<T>[] comparisons)
        {
            foreach (var comparison in comparisons)
            {
                var result = comparison(x, y);
                if (result != 0)
                    return result;
            }

            return 0;
        }
    }
}
