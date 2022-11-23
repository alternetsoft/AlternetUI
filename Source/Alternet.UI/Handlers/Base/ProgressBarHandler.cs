
namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ProgressBar"/> behavior and appearance.
    /// </summary>
    public abstract class ProgressBarHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new ProgressBar Control => (ProgressBar)base.Control;

        /// <summary>
        /// Gets or sets whether the <see cref="ProgressBar"/> shows actual values or generic, continuous progress
        /// feedback.
        /// </summary>
        public abstract bool IsIndeterminate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the horizontal or vertical orientation of the progress bar.
        /// </summary>
        /// <value>One of the <see cref="ProgressBarOrientation"/> values.</value>
        public abstract ProgressBarOrientation Orientation { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}