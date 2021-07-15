using System;

namespace Alternet.UI
{
    /// <summary>
    /// Arranges child controls into a single line that can be oriented horizontally or vertically.
    /// </summary>
    public class StackPanel : Control
    {
        private StackPanelOrientation orientation;

        /// <summary>
        /// Occurs when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        public event EventHandler? OrientationChanged;

        /// <summary>
        /// Gets or sets a value that indicates the dimension by which child controls are stacked.
        /// </summary>
        public StackPanelOrientation Orientation
        {
            get => orientation;

            set
            {
                if (orientation == value)
                    return;

                orientation = value;

                PerformLayout();
                OnOrientationChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Orientation"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnOrientationChanged(EventArgs e) => OrientationChanged?.Invoke(this, e);
    }
}