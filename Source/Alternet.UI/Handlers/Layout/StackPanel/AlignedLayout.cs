using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class AlignedLayout
    {
        public static AlignedPosition AlignHorizontal(
            Rect layoutBounds,
            Control childControl,
            Size childPreferredSize) => AlignHorizontal(
                layoutBounds,
                childControl,
                childPreferredSize,
                childControl.HorizontalAlignment);

        public static AlignedPosition AlignVertical(
            Rect layoutBounds,
            Control childControl,
            Size childPreferredSize) => AlignVertical(
                layoutBounds,
                childControl,
                childPreferredSize,
                childControl.VerticalAlignment);

        public static AlignedPosition AlignHorizontal(
            Rect layoutBounds,
            Control childControl,
            Size childPreferredSize,
            HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return new AlignedPosition(layoutBounds.Left + childControl.Margin.Left, childPreferredSize.Width);
                case HorizontalAlignment.Center:
                    return new AlignedPosition(
                        layoutBounds.Left +
                        ((layoutBounds.Width - (childPreferredSize.Width + childControl.Margin.Horizontal)) / 2) +
                        childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Right:
                    return new AlignedPosition(
                        layoutBounds.Right -
                        childControl.Margin.Right - childPreferredSize.Width,
                        childPreferredSize.Width);
                case HorizontalAlignment.Stretch:
                    return new AlignedPosition(
                        layoutBounds.Left + childControl.Margin.Left,
                        layoutBounds.Width - childControl.Margin.Horizontal);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static AlignedPosition AlignVertical(
            Rect layoutBounds,
            Control control,
            Size childPreferredSize,
            VerticalAlignment alignment)
        {
            switch (alignment)
            {
                case VerticalAlignment.Top:
                    return new AlignedPosition(layoutBounds.Top + control.Margin.Top, childPreferredSize.Height);
                case VerticalAlignment.Center:
                    return new AlignedPosition(
                        layoutBounds.Top +
                        ((layoutBounds.Height - (childPreferredSize.Height + control.Margin.Vertical)) / 2) +
                        control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Bottom:
                    return new AlignedPosition(
                        layoutBounds.Bottom - control.Margin.Bottom - childPreferredSize.Height,
                        childPreferredSize.Height);
                case VerticalAlignment.Stretch:
                    return new AlignedPosition(
                        layoutBounds.Top + control.Margin.Top,
                        layoutBounds.Height - control.Margin.Vertical);
                default:
                    throw new InvalidOperationException();
            }
        }

        public class AlignedPosition
        {
            public AlignedPosition(double origin, double size)
            {
                Origin = origin;
                Size = size;
            }

            public double Origin { get; }

            public double Size { get; }
        }
    }
}