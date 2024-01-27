namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ProgressBar"/> with custom painted fancy look.
    /// </summary>
    public class FancyProgressBar : ProgressBar
    {
        internal new FancyProgressBarHandler Handler => (FancyProgressBarHandler)base.Handler;

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new FancyProgressBarHandler();
        }
    }
}