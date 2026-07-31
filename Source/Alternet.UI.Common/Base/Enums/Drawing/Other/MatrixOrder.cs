using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the order for matrix transform operations.
    /// </summary>
    public enum MatrixOrder
    {
        /// <summary>
        /// The new operation is applied before the old operation.
        /// </summary>
        Prepend,

        /// <summary>
        /// The new operation is applied after the old operation.
        /// </summary>
        Append,
    }
}
