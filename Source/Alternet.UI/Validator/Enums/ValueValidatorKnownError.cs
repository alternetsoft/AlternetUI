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
        /// Error: "A number is expected. Range is [{0}..{1}].".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationNumberIsExpected"/> and
        /// <see cref="ErrorMessages.ValidationRangeFormat"/> for the localization.
        /// </remarks>
        NumberIsExpected,

        /// <summary>
        /// Error: "A floating point number is expected.".
        /// </summary>
        /// <remarks>
        /// Use <see cref="ErrorMessages.ValidationFloatIsExpected"/> for the localization.
        /// </remarks>
        FloatIsExpected,

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
    }
}