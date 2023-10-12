using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_flex_grid_sizer.html
    // A flex grid sizer is a sizer which lays out its children in a two-dimensional
    // table with all table fields in one row having the same height and all fields
    // in one column having the same width, but all rows or all columns are not
    // necessarily the same height or width as in the wxGridSizer.
    internal interface IFlexGridSizer : IGridSizer
    {
        // Specifies that column idx(starting from zero) should be grown if there
        // is extra space available to the sizer.
        void AddGrowableCol(int idx, int proportion = 0);

        // Specifies that row idx(starting from zero) should be grown if there
        // is extra space available to the sizer.
        void AddGrowableRow(int idx, int proportion = 0);

        // Returns a wxOrientation value that specifies whether the sizer flexibly
        // resizes its columns, rows, or both(default).
        // int GetFlexibleDirection();

        // Returns the value that specifies how the sizer grows in the "non-flexible"
        // direction if there is one.
        // /*wxFlexSizerGrowMode*/ int GetNonFlexibleGrowMode();

        // Returns true if column idx is growable.
        bool IsColGrowable(int idx);

        // Returns true if row idx is growable.
        bool IsRowGrowable(int idx);

        // Specifies that the idx column index is no longer growable.
        void RemoveGrowableCol(int idx);

        // Specifies that the idx row index is no longer growable.
        void RemoveGrowableRow(int idx);

        // Specifies whether the sizer should flexibly resize its columns, rows, or both.
        // void SetFlexibleDirection(int direction);

        // Specifies how the sizer should grow in the non-flexible direction if there
        // is one (so SetFlexibleDirection() must have been called previously).
        // void SetNonFlexibleGrowMode(int /*wxFlexSizerGrowMode*/ mode);

        // Returns a wxArrayInt read-only array containing the heights of the rows in the sizer.
        // IntPtr GetRowHeights();

        // Returns a read-only array containing the widths of the columns in the sizer.
        // IntPtr GetColWidths();
    }
}
