#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_sizer_flags.html
    public class SizerFlags
    {
        public static IntPtr CreateSizerFlags(int proportion = 0) => default;
        public static int GetDefaultBorder() => default;
        public static float GetDefaultBorderFractional() => default;
        public static int GetProportion(IntPtr handle) => default;
        public static int GetFlags(IntPtr handle) => default;
        public static int GetBorderInPixels(IntPtr handle) => default;

        // chained
        public static void Proportion(IntPtr handle, int proportion) { }
        public static void Expand(IntPtr handle) { }
        public static void Align(IntPtr handle, int alignment) { } // combination of wxAlignment values
        public static void Center(IntPtr handle) { }
        public static void CenterVertical(IntPtr handle) { }
        public static void CenterHorizontal(IntPtr handle) { }
        public static void Top(IntPtr handle) { }
        public static void Left(IntPtr handle) { }
        public static void Right(IntPtr handle) { }
        public static void Bottom(IntPtr handle) { }
        public static void Border(IntPtr handle, int direction, int borderInPixels) { } // wxDirection
        public static void Border2(IntPtr handle, int direction /*= wxALL*/) { }
        public static void DoubleBorder(IntPtr handle, int direction /*= wxALL*/) { }
        public static void TripleBorder(IntPtr handle, int direction /*= wxALL*/) { }
        public static void HorzBorder(IntPtr handle) { }
        public static void DoubleHorzBorder(IntPtr handle) { }
        public static void Shaped(IntPtr handle) { }
        public static void FixedMinSize(IntPtr handle) { }
        public static void ReserveSpaceEvenIfHidden(IntPtr handle) { } // makes the item ignore window's visibility status
    }
}