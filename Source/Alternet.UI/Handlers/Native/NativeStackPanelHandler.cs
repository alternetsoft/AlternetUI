using System;
using System.Drawing;

namespace Alternet.UI
{
    internal class NativeStackPanelHandler : NativeControlHandler<StackPanel, Native.Panel>
    {
        public override void OnLayout()
        {
            var size = Bounds.Size;
            var orientation = Control.Orientation;

            if (orientation == StackPanelOrientation.Vertical)
            {
                float y = 0;
                foreach (var control in Control.Controls)
                {
                    var margin = control.Margin;
                    var verticalMargin = margin.Vertical;

                    var preferredSize = control.GetPreferredSize(new SizeF(size.Width, size.Height - y - verticalMargin));
                    control.Handler.Bounds = new RectangleF(0, y + margin.Top, size.Width, preferredSize.Height);
                    y += preferredSize.Height + verticalMargin;
                }
            }
            else if (orientation == StackPanelOrientation.Horizontal)
            {
                float x = 0;
                foreach (var control in Control.Controls)
                {
                    var margin = control.Margin;
                    var horizontalMargin = margin.Horizontal;

                    var preferredSize = control.GetPreferredSize(new SizeF(size.Width - x - horizontalMargin, size.Height));
                    control.Handler.Bounds = new RectangleF(x + margin.Left, 0, preferredSize.Width, size.Height);
                    x += preferredSize.Width + horizontalMargin;
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