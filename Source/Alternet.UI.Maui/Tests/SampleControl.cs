using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class SampleControl : UserControl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.Graphics;

            dc.DrawText(
                "hello text",
                (0, 0),
                Font.Default.Scaled(3),
                Color.White,
                Color.LightGreen);
        }
}
}
