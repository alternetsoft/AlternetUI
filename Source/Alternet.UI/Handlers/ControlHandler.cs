using System;
using System.Collections.Generic;
using Alternet.Drawing;
using System.Linq;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Control"/> behavior and appearance.
    /// </summary>
    public abstract class ControlHandler
    {
        private int layoutSuspendCount;

        private bool inLayout;

        private Control? control;

        private RectangleF bounds;

        private Native.Control? nativeControl;
        private bool isVisualChild;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public ControlHandler()
        {
            VisualChildren.ItemInserted += VisualChildren_ItemInserted;
            VisualChildren.ItemRemoved += VisualChildren_ItemRemoved;
        }

        /// <summary>
        /// Gets a <see cref="Control"/> this handler provides the implementation for.
        /// </summary>
        public Control Control
        {
            get => control ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ControlHandler"/> is attached to a <see cref="Control"/>.
        /// </summary>
        public bool IsAttached => control != null;

        /// <summary>
        /// Gets or sets the <see cref="Control"/> bounds relative to the parent, in device-independent units (1/96th inch per unit).
        /// </summary>
        public virtual RectangleF Bounds
        {
            get => NativeControl != null ? NativeControl.Bounds : bounds;
            set
            {
                if (NativeControl != null)
                    NativeControl.Bounds = value;
                else
                    bounds = value;

                PerformLayout(); // todo: use event
            }
        }

        /// <summary>
        /// Gets or sets size of the <see cref="Control"/>'s client area, in device-independent units (1/96th inch per unit).
        /// </summary>
        public SizeF ClientSize
        {
            get => NativeControl != null ? NativeControl.ClientSize : bounds.Size;
            set
            {
                if (NativeControl != null)
                    NativeControl.ClientSize = value;
                else
                    bounds = new RectangleF(bounds.Location, value);

                PerformLayout(); // todo: use event
            }
        }

        /// <summary>
        /// Gets a rectangle which describes an area inside of the <see cref="Control"/> available
        /// for positioning (layout) of its child controls, in device-independent units (1/96th inch per unit).
        /// </summary>
        public virtual RectangleF ChildrenLayoutBounds
        {
            get
            {
                var childrenBounds = ClientRectangle;
                if (childrenBounds.IsEmpty)
                    return RectangleF.Empty;

                var padding = Control.Padding;

                var intrinsicPadding = new Thickness();
                var nativeControl = NativeControl;
                if (nativeControl != null)
                    intrinsicPadding = nativeControl.IntrinsicLayoutPadding;

                return new RectangleF(
                    new PointF(padding.Left + intrinsicPadding.Left, padding.Top + intrinsicPadding.Top),
                    childrenBounds.Size - padding.Size - intrinsicPadding.Size);
            }
        }

        /// <summary>
        /// Gets a rectangle which describes the client area inside of the <see cref="Control"/>,
        /// in device-independent units (1/96th inch per unit).
        /// </summary>
        public virtual RectangleF ClientRectangle => new RectangleF(new PointF(), ClientSize);

        /// <summary>
        /// Gets a value indicating whether the mouse pointer is over the <see cref="Control"/>.
        /// </summary>
        public bool IsMouseOver
        {
            get
            {
                if (NativeControl == null)
                    return false;

                return NativeControl.IsMouseOver;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Control"/> is contained in a <see cref="VisualChildren"/> collection.
        /// </summary>
        public bool IsVisualChild
        {
            get => isVisualChild;
            private set
            {
                if (isVisualChild == value)
                    return;

                isVisualChild = value;
                OnIsVisualChildChanged();
            }
        }

        /// <summary>
        /// Gets the collection of visual child controls contained within the control.
        /// </summary>
        public Collection<Control> VisualChildren { get; } = new Collection<Control>();

        /// <summary>
        /// Gets the collection of all elements of <see cref="Control.Children"/> and <see cref="VisualChildren"/> collections.
        /// </summary>
        public IEnumerable<Control> AllChildren => VisualChildren.Concat(Control.Children);

        /// <summary>
        /// Gets the collection of all elements of <see cref="AllChildren"/> collection included in layout (i.e. visible).
        /// </summary>
        public IEnumerable<Control> AllChildrenIncludedInLayout => AllChildren.Where(x => x.Visible);

        internal Native.Control? NativeControl
        {
            get
            {
                if (nativeControl == null)
                {
                    if (NeedsNativeControl())
                    {
                        nativeControl = CreateNativeControl();
                        OnNativeControlCreated();
                    }
                }

                return nativeControl;
            }
        }

        /// <summary>
        /// This property may be overridden by control handlers to indicate that the handler needs
        /// <see cref="OnPaint(DrawingContext)"/> to be called. Default value is <c>false</c>.
        /// </summary>
        protected virtual bool NeedsPaint => false;

        /// <summary>
        /// This property may be overridden by control handlers to indicate that the native control creation is required
        /// even if the control is a visual child. Default value is <c>false</c>.
        /// </summary>
        protected virtual bool VisualChildNeedsNativeControl => false;

        private bool IsLayoutSuspended => layoutSuspendCount != 0;

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this handler to.</param>
        public void Attach(Control control)
        {
            this.control = control;
            OnAttach();
        }

        /// <summary>
        /// Attaches this handler from the <see cref="Control"/> it is attached to.
        /// </summary>
        public void Detach()
        {
            OnDetach();

            DisposeNativeControl();
            control = null;
        }

        /// <summary>
        /// Causes the control to redraw.
        /// </summary>
        public void Update()
        {
            var nativeControl = NativeControl;
            if (nativeControl != null)
                nativeControl.Update();
            else
            {
                var parent = TryFindClosestParentWithNativeControl();
                if (parent != null)
                    parent.Update();
            }
        }

        /// <summary>
        /// This property may be overridden by control handlers to paint the control visual representation.
        /// </summary>
        /// <remarks>
        /// <see cref="NeedsPaint"/> for this handler should return <c>true</c> in order for <see cref="OnPaint(DrawingContext)"/>
        /// to be called.
        /// </remarks>
        /// <param name="drawingContext">The <see cref="DrawingContext"/> to paint on.</param>
        public virtual void OnPaint(DrawingContext drawingContext)
        {
        }

        /// <summary>
        /// Called when the handler should reposition the child controls of the control it is attached to.
        /// </summary>
        public virtual void OnLayout()
        {
            var childrenLayoutBounds = ChildrenLayoutBounds;
            foreach (var control in AllChildrenIncludedInLayout)
            {
                var preferredSize = control.GetPreferredSize(childrenLayoutBounds.Size);

                var horizontalPosition = AlignedLayout.AlignHorizontal(childrenLayoutBounds, control, preferredSize);
                var verticalPosition = AlignedLayout.AlignVertical(childrenLayoutBounds, control, preferredSize);

                control.Handler.Bounds = new RectangleF(
                    horizontalPosition.Origin,
                    verticalPosition.Origin,
                    horizontalPosition.Size,
                    verticalPosition.Size);

                //var margin = control.Margin;

                //var specifiedWidth = control.Width;
                //var specifiedHeight = control.Height;

                //control.Handler.Bounds = new RectangleF(
                //    childrenLayoutBounds.Location + new SizeF(margin.Left, margin.Top),
                //    new SizeF(
                //        float.IsNaN(specifiedWidth) ? childrenLayoutBounds.Width - margin.Horizontal : specifiedWidth,
                //        float.IsNaN(specifiedHeight) ? childrenLayoutBounds.Height - margin.Vertical : specifiedHeight));
            }
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which the control can be fitted, in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="availableSize">The available space that a parent element can allocate a child control.</param>
        /// <returns>A <see cref="Size"/> representing the width and height of a rectangle, in device-independent units (1/96th inch per unit).</returns>
        public virtual SizeF GetPreferredSize(SizeF availableSize)
        {
            if (Control.Children.Count == 0 && VisualChildren.Count == 0)
            {
                var s = NativeControl?.GetPreferredSize(availableSize) ?? new SizeF();
                return new SizeF(
                    float.IsNaN(Control.Width) ? s.Width : Control.Width,
                    float.IsNaN(Control.Height) ? s.Height : Control.Height);
            }
            else
            {
                return GetChildrenMaxPreferredSize(availableSize);
            }
        }

        /// <summary>
        /// Temporarily suspends the layout logic for the control.
        /// </summary>
        public void SuspendLayout()
        {
            layoutSuspendCount++;
        }

        /// <summary>
        /// Resumes the usual layout logic.
        /// </summary>
        /// <param name="performLayout"><c>true</c> to execute pending layout requests; otherwise, <c>false</c>.</param>
        public void ResumeLayout(bool performLayout = true)
        {
            layoutSuspendCount--;
            if (layoutSuspendCount < 0)
                throw new InvalidOperationException();

            if (!IsLayoutSuspended)
            {
                if (performLayout)
                    PerformLayout();
            }
        }

        /// <summary>
        /// Maintains performance while performing slow operations on a control by preventing the control from
        /// drawing until the <see cref="EndUpdate"/> method is called.
        /// </summary>
        public void BeginUpdate()
        {
            if (NativeControl != null)
                NativeControl.BeginUpdate();
        }

        /// <summary>
        /// Resumes painting the control after painting is suspended by the <see cref="BeginUpdate"/> method.
        /// </summary>
        public void EndUpdate()
        {
            if (NativeControl != null)
                NativeControl.EndUpdate();
        }

        /// <summary>
        /// Forces the control to apply layout logic to child controls.
        /// </summary>
        public void PerformLayout()
        {
            if (IsLayoutSuspended)
                return;

            if (inLayout)
                return;

            inLayout = true;
            try
            {
                // todo: we need a system to detect when parent relayout is needed?
                var parent = Control.Parent;
                if (parent != null)
                    parent.PerformLayout();

                Control.InvokeOnLayout();
            }
            finally
            {
                inLayout = false;
            }
        }

        /// <summary>
        /// Captures the mouse to the control.
        /// </summary>
        public void CaptureMouse()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.SetMouseCapture(true);
        }

        /// <summary>
        /// Releases the mouse capture, if the control held the capture.
        /// </summary>
        public void ReleaseMouseCapture()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.SetMouseCapture(false);
        }

        internal DrawingContext CreateDrawingContext()
        {
            var nativeControl = NativeControl;
            if (nativeControl == null)
            {
                nativeControl = TryFindClosestParentWithNativeControl()?.Handler.NativeControl;
                // todo: visual offset for handleless controls
                if (nativeControl == null)
                    throw new Exception(); // todo: maybe use parking window here?
            }

            return new DrawingContext(nativeControl.OpenClientDrawingContext());
        }

        internal virtual Native.Control CreateNativeControl() => new Native.Panel();

        /// <summary>
        /// Called when the value of the <see cref="IsVisualChild"/> property changes.
        /// </summary>
        protected virtual void OnIsVisualChildChanged()
        {
            if (!NeedsNativeControl())
                DisposeNativeControl();
        }

        /// <summary>
        /// Gets a value indicating whether the control needs a native control to be created.
        /// </summary>
        protected virtual bool NeedsNativeControl()
        {
            if (IsVisualChild)
                return VisualChildNeedsNativeControl;

            return true;
        }

        /// <summary>
        /// Gets the size of the area which can fit all the children of this control.
        /// </summary>
        protected SizeF GetChildrenMaxPreferredSize(SizeF availableSize)
        {
            var specifiedWidth = Control.Width;
            var specifiedHeight = Control.Height;
            if (!float.IsNaN(specifiedWidth) && !float.IsNaN(specifiedHeight))
                return new SizeF(specifiedWidth, specifiedHeight);

            float maxWidth = 0;
            float maxHeight = 0;

            foreach (var control in AllChildrenIncludedInLayout)
            {
                var preferredSize = control.GetPreferredSize(availableSize) + control.Margin.Size;
                maxWidth = Math.Max(preferredSize.Width, maxWidth);
                maxHeight = Math.Max(preferredSize.Height, maxHeight);
            }

            var padding = Control.Padding;

            var intrinsicPadding = new Thickness();
            var nativeControl = NativeControl;
            if (nativeControl != null)
                intrinsicPadding = nativeControl.IntrinsicPreferredSizePadding;

            var width = float.IsNaN(specifiedWidth) ? maxWidth + padding.Horizontal + intrinsicPadding.Horizontal : specifiedWidth;
            var height = float.IsNaN(specifiedHeight) ? maxHeight + padding.Vertical + intrinsicPadding.Vertical : specifiedHeight;

            return new SizeF(width, height);
        }

        /// <summary>
        /// Called after this handler has been attached to a <see cref="Control"/>.
        /// </summary>
        protected virtual void OnAttach()
        {
            ApplyVisible();
            ApplyEnabled();
            ApplyBorderColor();
            ApplyBackgroundColor();
            ApplyForegroundColor();
            ApplyFont();
            ApplyChildren();

            Control.MarginChanged += Control_MarginChanged;
            Control.PaddingChanged += Control_PaddingChanged;
            Control.BackgroundChanged += Control_BackgroundChanged;
            Control.ForegroundChanged += Control_ForegroundChanged;
            Control.FontChanged += Control_FontChanged;
            Control.BorderBrushChanged += Control_BorderBrushChanged;
            Control.VisibleChanged += Control_VisibleChanged;
            Control.EnabledChanged += Control_EnabledChanged;
            Control.VerticalAlignmentChanged += Control_VerticalAlignmentChanged;
            Control.HorizontalAlignmentChanged += Control_HorizontalAlignmentChanged;

            Control.Children.ItemInserted += Children_ItemInserted;
            VisualChildren.ItemInserted += Children_ItemInserted;

            Control.Children.ItemRemoved += Children_ItemRemoved;
            VisualChildren.ItemRemoved += Children_ItemRemoved;
        }

        private void Control_FontChanged(object? sender, EventArgs e)
        {
            ApplyFont();
        }

        /// <summary>
        /// Called after this handler has been detached from the <see cref="Control"/>.
        /// </summary>
        protected virtual void OnDetach()
        {
            // todo: consider clearing the native control's children.
            Control.MarginChanged -= Control_MarginChanged;
            Control.PaddingChanged -= Control_PaddingChanged;
            Control.BackgroundChanged -= Control_BackgroundChanged;
            Control.ForegroundChanged -= Control_ForegroundChanged;
            Control.FontChanged -= Control_FontChanged;
            Control.BorderBrushChanged -= Control_BorderBrushChanged;
            Control.VisibleChanged -= Control_VisibleChanged;
            Control.EnabledChanged -= Control_EnabledChanged;
            Control.VerticalAlignmentChanged -= Control_VerticalAlignmentChanged;
            Control.HorizontalAlignmentChanged -= Control_HorizontalAlignmentChanged;

            Control.Children.ItemInserted -= Children_ItemInserted;
            Control.Children.ItemRemoved -= Children_ItemRemoved;

            VisualChildren.Clear();

            if (NativeControl != null)
            {
                NativeControl.Paint -= NativeControl_Paint;
                NativeControl.VisibleChanged -= NativeControl_VisibleChanged;
                NativeControl.MouseEnter -= NativeControl_MouseEnter;
                NativeControl.MouseLeave -= NativeControl_MouseLeave;
                NativeControl.MouseMove -= NativeControl_MouseMove;
                NativeControl.MouseLeftButtonDown -= NativeControl_MouseLeftButtonDown;
                NativeControl.MouseLeftButtonUp -= NativeControl_MouseLeftButtonUp;
            }
        }

        private protected virtual void OnNativeControlCreated()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            var parent = Control.Parent;
            if (parent != null)
            {
                parent.Handler.TryInsertNativeControl(parent.Children.IndexOf(Control), Control); // todo: sort out indexof in case of visual children.
                parent.PerformLayout();
            }

            NativeControl.Paint += NativeControl_Paint;
            NativeControl.VisibleChanged += NativeControl_VisibleChanged;
            NativeControl.MouseEnter += NativeControl_MouseEnter;
            NativeControl.MouseLeave += NativeControl_MouseLeave;
            NativeControl.MouseMove += NativeControl_MouseMove;
            NativeControl.MouseLeftButtonDown += NativeControl_MouseLeftButtonDown;
            NativeControl.MouseLeftButtonUp += NativeControl_MouseLeftButtonUp;
        }

        /// <summary>
        /// Called when the mouse cursor changes position.
        /// </summary>
        protected virtual void OnMouseMove()
        {
        }

        /// <summary>
        /// Called when the mouse cursor enters the boundary of the control.
        /// </summary>
        protected virtual void OnMouseEnter()
        {
        }

        /// <summary>
        /// Called when the mouse cursor leaves the boundary of the control.
        /// </summary>
        protected virtual void OnMouseLeave()
        {
        }

        /// <summary>
        /// Called when the mouse left button is released while the mouse pointer is over
        /// the control or when the control has captured the mouse.
        /// </summary>
        protected virtual void OnMouseLeftButtonUp()
        {
        }

        /// <summary>
        /// Called when the mouse left button is pressed while the mouse pointer is over
        /// the control or when the control has captured the mouse.
        /// </summary>
        protected virtual void OnMouseLeftButtonDown()
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is inserted into the <see cref="Control.Children"/> or <see cref="VisualChildren"/> collection.
        /// </summary>
        protected virtual void OnChildInserted(int childIndex, Control childControl)
        {
            // todo: the childIndex passed to this method is wrong as should take VisualChildren into account.
            TryInsertNativeControl(childIndex, childControl);
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the <see cref="Control.Children"/> or <see cref="VisualChildren"/> collections.
        /// </summary>
        protected virtual void OnChildRemoved(int childIndex, Control childControl)
        {
            // todo: the childIndex passed to this method is wrong as should take VisualChildren into account.
            TryRemoveNativeControl(childIndex, childControl);
        }

        private void Control_VerticalAlignmentChanged(object? sender, EventArgs e)
        {
            PerformLayout();
        }

        private void Control_HorizontalAlignmentChanged(object? sender, EventArgs e)
        {
            PerformLayout();
        }

        private void ApplyChildren()
        {
            for (var i = 0; i < Control.Children.Count; i++)
                OnChildInserted(i, Control.Children[i]);
        }

        private void VisualChildren_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = Control;
            e.Item.Handler.IsVisualChild = true;

            OnChildInserted(e.Index, e.Item);
            PerformLayout();
        }

        private void VisualChildren_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = null;
            e.Item.Handler.IsVisualChild = false;

            OnChildRemoved(e.Index, e.Item);
            PerformLayout();
        }

        private void DisposeNativeControl()
        {
            if (nativeControl != null)
            {
                nativeControl.Dispose();
                nativeControl = null;
            }
        }

        private void Control_BorderBrushChanged(object? sender, EventArgs? e)
        {
            ApplyBorderColor();
        }

        private void Control_BackgroundChanged(object? sender, EventArgs? e)
        {
            ApplyBackgroundColor();
        }

        private void Control_ForegroundChanged(object? sender, EventArgs? e)
        {
            ApplyForegroundColor();
        }

        private void Control_VisibleChanged(object? sender, EventArgs e)
        {
            ApplyVisible();
            if (NeedRelayoutParentOnVisibleChanged)
                Control.Parent?.PerformLayout();
        }

        private void Control_EnabledChanged(object? sender, EventArgs e)
        {
            ApplyEnabled();
        }

        private protected virtual bool NeedRelayoutParentOnVisibleChanged => !(Control.Parent is TabControl); // todo

        private Color GetBrushColor(Brush? brush)
        {
            var solidBrush = brush as SolidBrush;
            if (solidBrush == null)
                return Color.Empty;

            return solidBrush.Color;
        }

        private void ApplyBackgroundColor()
        {
            if (NativeControl != null)
                NativeControl.BackgroundColor = GetBrushColor(Control.Background);
            Update();
        }

        private void ApplyVisible()
        {
            if (NativeControl != null)
                NativeControl.Visible = Control.Visible;
        }

        private void ApplyEnabled()
        {
            if (NativeControl != null)
                NativeControl.Enabled = Control.Enabled;
        }

        private void ApplyForegroundColor()
        {
            if (NativeControl != null)
                NativeControl.ForegroundColor = GetBrushColor(Control.Foreground);
            Update();
        }

        private void ApplyFont()
        {
            if (NativeControl != null)
                NativeControl.Font = Control.Font?.NativeFont;

            Update();
        }

        private void ApplyBorderColor()
        {
            //if (NativeControl != null)
            //    NativeControl.BorderColor = GetBrushColor(Control.BorderBrush);
            Update();
        }

        private void TryInsertNativeControl(int childIndex, Control childControl)
        {
            // todo: use index

            var childNativeControl = childControl.Handler.NativeControl;
            if (childNativeControl == null)
                return;

            if (childNativeControl.Parent != null)
                return;

            var parentNativeControl = NativeControl;
            if (parentNativeControl == null)
                parentNativeControl = TryFindClosestParentWithNativeControl()?.Handler.NativeControl;

            if (parentNativeControl != null)
                parentNativeControl.AddChild(childNativeControl);
        }

        private void TryRemoveNativeControl(int childIndex, Control childControl)
        {
            if (nativeControl != null && childControl.Handler.nativeControl != null)
                nativeControl?.RemoveChild(childControl.Handler.nativeControl);
        }

        private Control? TryFindClosestParentWithNativeControl()
        {
            var control = Control;
            if (control.Handler.NativeControl != null)
                return control;

            while (true)
            {
                control = control.Parent;
                if (control == null)
                    return null;

                if (control.Handler.NativeControl != null)
                    return control;
            }
        }

        private void NativeControl_VisibleChanged(object? sender, EventArgs e)
        {
            if (NativeControl != null)
            {
                Control.Visible = NativeControl.Visible;

#if NETCOREAPP
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                {
                    if (NativeControl.Visible)
                    {
                        // todo: this is a workaround for a problem on Linux when
                        // ClientSize is not reported correctly until the window is shown
                        // So we need to relayout all after the proper client size is available
                        // This should be changed later in respect to RedrawOnResize functionality.
                        // Also we may need to do this for top-level windows.
                        // Doing this on Windows results in strange glitches like disappearing tab controls' tab.
                        // See https://forums.wxwidgets.org/viewtopic.php?f=1&t=47439
                        PerformLayout();
                    }
                }
#endif
            }
        }

        private void NativeControl_MouseEnter(object? sender, EventArgs? e)
        {
            OnMouseEnter();
        }

        private void NativeControl_MouseLeave(object? sender, EventArgs? e)
        {
            OnMouseLeave();
        }

        private void NativeControl_MouseMove(object? sender, EventArgs? e)
        {
            var handler = this;
            while (true)
            {
                handler.OnMouseMove();
                var parent = handler.Control.Parent;
                if (parent == null)
                    break;
                handler = parent.Handler;
            }
        }

        private void NativeControl_MouseLeftButtonDown(object? sender, EventArgs? e)
        {
            var handler = this;
            while (true)
            {
                handler.OnMouseLeftButtonDown();
                if (!handler.IsAttached)
                    break;
                var parent = handler.Control.Parent;
                if (parent == null)
                    break;
                handler = parent.Handler;
            }
        }

        private void NativeControl_MouseLeftButtonUp(object? sender, EventArgs? e)
        {
            var handler = this;
            while (true)
            {
                handler.OnMouseLeftButtonUp();
                if (!handler.IsAttached)
                    break;
                var parent = handler.Control.Parent;
                if (parent == null)
                    break;
                handler = parent.Handler;
            }
        }

        private void Control_MarginChanged(object? sender, EventArgs? e)
        {
            PerformLayout();
        }

        private void Control_PaddingChanged(object? sender, EventArgs? e)
        {
            PerformLayout();
        }

        private void Children_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            OnChildInserted(e.Index, e.Item);
            PerformLayout();
        }

        private void Children_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            OnChildRemoved(e.Index, e.Item);
            PerformLayout();
        }

        private void NativeControl_Paint(object? sender, System.EventArgs? e)
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            bool hasVisualChildren = VisualChildren.Count > 0;

            if (Control.UserPaint)
            {
                using (var dc = new DrawingContext(NativeControl.OpenPaintDrawingContext()))
                    Control.RaisePaint(new PaintEventArgs(dc, ClientRectangle));
            }
            else if (NeedsPaint || hasVisualChildren)
            {
                using (var dc = new DrawingContext(NativeControl.OpenPaintDrawingContext()))
                    PaintSelfAndVisualChildren(dc);
            }
        }

        private void PaintSelfAndVisualChildren(DrawingContext dc)
        {
            if (NeedsPaint)
                OnPaint(dc);

            foreach (var visualChild in VisualChildren)
            {
                dc.PushTransform(Transform.FromTranslation(visualChild.Handler.Bounds.Location));
                visualChild.Handler.PaintSelfAndVisualChildren(dc);
                dc.Pop();
            }
        }
    }
}