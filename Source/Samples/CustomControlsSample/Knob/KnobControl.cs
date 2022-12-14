using Alternet.UI;

namespace CustomControlsSample
{
    public class KnobControl : Slider
    {
        public new KnobHandler Handler => (KnobHandler)base.Handler;

        protected override ControlHandler CreateHandler()
        {
            return new KnobHandler();
        }
    }
}