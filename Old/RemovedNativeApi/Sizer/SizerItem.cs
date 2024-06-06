#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class SizerItem
    {
        public static IntPtr CreateSizerItem(
            IntPtr window,
            int proportion = 0,
            int flag = 0,
            int border = 0,
            IntPtr userData = default) => default;

        public static IntPtr CreateSizerItem2(IntPtr window, IntPtr sizerFlags) => default;

        public static IntPtr CreateSizerItem3(
            IntPtr sizer,
            int proportion = 0,
            int flag = 0,
            int border = 0,
            IntPtr userData = default) => default;

        public static IntPtr CreateSizerItem4(IntPtr sizer, IntPtr sizerFlags) => default;

        public static IntPtr CreateSizerItem5(
            int width,
            int height,
            int proportion = 0,
            int flag = 0,
            int border = 0,
            IntPtr userData = default) => default;

        public static IntPtr CreateSizerItem6(int width, int height, IntPtr sizerFlags) => default;

        public static IntPtr CreateSizerItem7() => default;

        public static void DeleteWindows(IntPtr handle) { }

        // Enable deleting the SizerItem without destroying the contained sizer.
        public static void DetachSizer(IntPtr handle) { }

        // Enable deleting the SizerItem without resetting the sizer in the
        // contained window.
        public static void DetachWindow(IntPtr handle) { }

        public static SizeI GetSize(IntPtr handle) => default;
        public static SizeI CalcMin(IntPtr handle) => default;
        public static void SetDimension(IntPtr handle, PointI pos, SizeI size) { }

        public static SizeI GetMinSize(IntPtr handle) => default;
        public static SizeI GetMinSizeWithBorder(IntPtr handle) => default;
        public static SizeI GetMaxSize(IntPtr handle) => default;
        public static SizeI GetMaxSizeWithBorder(IntPtr handle) => default;

        public static void SetMinSize(IntPtr handle, int x, int y) { }
        public static void SetInitSize(IntPtr handle, int x, int y) { }

        // if either of dimensions is zero, ratio is assumed to be 1
        // to avoid "divide by zero" errors
        public static void SetRatio(IntPtr handle, int width, int height) { }
        public static void SetRatio2(IntPtr handle, float ratio) { }
        public static float GetRatio(IntPtr handle) => default;

        public static RectI GetRect(IntPtr handle) => default;

        // set a sizer item id (different from a window id, all sizer items,
        // including spacers, can have an associated id)
        public static void SetId(IntPtr handle, int id) { }
        public static int GetId(IntPtr handle) => default;

        public static bool IsWindow(IntPtr handle) => default;
        public static bool IsSizer(IntPtr handle) => default;
        public static bool IsSpacer(IntPtr handle) => default;

        public static void SetProportion(IntPtr handle, int proportion) { }
        public static int GetProportion(IntPtr handle) => default;
        public static void SetFlag(IntPtr handle, int flag) { }
        public static int GetFlag(IntPtr handle) => default;
        public static void SetBorder(IntPtr handle, int border) { }
        public static int GetBorder(IntPtr handle) => default;

        public static IntPtr GetWindow(IntPtr handle) => default;
        public static IntPtr GetSizer(IntPtr handle) => default;
        public static SizeI GetSpacer(IntPtr handle) => default;

        // This function behaves obviously for the windows and spacers but for the
        // sizers it returns true if any sizer element is shown and only returns
        // false if all of them are hidden. Also, it always returns true if
        // wxRESERVE_SPACE_EVEN_IF_HIDDEN flag was used.
        public static bool IsShown(IntPtr handle) => default;

        public static void Show(IntPtr handle, bool show) { }

        public static void SetUserData(IntPtr handle, IntPtr userData) { }
        public static IntPtr GetUserData(IntPtr handle) => default;
        public static PointI GetPosition(IntPtr handle) => default;

        // Called once the first component of an item has been decided. This is
        // used in algorithms that depend on knowing the size in one direction
        // before the min size in the other direction can be known.
        // Returns true if it made use of the information (and min size was changed).
        public static bool InformFirstDirection(IntPtr handle, int direction,
            int size, int availableOtherDir = -1) => default;

        // these functions delete the current contents of the item if it's a sizer
        // or a spacer but not if it is a window
        public static void AssignWindow(IntPtr handle, IntPtr window) { }
        public static void AssignSizer(IntPtr handle, IntPtr sizer) { }
        public static void AssignSpacer(IntPtr handle, int w, int h) { }

    }
}