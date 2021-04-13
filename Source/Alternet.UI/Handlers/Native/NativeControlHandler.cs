using System.Drawing;

namespace Alternet.UI
{
    internal class NativeControlHandler : ControlHandler
    {
        public NativeControlHandler(Control control) : base(control)
        {
        }

        public override RectangleF Bounds
        {
            get => NativeControl!.Bounds;
            set
            {
                NativeControl!.Bounds = value;
                PerformLayout(); // todo: use event
            }
        }

        private protected override bool NeedToCreateNativeControl() => NativeControl == null;
    }
}