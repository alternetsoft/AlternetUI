using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Styles used by <see cref="ValueValidatorFactory.CreateValueValidatorNum"/>.
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
