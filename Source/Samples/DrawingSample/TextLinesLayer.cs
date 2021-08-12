using Alternet.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DrawingSample
{
    public sealed class TextLinesLayer : Layer
    {
        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nSuspendisse tincidunt orci vitae arcu congue commodo.\nProin fermentum rhoncus dictum.\nPhasellus mattis nibh eu euismod placerat.\nNam luctus euismod ex, vel aliquam libero auctor placerat.\nDonec et dignissim justo, eget dignissim massa.\nDonec dapibus, erat ut feugiat sollicitudin, est elit malesuada massa, ac convallis mi turpis auctor elit.\nIn semper ullamcorper purus et ultrices.\nQuisque mollis mauris sit amet enim sollicitudin vulputate.\nDonec eu tortor tincidunt ligula tempor semper ut sit amet velit.\nIn mollis libero dolor, ut ultrices lectus elementum at.\nProin eget nisi in arcu semper bibendum vel efficitur mi.";

        private static readonly Alternet.UI.Font[] fonts;

        static TextLinesLayer()
        {
            fonts = CreateFonts().ToArray();
        }

        public override string Name => "Text Lines";

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            var color = Color.MidnightBlue;
            float lighten = 10;

            float y = 20;
            foreach (var font in fonts)
            {
                dc.DrawText(LoremIpsum, new PointF(20, y), font, color);
                y += dc.MeasureText(LoremIpsum, font).Height + 20;
                var c = new Skybrud.Colors.RgbColor(color.R, color.G, color.B).Lighten(lighten).ToRgb();
                color = Color.FromArgb(c.R, c.G, c.B);
            }
        }

        private static IEnumerable<Alternet.UI.Font> CreateFonts()
        {
            for (float size = 5; size < 30; size += 5)
            {
                yield return new Alternet.UI.Font("Times New Roman", size);
            }
        }
    }
}