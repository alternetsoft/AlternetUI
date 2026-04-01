using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class LayoutManager
    {
        public virtual void LayoutWhenVertical(
            ILayoutItem container,
            RectD lBounds,
            IReadOnlyList<ILayoutItem> items)
        {
            Coord stretchedSize = 0;

            if (items.Count > 0)
            {
                foreach (var control in items)
                {
                    if (control.VerticalAlignment == UI.VerticalAlignment.Fill)
                        continue;

                    var verticalMargin = control.Margin.Vertical;
                    var freeSize = new SizeD(
                            lBounds.Width,
                            lBounds.Height - stretchedSize - verticalMargin);
                    var preferredSize = control.GetPreferredSizeLimited(new PreferredSizeContext(freeSize));
                    stretchedSize += preferredSize.Height + verticalMargin;
                }
            }

            stretchedSize = lBounds.Height - stretchedSize;

            Coord y = 0;
            Coord h = 0;
            int bottomCount = 0;

            foreach (var control in items)
            {
                if (control.VerticalAlignment == UI.VerticalAlignment.Bottom)
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
                    if (control.VerticalAlignment != UI.VerticalAlignment.Bottom)
                        continue;
                    DoAlignControl(control);
                    bottomCount--;
                    if (bottomCount == 0)
                        break;
                }
            }

            void DoAlignControl(ILayoutItem control)
            {
                var stretch = control.VerticalAlignment == UI.VerticalAlignment.Fill;

                bool bottom = control.VerticalAlignment == UI.VerticalAlignment.Bottom;

                var margin = control.Margin;
                var vertMargin = margin.Vertical;

                var freeSize = new SizeD(
                    lBounds.Width,
                    lBounds.Height - y - vertMargin - h);
                var preferSize = control.GetPreferredSizeLimited(new PreferredSizeContext(freeSize));
                if (stretch)
                    preferSize.Height = stretchedSize - vertMargin;
                var alignedPos = AlignHorizontal(
                    lBounds,
                    control,
                    preferSize,
                    control.HorizontalAlignment);
                if (bottom)
                {
                    control.Bounds =
                        new RectD(
                            alignedPos.Origin,
                            lBounds.Bottom - h - margin.Bottom - preferSize.Height,
                            alignedPos.Size,
                            preferSize.Height);
                    h += preferSize.Height + vertMargin;
                }
                else
                {
                    control.Bounds =
                        new RectD(
                            alignedPos.Origin,
                            lBounds.Top + y + margin.Top,
                            alignedPos.Size,
                            preferSize.Height);
                    y += preferSize.Height + vertMargin;
                }
            }
        }

/*
        public virtual SizeD GetPreferredSizeWhenVertical(
            ILayoutItem container,
            PreferredSizeContext context)
        {
            var isNanWidth = Coord.IsNaN(container.SuggestedWidth);
            var isNanHeight = Coord.IsNaN(container.SuggestedHeight);

            var padding = container.Padding;

            Coord maxWidth = 0;
            Coord height = 0;
            foreach (var control in container.AllChildrenInLayout)
            {
                if (control.Dock != DockStyle.None)
                    continue;

                var margin = control.Margin;

                var preferredSize = control.GetPreferredSizeLimited(
                    new PreferredSizeContext(
                        context.AvailableSize.Width,
                        context.AvailableSize.Height - height));

                maxWidth = Math.Max(maxWidth, preferredSize.Width + margin.Horizontal);
                height += preferredSize.Height + margin.Vertical;
            }

            var newWidth = isNanWidth ? maxWidth + padding.Horizontal : container.SuggestedWidth;
            var newHeight = isNanHeight ? height + padding.Vertical : container.SuggestedHeight;
            return new SizeD(newWidth, newHeight);
        }
*/
    }
}
