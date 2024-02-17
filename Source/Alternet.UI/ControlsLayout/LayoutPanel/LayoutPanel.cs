using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls using different methods.
    /// </summary>
    /// <remarks>
    /// Currently only default layout method is implemeted.
    /// Use <see cref="Control.Dock"/> to specify child controls dock style.
    /// If it dock style is not specified, controls are positioned absolutely using
    /// <see cref="Control.Bounds"/>.
    /// </remarks>
    [ControlCategory("Containers")]
    public partial class LayoutPanel : Control
    {
        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.LayoutPanel;

        internal static void SetAnchor(Control control, AnchorStyles value)
        {
            if (control == null)
                return;
            control.ExtendedProps.Anchor = value;
        }

        internal static AnchorStyles GetAnchor(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return AnchorStyles.LeftTop;
            return control.ExtendedProps.Anchor;
        }

        internal static void SetDistanceRight(Control control, double value)
        {
            if (control == null)
                return;
            control.ExtendedProps.DistanceRight = value;
        }

        internal static double GetDistanceRight(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return 0;
            return control.ExtendedProps.DistanceRight;
        }

        internal static void SetDistanceBottom(Control control, double value)
        {
            if (control == null)
                return;
            control.ExtendedProps.DistanceBottom = value;
        }

        internal static double GetDistanceBottom(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return 0;
            return control.ExtendedProps.DistanceBottom;
        }

        internal static void SetAutoSizeMode(Control control, AutoSizeMode value)
        {
            if (control == null)
                return;
            control.ExtendedProps.AutoSizeMode = value;
        }

        /// <summary>
        /// Sets a value that indicates whether the control
        /// resizes based on its contents.
        /// </summary>
        /// <param name="control">Control instance for which property
        /// value is set.</param>
        /// <param name="value">New value for the AutoSize property</param>
        internal static void SetAutoSize(Control control, bool value)
        {
            if (control == null)
                return;
            control.ExtendedProps.AutoSize = value;
        }

        /// <summary>
        /// Gets a value that indicates whether the control
        /// resizes based on its contents.
        /// </summary>
        /// <param name="control">Control instance for which property
        /// value is returned.</param>
        /// <returns>true if auto sizing is enabled; otherwise, false.</returns>
        internal static bool GetAutoSize(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return true;
            return control.ExtendedProps.AutoSize;
        }

        internal static AutoSizeMode GetAutoSizeMode(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return AutoSizeMode.GrowOnly;
            return control.ExtendedProps.AutoSizeMode;
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

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Dock;
        }
    }
}
