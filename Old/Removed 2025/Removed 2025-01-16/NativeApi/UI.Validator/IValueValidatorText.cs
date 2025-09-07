using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Validates text controls, providing a variety of filtering behaviours.
    /// </summary>
    internal interface IValueValidatorText : IValueValidator
    {
        /// <summary>
        /// Gets or sets whether non-ASCII characters are filtered out.
        /// </summary>
        bool ExcludeNonAscii { get; set; }

        /// <summary>
        /// Gets or sets whether non-alpha characters are filtered out. This is locale-dependent.
        /// </summary>
        bool ExcludeNonAlpha { get; set; }

        /// <summary>
        /// Gets or sets whether non-alphanumeric characters are filtered out. This is locale-dependent.
        /// </summary>
        /// <remarks>
        /// Equivalent to <see cref="ValueValidatorTextStyle.Alpha"/> combined with
        /// <see cref="ValueValidatorTextStyle.Digits"/>
        /// or <see cref="ValueValidatorTextStyle.XDigits"/>, or with both of them.
        /// </remarks>
        bool ExcludeNonAlphaNumeric { get; set; }

        /// <summary>
        /// Gets or sets whether non-digit characters are filtered out. This is locale-dependent.
        /// </summary>
        bool ExcludeNonDigits { get; set; }

        /// <summary>
        /// Gets or sets whether non-numeric characters are filtered out.
        /// </summary>
        /// <remarks>
        /// Works like <see cref="ValueValidatorTextStyle.Digits"/> but
        /// allows also decimal points, minus/plus signs and the 'e' or 'E' character
        /// to input exponents.
        /// </remarks>
        bool ExcludeNonNumeric { get; set; }

        /// <summary>
        /// Gets or sets whether non-hexadecimal characters are filtered out. This is locale-dependent.
        /// </summary>
        bool ExcludeNonHexDigits { get; set; }

        /// <summary>
        /// Gets or sets whether empty strings are filtered out.
        /// </summary>
        /// <remarks>
        /// If this style is not specified then empty strings
        /// are accepted only if they pass the other checks (if you use more than
        /// one validator style).
        /// </remarks>
        bool ExcludeEmptyStr { get; set; }

        /// <summary>
        /// Gets or sets whether to use an include list.
        /// </summary>
        /// <remarks>
        /// The validator checks if the user input is on the list,
        /// complaining if not. See <see cref="IValueValidatorText.AddCharIncludes"/>.
        /// </remarks>
        bool UseIncludeList { get; set; }

        /// <summary>
        /// Gets or sets whether to use an include char list.
        /// </summary>
        /// <remarks>
        /// Characters in the include char list will be allowed to
        /// be in the user input. See <see cref="IValueValidatorText.CharIncludes"/>. If this
        /// style is set with one or more of the following styles:
        /// <see cref="ValueValidatorTextStyle.Ascii"/>,
        /// <see cref="ValueValidatorTextStyle.Alpha"/>,
        /// <see cref="ValueValidatorTextStyle.AlphaNumeric"/>,
        /// <see cref="ValueValidatorTextStyle.Digits"/>,
        /// <see cref="ValueValidatorTextStyle.XDigits"/>,
        /// <see cref="ValueValidatorTextStyle.Numeric"/> it just extends the character
        /// class denoted by the aforementioned styles with those specified in
        /// the include char list. If set alone, then the characters allowed to be
        /// in the user input are restricted to those, and only those, present in
        /// the include char list.
        /// </remarks>
        bool UseIncludeCharList { get; set; }

        /// <summary>
        /// Gets or sets whether to use an exclude list.
        /// </summary>
        /// <remarks>
        /// The validator checks if the user input is on the list,
        /// complaining if it is. See <see cref="IValueValidatorText.AddCharExcludes"/>.
        /// </remarks>
        bool UseExcludeList { get; set; }

        /// <summary>
        /// Gets or sets whether to use an exclude char list.
        /// </summary>
        /// <remarks>
        /// Characters in the exclude char list won't be allowed
        /// to be in the user input. See <see cref="IValueValidatorText.CharExcludes"/>.
        /// </remarks>
        bool UseExcludeCharList { get; set; }

        /// <summary>
        /// Gets or sets whether space character is allowed.
        /// </summary>
        /// <remarks>
        /// A convenience flag for use with the other flags. The space character is more
        /// often used with alphanumeric characters which makes setting a flag more
        /// easier than calling <see cref="IValueValidatorText.AddCharIncludes"/> ") for
        /// that matter.
        /// </remarks>
        bool AllowSpaceChar { get; set; }

        /// <summary>
        /// Gets or sets the exclude char list (the list of invalid characters).
        /// </summary>
        string CharExcludes { get; set; }

        /// <summary>
        /// Gets or sets style of the validator.
        /// </summary>
        ValueValidatorTextStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the include char list (the list of additional valid characters).
        /// </summary>
        string CharIncludes { get; set; }

        /// <summary>
        /// Adds chars to the list of included characters.
        /// </summary>
        /// <param name="chars">Characters to include.</param>
        void AddCharIncludes(string chars);

        /// <summary>
        /// Adds include to the list of included values.
        /// </summary>
        /// <param name="include">Value to include.</param>
        void AddInclude(string include);

        /// <summary>
        /// Adds exclude to the list of excluded values.
        /// </summary>
        /// <param name="exclude">Value to exclude.</param>
        void AddExclude(string exclude);

        /// <summary>
        /// Adds chars to the list of excluded (invalid) characters.
        /// </summary>
        /// <param name="chars">Characters to exclude.</param>
        void AddCharExcludes(string chars);

        /// <summary>
        /// Clears the list of excluded values.
        /// </summary>
        void ClearExcludes();

        /// <summary>
        /// Clears the list of included values.
        /// </summary>
        void ClearIncludes();

        /// <summary>
        /// Returns the error message if the contents of <paramref name="val"/> are invalid or
        /// the empty string if <paramref name="val"/> is valid.
        /// </summary>
        /// <param name="val">String value.</param>
        string IsValid(string val);
    }
}