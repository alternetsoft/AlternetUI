using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_grid_bag_sizer.html
    // A wxSizer that can lay out items in a virtual grid like a wxFlexGridSizer but in
    // this case explicit positioning of the items is allowed using wxGBPosition,
    // and items can optionally span more than one row and/or column using wxGBSpan.
    internal interface IGridBagSizer : IFlexGridSizer
    {
        // Return the sizer item located at the point given in pt, or NULL if there is
        // no item at that point.
        // IntPtr FindItemAtPoint(Int32Point pt);

        // Return the sizer item for the given grid cell, or NULL if there is no item
        // at that position.
        // IntPtr FindItemAtPosition(Int32Point pos);

        // Return the sizer item that has a matching user data(it only compares
        // pointer values) or NULL if not found.
        // IntPtr FindItemWithData(IntPtr userData);

        // Get the size of the specified cell, including hgap and vgap.
        Int32Size GetCellSize(int row, int col);

        // Get the size used for cells in the grid with no item.
        Int32Size GetEmptyCellSize();

        // Set the size used for cells in the grid with no item.
        void SetEmptyCellSize(Int32Size sz);

        // Adds the given item to the given position.
        // IntPtr Add(Control window, Int32Point pos,
        //    Int32Size span, int flag /*= 0*/, int border /*= 0*/,
        //    IntPtr userData);

        // Adds the given item to the given position.
        // IntPtr Add(IntPtr sizer, Int32Point pos,
        //    Int32Size span, int flag /*= 0*/,
        //    int border /*= 0*/, IntPtr userData);

        // Adds the given item to the given position.
        // IntPtr Add(IntPtr item);

        // Adds a spacer to the given position.
        // IntPtr Add(int width, int height, Int32Point pos,
        //    Int32Size span, int flag /*= 0*/,
        //    int border /*= 0*/, IntPtr userData);

        // Look at all items and see if any intersect(or would overlap) the given item.
        // bool CheckForIntersection(IntPtr item, IntPtr excludeItem);

        // Look at all items and see if any intersect(or would overlap) the given item.
        // bool CheckForIntersection2(Int32Point pos,
        //    Int32Size span, IntPtr excludeItem);

        // Find the sizer item for the given window or subsizer, returns NULL if not found.
        ISizer FindItem(Control window);

        // Find the sizer item for the given window or subsizer, returns NULL if not found.
        ISizer FindItem2(ISizer sizer);

        // Get the grid position of the specified item.
        Int32Point GetItemPosition(Control window);

        // Get the grid position of the specified item.
        Int32Point GetItemPosition(ISizer sizer);

        // Get the grid position of the specified item.
        Int32Point GetItemPosition(int index);

        // Get the row/col spanning of the specified item.
        Int32Point GetItemSpan(Control window);

        // Get the row/col spanning of the specified item.
        Int32Point GetItemSpan(ISizer sizer);

        // Get the row/col spanning of the specified item.
        Int32Point GetItemSpan(int index);

        // Set the grid position of the specified item.
        bool SetItemPosition(Control window, Int32Point pos);

        // Set the grid position of the specified item.
        bool SetItemPosition(ISizer sizer, Int32Point pos);

        // Set the grid position of the specified item.
        bool SetItemPosition(int index, Int32Point pos);

        // Set the row/col spanning of the specified item.
        bool SetItemSpan(Control window, Int32Size span);

        // Set the row/col spanning of the specified item.
        bool SetItemSpan(ISizer sizer, Int32Size span);

        bool SetItemSpan(int index, Int32Size span);
    }
}
