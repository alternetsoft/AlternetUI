using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known validator errors.
    /// </summary>
    public enum ValueValidatorKnownError
    {
        /// <summary>
        /// Error: "A number is expected.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationNumberIsExpected"/> for the localization.
        /// </remarks>
        NumberIsExpected,

        /// <summary>
        /// Error: "An unsigned number is expected.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationUnsignedNumberIsExpected"/> for the localization.
        /// </remarks>
        UnsignedNumberIsExpected,

        /// <summary>
        /// Error: "A floating point number is expected.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationFloatIsExpected"/> for the localization.
        /// </remarks>
        FloatIsExpected,

        /// <summary>
        /// Error: "An unsigned floating point number is expected.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationUnsignedFloatIsExpected"/> for the localization.
        /// </remarks>
        UnsignedFloatIsExpected,

        /// <summary>
        /// Error: "A hexadecimal number is expected.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationHexNumberIsExpected"/> for the localization.
        /// </remarks>
        HexNumberIsExpected,

        /// <summary>
        /// Error: "Invalid format.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationInvalidFormat"/> for the localization.
        /// </remarks>
        InvalidFormat,

        /// <summary>
        /// Error: "Minimum length is {0} characters.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationMinimumLength"/> for the localization.
        /// </remarks>
        MinimumLength,

        /// <summary>
        /// Error: "Maximum length is {0} characters.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationMaximumLength"/> for the localization.
        /// </remarks>
        MaximumLength,

        /// <summary>
        /// Error: "Valid length is {0} to {1} characters.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationMinMaxLength"/> for the localization.
        /// </remarks>
        MinMaxLength,

        /// <summary>
        /// Error: "Minimum value is {0}.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationMinimumValue"/> for the localization.
        /// </remarks>
        MinimumValue,

        /// <summary>
        /// Error: "Maximum value is {0}.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationMaximumValue"/> for the localization.
        /// </remarks>
        MaximumValue,

        /// <summary>
        /// Error: "Value is required.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationValueIsRequired"/> for the localization.
        /// </remarks>
        ValueIsRequired,

        /// <summary>
        /// Error: "E-mail is expected.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationEMailIsExpected"/> for the localization.
        /// </remarks>
        EMailIsExpected,

        /// <summary>
        /// Error: "Url is expected.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationUrlIsExpected"/> for the localization.
        /// </remarks>
        UrlIsExpected,
    }
}