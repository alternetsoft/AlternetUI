using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// 
    /// </summary>
    public enum ValueValidatorKind
    {
        /// <summary>
        /// <see cref="IValueValidatorText"/> with default settings.
        /// </summary>
        Generic,

        /// <summary>
        /// <see cref="IValueValidatorText"/> which allows to enter signed integer numbers.
        /// </summary>
        SignedInt,

        /// <summary>
        /// <see cref="IValueValidatorText"/> which allows to enter unsigned integer numbers.
        /// </summary>
        UnsignedInt,

        /// <summary>
        /// <see cref="IValueValidatorText"/> which allows to enter floating point numbers.
        /// </summary>
        Float,

        /// <summary>
        /// <see cref="IValueValidatorText"/> which allows to enter signed hexadecimal integer numbers.
        /// </summary>
        SignedHex,

        /// <summary>
        /// <see cref="IValueValidatorText"/> which allows to enter unsigned hexadecimal integer numbers.
        /// </summary>
        UnsignedHex,
    }
}
