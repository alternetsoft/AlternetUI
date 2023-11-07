#pragma warning disable
using ApiCommon;
using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_rich_tool_tip.html
    // https://docs.wxwidgets.org/3.2/classwx_tool_tip.html
    // https://docs.wxwidgets.org/3.2/classwx_display.html
    // https://docs.wxwidgets.org/3.2/classwx_caret.html
    // https://docs.wxwidgets.org/3.2/classwx_cursor.html
    public class WxOtherFactory
    {
        // =================== RichToolTip

        // Ctor must specify the tooltip title and main message, additional
        // attributes can be set later.
        public static IntPtr CreateRichToolTip(string title, string message) => default;
        public static void DeleteRichToolTip(IntPtr handle) { }

        // Set the background color: if two colors are specified, the background
        // is drawn using a gradient from top to bottom, otherwise a single solid
        // color is used.
        public static void RichToolTipSetBkColor(IntPtr handle, Color color, Color endColor) { }

        // Set the small icon to show: either one of the standard information/
        // warning/error ones(the question icon doesn't make sense for a tooltip)
        // or a custom icon.
        public static void RichToolTipSetIcon(IntPtr handle, ImageSet? bitmapBundle) { }
        public static void RichToolTipSetIcon2(IntPtr handle, int icon) { } // wxICON_* in defs.h

        // Set timeout after which the tooltip should disappear, in milliseconds.
        // By default the tooltip is hidden after system-dependent interval of time
        // elapses but this method can be used to change this or also disable
        // hiding the tooltip automatically entirely by passing 0 in this parameter
        //(but doing this can result in native version not being used).
        // Optionally specify a show delay.
        public static void RichToolTipSetTimeout(IntPtr handle, uint milliseconds, uint millisecondsShowdelay = 0) { }

        // Choose the tip kind, possibly none. By default the tip is positioned
        // automatically, as if wxTipKind_Auto was used.
        public static void RichToolTipSetTipKind(IntPtr handle, int tipKind) { } // wxTipKind

        // Set the title text font. By default it's emphasized using the font style
        // or colour appropriate for the current platform.
        public static void RichToolTipSetTitleFont(IntPtr handle, Font? font) {}

        // Show the tooltip for the given window and optionally a specified area.
        public static void RichToolTipShowFor(IntPtr handle, IntPtr window, Int32Rect rect) { }

        // =================== ToolTip

        public static IntPtr CreateToolTip(string tip) => default;
        public static void DeleteToolTip(IntPtr handle) { }
        public static string ToolTipGetTip(IntPtr handle) => default;
        public static IntPtr ToolTipGetWindow(IntPtr handle) => default;
        public static void ToolTipSetTip(IntPtr handle, string tip) { }

        // Enable or disable tooltips globally. 
        // May not be supported on all platforms(eg. wxCocoa).
        public static void ToolTipEnable(bool flag) { }

        // Set the delay after which the tooltip disappears or how long a tooltip remains visible. 	
        // May not be supported on all platforms(eg. wxCocoa, GTK).
        public static void ToolTipSetAutoPop(long msecs) { }

        // Set the delay after which the tooltip appears. 
        // May not be supported on all platforms.
        public static void ToolTipSetDelay(long msecs) { }

        // Set tooltip maximal width in pixels. By default, tooltips are wrapped at a suitably
        // chosen width. You can pass -1 as width to disable wrapping them completely,
        // 0 to restore the default behaviour or an arbitrary positive value to wrap
        // them at the given width. Notice that this function does not change the width of
        // the tooltips created before calling it. Currently this function is wxMSW-only.
        public static void ToolTipSetMaxWidth(int width) { }

        // Set the delay between subsequent tooltips to appear. 
        public static void ToolTipSetReshow(long msecs) { }

        // =================== Cursor

        public static IntPtr CreateCursor() => default;

        //ructs a cursor using a cursor identifier.
        public static IntPtr CreateCursor2(int cursorId) => default; // wxStockCursor     

        //ructs a cursor by passing a string resource name or filename.
        public static IntPtr CreateCursor3(string cursorName, int type,
            int hotSpotX = 0, int hotSpotY = 0) => default; // =wxCURSOR_DEFAULT_TYPE

        //ructs a cursor from an image.
        public static IntPtr CreateCursor4(Image image) => default;

        public static void DeleteCursor(IntPtr handle) { }

        // Returns true if cursor data is present. 
        public static bool CursorIsOk(IntPtr handle) => default;

        public static Int32Point CursorGetHotSpot(IntPtr handle) => default;

        // =================== Caret

        public static void DeleteCaret(IntPtr handle) { }

        // Get the caret position(in pixels).
        public static Int32Point CaretGetPosition(IntPtr handle) => default;

        // Get the caret size.
        public static Int32Size CaretGetSize(IntPtr handle) => default;

        // Move the caret to given position(in logical coordinates).
        public static void CaretMove(IntPtr handle, int x, int y) { }

        // Changes the size of the caret.
        public static void CaretSetSize(IntPtr handle, int width, int height) { }

        public static IntPtr CreateCaret() => default;

        // Creates a caret with the given size(in pixels) and associates it with the window.
        public static IntPtr CreateCaret2(IntPtr window, int width, int height) => default;

        // Get the window the caret is associated with.
        public static IntPtr CaretGetWindow(IntPtr handle) => default;

        // Hides the caret, same as Show(false).
        public static void CaretHide(IntPtr handle) { }

        // Returns true if the caret was created successfully.
        public static bool CaretIsOk(IntPtr handle) => default;

        // Returns true if the caret is visible and false if it
        // is permanently hidden(if it is blinking and not shown
        // currently but will be after the next blink, this method still returns true).
        public static bool CaretIsVisible(IntPtr handle) => default;

        // Shows or hides the caret.
        public static void CaretShow(IntPtr handle, bool show = true) { }

        // =================== Display

        // Defaultructor creating display object representing the primary display.
        public static IntPtr CreateDisplay() => default;

        //ructor, setting up a wxDisplay instance with the specified display.
        public static IntPtr CreateDisplay2(uint index) => default;

        //ructor creating the display object associated with the given window.
        public static IntPtr CreateDisplay3(IntPtr window) => default;

        public static void DeleteDisplay(IntPtr handle) { }

        // Returns the number of connected displays.
        public static uint DisplayGetCount() => default;

        // Returns the index of the display on which the given point lies,
        // or -1 if the point is not on any connected display.
        public static int DisplayGetFromPoint(Int32Point pt) => default;

        // Returns the index of the display on which the given window lies.
        public static int DisplayGetFromWindow(IntPtr win) => default;

        // Returns default display resolution for the current platform in pixels per inch. 
        public static int DisplayGetStdPPIValue() => default;

        // Returns default display resolution for the current platform as wxSize. 
        public static Int32Size DisplayGetStdPPI() => default;

        // Returns the display's name.
        public static string DisplayGetName(IntPtr handle) => default;

        // Returns display resolution in pixels per inch.
        public static Int32Size DisplayGetPPI(IntPtr handle) => default;

        // Returns scaling factor used by this display. 
        public static double DisplayGetScaleFactor(IntPtr handle) => default;

        // Returns true if the display is the primary display. 
        public static bool DisplayIsPrimary(IntPtr handle) => default;

        // Returns the client area of the display.
        public static Int32Rect DisplayGetClientArea(IntPtr handle) => default;

        // Returns the bounding rectangle of the display
        public static Int32Rect DisplayGetGeometry(IntPtr handle) => default;

        // ===================
    }
}

/*
 
*/