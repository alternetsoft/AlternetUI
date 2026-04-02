using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class LayoutManager
    {
        /// <summary>
        /// Arranges the specified child layout item within the given bounds according to the provided dock style,
        /// updating the bounds to reflect the remaining available space.
        /// </summary>
        /// <remarks>If the dock style specifies an auto-size variant, the child's preferred size is used
        /// for layout. When the UseMarginsWhenDock flag is set, the child's margins are applied during arrangement.
        /// This method is typically used by container layouts to sequentially arrange multiple children.</remarks>
        /// <param name="bounds">A reference to the bounding rectangle in which to arrange the child. Updated to represent the remaining
        /// space after layout.</param>
        /// <param name="child">The layout item to be positioned within the bounds according to the specified dock style.</param>
        /// <param name="value">The dock style that determines how the child is positioned and sized within the bounds.</param>
        /// <param name="containerFlags">Flags that influence layout behavior, such as whether to apply margins when docking.</param>
        public virtual void LayoutWhenDocked(ref RectD bounds, ILayoutItem child, DockStyle value, LayoutFlags containerFlags)
        {
            SizeD child_size = child.Bounds.Size;
            var space = bounds;

            var useMargins = containerFlags.HasFlag(LayoutFlags.UseMarginsWhenDock);

            Thickness GetMargin(bool autoSize)
            {
                if (autoSize || useMargins)
                {
                    return child.Margin;
                }
                else
                {
                    return Thickness.Empty;
                }
            }

            switch (value)
            {
                case DockStyle.Left:
                    LayoutLeft(autoSize: false);
                    break;
                case DockStyle.Top:
                    LayoutTop(autoSize: false);
                    break;
                case DockStyle.Right:
                    LayoutRight(autoSize: false);
                    break;
                case DockStyle.Bottom:
                    LayoutBottom(autoSize: false);
                    break;
                case DockStyle.Fill:
                    LayoutFill(autoSize: false);
                    break;
                case DockStyle.RightAutoSize:
                    LayoutRight(autoSize: true);
                    break;
                case DockStyle.LeftAutoSize:
                    LayoutLeft(autoSize: true);
                    break;
                case DockStyle.TopAutoSize:
                    LayoutTop(autoSize: true);
                    break;
                case DockStyle.BottomAutoSize:
                    LayoutBottom(autoSize: true);
                    break;
            }

            bounds = space;

            void LayoutTop(bool autoSize)
            {
                var margin = GetMargin(autoSize);

                if (autoSize)
                {
                    child_size = child.GetPreferredSizeLimited(new SizeD(space.Width, child_size.Height));
                }
                else
                {
                }

                child.SetBounds(
                    space.Left + margin.Left,
                    space.Top + margin.Top,
                    space.Width - margin.Horizontal,
                    child_size.Height,
                    BoundsSpecified.All);
                var delta = child_size.Height + margin.Vertical;
                space.Y += delta;
                space.Height -= delta;
            }

            void LayoutRight(bool autoSize)
            {
                var margin = GetMargin(autoSize);

                if (autoSize)
                {
                    child_size = child.GetPreferredSizeLimited(new SizeD(child_size.Width, space.Height));
                }
                else
                {
                }

                child.SetBounds(
                    space.Right - child_size.Width - margin.Right,
                    space.Top + margin.Top,
                    child_size.Width,
                    space.Height - margin.Vertical,
                    BoundsSpecified.All);
                space.Width -= child_size.Width + margin.Horizontal;
            }

            void LayoutLeft(bool autoSize)
            {
                var margin = GetMargin(autoSize);

                if (autoSize)
                {
                    child_size = child.GetPreferredSizeLimited(new SizeD(child_size.Width, space.Height));
                }
                else
                {
                }

                child.SetBounds(
                    space.Left + margin.Left,
                    space.Top + margin.Top,
                    child_size.Width,
                    space.Height - margin.Vertical,
                    BoundsSpecified.All);
                var delta = child_size.Width + margin.Horizontal;
                space.X += delta;
                space.Width -= delta;
            }

            void LayoutBottom(bool autoSize)
            {
                var margin = GetMargin(autoSize);

                if (autoSize)
                {
                    child_size = child.GetPreferredSizeLimited(new SizeD(space.Width, child_size.Height));
                }
                else
                {
                }

                child.SetBounds(
                    space.Left + margin.Left,
                    space.Bottom - child_size.Height - margin.Bottom,
                    space.Width - margin.Horizontal,
                    child_size.Height,
                    BoundsSpecified.All);
                space.Height -= child_size.Height + margin.Vertical;
            }

            void LayoutFill(bool autoSize)
            {
                var margin = GetMargin(autoSize);

                child_size = new SizeD(space.Width, space.Height);
                if (autoSize)
                {
                    child_size = child.GetPreferredSizeLimited(child_size);
                }
                else
                {
                }

                child.SetBounds(
                    space.Left + margin.Left,
                    space.Top + margin.Top,
                    child_size.Width - margin.Horizontal,
                    child_size.Height - margin.Vertical,
                    BoundsSpecified.All);
            }
        }

        private SizeD GetPreferredSizeDockLayout(ILayoutItem container, PreferredSizeContext context)
        {
            if (container.HasChildren)
                return GetBestSizeWithChildren(container, context);
            return container.GetBestSizeWithPadding(context);
        }

        /// <summary>
        /// Arranges the child controls of a container according to their docking styles, updating the available bounds
        /// to reflect the remaining space after docking.
        /// </summary>
        /// <remarks>Fill-docked controls are not counted when updating the remaining bounds. The method
        /// processes children in either forward or reverse order depending on the container's layout flags, which may
        /// affect the visual stacking of docked controls.</remarks>
        /// <param name="container">The container control whose child controls are to be arranged based on their docking styles.</param>
        /// <param name="bounds">The bounding rectangle representing the available layout space. On return, this value is updated to reflect
        /// the remaining space after docking non-fill child controls.</param>
        /// <param name="children">A read-only list of child controls to be arranged within the container.</param>
        /// <returns>The number of child controls that were docked during the layout operation.</returns>
        // On return, 'bounds' has an empty space left after docking the controls to sides
        // of the container (fill controls are not counted).
        // On return, 'result' has number of controls with Dock != None.
        private int LayoutWhenDocked(
            ILayoutItem container,
            ref RectD bounds,
            IReadOnlyList<ILayoutItem> children)
        {
            var result = 0;

            var space = bounds;

            var layoutOffset = container.LayoutOffset;
            space.Offset(-layoutOffset.X, -layoutOffset.Y);

            var containerFlags = container.LayoutFlags;

            if (containerFlags.HasFlag(LayoutFlags.IterateBackward))
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
                var child = children[index];
                DockStyle dock = child.Dock;

                if (dock == DockStyle.None || ChildIgnoresLayout(child))
                    return;

                result++;

                LayoutWhenDocked(ref space, child, dock, containerFlags);
            }

            bounds = space;
            return result;
        }
    }
}
