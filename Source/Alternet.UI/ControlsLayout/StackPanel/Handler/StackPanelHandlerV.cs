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
                foreach (var control in Control.AllChildrenInLayout)
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

            /*public void LayoutOld()
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
            }*/

            public override void Layout()
            {
                var lBounds = Control.ChildrenLayoutBounds;
                var items = Control.AllChildrenInLayout;

                double stretchedSize = 0;

                if (AllowStretch && items.Count > 0)
                {
                    foreach (var control in items)
                    {
                        if (control.VerticalAlignment == VerticalAlignment.Stretch)
                            continue;

                        var verticalMargin = control.Margin.Vertical;
                        var freeSize = new SizeD(
                                lBounds.Width,
                                lBounds.Height - stretchedSize - verticalMargin);
                        var preferredSize = control.GetPreferredSizeLimited(freeSize);
                        stretchedSize += preferredSize.Height + verticalMargin;
                    }
                }

                stretchedSize = lBounds.Height - stretchedSize;

                double y = 0;
                bool hasBottom = false;

                foreach (var control in items)
                {
                    if (control.VerticalAlignment == VerticalAlignment.Bottom)
                    {
                        hasBottom = true;
                        continue;
                    }

                    if (AllowStretch && control.VerticalAlignment == VerticalAlignment.Stretch)
                        DoAlignControl(control, true);
                    else
                        DoAlignControl(control);
                }

                if (hasBottom)
                {
                    foreach (var control in items)
                    {
                        if (control.VerticalAlignment != VerticalAlignment.Bottom)
                            continue;
                        DoAlignControl(control);
                    }
                }

                void DoAlignControl(Control control, bool stretch = false)
                {
                    var margin = control.Margin;
                    var vertMargin = margin.Vertical;

                    var freeSize = new SizeD(
                        lBounds.Width,
                        lBounds.Height - y - vertMargin);
                    var preferSize = control.GetPreferredSizeLimited(freeSize);
                    if (stretch)
                        preferSize.Height = stretchedSize;
                    var alignedPos = AlignedLayout.AlignHorizontal(lBounds, control, preferSize);
                    control.Handler.Bounds =
                        new RectD(
                            alignedPos.Origin,
                            lBounds.Top + y + margin.Top,
                            alignedPos.Size,
                            preferSize.Height);
                    y += preferSize.Height + vertMargin;
                }
            }
        }
    }
}