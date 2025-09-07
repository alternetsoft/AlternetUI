using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_grid_sizer.html

    /// <summary>
    /// A grid sizer is a sizer which lays out its children in a two-dimensional table with
    /// all table fields having the same size, i.e.
    /// the width of each field is the width of the widest child, the height of
    /// each field is the height of the tallest child.
    /// </summary>
    internal interface IGridSizer : ISizer
    {
        /// <summary>
        /// Gets or sets the number of columns that has been specified for the sizer.
        /// </summary>
        int ColCount { get; set; }

        /// <summary>
        /// Gets or sets the number of rows that has been specified for the sizer.
        /// </summary>
        int RowCount { get; set; }

        /// <summary>
        /// Gets the number of columns currently used by the sizer.
        /// </summary>
        int EffectiveColsCount { get; }

        /// <summary>
        /// Gets the number of rows currently used by the sizer.
        /// </summary>
        int EffectiveRowsCount { get; }

        /// <summary>
        /// Gets or sets the horizontal gap(in pixels) between cells in the sizer.
        /// </summary>
        int HGap { get; set; }

        /// <summary>
        /// Gets the vertical gap(in pixels) between the cells in the sizer.
        /// </summary>
        int VGap { get; set; }
    }
}
