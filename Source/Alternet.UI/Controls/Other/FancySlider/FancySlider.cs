using Alternet.UI;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="Slider"/> with custom painted fancy look.
    /// </summary>
    public class FancySlider : Slider
    {
        internal new FancySliderHandler Handler => (FancySliderHandler)base.Handler;

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new FancySliderHandler();
        }
    }
}