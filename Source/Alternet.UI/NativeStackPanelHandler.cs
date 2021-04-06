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
            float y = 0;
            foreach (var control in Control.Controls)
            {
                var preferredSize = control.GetPreferredSize(size);
                control.Handler.Bounds = new RectangleF(0, y, size.Width, preferredSize.Height);
                y += preferredSize.Height;
            }
        }
    }
}