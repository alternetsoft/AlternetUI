using System.Drawing;

namespace Alternet.UI
{
    internal class NativeControlHandler : ControlHandler
    {
        public override RectangleF Bounds
        {
            get => NativeControl!.Bounds;
            set
            {
                NativeControl!.Bounds = value;
                PerformLayout(); // todo: use event
            }
        }

        internal override bool NeedToCreateNativeControl()
        {
            return NativeControl == null;
        }
    }
}