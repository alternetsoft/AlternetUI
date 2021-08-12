using Alternet.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FontFamily = Alternet.UI.FontFamily;

namespace DrawingSample
{
    public sealed class TextLinesLayer : Layer
    {
        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nSuspendisse tincidunt orci vitae arcu congue commodo.\nProin fermentum rhoncus dictum.";

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
            yield return Control.DefaultFont;
            yield return new Alternet.UI.Font(FontFamily.GenericSerif, 10);
            yield return new Alternet.UI.Font(FontFamily.GenericSansSerif, 10);
            yield return new Alternet.UI.Font(FontFamily.GenericMonospace, 10);
        }
    }
}