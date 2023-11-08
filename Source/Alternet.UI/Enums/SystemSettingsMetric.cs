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
        BorderX,

        /// <summary>
        /// Height of single border.
        /// </summary>
        BorderY,

        /// <summary>
        /// Width of cursor.
        /// </summary>
        CursorX,

        /// <summary>
        /// Height of cursor.
        /// </summary>
        CursorY,

        /// <summary>
        /// Width in pixels of rectangle within which two successive mouse clicks must fall
        /// to generate a double-click.
        /// </summary>
        DClickX,

        /// <summary>
        /// Height in pixels of rectangle within which two successive mouse clicks must
        /// fall to generate a double-click.
        /// </summary>
        DClickY,

        /// <summary>
        /// Width in pixels of a rectangle centered on a drag point to allow for limited
        /// movement of the mouse pointer before a drag operation begins.
        /// </summary>
        DragX,

        /// <summary>
        /// Height in pixels of a rectangle centered on a drag point to allow for limited
        /// movement of the mouse pointer before a drag operation begins.
        /// </summary>
        DragY,

        /// <summary>
        /// Width of a 3D border, in pixels.
        /// </summary>
        EdgeX,

        /// <summary>
        /// Height of a 3D border, in pixels.
        /// </summary>
        EdgeY,

        /// <summary>
        /// Width of arrow bitmap on horizontal scrollbar.
        /// </summary>
        HScrollArrowX,

        /// <summary>
        /// Height of arrow bitmap on horizontal scrollbar.
        /// </summary>
        HScrollArrowY,

        /// <summary>
        /// Width of horizontal scrollbar thumb.
        /// </summary>
        HThumbX,

        /// <summary>
        /// The default width of an icon.
        /// </summary>
        IconX,

        /// <summary>
        /// The default height of an icon.
        /// </summary>
        IconY,

        /// <summary>
        /// Width of a grid cell for items in large icon view, in pixels. Each item
        /// fits into a rectangle of this size when arranged.
        /// </summary>
        IconSpacingX,

        /// <summary>
        /// Height of a grid cell for items in large icon view, in pixels. Each item
        /// fits into a rectangle of this size when arranged.
        /// </summary>
        IconSpacingY,

        /// <summary>
        /// Minimum width of a window.
        /// </summary>
        WindowMinX,

        /// <summary>
        /// Minimum height of a window.
        /// </summary>
        WindowMinY,

        /// <summary>
        /// Width of the screen in pixels.
        /// </summary>
        ScreenX,

        /// <summary>
        /// Height of the screen in pixels.
        /// </summary>
        ScreenY,

        /// <summary>
        /// Width of the window frame for a wxTHICK_FRAME window.
        /// </summary>
        FrameSizeX,

        /// <summary>
        /// Height of the window frame for a wxTHICK_FRAME window.
        /// </summary>
        FrameSizeY,

        /// <summary>
        /// Recommended width of a small icon (in window captions, and small icon view).
        /// </summary>
        SmallIconX,

        /// <summary>
        /// Recommended height of a small icon (in window captions, and small icon view).
        /// </summary>
        SmallIconY,

        /// <summary>
        /// Height of horizontal scrollbar in pixels.
        /// </summary>
        HScrollY,

        /// <summary>
        /// Width of vertical scrollbar in pixels.
        /// </summary>
        VScrollX,

        /// <summary>
        /// Width of arrow bitmap on a vertical scrollbar.
        /// </summary>
        VScrollArrowX,

        /// <summary>
        /// Height of arrow bitmap on a vertical scrollbar.
        /// </summary>
        VScrollArrowY,

        /// <summary>
        /// Height of vertical scrollbar thumb.
        /// </summary>
        VThumbY,

        /// <summary>
        /// Height of normal caption area.
        /// </summary>
        CaptionY,

        /// <summary>
        /// Height of single-line menu bar.
        /// </summary>
        MenuY,

        /// <summary>
        /// Equals 1 if there is a network present, 0 otherwise.
        /// </summary>
        NetworkPresent,

        /// <summary>
        /// Equals 1 if PenWindows is installed, 0 otherwise.
        /// </summary>
        PenWindowsPresent,

        /// <summary>
        /// Non-zero if the user requires an application to present information visually
        /// in situations where it would otherwise present the information only in
        /// audible form; zero otherwise.
        /// </summary>
        ShowSounds,

        /// <summary>
        /// Non-zero if the meanings of the left and right mouse buttons are swapped; zero otherwise.
        /// </summary>
        SwapButtons,

        /// <summary>
        /// Maximal time, in milliseconds, which may pass between subsequent clicks for
        /// a double click to be generated.
        /// </summary>
        DClickMSec,

        /// <summary>
        /// Time, in milliseconds, for how long a blinking caret should stay visible during a
        /// single blink cycle before it disappears. If this value is zero, caret should be
        /// visible all the time instead of blinking. If the value is negative, the platform
        /// does not support the user setting.
        /// </summary>
        CaretOnMSec,

        /// <summary>
        /// Time, in milliseconds, for how long a blinking caret should stay invisible during
        /// a single blink cycle before it reappears. If this value is zero, caret should be
        /// visible all the time instead of blinking. If the value is negative, the
        /// platform does not support the user setting.
        /// </summary>
        CaretOffMSec,

        /// <summary>
        /// Time, in milliseconds, for how long a caret should blink after a user interaction.
        /// After this timeout has expired, the caret should stay continuously visible until
        /// the user interacts with the caret again (for example by entering, deleting
        /// or cutting text). If this value is negative, carets should blink forever;
        /// if it is zero, carets should not blink at all.
        /// </summary>
        CaretTimeoutMSec,
    }
}