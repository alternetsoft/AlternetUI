using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_box_sizer.html

    /// <summary>
    /// The basic idea behind a box sizer is that windows will most often be laid out in
    /// rather simple basic geometry, typically in a row or a column or several hierarchies
    /// of either.
    /// </summary>
    internal interface IBoxSizer : ISizer
    {
        /// <summary>
        /// Returns the orientation of the box sizer.
        /// </summary>
        bool IsVertical { get; set; }
    }
}
