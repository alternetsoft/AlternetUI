using System;
using System.Drawing;

namespace Alternet.UI
{
    public abstract class ControlHandler
    {
        private int layoutSuspendCount;

        private bool inLayout;

        private Control? control;

        public Control Control
        {
            get => control ?? throw new InvalidOperationException();
        }

        public abstract RectangleF Bounds { get; set; }

        public virtual RectangleF DisplayRectangle => new RectangleF(new PointF(), Bounds.Size);

        internal Native.Control? NativeControl { get; private set; }

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
            var displayRectangle = DisplayRectangle;
            foreach (var control in Control.AllChildren)
            {
                var margin = control.Margin;
                control.Handler.Bounds = new RectangleF(displayRectangle.Location + new SizeF(margin.Left, margin.Top), displayRectangle.Size - margin.Size);
            }
        }

        public virtual SizeF GetPreferredSize(SizeF availableSize)
        {
            var s = NativeControl?.GetPreferredSize(availableSize) ?? new SizeF();
            return new SizeF(
                float.IsNaN(Control.Width) ? s.Width : Control.Width,
                float.IsNaN(Control.Height) ? s.Height : Control.Height);
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

        internal virtual Native.Control CreateNativeControl() => new Native.Panel();

        internal abstract bool NeedToCreateNativeControl();

        protected virtual void OnAttach()
        {
            Control.MarginChanged += Control_MarginChanged;

            Control.Children.ItemInserted += Children_ItemInserted;
            Control.VisualChildren.ItemInserted += Children_ItemInserted;

            Control.Children.ItemRemoved += Children_ItemRemoved;
            Control.VisualChildren.ItemRemoved += Children_ItemRemoved;
        }

        protected virtual void OnDetach()
        {
            Control.MarginChanged -= Control_MarginChanged;
            
            Control.Children.ItemInserted -= Children_ItemInserted;
            Control.VisualChildren.ItemInserted -= Children_ItemInserted;

            Control.Children.ItemRemoved -= Children_ItemRemoved;
            Control.VisualChildren.ItemRemoved -= Children_ItemRemoved;

            if (NativeControl != null)
                NativeControl.Paint -= NativeControl_Paint;
        }

        private protected virtual void OnNativeControlCreated()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.Paint += NativeControl_Paint;
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

        protected virtual bool NeedsPaint => false;

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