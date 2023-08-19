#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_grid_bag_sizer.html
    //A wxSizer that can lay out items in a virtual grid like a wxFlexGridSizer but in
    //this case explicit positioning of the items is allowed using wxGBPosition,
    //and items can optionally span more than one row and/or column using wxGBSpan.
    public class GridBagSizer : FlexGridSizer
    {
    }
}
