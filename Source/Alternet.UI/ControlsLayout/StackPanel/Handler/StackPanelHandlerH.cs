using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class StackPanelHandler
    {
        internal class HorizontalLayout : OrientedLayout
        {
            public HorizontalLayout(ControlHandler handler)
                : base(handler)
            {
            }

            public override SizeD GetPreferredSize(SizeD availableSize)
            {
                var stackPanelPadding = Control.Padding;

                double width = 0;
                double maxHeight = 0;
                foreach (var control in Control.AllChildrenInLayout)
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
                var controls = Control.AllChildrenInLayout;
                var childrenLayoutBounds = Handler.Control.ChildrenLayoutBounds;

                double x = 0;
                double w = 0;

                Stack<Control> rightControls = new();

                foreach (var control in controls)
                {
                    bool isRight = control.HorizontalAlignment == HorizontalAlignment.Right;
                    if (isRight)
                        rightControls.Push(control);
                    else
                        DoAlignControl(control);
                }

                foreach (var control in rightControls)
                {
                    DoAlignControl(control);
                }

                void DoAlignControl(Control control)
                {
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
                }
            }
        }
    }
}