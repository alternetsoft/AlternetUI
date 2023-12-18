#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_flex_grid_sizer.html
    //A flex grid sizer is a sizer which lays out its children in a two-dimensional
    //table with all table fields in one row having the same height and all fields
    //in one column having the same width, but all rows or all columns are not
    //necessarily the same height or width as in the wxGridSizer.
    public class FlexGridSizer : GridSizer
    {
        // Specifies that column idx(starting from zero) should be grown if there
        // is extra space available to the sizer.
        public static void AddGrowableCol(IntPtr handle, int idx, int proportion = 0) { }

        // Specifies that row idx(starting from zero) should be grown if there
        // is extra space available to the sizer.
        public static void AddGrowableRow(IntPtr handle, int idx, int proportion = 0) { }

        // Returns a wxOrientation value that specifies whether the sizer flexibly
        // resizes its columns, rows, or both(default). 
        public static int GetFlexibleDirection(IntPtr handle) => default;

        // Returns the value that specifies how the sizer grows in the "non-flexible"
        // direction if there is one.
        public static /*wxFlexSizerGrowMode*/ int GetNonFlexibleGrowMode(IntPtr handle) => default;

        // Returns true if column idx is growable.
        public static bool IsColGrowable(IntPtr handle, int idx) => default;

        // Returns true if row idx is growable.
        public static bool IsRowGrowable(IntPtr handle, int idx) => default;

        // Specifies that the idx column index is no longer growable.
        public static void RemoveGrowableCol(IntPtr handle, int idx) { }

        // Specifies that the idx row index is no longer growable.
        public static void RemoveGrowableRow(IntPtr handle, int idx) { }

        // Specifies whether the sizer should flexibly resize its columns, rows, or both.
        public static void SetFlexibleDirection(IntPtr handle, int direction) { }

        // Specifies how the sizer should grow in the non-flexible direction if there
        // is one (so SetFlexibleDirection() must have been called previously). 
        public static void SetNonFlexibleGrowMode(IntPtr handle, int /*wxFlexSizerGrowMode*/ mode) { }

        // Returns a wxArrayInt read-only array containing the heights of the rows in the sizer.
        public static IntPtr GetRowHeights(IntPtr handle) => default;

        // Returns a read-only array containing the widths of the columns in the sizer.
        public static IntPtr GetColWidths(IntPtr handle) => default;

        public static void RepositionChildren(IntPtr handle, SizeI minSize) { }

        public static SizeI CalcMin(IntPtr handle) => default;

        public static IntPtr CreateFlexGridSizer(int cols,
            int vgap, int hgap) => default;

        public static IntPtr CreateFlexGridSizer2(int rows,
            int cols, int vgap, int hgap) => default;
    }
}
