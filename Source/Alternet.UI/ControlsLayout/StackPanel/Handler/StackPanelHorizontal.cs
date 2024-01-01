using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler
    {
        private class HorizontalLayout : OrientedLayout
        {
            public HorizontalLayout(StackPanelHandler handler)
                : base(handler)
            {
            }

            public override SizeD GetPreferredSize(SizeD availableSize)
            {
                var stackPanelPadding = Control.Padding;

                double width = 0;
                double maxHeight = 0;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var preferredSize = control.GetPreferredSizeLimited(
                        new SizeD(availableSize.Width - width, availableSize.Height));
                    width += preferredSize.Width + margin.Horizontal;
                    maxHeight = Math.Max(maxHeight, preferredSize.Height + margin.Vertical);
                }

                var isNan = double.IsNaN(Control.SuggestedHeight);

                return new SizeD(
                    width + stackPanelPadding.Horizontal,
                    isNan ? maxHeight + stackPanelPadding.Vertical : Control.SuggestedHeight);
            }

            public override void Layout()
            {
                var childrenLayoutBounds = Handler.Control.ChildrenLayoutBounds;

                double x = 0;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var horizontalMargin = margin.Horizontal;

                    var preferredSize = control.GetPreferredSizeLimited(
                        new SizeD(
                            childrenLayoutBounds.Width - x - horizontalMargin,
                            childrenLayoutBounds.Height));
                    var alignedPosition =
                        AlignedLayout.AlignVertical(childrenLayoutBounds, control, preferredSize);
                    control.Handler.Bounds =
                        new RectD(
                            childrenLayoutBounds.Left + x + margin.Left,
                            alignedPosition.Origin,
                            preferredSize.Width,
                            alignedPosition.Size);
                    x += preferredSize.Width + horizontalMargin;
                }
            }
        }
    }
}