using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies result of calling <see cref="MathUtils.ValueInRange"/> and similar methods.
    /// </summary>
    public enum ValueInRangeResult
    {
        /// <summary>
        /// Unknown result.
        /// </summary>
        Unknown,

        /// <summary>
        /// Value is in range.
        /// </summary>
        Ok,

        /// <summary>
        /// Value is less than range's minimal value.
        /// </summary>
        Less,

        /// <summary>
        /// Value is greater than range's max value.
        /// </summary>
        Greater,
    }
}
