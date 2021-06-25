using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Alternet.UI
{
    public abstract class ControlHandler
    {
        private int layoutSuspendCount;

        private bool inLayout;

        private Control? control;

        private RectangleF bounds;

        private Native.Control? nativeControl;
        private bool isVisualChild;

        public ControlHandler()
        {
            VisualChildren.ItemInserted += VisualChildren_ItemInserted;
            VisualChildren.ItemRemoved += VisualChildren_ItemRemoved;
        }

        public Control Control
        {
            get => control ?? throw new InvalidOperationException();
        }

        public bool IsAttached => control != null;

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

        public virtual RectangleF ChildrenLayoutBounds
        {
            get
            {
                if (Bounds.IsEmpty)
                    return RectangleF.Empty;

                var padding = Control.Padding;

                var intrinsicPadding = new Thickness();
                var nativeControl = NativeControl;
                if (nativeControl != null)
                    intrinsicPadding = nativeControl.IntrinsicLayoutPadding;

                return new RectangleF(
                    new PointF(padding.Left + intrinsicPadding.Left, padding.Top + intrinsicPadding.Top),
                    Bounds.Size - padding.Size - intrinsicPadding.Size);
            }
        }

        public virtual RectangleF ChildrenBounds => new RectangleF(new PointF(), Bounds.Size);

        public bool IsMouseOver
        {
            get
            {
                if (NativeControl == null)
                    return false;

                return NativeControl.IsMouseOver;
            }
        }

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

        public Collection<Control> VisualChildren { get; } = new Collection<Control>();

        public IEnumerable<Control> AllChildren => VisualChildren.Concat(Control.Children);

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

        protected virtual bool NeedsPaint => false;

        protected virtual bool VisualChildNeedsNativeControl => false;

        // todo: for non-visual child, if native control is not created, create it on demand.
        private bool IsLayoutSuspended => layoutSuspendCount != 0;

        public void Attach(Control control)
        {
            this.control = control;
            OnAttach();
        }

        public void Detach()
        {
            OnDetach();

            DisposeNativeControl();
            control = null;
        }

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

        public virtual void OnPaint(DrawingContext drawingContext)
        {
        }

        public virtual void OnLayout()
        {
            var childrenLayoutBounds = ChildrenLayoutBounds;
            foreach (var control in AllChildren)
            {
                var margin = control.Margin;

                var specifiedWidth = control.Width;
                var specifiedHeight = control.Height;

                control.Handler.Bounds = new RectangleF(
                    childrenLayoutBounds.Location + new SizeF(margin.Left, margin.Top),
                    new SizeF(
                        float.IsNaN(specifiedWidth) ? childrenLayoutBounds.Width - margin.Horizontal : specifiedWidth,
                        float.IsNaN(specifiedHeight) ? childrenLayoutBounds.Height - margin.Vertical : specifiedHeight));
            }
        }

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

        public void SuspendLayout()
        {
            layoutSuspendCount++;
        }

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
                if (parent != null)
                    parent.PerformLayout();

                OnLayout();
            }
            finally
            {
                inLayout = false;
            }
        }

        public void CaptureMouse()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.SetMouseCapture(true);
        }

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

        protected virtual void OnIsVisualChildChanged()
        {
            if (!NeedsNativeControl())
                DisposeNativeControl();
        }

        protected virtual bool NeedsNativeControl()
        {
            if (IsVisualChild)
                return VisualChildNeedsNativeControl;

            return true;
        }

        protected SizeF GetChildrenMaxPreferredSize(SizeF availableSize)
        {
            var specifiedWidth = Control.Width;
            var specifiedHeight = Control.Height;
            if (!float.IsNaN(specifiedWidth) && !float.IsNaN(specifiedHeight))
                return new SizeF(specifiedWidth, specifiedHeight);

            float maxWidth = 0;
            float maxHeight = 0;

            foreach (var control in AllChildren)
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

        protected virtual void OnAttach()
        {
            ApplyVisible();
            ApplyBorderColor();
            ApplyBackgroundColor();
            ApplyForegroundColor();
            ApplyChildren();

            Control.MarginChanged += Control_MarginChanged;
            Control.PaddingChanged += Control_PaddingChanged;
            Control.BackgroundColorChanged += Control_BackgroundColorChanged;
            Control.ForegroundColorChanged += Control_ForegroundColorChanged;
            Control.BorderColorChanged += Control_BorderColorChanged;
            Control.VisibleChanged += Control_VisibleChanged;

            Control.Children.ItemInserted += Children_ItemInserted;
            VisualChildren.ItemInserted += Children_ItemInserted;

            Control.Children.ItemRemoved += Children_ItemRemoved;
            VisualChildren.ItemRemoved += Children_ItemRemoved;
        }

        private void ApplyChildren()
        {
            for (var i = 0; i < Control.Children.Count; i++)
                OnChildInserted(i, Control.Children[i]);
        }

        protected virtual void OnDetach()
        {
            // todo: consider clearing the native control's children.
            Control.MarginChanged -= Control_MarginChanged;
            Control.PaddingChanged -= Control_PaddingChanged;
            Control.BackgroundColorChanged -= Control_BackgroundColorChanged;
            Control.ForegroundColorChanged -= Control_ForegroundColorChanged;
            Control.BorderColorChanged -= Control_BorderColorChanged;
            Control.VisibleChanged -= Control_VisibleChanged;

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

        protected virtual void OnMouseMove()
        {
        }

        protected virtual void OnMouseEnter()
        {
        }

        protected virtual void OnMouseLeave()
        {
        }

        protected virtual void OnMouseLeftButtonUp()
        {
        }

        protected virtual void OnMouseLeftButtonDown()
        {
        }

        protected virtual void OnChildInserted(int childIndex, Control childControl)
        {
            TryInsertNativeControl(childIndex, childControl);
        }

        protected virtual void OnChildRemoved(int childIndex, Control childControl)
        {
            TryRemoveNativeControl(childIndex, childControl);
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

        private void Control_BorderColorChanged(object? sender, EventArgs? e)
        {
            ApplyBorderColor();
        }

        private void Control_BackgroundColorChanged(object? sender, EventArgs? e)
        {
            ApplyBackgroundColor();
        }

        private void Control_ForegroundColorChanged(object? sender, EventArgs? e)
        {
            ApplyForegroundColor();
        }

        private void Control_VisibleChanged(object? sender, EventArgs e)
        {
            ApplyVisible();
        }

        private void ApplyBackgroundColor()
        {
            if (NativeControl != null)
                NativeControl.BackgroundColor = Control.BackgroundColor ?? Color.Empty;
            Update();
        }

        private void ApplyVisible()
        {
            if (NativeControl != null)
                NativeControl.Visible = Control.Visible;
        }

        private void ApplyForegroundColor()
        {
            if (NativeControl != null)
                NativeControl.ForegroundColor = Control.ForegroundColor ?? Color.Empty;
            Update();
        }

        private void ApplyBorderColor()
        {
            //if (NativeControl != null)
            //    NativeControl.BackgroundColor = Control.BackgroundColor ?? Color.Empty;
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
            if (NativeControl != null && childControl.Handler.NativeControl != null)
                NativeControl?.RemoveChild(childControl.Handler.NativeControl);
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
                Control.Visible = NativeControl.Visible;
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
                    Control.InvokePaint(new PaintEventArgs(dc, Bounds));
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