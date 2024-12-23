using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known validator errors.
    /// </summary>
    public enum ValueValidatorKnownError
    {
        /// <summary>
        /// Report no error.
        /// </summary>
        None,

        /// <summary>
        /// Error: "A number is expected.".
        /// </summary>
        NumberIsExpected,

        /// <summary>
        /// Error: "An unsigned number is expected.".
        /// </summary>
        UnsignedNumberIsExpected,

        /// <summary>
        /// Error: "A floating point number is expected.".
        /// </summary>
        FloatIsExpected,

        /// <summary>
        /// Error: "An unsigned floating point number is expected.".
        /// </summary>
        UnsignedFloatIsExpected,

        /// <summary>
        /// Error: "A hexadecimal number is expected.".
        /// </summary>
        HexNumberIsExpected,

        /// <summary>
        /// Error: "Invalid format.".
        /// </summary>
        InvalidFormat,

        /// <summary>
        /// Error: "Minimum length is {0} characters.".
        /// </summary>
        MinimumLength,

        /// <summary>
        /// Error: "Maximum length is {0} characters.".
        /// </summary>
        MaximumLength,

        /// <summary>
        /// Error: "Valid length is {0} to {1} characters.".
        /// </summary>
        MinMaxLength,

        /// <summary>
        /// Error: "Minimum value is {0}.".
        /// </summary>
        MinimumValue,

        /// <summary>
        /// Error: "Maximum value is {0}.".
        /// </summary>
        MaximumValue,

        /// <summary>
        /// Error: "Value is required.".
        /// </summary>
        ValueIsRequired,

        /// <summary>
        /// Error: "E-mail is expected.".
        /// </summary>
        EMailIsExpected,

        /// <summary>
        /// Error: "Url is expected.".
        /// </summary>
        UrlIsExpected,
    }
}