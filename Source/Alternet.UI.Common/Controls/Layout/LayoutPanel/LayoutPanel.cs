using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls using different methods.
    /// </summary>
    /// <remarks>
    /// Currently only default layout method is implemeted.
    /// Use <see cref="AbstractControl.Dock"/> to specify child controls dock style.
    /// If it dock style is not specified, controls are positioned absolutely using
    /// <see cref="AbstractControl.Bounds"/>.
    /// </remarks>
    [ControlCategory("Containers")]
    public partial class LayoutPanel : ContainerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPanel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public LayoutPanel(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPanel"/> class.
        /// </summary>
        public LayoutPanel()
        {
            CanSelect = false;
            TabStop = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.LayoutPanel;

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        internal static void SetAnchor(AbstractControl control, AnchorStyles value)
        {
            if (control == null)
                return;
            control.ExtendedProps.Anchor = value;
        }

        internal static AnchorStyles GetAnchor(AbstractControl control)
        {
            if (control == null || !control.HasExtendedProps)
                return AnchorStyles.LeftTop;
            return control.ExtendedProps.Anchor;
        }

        internal static void SetDistanceRight(AbstractControl control, Coord value)
        {
            if (control == null)
                return;
            control.ExtendedProps.DistanceRight = value;
        }

        internal static Coord GetDistanceRight(AbstractControl control)
        {
            if (control == null || !control.HasExtendedProps)
                return 0;
            return control.ExtendedProps.DistanceRight;
        }

        internal static void SetDistanceBottom(AbstractControl control, Coord value)
        {
            if (control == null)
                return;
            control.ExtendedProps.DistanceBottom = value;
        }

        internal static Coord GetDistanceBottom(AbstractControl control)
        {
            if (control == null || !control.HasExtendedProps)
                return 0;
            return control.ExtendedProps.DistanceBottom;
        }

        internal static void SetAutoSizeMode(AbstractControl control, AutoSizeMode value)
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
        internal static void SetAutoSize(AbstractControl control, bool value)
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
        internal static bool GetAutoSize(AbstractControl control)
        {
            if (control == null || !control.HasExtendedProps)
                return true;
            return control.ExtendedProps.AutoSize;
        }

        internal static AutoSizeMode GetAutoSizeMode(AbstractControl control)
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
