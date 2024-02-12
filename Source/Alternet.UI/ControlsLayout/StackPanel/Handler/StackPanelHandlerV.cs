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
                double h = 0;
                int bottomCount = 0;

                foreach (var control in items)
                {
                    if (control.VerticalAlignment == VerticalAlignment.Bottom)
                    {
                        bottomCount++;
                        continue;
                    }

                    DoAlignControl(control);
                }

                if (bottomCount > 0)
                {
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        var control = items[i];
                        if (control.VerticalAlignment != VerticalAlignment.Bottom)
                            continue;
                        DoAlignControl(control);
                        bottomCount--;
                        if (bottomCount == 0)
                            break;
                    }
                }

                void DoAlignControl(Control control)
                {
                    var stretch = control.VerticalAlignment == VerticalAlignment.Stretch
                        && AllowStretch;

                    bool bottom = control.VerticalAlignment == VerticalAlignment.Bottom;

                    var margin = control.Margin;
                    var vertMargin = margin.Vertical;

                    var freeSize = new SizeD(
                        lBounds.Width,
                        lBounds.Height - y - vertMargin - h);
                    var preferSize = control.GetPreferredSizeLimited(freeSize);
                    if (stretch)
                        preferSize.Height = stretchedSize;
                    var alignedPos = AlignedLayout.AlignHorizontal(lBounds, control, preferSize);
                    if(bottom)
                    {
                        control.Handler.Bounds =
                            new RectD(
                                alignedPos.Origin,
                                lBounds.Bottom - h - margin.Bottom - preferSize.Height,
                                alignedPos.Size,
                                preferSize.Height);
                        h += preferSize.Height + vertMargin;
                    }
                    else
                    {
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
}


/*
                    var margin = control.Margin;
                    var horizontalMargin = margin.Horizontal;

                    var preferredSize = control.GetPreferredSizeLimited(
                        new SizeD(
                            childrenLayoutBounds.Width - x - horizontalMargin - w,
                            childrenLayoutBounds.Height));
                    var alignedPosition =
                        AlignedLayout.AlignVertical(childrenLayoutBounds, control, preferredSize);

                    bool isRight = control.HorizontalAlignment == HorizontalAlignment.Right;

                    if(isRight)
                    {
                        control.Handler.Bounds =
                            new RectD(
                                childrenLayoutBounds.Right - w - margin.Right - preferredSize.Width,
                                alignedPosition.Origin,
                                preferredSize.Width,
                                alignedPosition.Size);
                        w += preferredSize.Width + horizontalMargin;
                    }
                    else
                    {
                        control.Handler.Bounds =
                            new RectD(
                                childrenLayoutBounds.Left + x + margin.Left,
                                alignedPosition.Origin,
                                preferredSize.Width,
                                alignedPosition.Size);
                        x += preferredSize.Width + horizontalMargin;
                    }
*/