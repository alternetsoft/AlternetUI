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
                (15, 15),
                Font.Default,
                Color.Red,
                Color.White);
        }
}
}
