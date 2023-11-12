using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements different search methods in <see cref="IReadOnlyStrings"/>.
    /// </summary>
    public class StringSearch
    {
        private IReadOnlyStrings strings;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSearch"/> class.
        /// </summary>
        /// <param name="strings">Strings data source.</param>
        public StringSearch(IReadOnlyStrings strings)
        {
            this.strings = strings;
        }

        /// <summary>
        /// Default value of the <see cref="Culture"/> property.
        /// </summary>
        public static CultureInfo? DefaultCulture { get; set; }

        /// <summary>
        /// Default value of the <see cref="CompareOptions"/> property.
        /// </summary>
        public static CompareOptions DefaultCompareOptions { get; set; } = CompareOptions.None;

        /// <summary>
        /// Gets or sets <see cref="CultureInfo"/> used in find string methods.
        /// </summary>
        /// <remarks>
        /// If <see cref="Culture"/> is not assigned,
        /// <see cref="CultureInfo.CurrentCulture"/> is used.
        /// </remarks>
        public CultureInfo? Culture { get; set; } = DefaultCulture;

        /// <summary>
        /// Gets or sets <see cref="CompareOptions"/> used in find string methods.
        /// </summary>
        public CompareOptions CompareOptions { get; set; } = DefaultCompareOptions;

        /// <summary>
        /// Returns the index of the first item in the control that starts
        /// with the specified string.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to search for.</param>
        /// <returns>The zero-based index of the first item found;
        /// returns <c>null</c> if no match is found.</returns>
        /// <remarks>
        /// The search performed by this method is not case-sensitive.
        /// The <paramref name="s"/> parameter is a substring to compare against
        /// the text associated with the items in the control. The search performs
        /// a partial match starting from the beginning of the text, and returning
        /// the first item in the list that matches the specified substring.
        /// </remarks>
        public virtual int? FindString(string s)
        {
            return FindStringInternal(
                s,
                startIndex: null,
                exact: false,
                ignoreCase: true);
        }

        /// <summary>
        /// Returns the index of the first item in the control beyond the specified
        /// index that contains the specified string. The search is not case sensitive.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before
        /// the first item to be searched. Set to <c>null</c> to search from the beginning
        /// of the control.</param>
        /// <returns>The zero-based index of the first item found; returns <c>null</c> if
        /// no match is found.</returns>
        /// <remarks>
        /// The search performed by this method is not case-sensitive. The <paramref name="s"/>
        /// parameter is a substring to compare against the text associated with the
        /// items in the control. The search performs a partial match
        /// starting from the beginning of the text, returning the first item in the
        /// list that matches the specified substring.
        /// </remarks>
        public virtual int? FindString(string s, int? startIndex)
        {
            return FindStringInternal(
                s,
                startIndex,
                exact: false,
                ignoreCase: true);
        }

        /// <summary>
        /// Finds the first item in the combo box that matches the specified string.
        /// </summary>
        /// <param name="s">The string to search for.</param>
        /// <returns>The zero-based index of the first item found; returns
        /// <c>null</c> if no match is found.</returns>
        /// <remarks>
        /// The search performed by this method is not case-sensitive.
        /// </remarks>
        public virtual int? FindStringExact(string s)
        {
            return FindStringInternal(
                s,
                startIndex: null,
                exact: true,
                ignoreCase: true);
        }

        /// <summary>
        /// Returns the index of the first item in the control beyond the specified
        /// index that contains or equal the specified string.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before
        /// the first item to be searched. Set to <c>null</c> to search from the beginning
        /// of the control.</param>
        /// <returns>The zero-based index of the first item found; returns <c>null</c> if
        /// no match is found.</returns>
        /// <remarks>
        /// The <paramref name="str"/>
        /// parameter is a substring to compare against the text associated with the
        /// items in the control.
        /// The search performs a partial match (<paramref name="exact"/> is <c>false</c>)
        /// or exact match (<paramref name="exact"/> is <c>true</c>)
        /// Search starts from the beginning of the text, returning the first item in the
        /// list that matches the specified substring.
        /// </remarks>
        /// <param name="exact"><c>true</c> uses exact comparison; <c>false</c> uses partial
        /// compare.</param>
        /// <param name="ignoreCase">Whether to ignore text case or not.</param>
        public virtual int? FindStringEx(
         string? str,
         int? startIndex,
         bool exact,
         bool ignoreCase)
        {
            return FindStringInternal(
             str,
             startIndex,
             exact,
             ignoreCase);
        }

        private int? FindStringInternal(
             string? str,
             int? startIndex,
             bool exact,
             bool ignoreCase)
        {
            if (str is null)
                return null;
            var itemCount = strings.Count;
            if (itemCount == 0)
                return null;

            var startIndexInt = startIndex ?? -1;

            if (startIndexInt < -1 || startIndexInt >= itemCount)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            // Start from the start index and wrap around until we find the string
            // in question. Use a separate counter to ensure that we arent cycling
            // through the list infinitely.
            int numberOfTimesThroughLoop = 0;

            var culture = Culture ?? CultureInfo.CurrentCulture;
            var options = CompareOptions;

            if (ignoreCase)
                options |= CompareOptions.IgnoreCase;
            else
                options &= ~CompareOptions.IgnoreCase;

            for (
                int index = (startIndexInt + 1) % itemCount;
                numberOfTimesThroughLoop < itemCount;
                index = (index + 1) % itemCount)
            {
                numberOfTimesThroughLoop++;

                bool found;
                if (exact)
                {
                    found = string.Compare(
                        str,
                        strings[index],
                        culture,
                        options) == 0;
                }
                else
                {
                    found = string.Compare(
                        str,
                        0,
                        strings[index],
                        0,
                        str.Length,
                        culture,
                        options) == 0;
                }

                if (found)
                    return index;
            }

            return null;
        }
    }
}
