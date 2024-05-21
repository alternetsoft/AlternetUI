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

            var font = Font.Default
                .Scaled(3).GetWithStyle(FontStyle.Underline | FontStyle.Bold | FontStyle.Strikeout);

            dc.DrawText(
                "hello text",
                (0, 0),
                font,
                Color.Black,
                Color.LightGreen);

            dc.DrawText(
                "hello text 2",
                (50, 150),
                font.Scaled(2),
                Color.Black,
                Color.LightGreen);

            dc.SetPixel(0, 0, Color.Red);
            dc.SetPixel(50, 150, Color.Red);
        }
    }
}
