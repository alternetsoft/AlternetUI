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
                    var alignedPosition = AlignedLayout.AlignHorizontal(childrenLayoutBounds, control, preferredSize);
                    control.Handler.Bounds = new RectangleF(alignedPosition.Origin, y + margin.Top, alignedPosition.Size, preferredSize.Height);
                    y += preferredSize.Height + verticalMargin;
                }
            }

        }
    }
}