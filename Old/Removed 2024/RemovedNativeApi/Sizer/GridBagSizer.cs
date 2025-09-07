#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_grid_bag_sizer.html
    //A wxSizer that can lay out items in a virtual grid like a wxFlexGridSizer but in
    //this case explicit positioning of the items is allowed using wxGBPosition,
    //and items can optionally span more than one row and/or column using wxGBSpan.
    public class GridBagSizer : FlexGridSizer
    {
        // Constructor, with optional parameters to specify the gap between the rows and columns.
        public static IntPtr CreateGridBagSizer(int vgap = 0, int hgap = 0) => default;

        // Called when the managed size of the sizer is needed or when layout needs done. 
        public static SizeI CalcMin(IntPtr handle) => default;

        // Return the sizer item located at the point given in pt, or NULL if there is
        // no item at that point.
        public static IntPtr FindItemAtPoint(IntPtr handle, PointI pt) => default;

        // Return the sizer item for the given grid cell, or NULL if there is no item
        // at that position.
        public static IntPtr FindItemAtPosition(IntPtr handle, PointI pos) => default;

        // Return the sizer item that has a matching user data(it only compares
        // pointer values) or NULL if not found.
        public static IntPtr FindItemWithData(IntPtr handle, IntPtr userData) => default;

        // Get the size of the specified cell, including hgap and vgap.
        public static SizeI GetCellSize(IntPtr handle, int row, int col) => default;

        // Get the size used for cells in the grid with no item.
        public static SizeI GetEmptyCellSize(IntPtr handle) => default;

        // Called when the managed size of the sizer is needed or when layout needs done.
        public static void RepositionChildren(IntPtr handle, SizeI minSize) { }

        // Set the size used for cells in the grid with no item.
        public static void SetEmptyCellSize(IntPtr handle, SizeI sz) { }

        // Adds the given item to the given position.
        public static IntPtr Add(IntPtr handle, IntPtr window, PointI pos,
            SizeI span, int flag /*= 0*/, int border /*= 0*/,
            IntPtr userData) => default;

        // Adds the given item to the given position.
        public static IntPtr Add2(IntPtr handle, IntPtr sizer, PointI pos,
            SizeI span, int flag /*= 0*/,
            int border /*= 0*/, IntPtr userData) => default;

        // Adds the given item to the given position.
        public static IntPtr Add3(IntPtr handle, IntPtr item) => default;

        // Adds a spacer to the given position.
        public static IntPtr Add4(IntPtr handle, int width, int height, PointI pos,
            SizeI span, int flag /*= 0*/,
            int border /*= 0*/, IntPtr userData) => default;

        // Look at all items and see if any intersect(or would overlap) the given item.
        public static bool CheckForIntersection(IntPtr handle, IntPtr item,
            IntPtr excludeItem) => default;

        // Look at all items and see if any intersect(or would overlap) the given item.
        public static bool CheckForIntersection2(IntPtr handle, PointI pos,
            SizeI span, IntPtr excludeItem) => default;

        // Find the sizer item for the given window or subsizer, returns NULL if not found.
        public static IntPtr FindItem(IntPtr handle, IntPtr window) => default;

        // Find the sizer item for the given window or subsizer, returns NULL if not found. 
        public static IntPtr FindItem2(IntPtr handle, IntPtr sizer) => default;

        // Get the grid position of the specified item.
        public static PointI GetItemPosition(IntPtr handle, IntPtr window) => default;

        // Get the grid position of the specified item.
        public static PointI GetItemPosition2(IntPtr handle, IntPtr sizer) => default;

        // Get the grid position of the specified item.
        public static PointI GetItemPosition3(IntPtr handle, int index) => default;

        // Get the row/col spanning of the specified item. 
        public static PointI GetItemSpan(IntPtr handle, IntPtr window) => default;

        // Get the row/col spanning of the specified item. 
        public static PointI GetItemSpan2(IntPtr handle, IntPtr sizer) => default;

        // Get the row/col spanning of the specified item. 
        public static PointI GetItemSpan3(IntPtr handle, int index) => default;

        // Set the grid position of the specified item.
        public static bool SetItemPosition(IntPtr handle, IntPtr window, PointI pos) => default;

        // Set the grid position of the specified item.
        public static bool SetItemPosition2(IntPtr handle, IntPtr sizer, PointI pos) => default;

        // Set the grid position of the specified item.
        public static bool SetItemPosition3(IntPtr handle, int index, PointI pos) => default;

        // Set the row/col spanning of the specified item.
        public static bool SetItemSpan(IntPtr handle, IntPtr window, SizeI span) => default;

        // Set the row/col spanning of the specified item.
        public static bool SetItemSpan2(IntPtr handle, IntPtr sizer, SizeI span) => default;

        public static bool SetItemSpan3(IntPtr handle, int index, SizeI span) => default;
    }
}
