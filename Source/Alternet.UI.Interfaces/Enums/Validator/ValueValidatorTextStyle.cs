using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Styles used by <see cref="IValueValidatorText"/>.
    /// </summary>
    [Flags]
    public enum ValueValidatorTextStyle
    {
        /// <summary>
        /// No filtering takes place.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Empty strings are filtered out. If this style is not specified then empty strings
        /// are accepted only if they pass the other checks (if you use more than
        /// one validator style).
        /// </summary>
        Empty = 0x1,

        /// <summary>
        /// Non-ASCII characters are filtered out.
        /// </summary>
        Ascii = 0x2,

        /// <summary>
        /// Non-alpha characters are filtered out. This is locale-dependent.
        /// </summary>
        Alpha = 0x4,

        /// <summary>
        /// Non-alphanumeric characters are filtered out. This is locale-dependent.
        /// Equivalent to <see cref="Alpha"/> combined with <see cref="Digits"/>
        /// or <see cref="XDigits"/>, or with both of them.
        /// </summary>
        AlphaNumeric = 0x8,

        /// <summary>
        /// Non-digit characters are filtered out. This is locale-dependent.
        /// </summary>
        Digits = 0x10,

        /// <summary>
        /// Non-numeric characters are filtered out. Works like <see cref="Digits"/> but
        /// allows also decimal points, minus/plus signs and the 'e' or 'E' character
        /// to input exponents.
        /// </summary>
        Numeric = 0x20,

        /// <summary>
        /// Use an include list. The validator checks if the user input is on the list,
        /// complaining if not. See <see cref="IValueValidatorText.AddCharIncludes"/>.
        /// </summary>
        IncludeList = 0x40,

        /// <summary>
        /// Use an include char list. Characters in the include char list will be allowed to
        /// be in the user input. See <see cref="IValueValidatorText.CharIncludes"/>. If this
        /// style is set with one or more of the following styles: <see cref="Ascii"/>,
        /// <see cref="Alpha"/>, <see cref="AlphaNumeric"/>, <see cref="Digits"/>,
        /// <see cref="XDigits"/>, <see cref="Numeric"/> it just extends the character
        /// class denoted by the aforementioned styles with those specified in
        /// the include char list. If set alone, then the characters allowed to be
        /// in the user input are restricted to those, and only those, present in
        /// the include char list.
        /// </summary>
        IncludeCharList = 0x80,

        /// <summary>
        /// Use an exclude list. The validator checks if the user input is on the list,
        /// complaining if it is. See <see cref="IValueValidatorText.AddCharExcludes"/>.
        /// </summary>
        ExcludeList = 0x100,

        /// <summary>
        /// Use an exclude char list. Characters in the exclude char list won't be allowed
        /// to be in the user input. See <see cref="IValueValidatorText.CharExcludes"/>.
        /// </summary>
        ExcludeCharList = 0x200,

        /// <summary>
        /// Non-hexadecimal characters are filtered out. This is locale-dependent.
        /// </summary>
        XDigits = 0x400,

        /// <summary>
        /// A convenience flag for use with the other flags. The space character is more
        /// often used with alphanumeric characters which makes setting a flag more
        /// easier than calling <see cref="IValueValidatorText.AddCharIncludes"/> ") for
        /// that matter.
        /// </summary>
        Space = 0x800,
    }
}