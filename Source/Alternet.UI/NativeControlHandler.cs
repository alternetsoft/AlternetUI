using System.Drawing;

namespace Alternet.UI
{
    internal abstract class NativeControlHandler : ControlHandler
    {
        protected NativeControlHandler(Control control) : base(control)
        {
            NativeControl = CreateNativeControl();
        }

        public Native.Control NativeControl { get; }

        protected abstract Native.Control CreateNativeControl();

        protected override void OnControlInserted(int index, Control control)
        {
            NativeControl.AddChild(((NativeControlHandler)control.Handler).NativeControl);
        }

        protected override void OnControlRemoved(int index, Control control)
        {
            NativeControl.RemoveChild(((NativeControlHandler)control.Handler).NativeControl);
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