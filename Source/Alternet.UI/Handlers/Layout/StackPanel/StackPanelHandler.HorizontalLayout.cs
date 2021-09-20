using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler
    {
        class HorizontalLayout : OrientedLayout
        {
            public HorizontalLayout(StackPanelHandler handler) : base(handler)
            {
            }

            public override SizeF GetPreferredSize(SizeF availableSize)
            {
                var stackPanelPadding = Control.Padding;

                float width = 0;
                float maxHeight = 0;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var preferredSize = control.GetPreferredSize(new SizeF(availableSize.Width - width, availableSize.Height));
                    width += preferredSize.Width + margin.Horizontal;
                    maxHeight = Math.Max(maxHeight, preferredSize.Height + margin.Vertical);
                }

                return new SizeF(width + stackPanelPadding.Horizontal, float.IsNaN(Control.Height) ? maxHeight + stackPanelPadding.Vertical : Control.Height);
            }

            public override void Layout()
            {
                var childrenLayoutBounds = Handler.ChildrenLayoutBounds;

                float x = 0;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var horizontalMargin = margin.Horizontal;

                    var preferredSize = control.GetPreferredSize(new SizeF(childrenLayoutBounds.Width - x - horizontalMargin, childrenLayoutBounds.Height));
                    var alignedPosition = AlignChild(childrenLayoutBounds, control, preferredSize);
                    control.Handler.Bounds = new RectangleF(childrenLayoutBounds.Left + x + margin.Left, alignedPosition.Origin, preferredSize.Width, alignedPosition.Size);
                    x += preferredSize.Width + horizontalMargin;
                }
            }

            private AlignedPosition AlignChild(RectangleF layoutBounds, Control childControl, SizeF childPreferredSize)
            {
                switch (childControl.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        return new AlignedPosition(layoutBounds.Top + childControl.Margin.Top, childPreferredSize.Height);
                    case VerticalAlignment.Center:
                        return new AlignedPosition(
                            layoutBounds.Top + (layoutBounds.Height - (childPreferredSize.Height + childControl.Margin.Vertical)) / 2 + childControl.Margin.Top,
                            childPreferredSize.Height);
                    case VerticalAlignment.Bottom:
                        return new AlignedPosition(layoutBounds.Bottom - childControl.Margin.Vertical - childPreferredSize.Height, childPreferredSize.Height);
                    case VerticalAlignment.Stretch:
                        return new AlignedPosition(layoutBounds.Top + childControl.Margin.Top, layoutBounds.Height - childControl.Margin.Vertical);
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}