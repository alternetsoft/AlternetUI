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
    public partial class LayoutPanel : ContainerControl
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

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Dock;
        }
    }
}
