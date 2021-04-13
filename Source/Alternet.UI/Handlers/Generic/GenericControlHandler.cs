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
            }
        }

        internal override bool NeedToCreateNativeControl()
        {
            // todo: if parent already has handle, we dont needed it.
            return NativeControl != null;
        }
    }
}