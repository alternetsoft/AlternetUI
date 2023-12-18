#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_box_sizer.html
    //The basic idea behind a box sizer is that windows will most often be laid out in
    //rather simple basic geometry, typically in a row or a column or several hierarchies
    //of either.
    public class BoxSizer : Sizer
    {
        public static IntPtr CreateBoxSizer(int orient) => default;

        // Adds non-stretchable space to the main orientation of the sizer only.
        public static IntPtr AddSpacer(IntPtr handle, int size) => default;

        // Implements the calculation of a box sizer's minimal. 
        public static SizeI CalcMin(IntPtr handle) => default;

        // Returns the orientation of the box sizer, either wxVERTICAL or wxHORIZONTAL.
        public static int GetOrientation(IntPtr handle) => default;

        // Sets the orientation of the box sizer, either wxVERTICAL or wxHORIZONTAL. 
        public static void SetOrientation(IntPtr handle, int orient) { }

        public static void RepositionChildren(IntPtr handle, SizeI minSize) { }
    }
}
