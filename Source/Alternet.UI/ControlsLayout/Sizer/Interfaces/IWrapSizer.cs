using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_wrap_sizer.html

    /// <summary>
    /// A wrap sizer lays out its items in a single line, like a box sizer – as long as
    /// there is space available in that direction.
    /// Once all available space in the primary direction has been used, a new line is
    /// added and items are added there.
    /// So a wrap sizer has a primary orientation for adding items, and adds lines as needed
    /// in the secondary direction.
    /// </summary>
    internal interface IWrapSizer : IBoxSizer
    {
    }
}
