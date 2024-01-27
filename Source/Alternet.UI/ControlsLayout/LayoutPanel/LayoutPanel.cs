﻿namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls using different methods.
    /// </summary>
    /// <remarks>
    /// Currently only default layout method is implemeted.
    /// Use <see cref="SetDock"/> to specify child controls dock style.
    /// If it dock style is not specified, controls are positioned absolutely using
    /// <see cref="Control.Bounds"/>.
    /// </remarks>
    [ControlCategory("Containers")]
    public partial class LayoutPanel : Control
    {
        private GenericLayoutStyle layout;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.LayoutPanel;

        /// <summary>
        /// Gets or sets layout style of the child controls.
        /// </summary>
        [Browsable(false)]
        public GenericLayoutStyle Layout
        {
            get
            {
                return layout;
            }

            set
            {
                if (layout == value)
                    return;
                layout = value;
                PerformLayout();
            }
        }

        internal new LayoutPanelHandler Handler =>
            (LayoutPanelHandler)base.Handler;

        /// <summary>
        /// Sets which control borders are docked to its parent control
        /// and determines how a control is resized with its parent.
        /// </summary>
        /// <param name="control">Control instance for which property
        /// value is set.</param>
        /// <param name="value">New Dock property value</param>
        public static void SetDock(Control control, DockStyle value)
        {
            if (control == null)
                return;
            control.ExtendedProps.Dock = value;
        }

        /// <summary>
        /// Gets which control borders are docked to its parent control
        /// and determines how a control is resized with its parent.
        /// </summary>
        /// <param name="control">Control instance for which property
        /// value is returned.</param>
        public static DockStyle GetDock(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return DockStyle.None;
            return control.ExtendedProps.Dock;
        }

        /// <summary>
        /// Sets a value that indicates whether the control
        /// resizes based on its contents.
        /// </summary>
        /// <param name="control">Control instance for which property
        /// value is set.</param>
        /// <param name="value">New value for the AutoSize property</param>
        public static void SetAutoSize(Control control, bool value)
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
        public static bool GetAutoSize(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return true;
            return control.ExtendedProps.AutoSize;
        }

        /// <inheritdoc />
        public override void OnLayout()
        {
            base.OnLayout();
        }

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

        internal static void PerformDockStyleLayout(Control control)
        {
            LayoutDockStyle.Layout(control, control.ChildrenLayoutBounds);
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

        internal static AutoSizeMode GetAutoSizeMode(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return AutoSizeMode.GrowOnly;
            return control.ExtendedProps.AutoSizeMode;
        }

        /// <inheritdoc />
        internal override ControlHandler CreateHandler()
        {
            return new LayoutPanelHandler();
        }

        /// <inheritdoc />
        protected override void OnChildInserted(Control childControl)
        {
            base.OnChildInserted(childControl);
        }

        /// <inheritdoc />
        protected override void OnChildRemoved(Control childControl)
        {
            base.OnChildRemoved(childControl);
        }
    }
}
