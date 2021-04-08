using System.Drawing;

namespace Alternet.UI
{
    internal class NativeControlHandler : ControlHandler
    {
        public NativeControlHandler(Control control) : base(control)
        {
            NativeControl = CreateNativeControl();
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
            return NativeControl.GetPreferredSize(availableSize);
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