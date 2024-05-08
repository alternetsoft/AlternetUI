using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        internal static SizeD GetPreferredSizeHorizontalStackPanel(
            Control container,
            SizeD availableSize)
        {
            var isNanHeight = double.IsNaN(container.SuggestedHeight);
            var isNanWidth = double.IsNaN(container.SuggestedWidth);
            if (!isNanHeight && !isNanWidth)
                return container.SuggestedHeight;

            var stackPanelPadding = container.Padding;

            double width = 0;
            double maxHeight = 0;
            foreach (var control in container.AllChildrenInLayout)
            {
                var margin = control.Margin;
                var preferredSize = control.GetPreferredSizeLimited(
                    new SizeD(availableSize.Width - width, availableSize.Height));
                width += preferredSize.Width + margin.Horizontal;
                maxHeight = Math.Max(maxHeight, preferredSize.Height + margin.Vertical);
            }

            return new SizeD(
                isNanWidth ? width + stackPanelPadding.Horizontal : container.SuggestedWidth,
                isNanHeight ? maxHeight + stackPanelPadding.Vertical : container.SuggestedHeight);
        }

        internal static SizeD GetPreferredSizeStackPanel(
            Control container,
            SizeD availableSize,
            bool isVertical)
        {
            if (isVertical)
                return GetPreferredSizeVerticalStackPanel(container, availableSize);
            else
                return GetPreferredSizeHorizontalStackPanel(container, availableSize);
        }

        internal static SizeD GetPreferredSizeVerticalStackPanel(
            Control container,
            SizeD availableSize)
        {
            var isNanWidth = double.IsNaN(container.SuggestedWidth);
            var isNanHeight = double.IsNaN(container.SuggestedHeight);

            var padding = container.Padding;

            double maxWidth = 0;
            double height = 0;
            foreach (var control in container.AllChildrenInLayout)
            {
                var margin = control.Margin;
                var preferredSize = control.GetPreferredSizeLimited(
                    new SizeD(availableSize.Width, availableSize.Height - height));
                maxWidth = Math.Max(maxWidth, preferredSize.Width + margin.Horizontal);
                height += preferredSize.Height + margin.Vertical;
            }

            var newWidth = isNanWidth ? maxWidth + padding.Horizontal : container.SuggestedWidth;
            var newHeight = isNanHeight ? height + padding.Vertical : container.SuggestedHeight;
            return new SizeD(newWidth, newHeight);
        }

        internal static void LayoutHorizontalStackPanel(
            Control container,
            RectD childrenLayoutBounds,
            IReadOnlyList<Control> controls)
        {
            double x = 0;
            double w = 0;

            Stack<Control> rightControls = new();
            List<(Control Control, double Top, SizeD Size)>? centerControls = null;

            foreach (var control in controls)
            {
                bool isRight = control.HorizontalAlignment == UI.HorizontalAlignment.Right;
                if (isRight)
                    rightControls.Push(control);
                else
                    DoAlignControl(control);
            }

            foreach (var control in rightControls)
            {
                DoAlignControl(control);
            }

            if (centerControls is not null)
                AlignCenterControls();

            void AlignCenterControls()
            {
                double totalWidth = 0;
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

            void DoAlignControl(Control control)
            {
                var margin = control.Margin;
                var horizontalMargin = margin.Horizontal;

                var preferredSize = control.GetPreferredSizeLimited(
                    new SizeD(
                        childrenLayoutBounds.Width - x - horizontalMargin - w,
                        childrenLayoutBounds.Height));
                var alignedPosition =
                    AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.VerticalAlignment);

                switch (control.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                    case HorizontalAlignment.Fill:
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

        internal static void LayoutStackPanel(
            Control container,
            bool isVertical,
            RectD space,
            IReadOnlyList<Control> items)
        {
            if (isVertical)
                LayoutVerticalStackPanel(container, space, items);
            else
                LayoutHorizontalStackPanel(container, space, items);
        }

        internal static void LayoutVerticalStackPanel(
            Control container,
            RectD lBounds,
            IReadOnlyList<Control> items)
        {
            double stretchedSize = 0;

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

            void DoAlignControl(Control control)
            {
                var stretch = control.VerticalAlignment == UI.VerticalAlignment.Fill;

                bool bottom = control.VerticalAlignment == UI.VerticalAlignment.Bottom;

                var margin = control.Margin;
                var vertMargin = margin.Vertical;

                var freeSize = new SizeD(
                    lBounds.Width,
                    lBounds.Height - y - vertMargin - h);
                var preferSize = control.GetPreferredSizeLimited(freeSize);
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

        // On return, 'bounds' has an empty space left after docking the controls to sides
        // of the container (fill controls are not counted).
        internal static int LayoutDockedChildren(
            Control parent,
            ref RectD bounds,
            IReadOnlyList<Control> children)
        {
            var result = 0;

            var space = bounds;

            // Deal with docking; go through in reverse, MS docs say that
            // lowest Z-order is closest to edge
            for (int i = children.Count - 1; i >= 0; i--)
            {
                Control child = children[i];
                DockStyle dock = child.Dock;

                if (dock == DockStyle.None)
                    continue;

                result++;
                SizeD child_size = child.Bounds.Size;
                bool autoSize = false;

                switch (dock)
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
                }

                void LayoutLeft()
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

                void LayoutTop()
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

                void LayoutRight()
                {
                    if (autoSize)
                    {
                        child_size =
                            child.GetPreferredSizeLimited(
                                new SizeD(child_size.Width, space.Height));
                    }

                    child.SetBounds(
                        space.Right - child_size.Width,
                        space.Y,
                        child_size.Width,
                        space.Height,
                        BoundsSpecified.All);
                    space.Width -= child_size.Width;
                }

                void LayoutBottom()
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

                void LayoutFill()
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

            bounds = space;
            return result;
        }
    }
}
