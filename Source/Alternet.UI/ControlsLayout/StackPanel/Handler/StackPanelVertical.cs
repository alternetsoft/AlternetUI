using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler
    {
        internal class VerticalLayout : OrientedLayout
        {
            public VerticalLayout(ControlHandler handler)
                : base(handler)
            {
            }

            public override SizeD GetPreferredSize(SizeD availableSize)
            {
                var stackPanelPadding = Control.Padding;

                double maxWidth = 0;
                double height = 0;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var preferredSize = control.GetPreferredSizeLimited(
                        new SizeD(availableSize.Width, availableSize.Height - height));
                    maxWidth = Math.Max(maxWidth, preferredSize.Width + margin.Horizontal);
                    height += preferredSize.Height + margin.Vertical;
                }

                return new SizeD(
                    double.IsNaN(Control.SuggestedWidth) ? maxWidth + stackPanelPadding.Horizontal
                        : Control.SuggestedWidth,
                    height + stackPanelPadding.Vertical);
            }

            public void LayoutOld()
            {
                var childrenLayoutBounds = Handler.Control.ChildrenLayoutBounds;

                double y = childrenLayoutBounds.Top;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var verticalMargin = margin.Vertical;

                    var preferredSize = control.GetPreferredSizeLimited(
                        new SizeD(
                            childrenLayoutBounds.Width,
                            childrenLayoutBounds.Height - y - verticalMargin));
                    var alignedPosition =
                        AlignedLayout.AlignHorizontal(childrenLayoutBounds, control, preferredSize);
                    control.Handler.Bounds =
                        new RectD(
                            alignedPosition.Origin,
                            y + margin.Top,
                            alignedPosition.Size,
                            preferredSize.Height);
                    y += preferredSize.Height + verticalMargin;
                }
            }

            public override void Layout()
            {
                var childrenLayoutBounds = Handler.Control.ChildrenLayoutBounds;

                double y = childrenLayoutBounds.Top;
                foreach (var control in Handler.AllChildrenIncludedInLayout)
                {
                    var margin = control.Margin;
                    var verticalMargin = margin.Vertical;

                    var preferredSize = control.GetPreferredSizeLimited(
                        new SizeD(
                            childrenLayoutBounds.Width,
                            childrenLayoutBounds.Height - y - verticalMargin));
                    var alignedPosition =
                        AlignedLayout.AlignHorizontal(childrenLayoutBounds, control, preferredSize);
                    control.Handler.Bounds =
                        new RectD(
                            alignedPosition.Origin,
                            y + margin.Top,
                            alignedPosition.Size,
                            preferredSize.Height);
                    y += preferredSize.Height + verticalMargin;
                }
            }
        }
    }
}