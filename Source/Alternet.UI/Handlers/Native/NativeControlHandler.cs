using System.Drawing;

namespace Alternet.UI
{
    internal class NativeControlHandler : ControlHandler
    {
        public NativeControlHandler(Control control) : base(control)
        {
            NativeControl = CreateNativeControl();
            NativeControl.Paint += NativeControl_Paint;
        }

        private void NativeControl_Paint(object? sender, System.EventArgs? e)
        {
            if (Control.UserPaint)
            {
                using (var dc = NativeControl.OpenPaintDrawingContext())
                    Control.InvokePaint(new PaintEventArgs(new DrawingContext(dc), Bounds));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                NativeControl.Paint -= NativeControl_Paint;
            }

            base.Dispose(disposing);
        }

        public Native.Control NativeControl { get; }

        protected virtual Native.Control CreateNativeControl() => new Native.Panel();

        protected override void OnControlInserted(int index, Control control)
        {
            NativeControl.AddChild(((NativeControlHandler)control.Handler).NativeControl);
        }

        protected override void OnControlRemoved(int index, Control control)
        {
            NativeControl.RemoveChild(((NativeControlHandler)control.Handler).NativeControl);
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            var s = NativeControl.GetPreferredSize(availableSize);
            return new SizeF(
                float.IsNaN(Control.Width) ? s.Width : Control.Width,
                float.IsNaN(Control.Height) ? s.Height : Control.Height);
        }

        public override void Update()
        {
            NativeControl.Update();
        }

        public override RectangleF Bounds
        {
            get => NativeControl.Bounds;
            set
            {
                NativeControl.Bounds = value;
                PerformLayout(); // todo: use event
            }
        }
    }
}