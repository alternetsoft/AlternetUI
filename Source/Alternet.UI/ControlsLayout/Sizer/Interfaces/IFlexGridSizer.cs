using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_flex_grid_sizer.html

    /// <summary>
    /// A flex grid sizer is a sizer which lays out its children in a two-dimensional
    /// table with all table fields in one row having the same height and all fields
    /// in one column having the same width, but all rows or all columns are not
    /// necessarily the same height or width as in the <see cref="IGridSizer"/>.
    /// </summary>
    internal interface IFlexGridSizer : IGridSizer
    {
        /// <summary>
        /// Gets or sets whether the sizer flexibly resizes its columns, rows, or both(default).
        /// </summary>
        GenericOrientation FlexibleDirection { get; set; }

        /// <summary>
        /// Gets or sets how the sizer should grow in the non-flexible direction if there
        /// is one (so <see cref="FlexibleDirection"/> must have been set previously).
        /// </summary>
        FlexSizerGrowMode NonFlexibleGrowMode { get; set; }

        /// <summary>
        /// Specifies that column should be grown if there
        /// is extra space available to the sizer.
        /// </summary>
        /// <param name="idx">Column index (starting from zero).</param>
        /// <param name="proportion">Grow proportion.</param>
        void AddGrowableCol(int idx, int proportion = 0);

        /// <summary>
        /// Specifies that row should be grown if there
        /// is extra space available to the sizer.
        /// </summary>
        /// <param name="idx">Row index (starting from zero).</param>
        /// <param name="proportion">Grow proportion.</param>
        void AddGrowableRow(int idx, int proportion = 0);

        /// <summary>
        /// Returns <c>true</c> if column is growable.
        /// </summary>
        /// <param name="idx">Column index.</param>
        bool IsColGrowable(int idx);

        /// <summary>
        /// Returns <c>true</c> if row is growable.
        /// </summary>
        /// <param name="idx">Row index.</param>
        bool IsRowGrowable(int idx);

        /// <summary>
        /// Specifies that the column is no longer growable.
        /// </summary>
        /// <param name="idx">Column index.</param>
        void RemoveGrowableCol(int idx);

        /// <summary>
        /// Specifies that the row is no longer growable.
        /// </summary>
        /// <param name="idx">Row index.</param>
        void RemoveGrowableRow(int idx);

         // Returns a wxArrayInt read-only array containing the heights of the rows in the sizer.
        // IntPtr GetRowHeights();

        // Returns a read-only array containing the widths of the columns in the sizer.
        // IntPtr GetColWidths();
    }
}