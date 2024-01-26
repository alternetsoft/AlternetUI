using System;

namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls into a single line that can be oriented
    /// horizontally or vertically.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class StackPanel : Control
    {
        private StackPanelOrientation orientation;
        private bool allowStretch;

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event EventHandler? OrientationChanged;

        /// <summary>
        /// Gets or sets whether last child control can be stretched
        /// if it's alignment property is set to Stretch.
        /// </summary>
        /// <remarks>
        /// Currently this is implemented only for vertically aligned <see cref="StackPanel"/>.
        /// </remarks>
        public virtual bool AllowStretch
        {
            get => allowStretch;
            set
            {
                if (allowStretch == value)
                    return;
                allowStretch = value;
                PerformLayout(false);
            }
        }

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

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.StackPanel;

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateStackPanelHandler(this);
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