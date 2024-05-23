using System;
using System.ComponentModel;

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
        private StackPanelOrientation orientation;

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
        protected virtual void OnOrientationChanged(EventArgs e) =>
            OrientationChanged?.Invoke(this, e);
    }
}