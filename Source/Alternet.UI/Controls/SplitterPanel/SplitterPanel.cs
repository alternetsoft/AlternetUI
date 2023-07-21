using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// This control manages up to two subcontrols. The current view of the control
    /// can be split into two programmatically, and unsplit either
    /// programmatically or via the user interface.
    /// </summary>
    public class SplitterPanel : Control
    {
        private const int wxSPLIT_HORIZONTAL = 1;
        private const int wxSPLIT_VERTICAL = 2;

        private Control? control1;
        private Control? control2;

        public Control? Control1 => control1;

        public Control? Control2 => control2;

        /// <summary>
        /// Gets or sets the minimum pane size in pixels (defaults to zero).
        /// </summary>
        /// <remarks>
        /// The default minimum pane size is zero, which means that either pane
        /// can be reduced to zero by dragging the sash, thus removing one
        /// of the panes. To prevent this behaviour (and veto out-of-range
        /// sash dragging), set a minimum size (for example 20 pixels).
        /// </remarks>
        public int MinimumPaneSize
        {
            get
            {
                return Handler.MinimumPaneSize;
            }

            set
            {
                Handler.MinimumPaneSize = value;
            }
        }

        public bool IsSplitVertical
        {
            get
            {
                var result = IsSplit && Handler.SplitMode == wxSPLIT_VERTICAL;
                return result;
            }

            set
            {
                if (!IsSplit || IsSplitVertical == value)
                    return;
                if (value)
                    Handler.SplitMode = wxSPLIT_VERTICAL;
                else
                    Handler.SplitMode = wxSPLIT_HORIZONTAL;
                UpdateSize();
            }
        }

        public bool IsSplitHorizontal
        {
            get
            {
                var result = IsSplit && Handler.SplitMode == wxSPLIT_HORIZONTAL;
                return result;
            }

            set
            {
                if (!IsSplit || IsSplitHorizontal == value)
                    return;
                if(value)
                    Handler.SplitMode = wxSPLIT_HORIZONTAL;
                else
                    Handler.SplitMode = wxSPLIT_VERTICAL;
                UpdateSize();
            }
        }

        /// <summary>
        /// Gets or sets the sash gravity. Value between 0.0 and 1.0.
        /// </summary>
        /// <remarks>
        /// Gravity is real factor which controls position of sash while
        /// resizing SplitterPanel. Gravity tells how much will left/top
        /// control grow while resizing.
        /// </remarks>
        /// <remarks>
        /// Default value of sash gravity is 0.0.
        /// </remarks>
        /// <remarks>
        /// Example values:
        /// 0.0: only the bottom/right window is automatically resized.
        /// 0.5: both windows grow by equal size.
        /// 1.0: only left/top window grows.
        /// </remarks>
        public double SashGravity
        {
            get
            {
                return Handler.SashGravity;
            }

            set
            {
                Handler.SashGravity = value;
            }
        }

        /*
         * 
  Returns the default sash size in pixels or 0 if it is invisible.
         */
        public int SashSize
        {
            get
            {
                return Handler.SashSize;
            }

            set
            {
                Handler.SashSize = value;
            }
        }

        /*
Sets whether the sash should be invisible, even when the window is split.
When the sash is invisible, it doesn't appear on the screen at all and, in particular, doesn't allow the user to resize the windows.
Remarks
Only sets the internal variable; does not update the display.
Parameters
invisible
If true, the sash is always invisible, else it is shown when the window is split.

         
         */
        public bool SashVisible
        {
            get
            {
                return Handler.SashVisible;
            }

            set
            {
                Handler.SashVisible = value;
            }
        }

        /*
      Returns true if the window is split, false otherwise.
         */
        public bool IsSplit
        {
            get
            {
                return Handler.IsSplit;
            }

        }

        /*
Sets the sash position.
Parameters
position
The sash position in pixels.
redraw
If true, resizes the panes and redraws the sash and border.

Remarks
Does not currently check for an out-of-range value.
See also
GetSashPosition()
         
         */
        public int SashPosition
        {
            get
            {
                return Handler.SashPosition;
            }

            set
            {
                Handler.SashPosition = value;
            }
        }

        public bool RedrawOnSashPosition
        {
            get
            {
                return Handler.RedrawOnSashPosition;
            }

            set
            {
                Handler.RedrawOnSashPosition = value;
            }
        }

        /*
Returns the default sash size in pixels.
The size of the sash is its width for a vertically split window and its height for a horizontally split one. Its other direction is the same as the client size of the window in the corresponding direction.
The default sash size is platform-dependent because it conforms to the current platform look-and-feel and cannot be changed.
         
         */
        public int DefaultSashSize
        {
            get
            {
                return Handler.DefaultSashSize;
            }

        }

        /*
             #define wxSP_NOBORDER         0x0000
             #define wxSP_THIN_SASH        0x0000    // NB: the default is 3D sash
             #define wxSP_NOSASH           0x0010
             #define wxSP_PERMIT_UNSPLIT   0x0040
             #define wxSP_LIVE_UPDATE      0x0080
             #define wxSP_3DSASH           0x0100
             #define wxSP_3DBORDER         0x0200
             #define wxSP_NO_XP_THEME      0x0400
             #define wxSP_BORDER           wxSP_3DBORDER
             #define wxSP_3D               (wxSP_3DBORDER | wxSP_3DSASH)
             */
        internal long CreateStyles
        {
            get
            {
                return Handler.Styles;
            }

            set
            {
                Handler.Styles = value;
            }
        }

        internal new NativeSplitterPanelHandler Handler =>
            (NativeSplitterPanelHandler)base.Handler;

        /*
         Causes any pending sizing of the sash and child panes to take place immediately.
Such resizing normally takes place in idle time, in order to wait for layout to be completed. However, this can cause unacceptable flicker as the panes are resized after the window has been shown. To work around this, you can perform window layout (for example by sending a size event to the parent window), and then call this function, before showing the top-level window.

         */
        public void UpdateSize()
        {
            Handler.UpdateSize();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        protected override void OnChildInserted(int childIndex, Control childControl)
        {
            base.OnChildInserted(childIndex, childControl);
            /*int count = Children.Count;
            if (count > 2)
            {
                throw new Exception(
                    "More than two child controls are not allowed");
            }

            Control newControl1 = GetChildOrNull(0);
            Control newControl2 = GetChildOrNull(1);*/
        }

        /*
Initializes the splitter window to have one pane.
The child window is shown if it is currently hidden.
Parameters
window
The pane for the unsplit window.

Remarks
This should be called if you wish to initially view only a single pane in the splitter window.
         
         */
        public bool InitializeUnsplitted(Control window)
        {
            if (window == null)
                return false;
            if (window.Parent != this)
                return false;

            Native.Control? nc1 = window.Handler.NativeControl;
            if (nc1 == null)
                return false;

            Handler.NativeControl.Initialize(nc1);
            control1 = window;
            control2 = null;
            return true;
        }

        /*
This function replaces one of the windows managed by the wxSplitterWindow with another one.
It is in general better to use it instead of calling Unsplit() and then resplitting the window back because it will provoke much less flicker (if any). It is valid to call this function whether the splitter has two windows or only one.
Both parameters should be non-NULL and winOld must specify one of the windows managed by the splitter. If the parameters are incorrect or the window couldn't be replaced, false is returned. Otherwise the function will return true, but please notice that it will not delete the replaced window and you may wish to do it yourself.
         
         */
        public bool ReplaceControl(Control? winOld, Control? winNew)
        {
            if (winOld == null || winNew == null)
                return false;
            if (control1 != winOld && control2 != winOld)
                return false;
            if (winNew.Parent != this)
                return false;

            Native.Control? nc1 = winOld.Handler.NativeControl;
            Native.Control? nc2 = winNew.Handler.NativeControl;

            if (nc1 == null || nc2 == null)
                return false;

            var result = Handler.NativeControl.Replace(nc1, nc2);

            if (result)
            {
                winOld.Visible = false;
                winNew.Visible = true;

                if (control1 == winOld)
                    control1 = winNew;
                if (control2 == winOld)
                    control2 = winNew;
            }

            return result;
        }

        /*
Initializes the top and bottom panes of the splitter window.
The child windows are shown if they are currently hidden.
Parameters
window1
The top pane.
window2
The bottom pane.
sashPosition
The initial position of the sash. If this value is positive, it specifies the size of the upper pane. If it is negative, its absolute value gives the size of the lower pane. Finally, specify 0 (default) to choose the default position (half of the total window height).



Returns
true if successful, false otherwise (the window was already split).
Remarks
This should be called if you wish to initially view two panes. It can also be called at any subsequent time, but the application should check that the window is not currently split using IsSplit().
See also
SplitVertically(), IsSplit(), Unsplit()

         */
        public bool SplitHorizontal(
            Control? window1,
            Control? window2,
            int sashPosition = 0)
        {
            if (IsSplit)
                return false;
            if(window1 == null || window2 == null)
                return false;
            if (window1.Parent != this || window2.Parent != this)
                return false;

            Native.Control? nc1 = window1.Handler.NativeControl;
            Native.Control? nc2 = window2.Handler.NativeControl;

            if (nc1 == null || nc2 == null)
                return false;

            var r = Handler.NativeControl.SplitHorizontally(nc1, nc2, sashPosition);

            if (r)
            {
                control1 = window1;
                control2 = window2;
            }

            return r;
        }

        /*
Initializes the left and right panes of the splitter window.
The child windows are shown if they are currently hidden.
Parameters
window1
The left pane.
window2
The right pane.
sashPosition
The initial position of the sash. If this value is positive, it specifies the size of the left pane. If it is negative, it is absolute value gives the size of the right pane. Finally, specify 0 (default) to choose the default position (half of the total window width).

Returns
true if successful, false otherwise (the window was already split).
Remarks
This should be called if you wish to initially view two panes. It can also be called at any subsequent time, but the application should check that the window is not currently split using IsSplit().
         
         */
        public bool SplitVertical(
            Control? window1,
            Control? window2,
            int sashPosition = 0)
        {
            if (IsSplit)
                return false;
            if (window1 == null || window2 == null)
                return false;
            if (window1.Parent != this || window2.Parent != this)
                return false;

            Native.Control? nc1 = window1.Handler.NativeControl;
            Native.Control? nc2 = window2.Handler.NativeControl;

            if (nc1 == null || nc2 == null)
                return false;

            var r = Handler.NativeControl.SplitVertically(nc1, nc2, sashPosition);
            if (r)
            {
                control1 = window1;
                control2 = window2;
            }

            return r;
        }

        /// <summary>
        /// Leaves only one control inside the <see cref="SplitterPanel"/>.
        /// </summary>
        /// <param name="toRemove">The pane to remove, or NULL to remove the
        /// right or bottom pane.</param>
        /// <returns>true if successful, false otherwise
        /// (the <see cref="SplitterPanel"/> was not unsplit).</returns>
        /// <remarks>
        /// This call will not actually delete the control being removed.
        /// By default, the control being removed is hidden.
        /// </remarks>
        public bool DoUnsplit(Control? toRemove = null)
        {
            if (!IsSplit)
                return true;
            if (toRemove == null)
                toRemove = control2;
            if (toRemove == null)
                return false;
            Native.Control? nc1 = toRemove.Handler.NativeControl;
            if (nc1 == null)
                return false;
            var r = Handler.NativeControl.DoUnsplit(nc1);
            if (r)
            {
                if (control1 == toRemove)
                    control1 = null;
                if (control2 == toRemove)
                    control2 = null;
            }
            UpdateSize();
            return r;
        }

        /// <inheritdoc />
        protected override void OnChildRemoved(int childIndex, Control childControl)
        {
            base.OnChildRemoved(childIndex, childControl);
        }

        /// <inheritdoc />
        protected override ControlHandler CreateHandler()
        {
            return new NativeSplitterPanelHandler();
        }
    }
}
