using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_box_sizer.html
    // The basic idea behind a box sizer is that windows will most often be laid out in
    // rather simple basic geometry, typically in a row or a column or several hierarchies
    // of either.
    internal interface IBoxSizer : ISizer
    {
        // Returns the orientation of the box sizer, either wxVERTICAL or wxHORIZONTAL.
        bool IsVertical { get; set; }

        // Adds non-stretchable space to the main orientation of the sizer only.
        IntPtr AddSpacer(int size);
    }
}
