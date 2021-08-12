using Alternet.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FontFamily = Alternet.UI.FontFamily;

namespace DrawingSample
{
    sealed class TextPage : DrawingPage
    {
        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nSuspendisse tincidunt orci vitae arcu congue commodo.\nProin fermentum rhoncus dictum.";

        private Alternet.UI.Font[]? fonts;
        private float fontSize = 10;

        public override string Name => "Text Lines";

        public float FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                InvalidateFonts();
            }
        }

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            if (fonts == null)
                fonts = CreateFonts().ToArray();

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

        private void InvalidateFonts()
        {
            if (fonts != null)
            {
                foreach (var font in fonts)
                    font.Dispose();
                fonts = null;
            }

            Canvas?.Update();
        }

        private IEnumerable<Alternet.UI.Font> CreateFonts()
        {
            //yield return Control.DefaultFont;
            yield return new Alternet.UI.Font(FontFamily.GenericSerif, FontSize);
            yield return new Alternet.UI.Font(FontFamily.GenericSansSerif, FontSize);
            yield return new Alternet.UI.Font(FontFamily.GenericMonospace, FontSize);
        }

        protected override Control CreateSettingsControl() => new TextPageSettings(this);
    }
}