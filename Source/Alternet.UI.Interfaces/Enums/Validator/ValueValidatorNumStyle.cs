using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Validation styles used by the numbers validator.
    /// </summary>
    public enum ValueValidatorNumStyle
    {
        /// <summary>
        /// Value is signed non-float number.
        /// </summary>
        Signed = 0,

        /// <summary>
        /// Value is unsigned non-float number,
        /// </summary>
        Unsigned,

        /// <summary>
        /// Value is float number.
        /// </summary>
        Float,
    }
}
