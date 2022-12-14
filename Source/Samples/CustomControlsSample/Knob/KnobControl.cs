using Alternet.UI;

namespace CustomControlsSample
{
    public class KnobControl : Slider
    {
        protected override ControlHandler CreateHandler()
        {
            return new KnobHandler();
        }
    }
}