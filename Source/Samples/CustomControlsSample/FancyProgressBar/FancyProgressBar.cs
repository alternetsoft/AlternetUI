using Alternet.UI;
using CustomControlsSample.Gauge;

namespace CustomControlsSample
{
    public class FancyProgressBar : ProgressBar
    {
        public new FancyProgressBarHandler Handler => (FancyProgressBarHandler)base.Handler;

        protected override ControlHandler CreateHandler()
        {
            return new FancyProgressBarHandler();
        }
    }
}