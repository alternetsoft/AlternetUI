using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI;

namespace DrawingSample
{
    internal sealed class TextPage : DrawingPage
    {
        private const string LoremIpsum =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit." +
            " Suspendisse tincidunt orci vitae arcu congue commodo. Proin fermentum rhoncus dictum.";

        private static readonly Brush fontInfoBrush = Brushes.Black;
        private static readonly Pen textWidthLimitPen = new(Color.Gray, 1, DashStyle.Dash);
        private Paragraph[]? paragraphs;
        private FontStyle fontStyle;
        private string customFontFamilyName = Control.DefaultFont.FontFamily.Name;

        private int textWidthLimit = 450;
        private int textHeightValue = 40;

        private bool textWidthLimitEnabled = true;
        private bool textHeightSet = false;

        private TextHorizontalAlignment horizontalAlignment = TextHorizontalAlignment.Left;

        private TextVerticalAlignment verticalAlignment = TextVerticalAlignment.Top;

        private TextTrimming trimming = TextTrimming.Pixel;

        private TextWrapping wrapping = TextWrapping.Character;

        private static readonly Font fontInfoFont;
        private static double fontSize;

        static TextPage()
        {
            var defaultSize = Control.DefaultFont.SizeInPoints;
            fontInfoFont = new(FontFamily.GenericMonospace, defaultSize);
            fontSize = defaultSize;
        }

        public override string Name => "Text";

        public double FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                InvalidateParagraphs();
            }
        }

        public bool Bold
        {
            get => GetFontStyle(FontStyle.Bold);
            set => SetFontStyle(FontStyle.Bold, value);
        }

        public bool Italic
        {
            get => GetFontStyle(FontStyle.Italic);
            set => SetFontStyle(FontStyle.Italic, value);
        }

        public bool Underlined
        {
            get => GetFontStyle(FontStyle.Underlined);
            set => SetFontStyle(FontStyle.Underlined, value);
        }

        public bool Strikethrough
        {
            get => GetFontStyle(FontStyle.Strikethrough);
            set => SetFontStyle(FontStyle.Strikethrough, value);
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

        public int TextWidthLimit
        {
            get => textWidthLimit;
            set
            {
                textWidthLimit = value;
                Invalidate();
            }
        }

        public int TextHeightValue
        {
            get => textHeightValue;
            set
            {
                textHeightValue = value;
                Invalidate();
            }
        }

#pragma warning disable
        public int MinTextWidthLimit => 100;
        public int MaxTextWidthLimit => 1000;
        public int MinTextHeightValue => 20;
        public int MaxTextHeightValue => 200;
#pragma warning restore

        public bool TextWidthLimitEnabled
        {
            get => textWidthLimitEnabled;
            set
            {
                textWidthLimitEnabled = value;
                Invalidate();
            }
        }

        public bool TextHeightSet
        {
            get => textHeightSet;
            set
            {
                textHeightSet = value;
                Invalidate();
            }
        }

        public TextHorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                horizontalAlignment = value;
                Invalidate();
            }
        }

        public TextVerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                verticalAlignment = value;
                Invalidate();
            }
        }

        public TextTrimming Trimming
        {
            get => trimming;
            set
            {
                trimming = value;
                Invalidate();
            }
        }

        public TextWrapping Wrapping
        {
            get => wrapping;
            set
            {
                wrapping = value;
                Invalidate();
            }
        }

        private FontStyle FontStyle
        {
            get => fontStyle;
            set
            {
                fontStyle = value;
                InvalidateParagraphs();
            }
        }

        public override void Draw(DrawingContext dc, RectD bounds)
        {
            paragraphs ??= CreateParagraphs().ToArray();

            var color = Color.MidnightBlue;
            float lighten = 10;

            var textFormat = GetTextFormat();

            double x = 20;
            double y = 20;
            foreach (var paragraph in paragraphs)
            {
                dc.DrawText(paragraph.FontInfo, fontInfoFont, fontInfoBrush, new PointD(x, y));
                y += dc.MeasureText(paragraph.FontInfo, fontInfoFont).Height + 3;

                double textHeight;

                if (TextHeightSet)
                {
                    textHeight = TextHeightValue;
                }
                else
                {
                    if (TextWidthLimitEnabled)
                        textHeight = dc.MeasureText(LoremIpsum, paragraph.Font, TextWidthLimit, GetTextFormat()).Height;
                    else
                        textHeight = dc.MeasureText(LoremIpsum, paragraph.Font).Height;
                }

                if (TextWidthLimitEnabled)
                    dc.DrawText(LoremIpsum, paragraph.Font, new SolidBrush(color), new RectD(x, y, TextWidthLimit, textHeight), textFormat);
                else if (TextHeightSet)
                    dc.DrawText(LoremIpsum, paragraph.Font, new SolidBrush(color), new RectD(x, y, TextWidthLimitEnabled ? TextWidthLimit : double.MaxValue, textHeight), textFormat);
                else
                    dc.DrawText(LoremIpsum, paragraph.Font, new SolidBrush(color), new PointD(x, y), textFormat);

                y += textHeight + 20;

                var c = new Skybrud.Colors.RgbColor(color.R, color.G, color.B).Lighten(lighten).ToRgb();
                color = Color.FromArgb(c.R, c.G, c.B);
            }

            if (TextWidthLimitEnabled)
                dc.DrawLine(textWidthLimitPen, new PointD(TextWidthLimit + x, bounds.Top), new PointD(TextWidthLimit + x, bounds.Bottom));
        }

        protected override Control CreateSettingsControl()
        {
            var control = new TextPageSettings();
            control.Initialize(this);
            return control;
        }

        private TextFormat GetTextFormat()
        {
            return new TextFormat
            {
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment,
                Trimming = Trimming,
                Wrapping = Wrapping
            };
        }

        private void SetFontStyle(FontStyle style, bool value)
        {
            if (value)
                FontStyle |= style;
            else
                FontStyle &= ~style;
        }

        private bool GetFontStyle(FontStyle style)
        {
            return (FontStyle & style) != 0;
        }

        private void InvalidateParagraphs()
        {
            if (paragraphs != null)
            {
                foreach (var paragraph in paragraphs)
                    paragraph.Dispose();
                paragraphs = null;
            }

            Invalidate();
        }

        private void Invalidate()
        {
            Canvas?.Invalidate();
        }

        private IEnumerable<Paragraph> CreateParagraphs()
        {
            Paragraph CreateGenericFontParagraph(GenericFontFamily genericFamily) =>
                new(
                    new Font(new FontFamily(genericFamily), FontSize, FontStyle),
                    "Generic " + genericFamily.ToString());

            Paragraph CreateCustomFontParagraph(FontFamily family) =>
                new(
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