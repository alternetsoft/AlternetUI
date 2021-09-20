using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler
    {
        class VerticalLayout : OrientedLayout
        {
            public VerticalLayout(StackPanelHandler handler) : base(handler)
            {
            }

            public override SizeF GetPreferredSize(SizeF availableSize)
            {
                var stackPanelPadding = Control.Padding;

                float maxWidth = 0;
                float height = 0;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var preferredSize = control.GetPreferredSize(new SizeF(availableSize.Width, availableSize.Height - height));
                    maxWidth = Math.Max(maxWidth, preferredSize.Width + margin.Horizontal);
                    height += preferredSize.Height + margin.Vertical;
                }

                return new SizeF(float.IsNaN(Control.Width) ? maxWidth + stackPanelPadding.Horizontal : Control.Width, height + stackPanelPadding.Vertical);
            }

            public override void Layout()
            {
                var childrenLayoutBounds = Handler.ChildrenLayoutBounds;

                float y = childrenLayoutBounds.Top;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var verticalMargin = margin.Vertical;

                    var preferredSize = control.GetPreferredSize(new SizeF(childrenLayoutBounds.Width, childrenLayoutBounds.Height - y - verticalMargin));
                    var alignedPosition = AlignChild(childrenLayoutBounds, control, preferredSize);
                    control.Handler.Bounds = new RectangleF(alignedPosition.Origin, y + margin.Top, alignedPosition.Size, preferredSize.Height);
                    y += preferredSize.Height + verticalMargin;
                }
            }

            private AlignedPosition AlignChild(RectangleF layoutBounds, Control childControl, SizeF childPreferredSize)
            {
                switch (childControl.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        return new AlignedPosition(layoutBounds.Left + childControl.Margin.Left, childPreferredSize.Width);
                    case HorizontalAlignment.Center:
                        return new AlignedPosition(
                            layoutBounds.Left + (layoutBounds.Width - (childPreferredSize.Width + childControl.Margin.Horizontal)) / 2 + childControl.Margin.Left,
                            childPreferredSize.Width);
                    case HorizontalAlignment.Right:
                        return new AlignedPosition(layoutBounds.Right - childControl.Margin.Horizontal - childPreferredSize.Width, childPreferredSize.Width);
                    case HorizontalAlignment.Stretch:
                        return new AlignedPosition(layoutBounds.Left + childControl.Margin.Left, layoutBounds.Width - childControl.Margin.Horizontal);
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}