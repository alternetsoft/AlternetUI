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
    /// This control manages up to two subcontrols which are aligned vertically or horizontally
    /// with splitter control between them. The current view of the control
    /// can be split into two programmatically, and unsplit either
    /// programmatically or via the user interface.
    /// </summary>
    /// <remarks>
    /// Please do not use <see cref="SplitterPanel"/>, it has prolems under Linux. It is better to use
    /// <see cref="LayoutPanel"/> with <see cref="Splitter"/> control.
    /// See <see cref="SplittedTreeAndCards"/> source code for the example.
    /// </remarks>
    [ControlCategory("Containers")]
    internal partial class SplitterPanel : Control
    {
        /// <summary>
        /// Gets or sets whether to check sash size and make it at least equal to
        /// <see cref="PlatformDefaults.MinSplitterSashSize"/>.
        /// </summary>
        public static bool ApplyMinSashSize = true;

        private const int SPLITHORIZONTAL = 1;
        private const int SPLITVERTICAL = 2;

        private static bool minSashSizeApplied;

        private Control? control1;
        private Control? control2;
        private bool initAutoSplit = false;
        private SplitterPanelSplitMethod splitMethod =
            SplitterPanelSplitMethod.Manual;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitterPanel"/> class.
        /// </summary>
        public SplitterPanel()
        {
            if (ApplyMinSashSize && !minSashSizeApplied)
            {
                minSashSizeApplied = true;
                var minSize = AllPlatformDefaults.PlatformCurrent.MinSplitterSashSize;
                SetMinSashSize(minSize);
            }
        }

        /// <summary>
        /// Occurs when the splitter sash is double clicked.
        /// </summary>
        public event EventHandler<SplitterPanelEventArgs>? SplitterDoubleClick;

        /// <summary>
        /// Occurs when the splitter sash is moved.
        /// </summary>
        public event EventHandler<SplitterPanelEventArgs>? SplitterMoved;

        /// <summary>
        /// Occurs when the splitter sash is in the process of moving.
        /// </summary>
        public event EventHandler<SplitterPanelEventArgs>? SplitterMoving;

        /// <summary>
        /// Occurs when the control is unsplit.
        /// </summary>
        public event EventHandler<SplitterPanelEventArgs>? Unsplit;

        /// <summary>
        /// Occurs when the splitter sash is resized.
        /// </summary>
        public event EventHandler<SplitterPanelEventArgs>? SplitterResize;

        /// <summary>
        /// Defines default visual style for the newly created
        /// <see cref="SplitterPanel"/> controls.
        /// </summary>
        public static SplitterPanelCreateStyle DefaultCreateStyle { get; set; }
            = SplitterPanelCreateStyle.NoBorder | SplitterPanelCreateStyle.ThinSash
                | SplitterPanelCreateStyle.LiveUpdate;

        /// <summary>
        /// Gets or sets <see cref="SplitterPanel"/> automatic behavior when
        /// child controls are added.
        /// </summary>
        /// <remarks>
        /// Automatic splitting is performed only when new controls are added
        /// to the <see cref="SplitterPanel"/>. By default automatic splitting
        /// is turned off.
        /// </remarks>
        public virtual SplitterPanelSplitMethod SplitMethod
        {
            get
            {
                return splitMethod;
            }

            set
            {
                if (splitMethod == value)
                    return;
                splitMethod = value;
            }
        }

        /// <inheritdoc/>
        public override IReadOnlyList<Control> AllChildrenInLayout
            => Array.Empty<Control>();

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.SplitterPanel;

        /// <summary>
        /// Gets or sets whether splitter sash can be dragged by mouse.
        /// </summary>
        /// <remarks>Default value is true.</remarks>
        public virtual bool CanMoveSplitter
        {
            get
            {
                return Handler.CanMoveSplitter;
            }

            set
            {
                Handler.CanMoveSplitter = value;
            }
        }

        /// <summary>
        /// Gets or sets whether double click unsplits the control.
        /// </summary>
        /// <remarks>Default value is true.</remarks>
        public virtual bool CanDoubleClick
        {
            get
            {
                return Handler.CanDoubleClick;
            }

            set
            {
                Handler.CanDoubleClick = value;
            }
        }

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
        public virtual int MinPaneSize
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
        public virtual bool IsSplitVertical
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
        public virtual bool IsSplitHorizontal
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
        /// resizing <see cref="SplitterPanel"/>. Gravity tells how much will
        /// left/top control grow while resizing.
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
        public virtual double SashGravity
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
        /// Gets current sash size in pixels.
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
        public virtual bool SashVisible
        {
            get
            {
                return Handler.SashVisible;
            }

            set
            {
                if (Handler.SashVisible == value)
                    return;
                Handler.SashVisible = value;
                UpdateSize();
            }
        }

        /// <summary>
        /// Gets whether the control is split.
        /// </summary>
        /// <returns>
        /// Returns true if the control is split, false otherwise.
        /// </returns>
        public virtual bool IsSplit
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
        public virtual int SashPosition
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
        /// Gets or sets the sash position in
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        public virtual double SashPositionDip
        {
            get
            {
                return PixelToDip(SashPosition);
            }

            set
            {
                SashPosition = PixelFromDip(value);
            }
        }

        /// <summary>
        /// Gets maximal splitter sash position if control is splitted or -1
        /// if it's unsplitted.
        /// </summary>
        public virtual int MaxSashPosition
        {
            get
            {
                double v;
                if (IsSplitHorizontal)
                    v = ClientSize.Height;
                else
                if (IsSplitVertical)
                    v = ClientSize.Width;
                else
                    return -1;

                var inPixels = ScreenToDevice(new(v, 0));

                int result = inPixels.X - MinPaneSize;
                if (result < -1)
                    result = -1;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets whether to resize the panes and redraw the sash and
        /// border on <see cref="SashPosition"/> property change.
        /// </summary>
        public virtual bool RedrawOnSashPosition
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
        /// Gets the default sash size in pixels. Returns value for the native control.
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

        /// <summary>
        /// Defines visual style and behavior of the <see cref="SplitterPanel"/> control.
        /// </summary>
        /// <remarks>
        /// When this property is changed, control is recreated.
        /// </remarks>
        public virtual SplitterPanelCreateStyle CreateStyle
        {
            get
            {
                return (SplitterPanelCreateStyle)Enum.ToObject(
                    typeof(SplitterPanelCreateStyle),
                    Handler.Styles);
            }

            set
            {
                Handler.Styles = (int)value;
            }
        }

        internal new SplitterPanelHandler Handler =>
            (SplitterPanelHandler)base.Handler;

        /// <summary>
        /// Sets minimal value for the sash size in dips (1/96 inch).
        /// </summary>
        /// <param name="minSize">Minimal sash size.</param>
        public static void SetMinSashSize(int minSize)
        {
            minSize = Display.Primary.PixelFromDip(minSize);
            Native.SplitterPanel.SetMinSashSize(minSize);
        }

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
        public virtual void UpdateSize()
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
        public virtual bool InitUnsplitted(Control window)
        {
            if (window is null)
                return false;
            if (window.Parent != this)
                return false;

            Native.Control? nc1 = (window.Handler as WxControlHandler)?.NativeControl;
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
        public virtual bool ReplaceControl(Control? winOld, Control? winNew)
        {
            if (winOld == null || winNew == null)
                return false;
            if (control1 != winOld && control2 != winOld)
                return false;
            if (winNew.Parent != this)
                return false;

            Native.Control? nc1 = (winOld.Handler as WxControlHandler)?.NativeControl;
            Native.Control? nc2 = (winNew.Handler as WxControlHandler)?.NativeControl;

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
        public virtual bool SplitHorizontal(
            Control? window1,
            Control? window2,
            int sashPosition = 0)
        {
            if (IsSplit)
                return false;
            if(window1 == null || window2 == null)
                return false;

            window1.Parent = this;
            window2.Parent = this;

            Native.Control? nc1 = (window1.Handler as WxControlHandler)?.NativeControl;
            Native.Control? nc2 = (window2.Handler as WxControlHandler)?.NativeControl;

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
        public virtual bool SplitVertical(
            Control? window1,
            Control? window2,
            int sashPosition = 0)
        {
            if (IsSplit)
                return false;
            if (window1 == null || window2 == null)
                return false;

            window1.Parent = this;
            window2.Parent = this;

            Native.Control? nc1 = (window1.Handler as WxControlHandler)?.NativeControl;
            Native.Control? nc2 = (window2.Handler as WxControlHandler)?.NativeControl;

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

        /// <inheritdoc cref="SplitVertical"/>
        /// <remarks>
        /// Same as <see cref="SplitVertical"/>, but <paramref name="sashPosition"/> is in
        /// device-independent units (1/96th inch per unit).
        /// </remarks>
        public virtual bool SplitVerticalDip(
            Control? window1,
            Control? window2,
            double sashPosition = 0)
        {
            return SplitVertical(window1, window2, PixelFromDip(sashPosition));
        }

        /// <inheritdoc cref="SplitHorizontal"/>
        /// <remarks>
        /// Same as <see cref="SplitHorizontal"/>, but <paramref name="sashPosition"/> is in
        /// device-independent units (1/96th inch per unit).
        /// </remarks>
        public virtual bool SplitHorizontalDip(
            Control? window1,
            Control? window2,
            double sashPosition = 0)
        {
            return SplitHorizontal(window1, window2, PixelFromDip(sashPosition));
        }

        /// <inheritdoc/>
        public override void OnLayout()
        {
            InitAutoSplit();
            PerformChildsLayout();
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
        public virtual bool DoUnsplit(Control? toRemove = null)
        {
            if (!IsSplit)
                return true;
            toRemove ??= control2;
            if (toRemove == null)
                return false;
            Native.Control? nc1 = (toRemove.Handler as WxControlHandler)?.NativeControl;
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

        internal void RaiseSplitterDoubleClick(SplitterPanelEventArgs e)
        {
            OnSplitterDoubleClick(e);
            SplitterDoubleClick?.Invoke(this, e);
            if (!e.Cancel)
                PerformChildsLayout();
        }

        internal void RaiseSplitterMoved(SplitterPanelEventArgs e)
        {
            OnSplitterMoved(e);
            SplitterMoved?.Invoke(this, e);
            if (!e.Cancel)
                PerformChildsLayout();
        }

        internal void RaiseSplitterResize(SplitterPanelEventArgs e)
        {
            OnSplitterResize(e);
            SplitterResize?.Invoke(this, e);
            if (!e.Cancel)
                PerformChildsLayout();
        }

        internal void RaiseSplitterMoving(SplitterPanelEventArgs e)
        {
            OnSplitterMoving(e);
            SplitterMoving?.Invoke(this, e);
            if (!e.Cancel)
                PerformChildsLayout();
        }

        internal void RaiseUnsplit(SplitterPanelEventArgs e)
        {
            OnUnsplit(e);
            Unsplit?.Invoke(this, e);
            if (!e.Cancel)
                PerformChildsLayout();
        }

        /// <inheritdoc />
        protected override IControlHandler CreateHandler()
        {
            return new SplitterPanelHandler();
        }

        /// <summary>
        /// Called when the splitter sash is double clicked.
        /// </summary>
        /// <param name="e">
        /// An <see cref="SplitterPanelEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnSplitterDoubleClick(SplitterPanelEventArgs e)
        {
        }

        /// <summary>
        /// Called when the splitter sash is moved.
        /// </summary>
        /// <param name="e">
        /// An <see cref="SplitterPanelEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnSplitterMoved(SplitterPanelEventArgs e)
        {
        }

        /// <summary>
        /// Called when the splitter sash is moved.
        /// </summary>
        /// <param name="e">
        /// An <see cref="SplitterPanelEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnSplitterResize(SplitterPanelEventArgs e)
        {
        }

        /// <summary>
        /// Called when the splitter sash is in the process of moving.
        /// </summary>
        /// <param name="e">
        /// An <see cref="SplitterPanelEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnSplitterMoving(SplitterPanelEventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is unsplit.
        /// </summary>
        /// <param name="e">
        /// An <see cref="SplitterPanelEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnUnsplit(SplitterPanelEventArgs e)
        {
        }

        /// <inheritdoc />
        protected override void OnChildInserted(Control childControl)
        {
            base.OnChildInserted(childControl);
        }

        /// <inheritdoc />
        protected override void OnChildRemoved(Control childControl)
        {
            base.OnChildRemoved(childControl);
        }

        private void PerformChildsLayout()
        {
            Control1?.PerformLayout(false);
            Control2?.PerformLayout(false);
       }

        private void InitAutoSplit()
        {
            if (initAutoSplit)
                return;
            initAutoSplit = true;

            if (SplitMethod == SplitterPanelSplitMethod.Manual)
                return;
            if (IsSplit || control1 != null)
                return;

            Control? newControl1 = GetVisibleChildOrNull(0);
            Control? newControl2 = GetVisibleChildOrNull(1);

            if (newControl1 == null)
                return;

            switch (SplitMethod)
            {
                case SplitterPanelSplitMethod.Horizontal:
                    if (newControl2 == null)
                        return;
                    SplitHorizontal(newControl1, newControl2, SashPosition);
                    return;
                case SplitterPanelSplitMethod.Vertical:
                    if (newControl2 == null)
                        return;
                    SplitVertical(newControl1, newControl2, SashPosition);
                    return;
                case SplitterPanelSplitMethod.Unsplitted:
                    InitUnsplitted(newControl1);
                    return;
            }
        }
    }
}