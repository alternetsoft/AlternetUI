using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class LayoutManager
    {
        public virtual SizeD GetPreferredSizeWhenScroll(
            AbstractControl container,
            PreferredSizeContext context)
        {
            var result = container.LayoutMaxSize
                ?? GetPreferredSizeDefaultLayout(container, context);

            var cornerSize = ScrollBar.GetCornerSize(container);

            if (result.Width > context.AvailableSize.Width)
            {
                result.Width += cornerSize.Width;
            }

            if (result.Height > context.AvailableSize.Height)
            {
                result.Height += cornerSize.Height;
            }

            return result;
        }

        public virtual void LayoutWhenScroll(
            AbstractControl container,
            Func<RectD> getBounds,
            IReadOnlyList<ILayoutItem> controls,
            bool updateScrollbars)
        {
            const Coord sizeMultiplicator = 1;

            var childrenLayoutBounds = getBounds();

            if (updateScrollbars && container.IsScrollable)
            {
                var totalSize = container.LayoutMaxSize
                    ?? container.GetChildrenMaxPreferredSizePadded(SizeD.PositiveInfinity);

                // This multiplication is here because:
                // 1. We need to have the ability to scroll further than calculated
                // max total size because there could be on-screen keyboard shown.
                // 2. Currently totalSize is calculated incorrectly.
                totalSize.Width *= ScrollViewer.DefaultScrollBarTotalSizeMultiplier.Width;
                totalSize.Height *= ScrollViewer.DefaultScrollBarTotalSizeMultiplier.Height;

                container.SetScrollBarInfo(childrenLayoutBounds.Size, totalSize);
            }

            foreach (var control in controls)
            {
                if (control.Dock != DockStyle.None || control.IgnoreLayout)
                    continue;

                var boundedPreferredSize = control.GetPreferredSize(new PreferredSizeContext(childrenLayoutBounds.Size));
                var unboundedPreferredSize =
                    control.GetPreferredSize(new PreferredSizeContext(SizeD.PositiveInfinity));

                boundedPreferredSize.Width *= sizeMultiplicator;
                boundedPreferredSize.Height *= sizeMultiplicator;
                unboundedPreferredSize.Width *= sizeMultiplicator;
                unboundedPreferredSize.Height *= sizeMultiplicator;

                var verticalAlignment = control.VerticalAlignment;
                var horizontalAlignment = control.HorizontalAlignment;

                if (unboundedPreferredSize.Width > childrenLayoutBounds.Width)
                {
                    horizontalAlignment = UI.HorizontalAlignment.Left;
                }

                if (unboundedPreferredSize.Height > childrenLayoutBounds.Height)
                {
                    verticalAlignment = UI.VerticalAlignment.Top;
                }

                var horizontalPosition =
                    AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        horizontalAlignment);
                var verticalPosition =
                    AlignVertical(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        verticalAlignment);

                var layoutOffset = container.LayoutOffset;

                control.Bounds = new RectD(
                    horizontalPosition.Origin + layoutOffset.X,
                    verticalPosition.Origin + layoutOffset.Y,
                    unboundedPreferredSize.Width,
                    unboundedPreferredSize.Height);
            }
        }
    }
}
