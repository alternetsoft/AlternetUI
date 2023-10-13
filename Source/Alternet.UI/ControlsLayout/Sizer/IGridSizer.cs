using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_grid_sizer.html
    // A grid sizer is a sizer which lays out its children in a two-dimensional table with
    // all table fields having the same size, i.e.
    // the width of each field is the width of the widest child, the height of
    // each field is the height of the tallest child.
    internal interface IGridSizer : ISizer
    {
        // Returns the number of columns that has been specified for the sizer.
        int ColCount { get; set; }

        // Returns the number of rows that has been specified for the sizer.
        int RowCount { get; set; }

        // Returns the number of columns currently used by the sizer.
        int EffectiveColsCount { get; }

        // Returns the number of rows currently used by the sizer.
        int EffectiveRowsCount { get; }

        // Returns the horizontal gap(in pixels) between cells in the sizer.
        int HGap { get; set; }

        // Returns the vertical gap(in pixels) between the cells in the sizer.
        int VGap { get; set; }
    }
}
