using System;
using System.ComponentModel;
using System.Diagnostics;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls into a single line that can be oriented
    /// horizontally or vertically.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class StackPanel : ContainerControl
    {
        /// <summary>
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static new bool ShowDebugCorners = false;

        private StackPanelOrientation orientation;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public StackPanel(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanel"/> class.
        /// </summary>
        public StackPanel()
        {
            CanSelect = false;
            TabStop = false;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event EventHandler? OrientationChanged;

        /// <summary>
        /// Gets or sets a value that indicates the dimension by which child
        /// controls are stacked.
        /// </summary>
        public virtual StackPanelOrientation Orientation
        {
            get => orientation;

            set
            {
                if (orientation == value)
                    return;

                orientation = value;

                OnOrientationChanged(EventArgs.Empty);
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="Orientation"/> is horizontal.
        /// </summary>
        [Browsable(false)]
        public bool IsHorizontal
        {
            get => Orientation == StackPanelOrientation.Horizontal;
            set => Orientation = StackPanelOrientation.Horizontal;
        }

        /// <summary>
        /// Gets or sets whether <see cref="Orientation"/> is vertical.
        /// </summary>
        [Browsable(false)]
        public bool IsVertical
        {
            get => Orientation == StackPanelOrientation.Vertical;
            set => Orientation = StackPanelOrientation.Vertical;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.StackPanel;

        /// <inheritdoc/>
        [Browsable(false)]
        public override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
            DefaultPaintDebug(e);
        }

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            if (IsVertical)
                return LayoutStyle.Vertical;
            else
                return LayoutStyle.Horizontal;
        }

        /// <summary>
        /// Called when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnOrientationChanged(EventArgs e)
        {
            OrientationChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void DefaultPaintDebug(PaintEventArgs e)
        {
            if (ShowDebugCorners)
                BorderSettings.DrawDesignCorners(e.Graphics, e.ClipRectangle);
        }
    }
}