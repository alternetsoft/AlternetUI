using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

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

        private Rect bounds;

        private Native.Control? nativeControl;
        private bool isVisualChild;
        Collection<Control>? visualChildren;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public ControlHandler()
        {
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
        public virtual Rect Bounds
        {
            get => NativeControl != null ? NativeControl.Bounds : bounds;
            set
            {
                var oldBounds = Bounds;

                if (NativeControl != null)
                    NativeControl.Bounds = value;
                else
                    bounds = value;

                if (oldBounds.Location != value.Location)
                    Control.RaiseLocationChanged(EventArgs.Empty);

                if (oldBounds.Size != value.Size)
                    Control.RaiseSizeChanged(EventArgs.Empty);

                if (oldBounds != Bounds)
                    PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets size of the <see cref="Control"/>'s client area, in device-independent units (1/96th inch per unit).
        /// </summary>
        public Size ClientSize
        {
            get => NativeControl != null ? NativeControl.ClientSize : bounds.Size;
            set
            {
                if (NativeControl != null)
                    NativeControl.ClientSize = value;
                else
                    bounds = new Rect(bounds.Location, value);

                PerformLayout(); // todo: use event
            }
        }

        /// <inheritdoc cref="Control.GetDPI"/>
        public Size GetDPI()
        {
            if (NativeControl == null)
                return Size.Empty;
            return NativeControl.GetDPI();
        }

        internal IntPtr GetHandle()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            return NativeControl.Handle;
        }

        /// <summary>
        /// Gets a rectangle which describes an area inside of the
        /// <see cref="Control"/> available
        /// for positioning (layout) of its child controls, in device-independent
        /// units (1/96th inch per unit).
        /// </summary>
        public virtual Rect ChildrenLayoutBounds
        {
            get
            {
                var childrenBounds = ClientRectangle;
                if (childrenBounds.IsEmpty)
                    return Rect.Empty;

                var padding = Control.Padding;

                var intrinsicPadding = new Thickness();
                var nativeControl = NativeControl;
                if (nativeControl != null)
                    intrinsicPadding = nativeControl.IntrinsicLayoutPadding;

                return new Rect(
                    new Point(
                        padding.Left + intrinsicPadding.Left,
                        padding.Top + intrinsicPadding.Top),
                    childrenBounds.Size - padding.Size - intrinsicPadding.Size);
            }
        }

        /// <summary>
        /// Gets a rectangle which describes the client area inside of the
        /// <see cref="Control"/>,
        /// in device-independent units (1/96th inch per unit).
        /// </summary>
        public virtual Rect ClientRectangle => new(new Point(), ClientSize);

        /// <inheritdoc cref="Control.DrawClientRectangle"/>
        public virtual Rect DrawClientRectangle => Control.DrawClientRectangle;

        /// <summary>
        /// Gets a value indicating whether the mouse pointer is over the
        /// <see cref="Control"/>.
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
        /// Gets the collection of visual child controls contained within
        /// the control.
        /// </summary>
        public virtual Collection<Control> VisualChildren
        {
            get
            {
                if (visualChildren == null)
                {
                    visualChildren = new Collection<Control>();
                    visualChildren.ItemInserted += VisualChildren_ItemInserted;
                    visualChildren.ItemRemoved += VisualChildren_ItemRemoved;
                }

                return visualChildren;
            }
        }

        /// <summary>
        /// Gets whether there are any items in the <see cref="VisualChildren"/> list.
        /// </summary>
        public virtual bool HasVisualChildren =>
            visualChildren != null && VisualChildren.Count > 0;

        /// <summary>
        /// Gets the collection of all elements of <see cref="Control.Children"/>
        /// and <see cref="VisualChildren"/> collections.
        /// </summary>
        public virtual IEnumerable<Control> AllChildren
        {
            get
            {
                if (Control.HasChildren)
                {
                    if (HasVisualChildren)
                        return VisualChildren.Concat(Control.Children);
                    return Control.Children;
                }

                if (HasVisualChildren)
                    return VisualChildren;
                return Array.Empty<Control>();
            }
        }

        /// <summary>
        /// Gets the collection of all elements of <see cref="AllChildren"/>
        /// collection included in layout (i.e. visible).
        /// </summary>
        public virtual IEnumerable<Control> AllChildrenIncludedInLayout
            => AllChildren.Where(x => x.Visible);

        internal Native.Control? NativeControl
        {
            get
            {
                if (nativeControl == null)
                {
                    if (NeedsNativeControl())
                    {
                        nativeControl = CreateNativeControl();
                        //handlersByNativeControls.Add(nativeControl, this);
                        nativeControl.handler = this;
                        OnNativeControlCreated();
                    }
                }

                return nativeControl;
            }
        }

        internal bool NativeControlCreated => nativeControl != null;

        /// <summary>
        /// This property may be overridden by control handlers to indicate that
        /// the handler needs
        /// <see cref="OnPaint(DrawingContext)"/> to be called. Default value
        /// is <c>false</c>.
        /// </summary>
        protected virtual bool NeedsPaint => false;

        /// <summary>
        /// This property may be overridden by control handlers to indicate that
        /// the native control creation is required
        /// even if the control is a visual child. Default value is <c>false</c>.
        /// </summary>
        protected virtual bool VisualChildNeedsNativeControl => false;

        private bool IsLayoutSuspended => layoutSuspendCount != 0;

        /// <summary>
        /// Attaches this handler to the specified <see cref="Control"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to attach this
        /// handler to.</param>
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
        /// Causes the control to redraw the invalidated regions.
        /// </summary>
        public void Update()
        {
            var nativeControl = NativeControl;
            if (nativeControl != null)
                nativeControl.Update();
            else
            {
                var parent = TryFindClosestParentWithNativeControl();
                parent?.Update();
            }
        }

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        public void Invalidate()
        {
            var nativeControl = NativeControl;
            if (nativeControl != null)
                nativeControl.Invalidate();
            else
            {
                var parent = TryFindClosestParentWithNativeControl();
                parent?.Invalidate();
            }
        }

        /// <summary>
        /// Forces the control to invalidate itself and immediately redraw itself
        /// and any child controls.
        /// </summary>
        public void Refresh()
        {
            Invalidate();
            Update();
        }

        /// <summary>
        /// This property may be overridden by control handlers to paint the
        /// control visual representation.
        /// </summary>
        /// <remarks>
        /// <see cref="NeedsPaint"/> for this handler should return <c>true</c> in
        /// order for <see cref="OnPaint(DrawingContext)"/>
        /// to be called.
        /// </remarks>
        /// <param name="drawingContext">The <see cref="DrawingContext"/> to paint
        /// on.</param>
        public virtual void OnPaint(DrawingContext drawingContext)
        {
        }

        /// <summary>
        /// Called when the handler should reposition the child controls of the
        /// control it is attached to.
        /// </summary>
        public virtual void OnLayout()
        {
            var childrenLayoutBounds = ChildrenLayoutBounds;
            foreach (var control in AllChildrenIncludedInLayout)
            {
                var preferredSize = control.GetPreferredSize(childrenLayoutBounds.Size);

                var horizontalPosition =
                    AlignedLayout.AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        preferredSize);
                var verticalPosition =
                    AlignedLayout.AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize);

                control.Handler.Bounds = new Rect(
                    horizontalPosition.Origin,
                    verticalPosition.Origin,
                    horizontalPosition.Size,
                    verticalPosition.Size);

                // var margin = control.Margin;

                // var specifiedWidth = control.Width;
                // var specifiedHeight = control.Height;

                // control.Handler.Bounds = new RectangleF(
                //    childrenLayoutBounds.Location + new SizeF(margin.Left, margin.Top),
                //    new SizeF(
                //        double.IsNaN(specifiedWidth) ? childrenLayoutBounds.Width - margin.Horizontal : specifiedWidth,
                //        double.IsNaN(specifiedHeight) ? childrenLayoutBounds.Height - margin.Vertical : specifiedHeight));
            }
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which the control can
        /// be fitted, in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="availableSize">The available space that a parent element
        /// can allocate a child control.</param>
        /// <returns>A <see cref="Size"/> representing the width and height of
        /// a rectangle, in device-independent units (1/96th inch per unit).</returns>
        public virtual Size GetPreferredSize(Size availableSize)
        {
            if (Control.HasChildren || HasVisualChildren)
                return GetSpecifiedOrChildrenPreferredSize(availableSize);
            return GetNativeControlSize(availableSize);
        }

        private protected Size GetNativeControlSize(Size availableSize)
        {
            if (Control.IsDummy)
                return Size.Empty;
            var s = NativeControl?.GetPreferredSize(availableSize) ?? Size.Empty;
            s += Control.Padding.Size;
            return new Size(
                double.IsNaN(Control.Width) ? s.Width : Control.Width,
                double.IsNaN(Control.Height) ? s.Height : Control.Height);
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
        /// <param name="performLayout"><c>true</c> to execute pending layout
        /// requests; otherwise, <c>false</c>.</param>
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
        /// Maintains performance while performing slow operations on a control
        /// by preventing the control from
        /// drawing until the <see cref="EndUpdate"/> method is called.
        /// </summary>
        public void BeginUpdate()
        {
            NativeControl?.BeginUpdate();
        }

        /// <summary>
        /// Resumes painting the control after painting is suspended by the
        /// <see cref="BeginUpdate"/> method.
        /// </summary>
        public void EndUpdate()
        {
            NativeControl?.EndUpdate();
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
                var parent = Control.Parent;
                parent?.PerformLayout();

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

        /// <summary>
        /// Gets a value indicating whether the mouse is captured to this control.
        /// </summary>
        public bool IsMouseCaptured
        {
            get
            {
                if (NativeControl == null)
                    throw new InvalidOperationException();

                return NativeControl.IsMouseCaptured;
            }
        }

        internal DrawingContext CreateDrawingContext()
        {
            var nativeControl = NativeControl;
            if (nativeControl == null)
            {
                nativeControl =
                    TryFindClosestParentWithNativeControl()?.Handler.NativeControl;

                // todo: visual offset for handleless controls
                if (nativeControl == null)
                    throw new Exception(); // todo: maybe use parking window here?
            }

            return new DrawingContext(nativeControl.OpenClientDrawingContext());
        }

        internal virtual Native.Control CreateNativeControl() =>
            new Native.Panel();

        /// <summary>
        /// Called when the value of the <see cref="IsVisualChild"/> property changes.
        /// </summary>
        protected virtual void OnIsVisualChildChanged()
        {
            if (!NeedsNativeControl())
                DisposeNativeControl();
        }

        /// <summary>
        /// Gets a value indicating whether the control needs a native control
        /// to be created.
        /// </summary>
        protected virtual bool NeedsNativeControl()
        {
            if (IsVisualChild)
                return VisualChildNeedsNativeControl;

            return true;
        }

        /// <summary>
        /// Gets the size of the control specified in its
        /// <see cref="Control.Width"/> and <see cref="Control.Height"/>
        /// properties or calculates preferred size from its children.
        /// </summary>
        protected Size GetSpecifiedOrChildrenPreferredSize(Size availableSize)
        {
            var specifiedWidth = Control.Width;
            var specifiedHeight = Control.Height;
            if (!double.IsNaN(specifiedWidth) && !double.IsNaN(specifiedHeight))
                return new Size(specifiedWidth, specifiedHeight);

            var maxSize = GetChildrenMaxPreferredSizePadded(availableSize);
            var maxWidth = maxSize.Width;
            var maxHeight = maxSize.Height;

            var width = double.IsNaN(specifiedWidth) ? maxWidth : specifiedWidth;
            var height = double.IsNaN(specifiedHeight) ? maxHeight : specifiedHeight;

            return new Size(width, height);
        }

        /// <summary>
        /// Returns a preferred size of control with an added padding.
        /// </summary>
        protected Size GetChildrenMaxPreferredSizePadded(Size availableSize)
        {
            return GetPaddedPreferredSize(GetChildrenMaxPreferredSize(availableSize));
        }

        /// <summary>
        /// Returns the size of the area which can fit all the children of this
        /// control, with an added padding.
        /// </summary>
        protected Size GetPaddedPreferredSize(Size preferredSize)
        {
            var padding = Control.Padding;

            var intrinsicPadding = new Thickness();
            var nativeControl = NativeControl;
            if (nativeControl != null)
                intrinsicPadding = nativeControl.IntrinsicPreferredSizePadding;

            return preferredSize + padding.Size + intrinsicPadding.Size;
        }

        /// <summary>
        /// Gets the size of the area which can fit all the children of this control.
        /// </summary>
        protected Size GetChildrenMaxPreferredSize(Size availableSize)
        {
            double maxWidth = 0;
            double maxHeight = 0;

            foreach (var control in AllChildrenIncludedInLayout)
            {
                var preferredSize =
                    control.GetPreferredSize(availableSize) + control.Margin.Size;
                maxWidth = Math.Max(preferredSize.Width, maxWidth);
                maxHeight = Math.Max(preferredSize.Height, maxHeight);
            }

            return new Size(maxWidth, maxHeight);
        }

        /// <summary>
        /// This methods is called when the layout of the control changes.
        /// </summary>
        protected virtual void OnLayoutChanged()
        {
        }

        /// <summary>
        /// Initiates invocation of <see cref="OnLayoutChanged"/> for this and
        /// all parent controls.
        /// </summary>
        public void RaiseLayoutChanged()
        {
            var control = Control;
            while (control != null)
            {
                control.Handler.OnLayoutChanged();
                control = control.Parent;
            }
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
            ApplyToolTip();
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
            Control.ToolTipChanged += Control_ToolTipChanged;

            Control.Children.ItemInserted += Children_ItemInserted;
            VisualChildren.ItemInserted += Children_ItemInserted;

            Control.Children.ItemRemoved += Children_ItemRemoved;
            VisualChildren.ItemRemoved += Children_ItemRemoved;
        }

        private void Control_ToolTipChanged(object? sender, EventArgs e)
        {
            ApplyToolTip();
        }

        private void ApplyToolTip()
        {
            if (NativeControl != null)
                NativeControl.ToolTip = Control.ToolTip;
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
            Control.ToolTipChanged -= Control_ToolTipChanged;

            Control.Children.ItemInserted -= Children_ItemInserted;
            Control.Children.ItemRemoved -= Children_ItemRemoved;

            VisualChildren.Clear();

            if (NativeControl != null)
            {
                NativeControl.Paint -= NativeControl_Paint;
                NativeControl.VisibleChanged -= NativeControl_VisibleChanged;
                NativeControl.MouseEnter -= NativeControl_MouseEnter;
                NativeControl.MouseLeave -= NativeControl_MouseLeave;
                NativeControl.MouseCaptureLost -= NativeControl_MouseCaptureLost;
                NativeControl.DragOver -= NativeControl_DragOver;
                NativeControl.DragEnter -= NativeControl_DragEnter;
                NativeControl.DragLeave -= NativeControl_DragLeave;
                NativeControl.DragDrop -= NativeControl_DragDrop;
                NativeControl.GotFocus -= NativeControl_GotFocus;
                NativeControl.LostFocus -= NativeControl_LostFocus;
            }
        }

        private protected virtual void OnNativeControlCreated()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            var parent = Control.Parent;
            if (parent != null)
            {
                parent.Handler.TryInsertNativeControl(Control);
                parent.PerformLayout();
            }

            NativeControl.Paint += NativeControl_Paint;
            NativeControl.VisibleChanged += NativeControl_VisibleChanged;
            NativeControl.MouseEnter += NativeControl_MouseEnter;
            NativeControl.MouseLeave += NativeControl_MouseLeave;
            NativeControl.MouseCaptureLost += NativeControl_MouseCaptureLost;
            NativeControl.DragOver += NativeControl_DragOver;
            NativeControl.DragEnter += NativeControl_DragEnter;
            NativeControl.DragLeave += NativeControl_DragLeave;
            NativeControl.DragDrop += NativeControl_DragDrop;
            NativeControl.GotFocus += NativeControl_GotFocus;
            NativeControl.LostFocus += NativeControl_LostFocus;
        }

        private void NativeControl_GotFocus(object? sender, EventArgs e)
        {
            Control.RaiseEvent(new RoutedEventArgs(UIElement.GotFocusEvent));
        }

        private void NativeControl_LostFocus(object? sender, EventArgs e)
        {
            Control.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
        }

        private void RaiseDragAndDropEvent(
            Native.NativeEventArgs<Native.DragEventData> e,
            Action<DragEventArgs> raiseAction)
        {
            var data = e.Data;
            var ea = new DragEventArgs(
                new UnmanagedDataObjectAdapter(
                    new Native.UnmanagedDataObject(data.data)),
                new Point(data.mouseClientLocationX, data.mouseClientLocationY),
                (DragDropEffects)data.effect);

            raiseAction(ea);

            e.Result = new IntPtr((int)ea.Effect);
        }

        private void NativeControl_DragOver(
            object? sender,
            Native.NativeEventArgs<Native.DragEventData> e) =>
            RaiseDragAndDropEvent(e, ea => Control.RaiseDragOver(ea));

        private void NativeControl_DragEnter(
            object? sender,
            Native.NativeEventArgs<Native.DragEventData> e) =>
            RaiseDragAndDropEvent(e, ea => Control.RaiseDragEnter(ea));

        private void NativeControl_DragDrop(
            object? sender,
            Native.NativeEventArgs<Native.DragEventData> e) =>
            RaiseDragAndDropEvent(e, ea => Control.RaiseDragDrop(ea));

        private void NativeControl_DragLeave(object? sender, EventArgs e) =>
            Control.RaiseDragLeave(e);

        private void NativeControl_MouseCaptureLost(object? sender, EventArgs e)
        {
            Control.RaiseMouseCaptureLost();
        }

        /// <summary>
        /// The ScreenToClient function converts the screen coordinates of a
        /// specified point on the screen to client-area coordinates.
        /// </summary>
        /// <param name="point">A <see cref="Point"/> that specifies the
        /// screen coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public Point ScreenToClient(Point point)
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            return NativeControl.ScreenToClient(point);
        }

        /// <summary>
        /// Converts the client-area coordinates of a specified point to
        /// screen coordinates.
        /// </summary>
        /// <param name="point">A <see cref="Point"/> that contains the
        /// client coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public Point ClientToScreen(Point point)
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            return NativeControl.ClientToScreen(point);
        }

        /// <summary>
        /// Converts the screen coordinates of a specified point in
        /// device-independent units (1/96th inch per unit) to device
        /// (pixel) coordinates.
        /// </summary>
        /// <param name="point">A <see cref="Point"/> that specifies the
        /// screen device-independent coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public Int32Point ScreenToDevice(Point point)
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            return NativeControl.ScreenToDevice(point);
        }

        /// <summary>
        /// Converts the device (pixel) coordinates of a specified point to
        /// coordinates in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="point">A <see cref="Point"/> that contains the
        /// coordinates in device-independent units (1/96th inch per unit)
        /// to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public Point DeviceToScreen(Int32Point point)
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            return NativeControl.DeviceToScreen(point);
        }

        /// <summary>
        /// Called when the mouse cursor enters the boundary of the control.
        /// </summary>
        protected virtual void OnMouseEnter()
        {
        }

        /// <summary>
        /// Called when the mouse cursor moves.
        /// </summary>
        protected virtual void OnMouseMove()
        {
        }

        /// <summary>
        /// Called when the mouse cursor leaves the boundary of the control.
        /// </summary>
        protected virtual void OnMouseLeave()
        {
        }

        /// <summary>
        /// Called when the mouse left button is released while the mouse pointer
        /// is over
        /// the control or when the control has captured the mouse.
        /// </summary>
        protected virtual void OnMouseLeftButtonUp()
        {
        }

        /// <summary>
        /// Called when the mouse left button is pressed while the mouse pointer
        /// is over
        /// the control or when the control has captured the mouse.
        /// </summary>
        protected virtual void OnMouseLeftButtonDown()
        {
        }

        private void RaiseChildInserted(Control childControl)
        {
            Control.RaiseChildInserted(childControl);
            OnChildInserted(childControl);
        }

        private void RaiseChildRemoved(Control childControl)
        {
            Control.RaiseChildRemoved(childControl);
            OnChildRemoved(childControl);
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is inserted into the
        /// <see cref="Control.Children"/> or <see cref="VisualChildren"/> collection.
        /// </summary>
        protected virtual void OnChildInserted(Control childControl)
        {
            TryInsertNativeControl(childControl);
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> or <see cref="VisualChildren"/> collections.
        /// </summary>
        protected virtual void OnChildRemoved(Control childControl)
        {
            TryRemoveNativeControl(childControl);
        }

        private void Control_VerticalAlignmentChanged(object? sender, EventArgs e)
        {
            RaiseLayoutChanged();
            PerformLayout();
        }

        private void Control_HorizontalAlignmentChanged(object? sender, EventArgs e)
        {
            RaiseLayoutChanged();
            PerformLayout();
        }

        private void ApplyChildren()
        {
            if (!Control.HasChildren)
                return;
            for (var i = 0; i < Control.Children.Count; i++)
                RaiseChildInserted(Control.Children[i]);
        }

        private void VisualChildren_ItemInserted(
            object? sender,
            CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = Control;
            e.Item.Handler.IsVisualChild = true;

            RaiseChildInserted(e.Item);
            PerformLayout();
        }

        private void VisualChildren_ItemRemoved(
            object? sender,
            CollectionChangeEventArgs<Control> e)
        {
            e.Item.Parent = null;
            e.Item.Handler.IsVisualChild = false;

            RaiseChildRemoved(e.Item);
            PerformLayout();
        }

        private void DisposeNativeControl()
        {
            if (nativeControl != null)
            {
                if (nativeControl.HasWindowCreated)
                {
                    nativeControl.Destroyed += NativeControl_Destroyed;
                    nativeControl.Destroy();
                }
                else
                    DisposeNativeControlCore(nativeControl);

                nativeControl = null;
            }
        }

        private void NativeControl_Destroyed(object? sender, EventArgs e)
        {
            var nativeControl = (Native.Control)sender!;
            nativeControl.Destroyed -= NativeControl_Destroyed;
            DisposeNativeControlCore(nativeControl);
        }

        private static void DisposeNativeControlCore(Native.Control nativeControl)
        {
            //handlersByNativeControls.Remove(nativeControl);
            nativeControl.handler = null;
            nativeControl.Dispose();
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

        /// <summary>
        /// Gets or set a value indicating whether the control paints itself rather than the operating system doing so.
        /// </summary>
        /// <value>If <c>true</c>, the control paints itself rather than the operating system doing so.
        /// If <c>false</c>, the <see cref="Control.Paint"/> event is not raised.</value>
        public bool UserPaint
        {
            get => NativeControl!.UserPaint;
            set => NativeControl!.UserPaint = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can accept data that the user drags onto it.
        /// </summary>
        /// <value><c>true</c> if drag-and-drop operations are allowed in the control; otherwise, <c>false</c>. The default is <c>false</c>.</value>
        public bool AllowDrop
        {
            get => NativeControl!.AllowDrop;
            set => NativeControl!.AllowDrop = value;
        }

        /// <summary>
        /// Begins a drag-and-drop operation.
        /// </summary>
        /// <remarks>
        /// Begins a drag operation. The <paramref name="allowedEffects"/> determine which drag operations can occur.
        /// </remarks>
        /// <param name="data">The data to drag.</param>
        /// <param name="allowedEffects">One of the <see cref="DragDropEffects"/> values.</param>
        /// <returns>
        /// A value from the <see cref="DragDropEffects"/> enumeration that represents the final effect that was
        /// performed during the drag-and-drop operation.
        /// </returns>
        public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return (DragDropEffects)NativeControl!.DoDragDrop(
                UnmanagedDataObjectService.GetUnmanagedDataObject(data),
                (Native.DragDropEffects)allowedEffects);
        }

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
            Invalidate();
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
            Invalidate();
        }

        private void ApplyFont()
        {
            if (NativeControl != null)
                NativeControl.Font = Control.Font?.NativeFont;

            Invalidate();
        }

        /// <summary>
        /// Starts the initialization process for this control.
        /// </summary>
        protected internal virtual void BeginInit()
        {
            NativeControl?.BeginInit();
        }

        /// <summary>
        /// Ends the initialization process for this control.
        /// </summary>
        protected internal virtual void EndInit()
        {
            NativeControl?.EndInit();
        }

        private void ApplyBorderColor()
        {
            // if (NativeControl != null)
            //    NativeControl.BorderColor = GetBrushColor(Control.BorderBrush);
            Invalidate();
        }

        private void TryInsertNativeControl(Control childControl)
        {
            // todo: use index
            var childNativeControl = childControl.Handler.NativeControl;
            if (childNativeControl == null)
                return;

            if (childNativeControl.ParentRefCounted != null)
                return;

            var parentNativeControl = NativeControl;
            parentNativeControl ??=
                TryFindClosestParentWithNativeControl()?.Handler.NativeControl;

            parentNativeControl?.AddChild(childNativeControl);
        }

        private void TryRemoveNativeControl(Control childControl)
        {
            if (nativeControl != null && childControl.Handler.nativeControl != null)
                nativeControl?.RemoveChild(childControl.Handler.nativeControl);
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        /// <returns><see langword="true"/> if the input focus request was successful; otherwise, <see langword="false"/>.</returns>
        /// <remarks>The <see cref="SetFocus"/> method returns true if the control successfully received input focus.</remarks>
        public bool SetFocus()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            return NativeControl.SetFocus();
        }

        /// <summary>
        /// Focuses the next control.
        /// </summary>
        public void FocusNextControl(bool forward, bool nested)
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.FocusNextControl(forward, nested);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.
        /// </summary>
        public virtual bool TabStop
        {
            get
            {
                if (NativeControl == null)
                    throw new InvalidOperationException();

                return NativeControl.TabStop;
            }

            set
            {
                if (NativeControl == null)
                    throw new InvalidOperationException();

                NativeControl.TabStop = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control has input focus.
        /// </summary>
        public bool IsFocused
        {
            get
            {
                if (NativeControl == null)
                    throw new InvalidOperationException();

                return NativeControl.IsFocused;
            }
        }

        /*
        private static Dictionary<Native.Control, ControlHandler>
            handlersByNativeControls = new();
        */

        internal static ControlHandler? NativeControlToHandler(
            Native.Control control)
        {
            /*
              handlersByNativeControls.TryGetValue(
                  control,
                  out var handler) ? handler : null;
            */
            return (ControlHandler?)control.handler;
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
            Control.RaiseMouseEnter();
        }

        private void NativeControl_MouseLeave(object? sender, EventArgs? e)
        {
            Control.RaiseMouseLeave();
        }

        private void Control_MarginChanged(object? sender, EventArgs? e)
        {
            PerformLayout();
        }

        private void Control_PaddingChanged(object? sender, EventArgs? e)
        {
            PerformLayout();
        }

        private void Children_ItemInserted(
            object? sender, CollectionChangeEventArgs<Control> e)
        {
            RaiseChildInserted(e.Item);
            RaiseLayoutChanged();
            PerformLayout();
        }

        private void Children_ItemRemoved(
            object? sender, CollectionChangeEventArgs<Control> e)
        {
            RaiseChildRemoved(e.Item);
            RaiseLayoutChanged();
            PerformLayout();
        }

        private void NativeControl_Paint(object? sender, System.EventArgs? e)
        {
            if (NativeControl == null)
                return;

            bool hasVisualChildren = HasVisualChildren;

            bool paint = hasVisualChildren || Control.UserPaint || NeedsPaint;
            if (!paint)
                return;

            using var dc =
                new DrawingContext(NativeControl.OpenPaintDrawingContext());

            if (Control.UserPaint)
                Control.RaisePaint(new PaintEventArgs(dc, ClientRectangle));

            if (NeedsPaint || hasVisualChildren)
                PaintSelfAndVisualChildren(dc);
        }

        private void PaintSelfAndVisualChildren(DrawingContext dc)
        {
            if (NeedsPaint)
                OnPaint(dc);

            if (!HasVisualChildren)
                return;
            foreach (var visualChild in VisualChildren)
            {
                var location = visualChild.Handler.Bounds.Location;
                dc.PushTransform(TransformMatrix.CreateTranslation(
                    location.X,
                    location.Y));
                visualChild.Handler.PaintSelfAndVisualChildren(dc);
                dc.Pop();
            }
        }

        internal void SaveScreenshot(string fileName)
        {
            Control.ScreenShotCounter++;
            try
            {
                if (NativeControl == null)
                    throw new InvalidOperationException();

                NativeControl.SaveScreenshot(fileName);
            }
            finally
            {
                Control.ScreenShotCounter--;
            }
        }

        /// <summary>
        /// Returns the currently focused control, or <see langword="null"/> if no control is focused.
        /// </summary>
        public static Control? GetFocusedControl()
        {
            var focusedNativeControl = Native.Control.GetFocusedControl();
            if (focusedNativeControl == null)
                return null;

            var handler = NativeControlToHandler(focusedNativeControl);
            if (handler == null)
                return null;

            return handler.Control;
        }

        /// <inheritdoc cref="Control.BeginIgnoreRecreate"/>
        public void BeginIgnoreRecreate()
        {
            NativeControl?.BeginIgnoreRecreate();
        }

        /// <inheritdoc cref="Control.EndIgnoreRecreate"/>
        public void EndIgnoreRecreate()
        {
            NativeControl?.EndIgnoreRecreate();
        }
    }
}