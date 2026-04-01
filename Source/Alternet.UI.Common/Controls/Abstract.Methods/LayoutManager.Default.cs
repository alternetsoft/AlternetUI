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
    internal partial class LayoutManager
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="LayoutManager"/> class.
        /// </summary>
        public static LayoutManager Instance { get; } = new LayoutManager();

        /// <summary>
        /// Called when the control should
        /// reposition its child controls.
        /// </summary>
        /// <remarks>
        /// This is a default implementation which is called from
        /// <see cref="AbstractControl.OnLayout"/>.
        /// </remarks>
        /// <param name="container">Container control which children will be processed.</param>
        /// <param name="layout">Layout style to use.</param>
        /// <param name="getBounds">Returns rectangle in which layout is performed.</param>
        /// <param name="items">List of controls to layout.</param>
        public virtual void DefaultOnLayout(
            AbstractControl container,
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

            if (container.DebugIdentifier is not null)
            {
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
        /// Retrieves the size of a rectangular area into which a control can
        /// be fitted, in device-independent units.
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
        public virtual SizeD DefaultGetPreferredSize(
            AbstractControl container,
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
        /// Calculates horizontal <see cref="AlignedPosition"/> using align parameters.
        /// </summary>
        /// <param name="layoutBounds">Rectangle in which alignment is performed.</param>
        /// <param name="childControl">Control to align.</param>
        /// <param name="childPreferredSize">Preferred size.</param>
        /// <param name="alignment">Alignment of the control.</param>
        /// <returns></returns>
        public virtual AlignedPosition AlignHorizontal(
                    RectD layoutBounds,
                    ILayoutItem childControl,
                    SizeD childPreferredSize,
                    HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return new AlignedPosition(
                        layoutBounds.Left + childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Center:
                    return new AlignedPosition(
                        layoutBounds.Left +
                        ((layoutBounds.Width
                        - (childPreferredSize.Width + childControl.Margin.Horizontal)) / 2) +
                        childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Right:
                    return new AlignedPosition(
                        layoutBounds.Right -
                        childControl.Margin.Right - childPreferredSize.Width,
                        childPreferredSize.Width);
                case HorizontalAlignment.Stretch:
                default:
                    return new AlignedPosition(
                        layoutBounds.Left + childControl.Margin.Left,
                        layoutBounds.Width - childControl.Margin.Horizontal);
            }
        }

        /// <summary>
        /// Calculates vertical <see cref="AlignedPosition"/> using align parameters.
        /// </summary>
        /// <param name="layoutBounds">Rectangle in which alignment is performed.</param>
        /// <param name="control">Control to align.</param>
        /// <param name="childPreferredSize">Preferred size.</param>
        /// <param name="alignment">Alignment of the control.</param>
        /// <returns></returns>
        public virtual AlignedPosition AlignVertical(
            RectD layoutBounds,
            ILayoutItem control,
            SizeD childPreferredSize,
            VerticalAlignment alignment)
        {
            switch (alignment)
            {
                case VerticalAlignment.Top:
                    return new AlignedPosition(
                        layoutBounds.Top + control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Center:
                    return new AlignedPosition(
                        layoutBounds.Top +
                        ((layoutBounds.Height
                        - (childPreferredSize.Height + control.Margin.Vertical)) / 2) +
                        control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Bottom:
                    return new AlignedPosition(
                        layoutBounds.Bottom - control.Margin.Bottom - childPreferredSize.Height,
                        childPreferredSize.Height);
                case VerticalAlignment.Stretch:
                default:
                    return new AlignedPosition(
                        layoutBounds.Top + control.Margin.Top,
                        layoutBounds.Height - control.Margin.Vertical);
            }
        }

        public virtual void PerformDefaultLayout(
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
                    horizontalPosition.Origin,
                    verticalPosition.Origin,
                    horizontalPosition.Size,
                    verticalPosition.Size);
            }
        }

        public virtual SizeD GetPreferredSizeDefaultLayout(
            AbstractControl container,
            PreferredSizeContext context)
        {
            if (container.HasChildren)
                return container.GetBestSizeWithChildren(context);
            return container.GetBestSizeWithPadding(context);
        }

        /// <summary>
        /// Contains location and size calculated by the align method.
        /// </summary>
        public class AlignedPosition
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AlignedPosition"/> class.
            /// </summary>
            /// <param name="origin">Location.</param>
            /// <param name="size">Size.</param>
            public AlignedPosition(Coord origin, Coord size)
            {
                Origin = origin;
                Size = size;
            }

            /// <summary>
            /// Gets location.
            /// </summary>
            public Coord Origin { get; }

            /// <summary>
            /// Gets size.
            /// </summary>
            public Coord Size { get; }
        }
    }
}
