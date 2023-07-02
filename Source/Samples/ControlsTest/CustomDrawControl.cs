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
            leftControl.Bounds = new(0, 0, 50, 50);
            fillControl.Bounds = new(0, 60, 50, 50);
            leftControl.Background = new SolidBrush(Color.Green);
            fillControl.Background = new SolidBrush(Color.Red);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;
            var bounds = ClientRectangle;
            LayoutFactory.PerformLayoutLeftFill(this, leftControl, fillControl);

            var brush = this.Background;
            if (brush != null)
                dc.FillRectangle(brush, bounds);
            dc.FillRectangle(leftControl.Background!, leftControl.Bounds);
            dc.FillRectangle(fillControl.Background!, fillControl.Bounds);
        }

    }
}
