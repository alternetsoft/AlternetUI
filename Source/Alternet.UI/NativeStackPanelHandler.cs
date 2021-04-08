using System;
using System.Drawing;

namespace Alternet.UI
{
    internal class NativeStackPanelHandler : NativeControlHandler<StackPanel, Native.Panel>
    {
        public NativeStackPanelHandler(StackPanel control) : base(control)
        {
        }

        protected override Native.Control CreateNativeControl()
        {
            return new Native.Panel();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }

        public override void OnLayout()
        {
            var size = Bounds.Size;
            var orientation = Control.Orientation;

            if (orientation == StackPanelOrientation.Verical)
            {
                float y = 0;
                foreach (var control in Control.Controls)
                {
                    var preferredSize = control.GetPreferredSize(size);
                    control.Handler.Bounds = new RectangleF(0, y, size.Width, preferredSize.Height);
                    y += preferredSize.Height;
                }
            }
            else if (orientation == StackPanelOrientation.Horizontal)
            {
                float x = 0;
                foreach (var control in Control.Controls)
                {
                    var preferredSize = control.GetPreferredSize(size);
                    control.Handler.Bounds = new RectangleF(x, 0, preferredSize.Width, size.Height);
                    x += preferredSize.Width;
                }
            }
            else
                throw new InvalidOperationException();
        }
    }
}