using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    public class CustomDrawControl : UserPaintControl
    {
        private readonly Control leftControl = new();
        private readonly Control fillControl = new();

        public CustomDrawControl()
            : base()
        {
            leftControl.Size = new(50, 50);
            fillControl.Size = new(50, 50);
            leftControl.Background = new SolidBrush(Color.Green);
            fillControl.Background = new SolidBrush(Color.Red);
            Children.Add(leftControl);
            Children.Add(fillControl);
        }

        internal ITestPageSite? Site {get;set;}

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Site?.LogEvent("cdc", $"l: {leftControl.Bounds}");
            Site?.LogEvent("cdc", $"f: {fillControl.Bounds}");
            LayoutFactory.PerformLayoutLeftFill(this, leftControl, fillControl);
            Site?.LogEvent("cdc", $"l: {leftControl.Bounds}");
            Site?.LogEvent("cdc", $"f: {fillControl.Bounds}");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;
            var bounds = DrawClientRectangle;

            var brush = this.Background;
            if (brush != null)
                dc.FillRectangle(brush, bounds);
            //dc.FillRectangle(leftControl.Background!, leftControl.Bounds);
            //dc.FillRectangle(fillControl.Background!, fillControl.Bounds);
        }

    }
}
