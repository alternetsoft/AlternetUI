using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the alignment of a drop-down relative to a reference element.
    /// </summary>
    public enum DropDownAlignment
    {
        /// <summary>
        /// Aligns the drop-down before the start edge of the reference element.
        /// </summary>
        BeforeStart,

        /// <summary>
        /// Aligns the drop-down after the start edge of the reference element.
        /// </summary>
        AfterStart,

        /// <summary>
        /// Aligns the drop-down before the end edge of the reference element.
        /// </summary>
        BeforeEnd,

        /// <summary>
        /// Aligns the drop-down after the end edge of the reference element.
        /// </summary>
        AfterEnd,

        /// <summary>
        /// Aligns the drop-down centered relative to the reference element.
        /// </summary>
        Center,

        /// <summary>
        /// Aligns the drop-down at a specific position relative to the reference element.
        /// </summary>
        Position,
    }
}
