using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides layout management functionality for arranging and sizing controls and items within a container. Supports various
    /// layout styles and alignment options.
    /// </summary>
    /// <remarks>The LayoutManager class offers default implementations for control layout and preferred size
    /// calculations, which are invoked by container controls during layout operations. It supports multiple layout
    /// styles, including dock, stack, and scroll, and provides alignment utilities for both horizontal and vertical
    /// positioning. This class is intended for use by control authors and advanced users who need to customize or
    /// extend layout behavior.</remarks>
    public partial class LayoutManager : BaseObject, ILayoutManager
    {
        /// <summary>
        /// Gets or sets the default instance of the <see cref="ILayoutManager"/> to be used by controls
        /// and containers that do not specify a custom layout manager.
        /// </summary>
        public static ILayoutManager Instance { get; set; } = new LayoutManager();

        /// <summary>
        /// Determines whether the specified child layout item should be excluded from layout calculations.
        /// </summary>
        /// <param name="control">The child layout item to evaluate. Cannot be null.</param>
        /// <returns>true if the child is either not visible or marked to ignore layout; otherwise, false.</returns>
        public virtual bool ChildIgnoresLayout(ILayoutItem control)
        {
            return !control.Visible || control.IgnoreLayout;
        }

        /// <summary>
        /// Called when the control or item should reposition its child controls.
        /// This method checks the layout style and calls the appropriate method to perform the layout of child controls.
        /// </summary>
        /// <remarks>
        /// This is a default implementation which is called from
        /// <see cref="AbstractControl.OnLayout"/>.
        /// </remarks>
        /// <param name="container">Container control which children will be processed.</param>
        /// <param name="layout">Layout style to use.</param>
        /// <param name="getBounds">Returns rectangle in which layout is performed.</param>
        /// <param name="items">List of controls to layout.</param>
        public virtual void OnLayout(
            ILayoutItem container,
            LayoutStyle layout,
            Func<RectD> getBounds,
            IReadOnlyList<ILayoutItem> items)
        {
            if (layout == LayoutStyle.Scroll)
            {
                LayoutWhenScroll(container, getBounds, items, true);
                return;
            }

            var space = getBounds();

            if (space.SizeIsEmpty)
                return;

            var number = LayoutWhenDocked(container, ref space, items);

            void UpdateItems()
            {
                if (number == 0 || number == items.Count)
                    return;
                var newItems = new List<ILayoutItem>();
                foreach (var item in items)
                {
                    if (item.Dock == DockStyle.None && !item.IgnoreLayout)
                        newItems.Add(item);
                }

                items = newItems;
            }

            switch (layout)
            {
                case LayoutStyle.Basic:
                    UpdateItems();
                    PerformDefaultLayout(container, space, items);
                    break;
                case LayoutStyle.Vertical:
                    UpdateItems();
                    LayoutWhenVertical(container, space, items);
                    break;
                case LayoutStyle.Horizontal:
                    UpdateItems();
                    LayoutWhenHorizontal(container, space, items);
                    break;
            }
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control or item can
        /// be fitted, in device-independent units. This method checks the layout style and
        /// calls the appropriate method to calculate the preferred size.
        /// </summary>
        /// <param name="context">The <see cref="PreferredSizeContext"/> providing
        /// the available size and other layout information.</param>
        /// <returns>A <see cref="ILayoutItem.SuggestedSize"/> representing the width and height of
        /// a rectangle, in device-independent units.</returns>
        /// <remarks>
        /// This is a default implementation which is called from
        /// <see cref="AbstractControl.GetPreferredSize(PreferredSizeContext)"/>.
        /// </remarks>
        /// <param name="container">Container control which children will be processed.</param>
        /// <param name="layout">Layout style to use.</param>
        public virtual SizeD OnGetPreferredSize(
            ILayoutItem container,
            PreferredSizeContext context,
            LayoutStyle layout)
        {
            switch (layout)
            {
                case LayoutStyle.Dock:
                    return GetPreferredSizeDockLayout(container, context);
                case LayoutStyle.None:
                default:
                    return GetPreferredSizeDefaultLayout(container, context);
                case LayoutStyle.Basic:
                    return GetPreferredSizeDefaultLayout(container, context);
                case LayoutStyle.Vertical:
                    return GetPreferredSizeWhenStack(container, context, isVert: true);
                case LayoutStyle.Horizontal:
                    return GetPreferredSizeWhenStack(container, context, isVert: false);
                case LayoutStyle.Scroll:
                    return GetPreferredSizeWhenScroll(container, context);
            }
        }

        /// <summary>
        /// Calculates horizontal position using align parameters.
        /// </summary>
        /// <param name="layoutBounds">Rectangle in which alignment is performed.</param>
        /// <param name="childControl">Control to align.</param>
        /// <param name="childPreferredSize">Preferred size.</param>
        /// <param name="alignment">Alignment of the control.</param>
        /// <returns></returns>
        public virtual AxisIntervalD AlignHorizontal(
                    RectD layoutBounds,
                    ILayoutItem childControl,
                    SizeD childPreferredSize,
                    HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return new (
                        layoutBounds.Left + childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Center:
                    return new (
                        layoutBounds.Left +
                        ((layoutBounds.Width
                        - (childPreferredSize.Width + childControl.Margin.Horizontal)) / 2) +
                        childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Right:
                    return new (
                        layoutBounds.Right -
                        childControl.Margin.Right - childPreferredSize.Width,
                        childPreferredSize.Width);
                case HorizontalAlignment.Stretch:
                default:
                    return new (
                        layoutBounds.Left + childControl.Margin.Left,
                        layoutBounds.Width - childControl.Margin.Horizontal);
            }
        }

        /// <summary>
        /// Calculates vertical position using align parameters.
        /// </summary>
        /// <param name="layoutBounds">Rectangle in which alignment is performed.</param>
        /// <param name="control">Control to align.</param>
        /// <param name="childPreferredSize">Preferred size.</param>
        /// <param name="alignment">Alignment of the control.</param>
        /// <returns></returns>
        public virtual AxisIntervalD AlignVertical(
            RectD layoutBounds,
            ILayoutItem control,
            SizeD childPreferredSize,
            VerticalAlignment alignment)
        {
            switch (alignment)
            {
                case VerticalAlignment.Top:
                    return new (
                        layoutBounds.Top + control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Center:
                    return new (
                        layoutBounds.Top +
                        ((layoutBounds.Height
                        - (childPreferredSize.Height + control.Margin.Vertical)) / 2) +
                        control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Bottom:
                    return new (
                        layoutBounds.Bottom - control.Margin.Bottom - childPreferredSize.Height,
                        childPreferredSize.Height);
                case VerticalAlignment.Stretch:
                default:
                    return new (
                        layoutBounds.Top + control.Margin.Top,
                        layoutBounds.Height - control.Margin.Vertical);
            }
        }

        private void PerformDefaultLayout(
            ILayoutItem container,
            RectD childrenLayoutBounds,
            IReadOnlyList<ILayoutItem> childControls)
        {
            foreach (var control in childControls)
            {
                if (control.Dock != DockStyle.None || control.IgnoreLayout)
                    continue;

                var preferredSize = control.GetPreferredSizeLimited(
                    new PreferredSizeContext(childrenLayoutBounds.Size - control.Margin.Size));

                var horizontalPosition =
                    AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.HorizontalAlignment);
                var verticalPosition =
                    AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.VerticalAlignment);

                control.Bounds = new RectD(
                    horizontalPosition.Start,
                    verticalPosition.Start,
                    horizontalPosition.Length,
                    verticalPosition.Length);
            }
        }

        /// <summary>
        /// Returns a maximal preferred size of the children with an added padding.
        /// </summary>
        private SizeD GetChildrenMaxPreferredSizePadded(ILayoutItem item, PreferredSizeContext context)
        {
            var preferredSize = item.GetChildrenMaxPreferredSize(context);
            var padded = item.GetPaddedPreferredSize(preferredSize);
            return padded;
        }

        /// <summary>
        /// Gets the size of the control specified in its
        /// <see cref="AbstractControl.SuggestedWidth"/>
        /// and <see cref="AbstractControl.SuggestedHeight"/>
        /// properties or calculates preferred size from its children.
        /// </summary>
        private SizeD GetBestSizeWithChildren(ILayoutItem item, PreferredSizeContext context)
        {
            var specifiedWidth = item.SuggestedWidth;
            var specifiedHeight = item.SuggestedHeight;
            if (!Coord.IsNaN(specifiedWidth) && !Coord.IsNaN(specifiedHeight))
                return new SizeD(specifiedWidth, specifiedHeight);

            var maxSize = GetChildrenMaxPreferredSizePadded(item, context);
            var maxWidth = maxSize.Width;
            var maxHeight = maxSize.Height;

            var width = Coord.IsNaN(specifiedWidth) ? maxWidth : specifiedWidth;
            var height = Coord.IsNaN(specifiedHeight) ? maxHeight : specifiedHeight;

            return new SizeD(width, height);
        }

        private SizeD GetPreferredSizeDefaultLayout(
            ILayoutItem container,
            PreferredSizeContext context)
        {
            if (container.HasChildren)
                return GetBestSizeWithChildren(container, context);
            return container.GetBestSizeWithPadding(context);
        }     
    }
}
