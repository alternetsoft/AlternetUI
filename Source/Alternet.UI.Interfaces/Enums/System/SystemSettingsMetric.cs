using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Possible values for SystemSettings.GetMetric index parameter.
    /// </summary>
    public enum SystemSettingsMetric
    {
        /// <summary>
        /// Number of buttons on mouse, or zero if no mouse was installed.
        /// </summary>
        MouseButtons = 1,

        /// <summary>
        /// Width of single border.
        /// </summary>
        BorderX = 2,

        /// <summary>
        /// Height of single border.
        /// </summary>
        BorderY = 3,

        /// <summary>
        /// Width of cursor.
        /// </summary>
        CursorX = 4,

        /// <summary>
        /// Height of cursor.
        /// </summary>
        CursorY = 5,

        /// <summary>
        /// Width in pixels of rectangle within which two successive mouse clicks must fall
        /// to generate a double-click.
        /// </summary>
        DClickX = 6,

        /// <summary>
        /// Height in pixels of rectangle within which two successive mouse clicks must
        /// fall to generate a double-click.
        /// </summary>
        DClickY = 7,

        /// <summary>
        /// Width in pixels of a rectangle centered on a drag point to allow for limited
        /// movement of the mouse pointer before a drag operation begins.
        /// </summary>
        DragX = 8,

        /// <summary>
        /// Height in pixels of a rectangle centered on a drag point to allow for limited
        /// movement of the mouse pointer before a drag operation begins.
        /// </summary>
        DragY = 9,

        /// <summary>
        /// Width of a 3D border, in pixels.
        /// </summary>
        EdgeX = 10,

        /// <summary>
        /// Height of a 3D border, in pixels.
        /// </summary>
        EdgeY = 11,

        /// <summary>
        /// Width of arrow bitmap on horizontal scrollbar.
        /// </summary>
        HScrollArrowX = 12,

        /// <summary>
        /// Height of arrow bitmap on horizontal scrollbar.
        /// </summary>
        HScrollArrowY = 13,

        /// <summary>
        /// Width of horizontal scrollbar thumb.
        /// </summary>
        HThumbX = 14,

        /// <summary>
        /// The default width of an icon.
        /// </summary>
        IconX = 15,

        /// <summary>
        /// The default height of an icon.
        /// </summary>
        IconY = 16,

        /// <summary>
        /// Width of a grid cell for items in large icon view, in pixels. Each item
        /// fits into a rectangle of this size when arranged.
        /// </summary>
        IconSpacingX = 17,

        /// <summary>
        /// Height of a grid cell for items in large icon view, in pixels. Each item
        /// fits into a rectangle of this size when arranged.
        /// </summary>
        IconSpacingY = 18,

        /// <summary>
        /// Minimum width of a window.
        /// </summary>
        WindowMinX = 19,

        /// <summary>
        /// Minimum height of a window.
        /// </summary>
        WindowMinY = 20,

        /*
        ScreenX = 21,

        ScreenY = 22,
        */

        /// <summary>
        /// Width of the window frame for a wxTHICK_FRAME window.
        /// </summary>
        FrameSizeX = 23,

        /// <summary>
        /// Height of the window frame for a wxTHICK_FRAME window.
        /// </summary>
        FrameSizeY = 24,

        /// <summary>
        /// Recommended width of a small icon (in window captions, and small icon view).
        /// </summary>
        SmallIconX = 25,

        /// <summary>
        /// Recommended height of a small icon (in window captions, and small icon view).
        /// </summary>
        SmallIconY = 26,

        /// <summary>
        /// Height of horizontal scrollbar in pixels.
        /// </summary>
        HScrollY = 27,

        /// <summary>
        /// Width of vertical scrollbar in pixels.
        /// </summary>
        VScrollX = 28,

        /// <summary>
        /// Width of arrow bitmap on a vertical scrollbar.
        /// </summary>
        VScrollArrowX = 29,

        /// <summary>
        /// Height of arrow bitmap on a vertical scrollbar.
        /// </summary>
        VScrollArrowY = 30,

        /// <summary>
        /// Height of vertical scrollbar thumb.
        /// </summary>
        VThumbY = 31,

        /// <summary>
        /// Height of normal caption area.
        /// </summary>
        CaptionY = 32,

        /// <summary>
        /// Height of single-line menu bar.
        /// </summary>
        MenuY = 33,

        /// <summary>
        /// Equals 1 if there is a network present, 0 otherwise.
        /// </summary>
        NetworkPresent = 34,

        /// <summary>
        /// Equals 1 if PenWindows is installed, 0 otherwise.
        /// </summary>
        PenWindowsPresent = 35,

        /// <summary>
        /// Non-zero if the user requires an application to present information visually
        /// in situations where it would otherwise present the information only in
        /// audible form; zero otherwise.
        /// </summary>
        ShowSounds = 36,

        /// <summary>
        /// Non-zero if the meanings of the left and right mouse buttons are swapped; zero otherwise.
        /// </summary>
        SwapButtons = 37,

        /// <summary>
        /// Maximal time, in milliseconds, which may pass between subsequent clicks for
        /// a double click to be generated.
        /// </summary>
        DClickMSec = 38,

        /// <summary>
        /// Time, in milliseconds, for how long a blinking caret should stay visible during a
        /// single blink cycle before it disappears. If this value is zero, caret should be
        /// visible all the time instead of blinking. If the value is negative, the platform
        /// does not support the user setting.
        /// </summary>
        CaretOnMSec = 39,

        /// <summary>
        /// Time, in milliseconds, for how long a blinking caret should stay invisible during
        /// a single blink cycle before it reappears. If this value is zero, caret should be
        /// visible all the time instead of blinking. If the value is negative, the
        /// platform does not support the user setting.
        /// </summary>
        CaretOffMSec = 40,

        /// <summary>
        /// Time, in milliseconds, for how long a caret should blink after a user interaction.
        /// After this timeout has expired, the caret should stay continuously visible until
        /// the user interacts with the caret again (for example by entering, deleting
        /// or cutting text). If this value is negative, carets should blink forever;
        /// if it is zero, carets should not blink at all.
        /// </summary>
        CaretTimeoutMSec = 41,

        /// <summary>
        /// Max value in the <see cref="SystemSettingsMetric"/> enumeration.
        /// </summary>
        Max = CaretTimeoutMSec,
    }
}