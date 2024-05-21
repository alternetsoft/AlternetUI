using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_grid_bag_sizer.html

    /// <summary>
    /// A sizer that can lay out items in a virtual grid like a <see cref="IFlexGridSizer"/> but in
    /// this case explicit positioning of the items is allowed,
    /// and items can optionally span more than one row and/or column.
    /// </summary>
    internal interface IGridBagSizer : IFlexGridSizer
    {
        /// <summary>
        /// Gets or sets the size used for cells in the grid with no item.
        /// </summary>
        SizeI EmptyCellSize { get; set; }

        /// <summary>
        /// Return the sizer item located at the point given in pt, or <c>null</c> if there is
        /// no item at that point.
        /// </summary>
        /// <param name="pt">Point.</param>
        ISizerItem? FindItemAtPoint(PointI pt);

        /// <summary>
        /// Return the sizer item for the given grid cell, or <c>null</c> if there is no item
        /// at that position.
        /// </summary>
        /// <param name="pos">Grid cell position.</param>
        ISizerItem? FindItemAtPosition(PointI pos);

        /// <summary>
        /// Adds the given item to the given position.
        /// </summary>
        /// <param name="control">Contents of the item.</param>
        /// <param name="pos">Item position.</param>
        /// <param name="span">Item row/column span.</param>
        /// <param name="flag">Item flags.</param>
        /// <param name="border">Item border.</param>
        ISizerItem Add(
            Control control,
            PointI pos,
            SizeI span,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Adds the given item to the given position.
        /// </summary>
        /// <param name="sizer">Contents of the item.</param>
        /// <param name="pos">Item position.</param>
        /// <param name="span">Item row/column span.</param>
        /// <param name="flag">Item flags.</param>
        /// <param name="border">Item border.</param>
        ISizerItem Add(
            ISizer sizer,
            PointI pos,
            SizeI span,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Adds a spacer to the given position.
        /// </summary>
        /// <param name="width">Item width.</param>
        /// <param name="height">Item height.</param>
        /// <param name="pos">Item position.</param>
        /// <param name="span">Item row/column span.</param>
        /// <param name="flag">Item flags.</param>
        /// <param name="border">Item border.</param>
        ISizerItem Add(
            int width,
            int height,
            PointI pos,
            SizeI span,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Looks at all items and see if any intersect(or would overlap) the given item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="excludeItem">Exclude item.</param>
        /// <returns>Returns <c>true</c> if so, <c>false</c> if there would be no overlap.</returns>
        /// <remarks>
        /// If an excludeItem is given then it will not be checked for intersection,
        /// for example it may be the item we are checking the position of.
        /// </remarks>
        bool CheckForIntersection(ISizerItem item, ISizerItem? excludeItem = null);

        /// <summary>
        /// Looks at all items and see if any intersect(or would overlap) the given item.
        /// </summary>
        /// <param name="pos">Position in the grid.</param>
        /// <param name="span">Row/Column span.</param>
        /// <param name="excludeItem">Exclude item.</param>
        /// <returns>Returns <c>true</c> if so, <c>false</c> if there would be no overlap.</returns>
        /// <remarks>
        /// If an excludeItem is given then it will not be checked for intersection,
        /// for example it may be the item we are checking the position of.
        /// </remarks>
        bool CheckForIntersection(PointI pos, SizeI span, ISizerItem? excludeItem = null);

        /// <summary>
        /// Gets the size of the specified cell, including hgap and vgap.
        /// </summary>
        /// <param name="row">Item row.</param>
        /// <param name="col">Item column.</param>
        SizeI GetCellSize(int row, int col);

        /// <summary>
        /// Finds the sizer item for the given control, returns <c>null</c> if not found.
        /// </summary>
        /// <param name="control">Control.</param>
        ISizerItem FindItem(Control control);

        /// <summary>
        /// Finds the sizer item for the given sub-sizer, returns <c>null</c> if not found.
        /// </summary>
        /// <param name="sizer">Sizer which is attached to the item.</param>
        ISizerItem FindItem(ISizer sizer);

        /// <summary>
        /// Gets the grid position of the specified item.
        /// </summary>
        /// <param name="control">Control which is attached to the item.</param>
        PointI GetItemPosition(Control control);

        /// <summary>
        /// Gets the grid position of the specified item.
        /// </summary>
        /// <param name="sizer">Sizer which is attached to the item.</param>
        PointI GetItemPosition(ISizer sizer);

        /// <summary>
        /// Gets the grid position of the specified item.
        /// </summary>
        /// <param name="index">Item index.</param>
        PointI GetItemPosition(int index);

        /// <summary>
        /// Gets the row/col spanning of the specified item.
        /// </summary>
        /// <param name="control">Control which is attached to the item.</param>
        PointI GetItemSpan(Control control);

        /// <summary>
        /// Gets the row/col spanning of the specified item.
        /// </summary>
        /// <param name="sizer">Sizer which is attached to the item.</param>
        PointI GetItemSpan(ISizer sizer);

        /// <summary>
        /// Gets the row/col spanning of the specified item.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        PointI GetItemSpan(int index);

        /// <summary>
        /// Sets the grid position of the specified item.
        /// </summary>
        /// <param name="control">Control which is attached to the item.</param>
        /// <param name="pos">Item position in the grid.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        bool SetItemPosition(Control control, PointI pos);

        /// <summary>
        /// Sets the grid position of the specified item.
        /// </summary>
        /// <param name="sizer">Sizer which is attached to the item.</param>
        /// <param name="pos">Item position in the grid.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        bool SetItemPosition(ISizer sizer, PointI pos);

        /// <summary>
        /// Sets the grid position of the specified item.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="pos">Item position in the grid.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        bool SetItemPosition(int index, PointI pos);

        /// <summary>
        /// Sets the row/col spanning of the specified item.
        /// </summary>
        /// <param name="control">Control which is attached to the item.</param>
        /// <param name="span">Number of rows and columns spanned by the item.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        bool SetItemSpan(Control control, SizeI span);

        /// <summary>
        /// Sets the row/col spanning of the specified item.
        /// </summary>
        /// <param name="span">Number of rows and columns spanned by the item.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        /// <param name="sizer">Sizer which is attached to the item.</param>
        bool SetItemSpan(ISizer sizer, SizeI span);

        /// <summary>
        /// Sets the row/col spanning of the specified item.
        /// </summary>
        /// <param name="span">Number of rows and columns spanned by the item.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        /// <param name="index">Item index.</param>
        bool SetItemSpan(int index, SizeI span);
    }
}
