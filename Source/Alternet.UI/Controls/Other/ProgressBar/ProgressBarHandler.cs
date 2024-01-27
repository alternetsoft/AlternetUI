namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ProgressBar"/> behavior and appearance.
    /// </summary>
    internal abstract class ProgressBarHandler : ControlHandler
    {
        /// <summary>
        /// Gets a <see cref="ProgressBar"/> this handler provides the implementation for.
        /// </summary>
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
    }
}