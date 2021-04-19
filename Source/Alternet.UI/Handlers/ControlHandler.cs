using System;
using System.Drawing;

namespace Alternet.UI
{
    public abstract class ControlHandler
    {
        private int layoutSuspendCount;

        private bool inLayout;

        private Control? control;

        private RectangleF bounds;

        public Control Control
        {
            get => control ?? throw new InvalidOperationException();
        }

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
                return new RectangleF(new PointF(padding.Left, padding.Top), Bounds.Size - padding.Size);
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

        internal Native.Control? NativeControl { get; private set; }

        protected virtual bool NeedsPaint => false;

        // todo: for non-visual child, if native control is not created, create it on demand.
        private bool IsLayoutSuspended => layoutSuspendCount != 0;

        public void Attach(Control control)
        {
            this.control = control;
            TryCreateNativeControl();
            OnAttach();
        }

        public void Detach()
        {
            OnDetach();
            control = null;
        }

        public void Update()
        {
            NativeControl?.Update();
        }

        public virtual void OnPaint(DrawingContext drawingContext)
        {
        }

        public virtual void OnLayout()
        {
            var childrenLayoutBounds = ChildrenLayoutBounds;
            foreach (var control in Control.AllChildren)
            {
                var margin = control.Margin;
                control.Handler.Bounds = new RectangleF(childrenLayoutBounds.Location + new SizeF(margin.Left, margin.Top), childrenLayoutBounds.Size - margin.Size);
            }
        }

        public virtual SizeF GetPreferredSize(SizeF availableSize)
        {
            if (Control.Children.Count == 0 && Control.VisualChildren.Count == 0)
            {
                var s = NativeControl?.GetPreferredSize(availableSize) ?? new SizeF();
                return new SizeF(
                    float.IsNaN(Control.Width) ? s.Width : Control.Width,
                    float.IsNaN(Control.Height) ? s.Height : Control.Height);
            }
            else
            {
                return GexChildrenMaxPreferredSize(availableSize);
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

        internal virtual Native.Control CreateNativeControl() => new Native.Panel();

        protected virtual bool NeedToCreateNativeControl()
        {
            return NativeControl == null;
        }

        protected SizeF GexChildrenMaxPreferredSize(SizeF availableSize)
        {
            float maxWidth = 0;
            float maxHeight = 0;

            foreach (var control in Control.AllChildren)
            {
                var preferredSize = control.GetPreferredSize(availableSize) + control.Margin.Size;
                maxWidth = Math.Max(preferredSize.Width, maxWidth);
                maxHeight = Math.Max(preferredSize.Height, maxHeight);
            }

            return new SizeF(maxWidth, maxHeight) + Control.Padding.Size;
        }

        protected virtual void OnAttach()
        {
            Control.MarginChanged += Control_MarginChanged;
            Control.PaddingChanged += Control_PaddingChanged;

            Control.Children.ItemInserted += Children_ItemInserted;
            Control.VisualChildren.ItemInserted += Children_ItemInserted;

            Control.Children.ItemRemoved += Children_ItemRemoved;
            Control.VisualChildren.ItemRemoved += Children_ItemRemoved;
        }

        protected virtual void OnDetach()
        {
            Control.MarginChanged -= Control_MarginChanged;
            Control.PaddingChanged -= Control_PaddingChanged;

            Control.Children.ItemInserted -= Children_ItemInserted;
            Control.VisualChildren.ItemInserted -= Children_ItemInserted;

            Control.Children.ItemRemoved -= Children_ItemRemoved;
            Control.VisualChildren.ItemRemoved -= Children_ItemRemoved;

            if (NativeControl != null)
            {
                NativeControl.Paint -= NativeControl_Paint;
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

            NativeControl.Paint += NativeControl_Paint;
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

        protected virtual void OnChildInserted(int index, Control control)
        {
            if (NativeControl != null && control.Handler.NativeControl != null)
                NativeControl?.AddChild(control.Handler.NativeControl);
        }

        protected virtual void OnChildRemoved(int index, Control control)
        {
            if (NativeControl != null && control.Handler.NativeControl != null)
                NativeControl?.RemoveChild(control.Handler.NativeControl);
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
                var parent = handler.Control.Parent;
                if (parent == null)
                    break;
                handler = parent.Handler;
            }
        }

        private void TryCreateNativeControl()
        {
            if (NeedToCreateNativeControl())
            {
                NativeControl = CreateNativeControl();
                OnNativeControlCreated();
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

            if (Control.UserPaint)
            {
                using (var dc = NativeControl.OpenPaintDrawingContext())
                    Control.InvokePaint(new PaintEventArgs(new DrawingContext(dc), Bounds));
            }
            else if (NeedsPaint)
            {
                using (var dc = NativeControl.OpenPaintDrawingContext())
                    OnPaint(new DrawingContext(dc));
            }
        }
    }
}