using Alternet.UI;
using System;
using System.Collections.Generic;
using Alternet.Drawing;
using System.Linq;

namespace DrawingSample
{
    internal sealed class TextPage : DrawingPage
    {
        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nSuspendisse tincidunt orci vitae arcu congue commodo.\nProin fermentum rhoncus dictum.";

        private static Font fontInfoFont = new Font(FontFamily.GenericMonospace, 8);
        private static Brush fontInfoBrush = Brushes.Black;
        private Paragraph[]? paragraphs;
        private float fontSize = 10;
        private FontStyle fontStyle;
        private string customFontFamilyName = Control.DefaultFont.FontFamily.Name;

        public override string Name => "Text";

        public float FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                InvalidateParagraphs();
            }
        }

        public FontStyle FontStyle
        {
            get => fontStyle;
            set
            {
                fontStyle = value;
                InvalidateParagraphs();
            }
        }

        public string CustomFontFamilyName
        {
            get => customFontFamilyName;
            set
            {
                customFontFamilyName = value;
                InvalidateParagraphs();
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
                dc.DrawText(paragraph.FontInfo, fontInfoFont, fontInfoBrush, new PointF(x, y));
                y += dc.MeasureText(paragraph.FontInfo, fontInfoFont).Height + 3;

                dc.DrawText(LoremIpsum, paragraph.Font, new SolidBrush(color), new PointF(20, y));
                y += dc.MeasureText(LoremIpsum, paragraph.Font).Height + 20;

                var c = new Skybrud.Colors.RgbColor(color.R, color.G, color.B).Lighten(lighten).ToRgb();
                color = Color.FromArgb(c.R, c.G, c.B);
            }
        }

        protected override Control CreateSettingsControl() => new TextPageSettings(this);

        private void InvalidateParagraphs()
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
            Paragraph CreateGenericFontParagraph(GenericFontFamily genericFamily) =>
                new Paragraph(
                    new Font(new FontFamily(genericFamily), FontSize, FontStyle),
                    "Generic " + genericFamily.ToString());

            Paragraph CreateCustomFontParagraph(FontFamily family) =>
                new Paragraph(
                    new Font(family, FontSize, FontStyle),
                    "Custom");

            yield return CreateCustomFontParagraph(new FontFamily(CustomFontFamilyName));
            yield return CreateGenericFontParagraph(GenericFontFamily.Serif);
            yield return CreateGenericFontParagraph(GenericFontFamily.SansSerif);
            yield return CreateGenericFontParagraph(GenericFontFamily.Monospace);
        }

        private class Paragraph : IDisposable
        {
            public Paragraph(Font font, string genericFamilyName)
            {
                Font = font;
                GenericFamilyName = genericFamilyName;
                FontInfo = $"{GenericFamilyName}: {Font.Name}, {Font.SizeInPoints}pt";
            }

            public Font Font { get; }

            public string GenericFamilyName { get; }

            public string FontInfo { get; }

            public void Dispose()
            {
                Font.Dispose();
            }
        }
    }
}