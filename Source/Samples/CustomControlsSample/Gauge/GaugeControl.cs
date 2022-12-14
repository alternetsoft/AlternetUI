using Alternet.UI;
using CustomControlsSample.Gauge;

namespace CustomControlsSample
{
    public class GaugeControl : ProgressBar
    {
        public new GaugeHandler Handler => (GaugeHandler)base.Handler;

        protected override ControlHandler CreateHandler()
        {
            return new GaugeHandler();
        }
    }
}