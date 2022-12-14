#nullable disable

using Alternet.Drawing;
using Alternet.UI;
using System;
using System.ComponentModel;

namespace CustomControlsSample.Gauge
{
    public class GaugeHandler : ProgressBarHandler
    {
        protected override bool NeedsPaint => true;

        public override bool IsIndeterminate { get; set; }
        public override ProgressBarOrientation Orientation { get; set; }

        public override Size GetPreferredSize(Size availableSize)
        {
            return new Size(50, 50);
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            UserPaint = true;
            Control.ValueChanged += Control_ValueChanged;
        }

        protected override void OnDetach()
        {
            Control.ValueChanged -= Control_ValueChanged;

            base.OnDetach();
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        public override void OnPaint(DrawingContext dc)
        {

        }
    }
}