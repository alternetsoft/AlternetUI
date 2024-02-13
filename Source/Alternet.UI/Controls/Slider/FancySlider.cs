using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="Slider"/> with custom painted fancy look.
    /// </summary>
    public partial class FancySlider : Slider
    {
        internal new FancySliderHandler Handler => (FancySliderHandler)base.Handler;

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            return new SizeD(100, 100);
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return new FancySliderHandler();
        }
    }
}