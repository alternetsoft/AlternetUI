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
        private const int SPLITHORIZONTAL = 1;
        private const int SPLITVERTICAL = 2;

        private const int SPNOBORDER = 0x0000;
        private const int SPTHINSASH = 0x0000;    // the default is 3D sash
        private const int SPNOSASH = 0x0010;
        private const int SPPERMITUNSPLIT = 0x0040;
        private const int SPLIVEUPDATE = 0x0080;
        private const int SP3DSASH = 0x0100;
        private const int SP3DBORDER = 0x0200;
        private const int SPNOXPTHEME = 0x0400;
        private const int SPBORDER = SP3DBORDER;
        private const int SP3D = SP3DBORDER | SP3DSASH;

        private Control? control1;
        private Control? control2;

        /// <summary>
        /// Occurs when the splitter sash is double clicked.
        /// </summary>
        public event EventHandler? SplitterDoubleClick;

        /// <summary>
        /// Occurs when the splitter sash is moved.
        /// </summary>
        public event EventHandler? SplitterMoved;

        /// <summary>
        /// Occurs when the splitter sash is in the process of moving.
        /// </summary>
        public event EventHandler? SplitterMoving;

        /// <summary>
        /// Occurs when the control is unsplit.
        /// </summary>
        public event EventHandler? Unsplit;

        /// <summary>
        /// Returns the left/top or only pane.
        /// </summary>
        public Control? Control1 => control1;

        /// <summary>
        /// Returns the right/bottom pane.
        /// </summary>
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

        /// <summary>
        /// Gets or sets whether the control is split vertically.
        /// </summary>
        /// <returns>
        /// Returns true if the control is split vertically, false otherwise.
        /// </returns>
        /// <remarks>
        /// If control is already split, you can use this property to change
        /// the split mode  (for example from horizontal to vertical).
        /// </remarks>
        public bool IsSplitVertical
        {
            get
            {
                var result = IsSplit && Handler.SplitMode == SPLITVERTICAL;
                return result;
            }

            set
            {
                if (!IsSplit || IsSplitVertical == value)
                    return;
                if (value)
                    Handler.SplitMode = SPLITVERTICAL;
                else
                    Handler.SplitMode = SPLITHORIZONTAL;
                UpdateSize();
            }
        }

        /// <summary>
        /// Gets or sets whether the control is split horizontally.
        /// </summary>
        /// <returns>
        /// Returns true if the control is split horizontally, false otherwise.
        /// </returns>
        /// <remarks>
        /// If control is already split, you can use this property to change
        /// the split mode (for example from vertical to horizontal).
        /// </remarks>
        public bool IsSplitHorizontal
        {
            get
            {
                var result = IsSplit && Handler.SplitMode == SPLITHORIZONTAL;
                return result;
            }

            set
            {
                if (!IsSplit || IsSplitHorizontal == value)
                    return;
                if(value)
                    Handler.SplitMode = SPLITHORIZONTAL;
                else
                    Handler.SplitMode = SPLITVERTICAL;
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

        /// <summary>
        /// Gets or sets the default sash size in pixels.
        /// </summary>
        /// <remarks>
        /// Returns 0 if sash is invisible.
        /// </remarks>
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

        /// <summary>
        /// Gets or sets whether the sash should be invisible, even when
        /// the control is split.
        /// </summary>
        /// <remarks>
        /// When the sash is invisible, it doesn't appear on the screen at
        /// all and, in particular, doesn't allow the user to resize the windows.
        /// </remarks>
        /// <remarks>
        /// Only sets the internal variable; does not update the display.
        /// </remarks>
        /// <returns>
        /// If false, the sash is always invisible, else it
        /// is shown when the window is split.
        /// </returns>
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

        /// <summary>
        /// Gets whether the control is split.
        /// </summary>
        /// <returns>
        /// Returns true if the control is split, false otherwise.
        /// </returns>
        public bool IsSplit
        {
            get
            {
                return Handler.IsSplit;
            }
        }

        /// <summary>
        /// Gets or sets the sash position in pixels.
        /// </summary>
        /// <remarks>
        /// If <see cref="RedrawOnSashPosition"/> is true, resizes the
        /// panes and redraws the sash and border.
        /// </remarks>
        /// <remarks>
        /// Does not currently check for an out-of-range value.
        /// </remarks>
        public int SashPosition
        {
            get
            {
                return Handler.SashPosition;
            }

            set
            {
                if (value < 0)
                    value = 0;
                Handler.SashPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to resize the panes and redraw the sash and
        /// border on <see cref="SashPosition"/> property change.
        /// </summary>
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

        /// <summary>
        /// Gets the default sash size in pixels.
        /// </summary>
        /// <remarks>
        /// The size of the sash is its width for a vertically split window
        /// and its height for a horizontally split one. Its other
        /// direction is the same as the client size of the window in
        /// the corresponding direction.
        /// </remarks>
        /// <remarks>
        /// The default sash size is platform-dependent because it
        /// conforms to the current platform look-and-feel and cannot be changed.
        /// </remarks>
        public int DefaultSashSize
        {
            get
            {
                return Handler.DefaultSashSize;
            }
        }

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

        /// <summary>
        /// Causes any pending sizing of the sash and child panes to
        /// take place immediately.
        /// </summary>
        /// <remarks>
        /// Such resizing normally takes place in idle time, in order to wait
        /// for layout to be completed. However, this can cause unacceptable
        /// flicker as the panes are resized after the control has been shown.
        /// To work around this, you can perform control layout (for example
        /// by sending a size event to the parent control), and then call
        /// this function, before showing the top-level window.
        /// </remarks>
        public void UpdateSize()
        {
            Handler.UpdateSize();
        }

        /// <summary>
        /// Initializes the <see cref="SplitterPanel"/> to have one pane.
        /// The child control is shown if it is currently hidden.
        /// </summary>
        /// <param name="window">The pane for the unsplit control.
        /// Pass only child controls of the <see cref="SplitterPanel"/> in
        /// this parameter.</param>
        /// <returns>
        /// true if initialization successfull.
        /// </returns>
        /// <remarks>
        /// This should be called if you wish to initially view only a single
        /// pane in the control.
        /// </remarks>
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
            control1.Visible = true;
            control2 = null;
            return true;
        }

        /// <summary>
        /// Replaces one of the controls managed by the
        /// <see cref="SplitterPanel"/> with another one.
        /// </summary>
        /// <param name="winOld">
        /// Subcontrol which needs to be replaced.
        /// </param>
        /// <param name="winNew">
        /// New subcontrol to replace the old one. Pass here only child
        /// controls of the<see cref="SplitterPanel"/>.
        /// </param>
        /// <returns>
        /// If the parameters are incorrect or the subcontrol couldn't be
        /// replaced, false is returned. Otherwise the function will return true.
        /// </returns>
        /// <remarks>
        /// It is in general better to use it instead of calling
        /// <see cref="DoUnsplit"/> and then resplitting the window back
        /// because it will provoke much less flicker (if any).
        /// </remarks>
        /// <remarks>
        /// It is valid to call this function whether the
        /// <see cref="SplitterPanel"/> has two subcontrols or only one.
        /// </remarks>
        /// <remarks>
        /// This function will not delete the replaced control but hide it.
        /// </remarks>
        /// <remarks>
        /// Both parameters should be non-NULL and winOld must specify one
        /// of the controls managed by the <see cref="SplitterPanel"/>.
        /// </remarks>
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

        /// <summary>
        /// Initializes the top and bottom panes of the
        /// <see cref="SplitterPanel"/>. The child controls are shown
        /// if they are currently hidden.
        /// </summary>
        /// <param name="window1">The top pane.</param>
        /// <param name="window2">The bottom pane.</param>
        /// <param name="sashPosition">
        /// The initial position of the sash. If this value is positive, it
        /// specifies the size of the upper pane. If it is negative, its
        /// absolute value gives the size of the lower pane.
        /// Specify 0 (default) to choose the default position
        /// (half of the total control height).
        /// </param>
        /// <returns>
        /// true if successful, false otherwise (the control was already split).
        /// </returns>
        /// <remarks>
        /// Call it if you want to initially view two panes. It can also be
        /// called at any time, but the application should check that the
        /// control is not currently split using <see cref="IsSplit"/>.
        /// </remarks>
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
                control1.Visible = true;
                control2.Visible = true;
            }

            return r;
        }

        /// <summary>
        /// Initializes the left and right panes of the <see cref="SplitterPanel"/>.
        /// The child controls are shown if they are currently hidden.
        /// </summary>
        /// <param name="window1">
        /// The left pane.
        /// </param>
        /// <param name="window2">
        /// The right pane.
        /// </param>
        /// <param name="sashPosition">
        /// The initial position of the sash. If this value is positive, it
        /// specifies the size of the left pane. If it is negative, it
        /// is absolute value gives the size of the right pane.
        /// Specify 0 (default value) to choose the default position
        /// (half of the total window width).
        /// </param>
        /// <returns>
        /// true if successful, false otherwise (the control was already split).
        /// </returns>
        /// <remarks>
        /// Call it if you want to initially view two panes. It can also be
        /// called at any time, but the application should check that the
        /// control is not currently split using <see cref="IsSplit"/>.
        /// </remarks>
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
                control1.Visible = true;
                control2.Visible = true;
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
            toRemove ??= control2;
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
                toRemove.Visible = false;
            }

            UpdateSize();
            return r;
        }

        internal void RaiseSplitterDoubleClick(EventArgs e)
        {
            OnSplitterDoubleClick(e);
            SplitterDoubleClick?.Invoke(this, e);
        }

        internal void RaiseSplitterMoved(EventArgs e)
        {
            OnSplitterMoved(e);
            SplitterMoved?.Invoke(this, e);
        }

        internal void RaiseSplitterMoving(EventArgs e)
        {
            OnSplitterMoving(e);
            SplitterMoving?.Invoke(this, e);
        }

        internal void RaiseUnsplit(EventArgs e)
        {
            OnUnsplit(e);
            Unsplit?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the splitter sash is double clicked.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnSplitterDoubleClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the splitter sash is moved.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnSplitterMoved(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the splitter sash is in the process of moving.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnSplitterMoving(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is unsplit.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnUnsplit(EventArgs e)
        {
        }

        /// <inheritdoc />
        protected override void OnChildInserted(int childIndex, Control childControl)
        {
            base.OnChildInserted(childIndex, childControl);
            /*
            Control newControl1 = GetChildOrNull(0);
            Control newControl2 = GetChildOrNull(1);*/
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