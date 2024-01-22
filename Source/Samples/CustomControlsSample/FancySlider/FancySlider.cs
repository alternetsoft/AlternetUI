using Alternet.UI;

namespace CustomControlsSample
{
    public class FancySlider : Slider
    {
        public new FancySliderHandler Handler => (FancySliderHandler)base.Handler;

        protected override ControlHandler CreateHandler()
        {
            return new FancySliderHandler();
        }
    }
}