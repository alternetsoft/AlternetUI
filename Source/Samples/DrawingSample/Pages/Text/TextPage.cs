using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Font = Alternet.UI.Font;
using FontFamily = Alternet.UI.FontFamily;

namespace DrawingSample
{
    sealed class TextPage : DrawingPage
    {
        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nSuspendisse tincidunt orci vitae arcu congue commodo.\nProin fermentum rhoncus dictum.";

        private Paragraph[]? paragraphs;

        private static Font fontInfoFont = new Alternet.UI.Font(FontFamily.GenericMonospace, 8);
        private static Color fontInfoColor = Color.Black;

        private float fontSize = 10;

        public override string Name => "Text Lines";

        public float FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                InvalidatParagraphs();
            }
        }

        class Paragraph : IDisposable
        {
            public Paragraph(Font font, string genericFamilyName)
            {
                Font = font;
                GenericFamilyName = genericFamilyName;
                FontInfo = $"Generic {GenericFamilyName}: {Font.Name}, {Font.SizeInPoints}pt";
            }

            public Font Font { get; }
            public string GenericFamilyName { get; }
            public string FontInfo { get; }

            public void Dispose()
            {
                Font.Dispose();
            }
        }

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            if (paragraphs == null)
                paragraphs = CreateParagraphs().ToArray();

            var color = Color.MidnightBlue;
            float lighten = 10;

            float x = 20;
            float y = 20;
            foreach (var paragraph in paragraphs)
            {
                dc.DrawText(paragraph.FontInfo, new PointF(x, y), fontInfoFont, fontInfoColor);
                y += dc.MeasureText(paragraph.FontInfo, fontInfoFont).Height + 3;

                dc.DrawText(LoremIpsum, new PointF(20, y), paragraph.Font, color);
                y += dc.MeasureText(LoremIpsum, paragraph.Font).Height + 20;

                var c = new Skybrud.Colors.RgbColor(color.R, color.G, color.B).Lighten(lighten).ToRgb();
                color = Color.FromArgb(c.R, c.G, c.B);
            }
        }

        private void InvalidatParagraphs()
        {
            if (paragraphs != null)
            {
                foreach (var paragraph in paragraphs)
                    paragraph.Dispose();
                paragraphs = null;
            }

            Canvas?.Update();
        }

        private IEnumerable<Paragraph> CreateParagraphs()
        {
            Paragraph CreateParagraph(GenericFontFamily genericFamily) =>
                new Paragraph(
                    new Font(new FontFamily(genericFamily), FontSize),
                    genericFamily.ToString());

            yield return CreateParagraph(GenericFontFamily.Serif);
            yield return CreateParagraph(GenericFontFamily.SansSerif);
            yield return CreateParagraph(GenericFontFamily.Monospace);
        }

        protected override Control CreateSettingsControl() => new TextPageSettings(this);
    }
}