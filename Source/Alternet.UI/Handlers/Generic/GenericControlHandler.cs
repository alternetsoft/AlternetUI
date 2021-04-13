using System.Drawing;

namespace Alternet.UI
{
    internal class GenericControlHandler : ControlHandler
    {
        private RectangleF bounds;

        public override RectangleF Bounds
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

        internal override bool NeedToCreateNativeControl()
        {
            // todo: visual children should not require a handler.
            return NativeControl == null;
        }
    }
}