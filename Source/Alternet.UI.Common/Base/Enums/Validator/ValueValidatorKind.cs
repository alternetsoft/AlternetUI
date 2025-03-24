using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates possible validator kinds used when validator is created.
    /// </summary>
    public enum ValueValidatorKind
    {
        /// <summary>
        /// Text validator with default settings.
        /// </summary>
        Generic,

        /// <summary>
        /// Text validator which allows to enter signed integer numbers.
        /// </summary>
        SignedInt,

        /// <summary>
        /// Text validator which allows to enter unsigned integer numbers.
        /// </summary>
        UnsignedInt,

        /// <summary>
        /// Text validator which allows to enter floating point numbers.
        /// </summary>
        SignedFloat,

        /// <summary>
        /// Text validator which allows to enter signed hexadecimal integer numbers.
        /// </summary>
        SignedHex,

        /// <summary>
        /// Text validator which allows to enter unsigned hexadecimal integer numbers.
        /// </summary>
        UnsignedHex,
    }
}
