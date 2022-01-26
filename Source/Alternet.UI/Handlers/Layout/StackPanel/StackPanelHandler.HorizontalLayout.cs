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

            public override Size GetPreferredSize(Size availableSize)
            {
                var stackPanelPadding = Control.Padding;

                float width = 0;
                float maxHeight = 0;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var preferredSize = control.GetPreferredSize(new Size(availableSize.Width - width, availableSize.Height));
                    width += preferredSize.Width + margin.Horizontal;
                    maxHeight = Math.Max(maxHeight, preferredSize.Height + margin.Vertical);
                }

                return new Size(width + stackPanelPadding.Horizontal, float.IsNaN(Control.Height) ? maxHeight + stackPanelPadding.Vertical : Control.Height);
            }

            public override void Layout()
            {
                var childrenLayoutBounds = Handler.ChildrenLayoutBounds;

                float x = 0;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var horizontalMargin = margin.Horizontal;

                    var preferredSize = control.GetPreferredSize(new Size(childrenLayoutBounds.Width - x - horizontalMargin, childrenLayoutBounds.Height));
                    var alignedPosition = AlignedLayout.AlignVertical(childrenLayoutBounds, control, preferredSize);
                    control.Handler.Bounds = new Rect(childrenLayoutBounds.Left + x + margin.Left, alignedPosition.Origin, preferredSize.Width, alignedPosition.Size);
                    x += preferredSize.Width + horizontalMargin;
                }
            }
        }
    }
}