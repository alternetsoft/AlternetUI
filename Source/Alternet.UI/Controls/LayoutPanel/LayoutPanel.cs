using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class LayoutPanel : Control
    {
        internal new NativeLayoutPanelHandler Handler =>
            (NativeLayoutPanelHandler)base.Handler;

        public static void SetDock(Control control, DockStyle value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            control.ExtendedProps.Dock = value;
        }

        public static DockStyle GetDock(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.ExtendedProps == null)
                return DockStyle.None;
            return control.ExtendedProps.Dock;
        }

        public static void SetAutoSize(Control control, bool value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            control.ExtendedProps.AutoSize = value;
        }

        public static bool GetAutoSize(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.ExtendedProps == null)
                return true;
            return control.ExtendedProps.AutoSize;
        }

        public static void SetMinimumSize(Control control, Size value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            control.ExtendedProps.MinimumSize = value;
        }

        public static Size GetMinimumSize(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.ExtendedProps == null)
                return Size.Empty;
            return control.ExtendedProps.MinimumSize;
        }

        public static void SetAnchor(Control control, AnchorStyles value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            control.ExtendedProps.Anchor = value;
        }

        public static AnchorStyles GetAnchor(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.ExtendedProps == null)
                return AnchorStyles.Top | AnchorStyles.Left;
            return control.ExtendedProps.Anchor;
        }

        public static void SetDistanceRight(Control control, double value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            control.ExtendedProps.DistanceRight = value;
        }

        public static double GetDistanceRight(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.ExtendedProps == null)
                return 0;
            return control.ExtendedProps.DistanceRight;
        }

        public static void SetDistanceBottom(Control control, double value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            control.ExtendedProps.DistanceBottom = value;
        }

        public static double GetDistanceBottom(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.ExtendedProps == null)
                return 0;
            return control.ExtendedProps.DistanceBottom;
        }

        public static void SetAutoSizeMode(Control control, AutoSizeMode value)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            control.ExtendedProps.AutoSizeMode = value;
        }

        public static AutoSizeMode GetAutoSizeMode(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control.ExtendedProps == null)
                return AutoSizeMode.GrowOnly;
            return control.ExtendedProps.AutoSizeMode;
        }

        /// <inheritdoc />
        protected override ControlHandler CreateHandler()
        {
            return new NativeLayoutPanelHandler();
        }

        /// <inheritdoc />
        protected override void OnChildInserted(int childIndex, Control childControl)
        {
            base.OnChildInserted(childIndex, childControl);
        }

        /// <inheritdoc />
        protected override void OnChildRemoved(int childIndex, Control childControl)
        {
            base.OnChildRemoved(childIndex, childControl);
        }

        /// <inheritdoc />
        protected override void OnLayout()
        {
            base.OnLayout();
        }
    }
}
