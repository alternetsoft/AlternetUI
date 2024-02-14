using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ProgressBar"/> with custom painted fancy look.
    /// </summary>
    public partial class FancyProgressBar : ProgressBar
    {
        internal new FancyProgressBarHandler Handler => (FancyProgressBarHandler)base.Handler;

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return new SizeD(200, 100);
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new FancyProgressBarHandler();
        }
    }
}