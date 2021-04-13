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
            foreach (var control in Control.Controls)
            {
                var margin = control.Margin;
                control.Handler.Bounds = new RectangleF(new PointF(margin.Left, margin.Top), Bounds.Size - margin.Size);
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
            Control.Controls.ItemInserted += Controls_ItemInserted;
            Control.Controls.ItemRemoved += Controls_ItemRemoved;
        }

        protected virtual void OnDetach()
        {
            Control.MarginChanged -= Control_MarginChanged;
            Control.Controls.ItemInserted -= Controls_ItemInserted;
            Control.Controls.ItemRemoved -= Controls_ItemRemoved;

            if (NativeControl != null)
                NativeControl.Paint -= NativeControl_Paint;
        }

        private protected virtual void OnNativeControlCreated()
        {
            if (NativeControl == null)
                throw new InvalidOperationException();

            NativeControl.Paint += NativeControl_Paint;
        }

        protected virtual void OnControlInserted(int index, Control control)
        {
            if (NativeControl != null && control.Handler.NativeControl != null)
                NativeControl?.AddChild(control.Handler.NativeControl);
        }

        protected virtual void OnControlRemoved(int index, Control control)
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

        private void Controls_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            OnControlInserted(e.Index, e.Item);
            PerformLayout();
        }

        private void Controls_ItemRemoved(object? sender, CollectionChangeEventArgs<Control> e)
        {
            OnControlRemoved(e.Index, e.Item);
            PerformLayout();
        }

        private void NativeControl_Paint(object? sender, System.EventArgs? e)
        {
            if (Control.UserPaint)
            {
                if (NativeControl == null)
                    throw new InvalidOperationException();

                using (var dc = NativeControl.OpenPaintDrawingContext())
                    Control.InvokePaint(new PaintEventArgs(new DrawingContext(dc), Bounds));
            }
        }
    }
}