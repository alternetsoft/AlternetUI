using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class OldLayout
    {
        internal static SizeD GetPreferredSizeWhenHorizontal(
            AbstractControl container,
            PreferredSizeContext context)
        {
            var isNanHeight = Coord.IsNaN(container.SuggestedHeight);
            var isNanWidth = Coord.IsNaN(container.SuggestedWidth);
            if (!isNanHeight && !isNanWidth)
                return container.SuggestedHeight;

            var stackPanelPadding = container.Padding;

            Coord width = 0;
            Coord maxHeight = 0;
            foreach (var control in container.AllChildrenInLayout)
            {
                if (control.Dock != DockStyle.None)
                    continue;

                var margin = control.Margin;
                var preferredSize = control.GetPreferredSizeLimited(
                    new PreferredSizeContext(
                        context.AvailableSize.Width - width,
                        context.AvailableSize.Height));
                width += preferredSize.Width + margin.Horizontal;
                maxHeight = Math.Max(maxHeight, preferredSize.Height + margin.Vertical);
            }

            return new SizeD(
                isNanWidth ? width + stackPanelPadding.Horizontal : container.SuggestedWidth,
                isNanHeight ? maxHeight + stackPanelPadding.Vertical : container.SuggestedHeight);
        }

        internal static SizeD GetPreferredSizeWhenVertical(
            AbstractControl container,
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

        internal static void LayoutWhenHorizontal(
            AbstractControl container,
            RectD childrenLayoutBounds,
            IReadOnlyList<AbstractControl> controls)
        {
            Coord x = 0;
            Coord w = 0;

            Stack<AbstractControl>? rightControls = null;
            List<AbstractControl>? fillControls = null;
            List<(AbstractControl Control, Coord Top, SizeD Size)>? centerControls = null;

            foreach (var control in controls)
            {
                bool isFill = control.HorizontalAlignment == UI.HorizontalAlignment.Fill;
                if (isFill)
                {
                    fillControls ??= new();
                    fillControls.Add(control);
                    continue;
                }

                bool isRight = control.HorizontalAlignment == UI.HorizontalAlignment.Right;
                if (isRight)
                {
                    rightControls ??= new();
                    rightControls.Push(control);
                }
                else
                    DoAlignControl(control);
            }

            if(rightControls is not null)
            {
                foreach (var control in rightControls)
                {
                    DoAlignControl(control);
                }
            }

            if (fillControls is not null)
            {
                foreach (var control in fillControls)
                {
                    DoAlignControl(control);
                }
            }

            if (centerControls is not null)
                AlignCenterControls();

            void AlignCenterControls()
            {
                Coord totalWidth = 0;
                foreach (var item in centerControls)
                    totalWidth += item.Size.Width;
                var offset = (childrenLayoutBounds.Width - totalWidth) / 2;

                foreach (var item in centerControls)
                {
                    var margin = item.Control.Margin;
                    item.Control.Bounds =
                        new RectD(
                            childrenLayoutBounds.Left + margin.Left + offset,
                            item.Top,
                            item.Size.Width,
                            item.Size.Height);
                    offset += item.Size.Width + margin.Horizontal;
                }
            }

            void DoAlignControl(AbstractControl control)
            {
                var margin = control.Margin;
                var horizontalMargin = margin.Horizontal;

                var availableWidth = childrenLayoutBounds.Width - x - horizontalMargin - w;

                var preferredSize = control.GetPreferredSizeLimited(
                    new PreferredSizeContext(
                        availableWidth,
                        childrenLayoutBounds.Height));

                var alignedPosition =
                    AbstractControl.AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.VerticalAlignment);

                switch (control.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                    case HorizontalAlignment.Stretch:
                    default:
                        control.Bounds =
                            new RectD(
                                childrenLayoutBounds.Left + x + margin.Left,
                                alignedPosition.Origin,
                                preferredSize.Width,
                                alignedPosition.Size);
                        x += preferredSize.Width + horizontalMargin;
                        break;
                    case HorizontalAlignment.Fill:
                        control.Bounds =
                            new RectD(
                                childrenLayoutBounds.Left + x + margin.Left,
                                alignedPosition.Origin,
                                availableWidth,
                                alignedPosition.Size);
                        x += availableWidth + horizontalMargin;
                        break;
                    case HorizontalAlignment.Center:
                        centerControls ??= new();
                        var item = (
                            control,
                            alignedPosition.Origin,
                            (preferredSize.Width, alignedPosition.Size));
                        centerControls.Add(item);
                        break;
                    case HorizontalAlignment.Right:
                        control.Bounds =
                            new RectD(
                                childrenLayoutBounds.Right - w - margin.Right - preferredSize.Width,
                                alignedPosition.Origin,
                                preferredSize.Width,
                                alignedPosition.Size);
                        w += preferredSize.Width + horizontalMargin;
                        break;
                }
            }
        }

        internal static void LayoutWhenVertical(
            AbstractControl container,
            RectD lBounds,
            IReadOnlyList<AbstractControl> items)
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

            void DoAlignControl(AbstractControl control)
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
                var alignedPos = AbstractControl.AlignHorizontal(
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

        internal static void LayoutWhenScroll(
            AbstractControl container,
            Func<RectD> getBounds,
            IReadOnlyList<AbstractControl> controls,
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
                    AbstractControl.AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        horizontalAlignment);
                var verticalPosition =
                    AbstractControl.AlignVertical(
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

        internal static void LayoutWhenDocked(
            ref RectD bounds,
            AbstractControl child,
            DockStyle value,
            bool autoSize)
        {
            SizeD child_size = child.Bounds.Size;
            var space = bounds;

            switch (value)
            {
                case DockStyle.Left:
                    LayoutLeft();
                    break;
                case DockStyle.Top:
                    LayoutTop();
                    break;
                case DockStyle.Right:
                    LayoutRight();
                    break;
                case DockStyle.Bottom:
                    LayoutBottom();
                    break;
                case DockStyle.Fill:
                    LayoutFill();
                    break;
                case DockStyle.RightAutoSize:
                    LayoutRight(true);
                    break;
            }

            bounds = space;

            void LayoutLeft(bool autoSize = false)
            {
                if (autoSize)
                {
                    child_size =
                        child.GetPreferredSizeLimited(
                            new SizeD(child_size.Width, space.Height));
                }

                child.SetBounds(
                    space.Left,
                    space.Y,
                    child_size.Width,
                    space.Height,
                    BoundsSpecified.All);
                space.X += child_size.Width;
                space.Width -= child_size.Width;
            }

            void LayoutTop(bool autoSize = false)
            {
                if (autoSize)
                {
                    child_size =
                        child.GetPreferredSizeLimited(
                            new SizeD(space.Width, child_size.Height));
                }

                child.SetBounds(
                    space.Left,
                    space.Y,
                    space.Width,
                    child_size.Height,
                    BoundsSpecified.All);
                space.Y += child_size.Height;
                space.Height -= child_size.Height;
            }

            void LayoutRight(bool autoSize = false)
            {
                Thickness margin;

                if (autoSize)
                {
                    child_size =
                        child.GetPreferredSizeLimited(
                            new SizeD(child_size.Width, space.Height));
                    margin = child.Margin;
                }
                else
                {
                    margin = Thickness.Empty;
                }

                child.SetBounds(
                    space.Right - child_size.Width - margin.Right,
                    space.Y,
                    child_size.Width,
                    space.Height,
                    BoundsSpecified.All);
                space.Width -= child_size.Width + margin.Horizontal;
            }

            void LayoutBottom(bool autoSize = false)
            {
                if (autoSize)
                {
                    child_size =
                        child.GetPreferredSizeLimited(
                            new SizeD(space.Width, child_size.Height));
                }

                child.SetBounds(
                    space.Left,
                    space.Bottom - child_size.Height,
                    space.Width,
                    child_size.Height,
                    BoundsSpecified.All);
                space.Height -= child_size.Height;
            }

            void LayoutFill(bool autoSize = false)
            {
                child_size = new SizeD(space.Width, space.Height);
                if (autoSize)
                    child_size = child.GetPreferredSizeLimited(child_size);
                child.SetBounds(
                    space.Left,
                    space.Top,
                    child_size.Width,
                    child_size.Height,
                    BoundsSpecified.All);
            }
        }

        // On return, 'bounds' has an empty space left after docking the controls to sides
        // of the container (fill controls are not counted).
        internal static int LayoutWhenDocked(
            AbstractControl container,
            ref RectD bounds,
            IReadOnlyList<AbstractControl> children)
        {
            var result = 0;

            var space = bounds;

            if (container.LayoutFlags.HasFlag(LayoutFlags.IterateBackward))
            {
                for (int i = 0; i < children.Count; i++)
                {
                    LayoutControl(i);
                }
            }
            else
            {
                // Deal with docking; go through in reverse, MS docs say that
                // lowest Z-order is closest to edge
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    LayoutControl(i);
                }
            }

            void LayoutControl(int index)
            {
                AbstractControl child = children[index];
                DockStyle dock = child.Dock;

                if (dock == DockStyle.None || container.ChildIgnoresLayout(child))
                    return;

                result++;

                LayoutWhenDocked(
                    ref space,
                    child,
                    dock,
                    autoSize: false);
            }

            bounds = space;
            return result;
        }
    }
}
