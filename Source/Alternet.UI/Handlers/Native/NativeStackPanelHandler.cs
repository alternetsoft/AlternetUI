using System;
using System.Drawing;

namespace Alternet.UI
{
    internal class StackPanelHandler: ControlHandler<StackPanel>
    {
        public override void OnLayout()
        {
            var childrenLayoutBounds = ChildrenLayoutBounds;

            var orientation = Control.Orientation;

            if (orientation == StackPanelOrientation.Vertical)
            {
                float y = childrenLayoutBounds.Top;
                foreach (var control in Control.AllChildren)
                {
                    var margin = control.Margin;
                    var verticalMargin = margin.Vertical;

                    var preferredSize = control.GetPreferredSize(new SizeF(childrenLayoutBounds.Width, childrenLayoutBounds.Height - y - verticalMargin));
                    control.Handler.Bounds = new RectangleF(childrenLayoutBounds.Left, y + margin.Top, childrenLayoutBounds.Width, preferredSize.Height);
                    y += preferredSize.Height + verticalMargin;
                }
            }
            else if (orientation == StackPanelOrientation.Horizontal)
            {
                float x = 0;
                foreach (var control in Control.AllChildren)
                {
                    var margin = control.Margin;
                    var horizontalMargin = margin.Horizontal;

                    var preferredSize = control.GetPreferredSize(new SizeF(childrenLayoutBounds.Width - x - horizontalMargin, childrenLayoutBounds.Height));
                    control.Handler.Bounds = new RectangleF(childrenLayoutBounds.Left + x + margin.Left, childrenLayoutBounds.Top, preferredSize.Width, childrenLayoutBounds.Height);
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
                foreach (var control in Control.AllChildren)
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
                foreach (var control in Control.AllChildren)
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