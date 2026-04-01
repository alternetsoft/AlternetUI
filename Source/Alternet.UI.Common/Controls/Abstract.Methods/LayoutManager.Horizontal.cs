using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal partial class LayoutManager
    {
        public virtual SizeD GetPreferredSizeWhenStack(
            ILayoutItem container,
            PreferredSizeContext context,
            bool isVert,
            IReadOnlyList<ILayoutItem>? children = null,
            bool ignoreDocked = true)
        {
            if (context.AvailableSize.AnyIsEmptyOrNegative)
                return SizeD.Empty;

            var containerSuggestedSize = container.SuggestedSize;
            if (!containerSuggestedSize.IsNanWidthOrHeight)
                return containerSuggestedSize;

            children ??= container.AllChildrenInLayout;

            var isNanWidth = containerSuggestedSize.IsNanWidth;
            var isNanHeight = containerSuggestedSize.IsNanHeight;
            var containerSize = container.GetSizeLimited(context.AvailableSize);

            SizeD result = SizeD.Empty;

            foreach (var child in children)
            {
                if (ignoreDocked)
                {
                    if (child.Dock != DockStyle.None)
                        continue;
                }

                var childMargin = child.Margin;

                if (isVert)
                {
                    var preferredSize = child.GetPreferredSizeLimited(
                        new(containerSize.Width, containerSize.Height - result.Height));
                    result.Width
                        = Math.Max(result.Width, preferredSize.Width + childMargin.Horizontal);
                    result.Height += preferredSize.Height + childMargin.Vertical;
                }
                else
                {
                    var preferredSize = child.GetPreferredSizeLimited(
                        new SizeD(containerSize.Width - result.Width, containerSize.Height));
                    result.Width += preferredSize.Width + childMargin.Horizontal;
                    result.Height
                        = Math.Max(result.Height, preferredSize.Height + childMargin.Vertical);
                }
            }

            var padding = container.Padding;
            var newWidth = isNanWidth
                ? result.Width + padding.Horizontal : container.SuggestedWidth;
            var newHeight = isNanHeight
                ? result.Height + padding.Vertical : container.SuggestedHeight;
            return new(newWidth, newHeight);
        }

        public virtual void LayoutWhenHorizontal(
            ILayoutItem container,
            RectD childrenLayoutBounds,
            IReadOnlyList<ILayoutItem> controls)
        {
            Coord x = 0;
            Coord w = 0;

            Stack<ILayoutItem>? rightControls = null;
            List<ILayoutItem>? fillControls = null;
            List<(ILayoutItem Control, Coord Top, SizeD Size)>? centerControls = null;

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

            if (rightControls is not null)
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

            void DoAlignControl(ILayoutItem control)
            {
                var margin = control.Margin;
                var horizontalMargin = margin.Horizontal;

                var availableWidth = childrenLayoutBounds.Width - x - horizontalMargin - w;

                var preferredSize = control.GetPreferredSizeLimited(
                    new PreferredSizeContext(
                        availableWidth,
                        childrenLayoutBounds.Height));

                var alignedPosition = AlignVertical(
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

        /*
        public virtual SizeD GetPreferredSizeWhenHorizontal(
            ILayoutItem container,
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
        */
    }
}
