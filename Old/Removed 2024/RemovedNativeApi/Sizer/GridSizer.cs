#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_grid_sizer.html
    //A grid sizer is a sizer which lays out its children in a two-dimensional table with
    //all table fields having the same size, i.e.
    //the width of each field is the width of the widest child, the height of
    //each field is the height of the tallest child.
    public class GridSizer : Sizer
    {
        // Returns the number of columns that has been specified for the sizer.
        public static int GetCols(IntPtr handle) => default;

        // Returns the number of rows that has been specified for the sizer.
        public static int GetRows(IntPtr handle) => default;

        // Returns the number of columns currently used by the sizer.
        public static int GetEffectiveColsCount(IntPtr handle) => default;

        // Returns the number of rows currently used by the sizer.
        public static int GetEffectiveRowsCount(IntPtr handle) => default;

        // Returns the horizontal gap(in pixels) between cells in the sizer.
        public static int GetHGap(IntPtr handle) => default;

        // Returns the vertical gap(in pixels) between the cells in the sizer.
        public static int GetVGap(IntPtr handle) => default;

        // Sets the number of columns in the sizer. 
        public static void SetCols(IntPtr handle, int cols) { }

        // Sets the horizontal gap (in pixels) between cells in the sizer.
        public static void SetHGap(IntPtr handle, int gap) { }

        // Sets the number of rows in the sizer. 
        public static void SetRows(IntPtr handle, int rows) { }

        // Sets the vertical gap (in pixels) between the cells in the sizer.
        public static void SetVGap(IntPtr handle, int gap) { }

        public static SizeI CalcMin(IntPtr handle) => default;

        public static void RepositionChildren(IntPtr handle, SizeI minSize) { }

        public static IntPtr CreateGridSizer(int cols, int vgap, int hgap) => default;

        public static IntPtr CreateGridSizer2(int rows, int cols, int vgap, int hgap) => default;
    }
}
