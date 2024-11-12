using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Gets or sets which layout method is used when controls are aligned.
        /// This is for internal use only.
        /// </summary>
        public static DefaultLayoutMethod UseLayoutMethod = DefaultLayoutMethod.New;

        /// <summary>
        /// Enumerates known layout methods. This is for internal use only.
        /// </summary>
        public enum DefaultLayoutMethod
        {
            /// <summary>
            /// Original layout method.
            /// </summary>
            Original,

            /// <summary>
            /// New layout method.
            /// </summary>
            New,
        }

        /// <summary>
        /// Aligns control in the parent using horizontal and vertical
        /// alignment options.
        /// </summary>
        /// <param name="horz">Horizontal alignment.</param>
        /// <param name="vert">Vertical alignment.</param>
        /// <param name="shrinkSize">Whether to shrink size of the rectangle
        /// to fit in the container. Optional. Default is <c>true</c>.</param>
        /// <remarks>
        /// This method changes <see cref="Bounds"/> so default layout must be disabled
        /// before using it. You can disable default layout using <see cref="IgnoreLayout"/> property
        /// of the control.
        /// </remarks>
        public virtual void AlignInParent(
            HorizontalAlignment? horz,
            VerticalAlignment? vert,
            bool shrinkSize = true)
        {
            if (Parent is null)
                return;
            AlignInRect(
                        Parent.ClientRectangle,
                        horz,
                        vert,
                        shrinkSize);
        }

        /// <summary>
        /// Aligns control in the parent using <see cref="DockStyle"/> alignment option.
        /// </summary>
        /// <param name="value"><see cref="DockStyle"/> value which specifies align option.</param>
        /// <remarks>
        /// This method changes <see cref="Bounds"/> so default layout must be disabled
        /// before using it. You can disable default layout using <see cref="IgnoreLayout"/> property
        /// of the control.
        /// </remarks>
        public virtual RectD DockInParent(DockStyle value)
        {
            if (Parent is null)
                return RectD.Empty;
            var result = DockInRect(Parent.ClientRectangle, value);
            return result;
        }

        /// <summary>
        /// Aligns control in the specified container rectangle
        /// using <see cref="DockStyle"/> alignment option.
        /// </summary>
        /// <param name="value"><see cref="DockStyle"/> value which specifies align option.</param>
        /// <param name="container">Container rectangle.</param>
        /// <remarks>
        /// This method changes <see cref="Bounds"/> so default layout must be disabled
        /// before using it. You can disable default layout using <see cref="IgnoreLayout"/> property
        /// of the control.
        /// </remarks>
        public virtual RectD DockInRect(RectD container, DockStyle value)
        {
            OldLayout.LayoutWhenDocked(
                        ref container,
                        this,
                        value,
                        false);
            return container;
        }

        /// <summary>
        /// Aligns control in the specified container rectangle using horizontal and vertical
        /// alignment options.
        /// </summary>
        /// <param name="container">Container rectangle.</param>
        /// <param name="horz">Horizontal alignment.</param>
        /// <param name="vert">Vertical alignment.</param>
        /// <param name="shrinkSize">Whether to shrink size of the rectangle
        /// to fit in the container. Optional. Default is <c>true</c>.</param>
        /// <remarks>
        /// This method changes <see cref="Bounds"/> so default layout must be disabled
        /// before using it. You can disable default layout using <see cref="IgnoreLayout"/> property
        /// of the control.
        /// </remarks>
        public virtual void AlignInRect(
            RectD container,
            HorizontalAlignment? horz,
            VerticalAlignment? vert,
            bool shrinkSize = true)
        {
            var newBounds = AlignUtils.AlignRectInRect(
                Bounds,
                container,
                horz,
                vert,
                shrinkSize);
            Bounds = newBounds;
        }

        internal static SizeD GetPreferredSizeWhenHorizontal(
            AbstractControl container,
            SizeD availableSize)
        {
            if (UseLayoutMethod == DefaultLayoutMethod.Original)
                return OldLayout.GetPreferredSizeWhenHorizontal(container, availableSize);
            else
                return GetPreferredSizeWhenStack(container, availableSize, isVert: false);
        }

        internal static SizeD GetPreferredSizeWhenVertical(
            AbstractControl container,
            SizeD availableSize)
        {
            if (UseLayoutMethod == DefaultLayoutMethod.Original)
                return OldLayout.GetPreferredSizeWhenVertical(container, availableSize);
            else
                return GetPreferredSizeWhenStack(container, availableSize, isVert: true);
        }

        internal static void LayoutWhenHorizontal(
            AbstractControl container,
            RectD childrenLayoutBounds,
            IReadOnlyList<AbstractControl> controls)
        {
            if (UseLayoutMethod == DefaultLayoutMethod.Original)
                OldLayout.LayoutWhenHorizontal(container, childrenLayoutBounds, controls);
            else
                OldLayout.LayoutWhenHorizontal(container, childrenLayoutBounds, controls);
        }

        internal static void LayoutWhenVertical(
            AbstractControl container,
            RectD lBounds,
            IReadOnlyList<AbstractControl> items)
        {
            if (UseLayoutMethod == DefaultLayoutMethod.Original)
                OldLayout.LayoutWhenVertical(container, lBounds, items);
            else
                OldLayout.LayoutWhenVertical(container, lBounds, items);
        }

        // On return, 'bounds' has an empty space left after docking the controls to sides
        // of the container (fill controls are not counted).
        // On return, 'result' has number of controls with Dock != None.
        internal static int LayoutWhenDocked(
            ref RectD bounds,
            IReadOnlyList<AbstractControl> children)
        {
            if (UseLayoutMethod == DefaultLayoutMethod.Original)
                return OldLayout.LayoutWhenDocked(ref bounds, children);
            else
                return OldLayout.LayoutWhenDocked(ref bounds, children);
        }

        internal static SizeD GetMinStretchedSize(
            SizeD availableSize,
            IReadOnlyList<AbstractControl> children)
        {
            SizeD result = 0;

            foreach (var child in children)
            {
                if (child.VerticalAlignment == VerticalAlignment.Stretch
                    || child.HorizontalAlignment == HorizontalAlignment.Stretch)
                {
                    var childMargin = child.Margin;
                    var childPreferredSize = child.GetPreferredSizeLimited(availableSize);
                    var childPreferredSizeWithMargin = childPreferredSize + childMargin.Size;
                    result = SizeD.Max(result, childPreferredSizeWithMargin);
                }
            }

            return result;
        }

        internal static SizeD GetPreferredSizeWhenStack(
            AbstractControl container,
            SizeD availableSize,
            bool isVert)
        {
            if (availableSize.AnyIsEmptyOrNegative)
                return SizeD.Empty;

            var containerSuggestedSize = container.SuggestedSize;
            if (!containerSuggestedSize.IsNanWidthOrHeight)
                return containerSuggestedSize;

            var children = container.AllChildrenInLayout;

            var isNanWidth = containerSuggestedSize.IsNanWidth;
            var isNanHeight = containerSuggestedSize.IsNanHeight;
            var containerSize = container.GetSizeLimited(availableSize);

            SizeD result = SizeD.Empty;

            foreach (var child in children)
            {
                var childMargin = child.Margin;

                if(isVert)
                {
                    var preferredSize = child.GetPreferredSizeLimited(
                        new(containerSize.Width, containerSize.Height - result.Height));
                    result.Width = Math.Max(result.Width, preferredSize.Width + childMargin.Horizontal);
                    result.Height += preferredSize.Height + childMargin.Vertical;
                }
                else
                {
                    var preferredSize = child.GetPreferredSizeLimited(
                        new SizeD(containerSize.Width - result.Width, containerSize.Height));
                    result.Width += preferredSize.Width + childMargin.Horizontal;
                    result.Height = Math.Max(result.Height, preferredSize.Height + childMargin.Vertical);
                }
            }

            var padding = container.Padding;
            var newWidth = isNanWidth ? result.Width + padding.Horizontal : container.SuggestedWidth;
            var newHeight = isNanHeight ? result.Height + padding.Vertical : container.SuggestedHeight;
            return new(newWidth, newHeight);
        }
    }
}