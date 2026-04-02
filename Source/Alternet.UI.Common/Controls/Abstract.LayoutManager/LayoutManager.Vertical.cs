using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class LayoutManager
    {
        private void LayoutWhenVertical(
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
                            alignedPos.Start,
                            lBounds.Bottom - h - margin.Bottom - preferSize.Height,
                            alignedPos.Length,
                            preferSize.Height);
                    h += preferSize.Height + vertMargin;
                }
                else
                {
                    control.Bounds =
                        new RectD(
                            alignedPos.Start,
                            lBounds.Top + y + margin.Top,
                            alignedPos.Length,
                            preferSize.Height);
                    y += preferSize.Height + vertMargin;
                }
            }
        }
    }
}
