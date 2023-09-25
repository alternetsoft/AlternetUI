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
        public CustomDrawControl()
            : base()
        {
        }

        internal ITestPageSite? Site { get; set; }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.DrawingContext;
            var bounds = DrawClientRectangle;

            var brush = this.Background;
            if (brush != null)
                dc.FillRectangle(brush, bounds);
        }
    }
}
