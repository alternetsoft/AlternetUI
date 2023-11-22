using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
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
    public class LayoutPanel : Control
    {
        private LayoutPanelKind layout;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.LayoutPanel;

        /// <summary>
        /// Gets or sets layout style of the child controls.
        /// </summary>
        public LayoutPanelKind Layout
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

        internal new NativeLayoutPanelHandler Handler =>
            (NativeLayoutPanelHandler)base.Handler;

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

        /// <summary>
        /// Sets the minimum size for the control.
        /// </summary>
        /// <param name="control">Control instance for which minimal
        /// size is set.</param>
        /// <param name="value">New minimal size value.</param>
        public static void SetMinSize(Control control, Size value)
        {
            if (control == null)
                return;
            control.ExtendedProps.MinimumSize = value;
        }

        /// <summary>
        /// Gets the minimum size for the control.
        /// </summary>
        /// <param name="control">Control instance for which minimal
        /// size is returned.</param>
        /// <returns>An ordered pair of type <see cref="Size"/> representing the
        /// width and height of a rectangle.</returns>
        public static Size GetMinSize(Control control)
        {
            if (control == null || !control.HasExtendedProps)
                return Size.Empty;
            return control.ExtendedProps.MinimumSize;
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
        protected override ControlHandler CreateHandler()
        {
            return new NativeLayoutPanelHandler();
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
