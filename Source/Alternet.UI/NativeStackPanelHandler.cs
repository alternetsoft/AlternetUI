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

            if (orientation == StackPanelOrientation.Vertical)
            {
                float y = 0;
                foreach (var control in Control.Controls)
                {
                    var preferredSize = control.GetPreferredSize(new SizeF(size.Width, size.Height - y));
                    control.Handler.Bounds = new RectangleF(0, y, size.Width, preferredSize.Height);
                    y += preferredSize.Height;
                }
            }
            else if (orientation == StackPanelOrientation.Horizontal)
            {
                float x = 0;
                foreach (var control in Control.Controls)
                {
                    var preferredSize = control.GetPreferredSize(new SizeF(size.Width - x, size.Height));
                    control.Handler.Bounds = new RectangleF(x, 0, preferredSize.Width, size.Height);
                    x += preferredSize.Width;
                }
            }
            else
                throw new InvalidOperationException();
        }

        public override SizeF GetPreferredSize(SizeF availableSize)
        {
            var orientation = Control.Orientation;
            if (orientation == StackPanelOrientation.Vertical)
            {
                float maxWidth = 0;
                float height = 0;
                foreach (var control in Control.Controls)
                {
                    var preferredSize = control.GetPreferredSize(new SizeF(availableSize.Width, availableSize.Height - height));
                    maxWidth = Math.Max(maxWidth, preferredSize.Width);
                    height += preferredSize.Height;
                }

                return new SizeF(float.IsNaN(Control.Width) ? maxWidth : Control.Width, height);
            }
            else if (orientation == StackPanelOrientation.Horizontal)
            {
                float width = 0;
                float maxHeight = 0;
                foreach (var control in Control.Controls)
                {
                    var preferredSize = control.GetPreferredSize(new SizeF(availableSize.Width - width, availableSize.Height));
                    width += preferredSize.Width;
                    maxHeight = Math.Max(maxHeight, preferredSize.Height);
                }

                return new SizeF(width, float.IsNaN(Control.Height) ? maxHeight : Control.Height);
            }
            else
                throw new InvalidOperationException();
        }
    }
}