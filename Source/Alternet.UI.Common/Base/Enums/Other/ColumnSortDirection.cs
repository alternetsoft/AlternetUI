using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the direction in which a column is sorted.
    /// </summary>
    public enum ColumnSortDirection
    {
        /// <summary>
        /// The column is sorted in ascending order.
        /// </summary>
        Ascending,

        /// <summary>
        /// The column is sorted in descending order.
        /// </summary>
        Descending,

        /// <summary>
        /// The sort direction is toggled or flipped.
        /// </summary>
        Flip,
    }
}
