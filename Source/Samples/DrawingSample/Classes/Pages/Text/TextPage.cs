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

        private readonly FormattedText formattedText = new()
        {
            HorizontalAlignment = TextHorizontalAlignment.Left,
            VerticalAlignment = TextVerticalAlignment.Top,
            Trimming = TextTrimming.Pixel,
            Wrapping = TextWrapping.Word,
        };

        private Paragraph[]? paragraphs;
        private FontStyle fontStyle;
        private bool shortText;
        private string customFontFamilyName = Control.DefaultFont.FontFamily.Name;

        private int textWidthLimit = 450;
        private int textHeightValue = 50;

        private bool textWidthLimitEnabled = true;
        private bool textHeightSet = false;

        private static readonly Font fontInfoFont;
        private static double fontSize;

        static TextPage()
        {
            var defaultSize = Control.DefaultFont.SizeInPoints;
            fontInfoFont = new(FontFamily.GenericMonospace, defaultSize);
            fontSize = defaultSize;
        }

        public override string Name => "Text";

        public FormattedText WrappedText => formattedText;

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
            get => GetFontStyle(FontStyle.Underline);
            set => SetFontStyle(FontStyle.Underline, value);
        }

        public bool Strikethrough
        {
            get => GetFontStyle(FontStyle.Strikeout);
            set => SetFontStyle(FontStyle.Strikeout, value);
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
            get => formattedText.HorizontalAlignment;
            set
            {
                formattedText.HorizontalAlignment = value;
                Invalidate();
            }
        }

        public TextVerticalAlignment VerticalAlignment
        {
            get => formattedText.VerticalAlignment;
            set
            {
                formattedText.VerticalAlignment = value;
                Invalidate();
            }
        }

        public TextTrimming Trimming
        {
            get => formattedText.Trimming;
            set
            {
                formattedText.Trimming = value;
                Invalidate();
            }
        }

        public TextWrapping Wrapping
        {
            get => formattedText.Wrapping;
            set
            {
                formattedText.Wrapping = value;
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

        public bool ShortText
        {
            get
            {
                return shortText;
            }

            set
            {
                shortText = value;
                InvalidateParagraphs();
            }
        }

        public string GetText()
        {
            if (ShortText)
                return "Your cat is hungry";
            return LoremIpsum;
        }

        public override void Draw(Graphics dc, RectD bounds)
        {
            paragraphs ??= CreateParagraphs().ToArray();

            var color = Color.MidnightBlue;
            float lighten = 10;

            var textFormat = GetTextFormat();

            Coord x = 20;
            Coord y = 20;
            foreach (var paragraph in paragraphs)
            {
                dc.DrawText(paragraph.FontInfo, fontInfoFont, fontInfoBrush, new PointD(x, y));
                y += dc.MeasureText(paragraph.FontInfo, fontInfoFont).Height + 3;

                formattedText.ScaleFactor = dc.ScaleFactor;
                formattedText.Text = GetText();
                formattedText.Font = paragraph.Font;
                formattedText.ForegroundColor = color;
                formattedText.MaxWidth = TextWidthLimitEnabled ? TextWidthLimit : null;
                formattedText.MaxHeight = textHeightSet ? textHeightValue : null;
                formattedText.Assign(textFormat);

                var formattedTextHeight = formattedText.RestrictedSize.Height;
                var formattedTextWidth = formattedText.RestrictedSize.Width;

                var width = TextWidthLimitEnabled ? TextWidthLimit : bounds.Width;
                var textHeight = TextHeightSet ? TextHeightValue : formattedTextHeight;
                RectD rect = (x, y, width, textHeight);

                formattedText.Draw(dc, rect);

                dc.FillRectangleBorder(Brushes.LightGray, rect);
                dc.FillRectangleBorder(
                    Brushes.LightGreen,
                    formattedText.GetBlockRect(dc.ScaleFactor, rect));

                /*if (TextHeightSet)
                {
                    textHeight = TextHeightValue;
                }
                else
                {
                    if (TextWidthLimitEnabled)
                    {
                        textHeight = ((IWxGraphics)dc).MeasureText(
                            LoremIpsum,
                            paragraph.Font,
                            TextWidthLimit,
                            GetTextFormat()).Height;
                    }
                    else
                        textHeight = dc.MeasureText(LoremIpsum, paragraph.Font).Height;
                }

                if (TextWidthLimitEnabled)
                {
                    ((IWxGraphics)dc).DrawText(
                        LoremIpsum,
                        paragraph.Font,
                        color.AsBrush,
                        new RectD(x, y, TextWidthLimit, textHeight),
                        textFormat);
                }
                else
                if (TextHeightSet)
                {
                    var width = TextWidthLimitEnabled ? TextWidthLimit : bounds.Width;
                    ((IWxGraphics)dc).DrawText(
                        LoremIpsum,
                        paragraph.Font,
                        color.AsBrush,
                        new RectD(x, y, width, textHeight),
                        textFormat);
                }
                else
                {
                    dc.DrawText(
                        LoremIpsum,
                        paragraph.Font,
                        color.AsBrush,
                        new PointD(x, y));
                }*/

                y += textHeight + 20;

                color = Lighten(color, lighten);
            }

            if (TextWidthLimitEnabled)
                dc.DrawLine(textWidthLimitPen, new PointD(TextWidthLimit + x, bounds.Top), new PointD(TextWidthLimit + x, bounds.Bottom));
        }

        //The amount of lightness (specified in percent) that should be added to the color.
        public static Color Lighten(Color color, float percent)
        {
            HSVValue hsv = (RGBValue)color;

            double val = hsv.Value + (double)(percent / 100f);

            RGBValue result = new HSVValue(
                hsv.Hue,
                hsv.Saturation,
                Math.Max(0.0, val));

            return Color.FromArgb(color.A, result);
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

        public void InvalidateParagraphs()
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
                new(this,
                    new Font(new FontFamily(genericFamily), FontSize, FontStyle),
                    "Generic " + genericFamily.ToString());

            Paragraph CreateCustomFontParagraph(FontFamily family) =>
                new(this,
                    new Font(family, FontSize, FontStyle),
                    "Custom");

            yield return CreateCustomFontParagraph(new FontFamily(CustomFontFamilyName));
            yield return CreateGenericFontParagraph(GenericFontFamily.Serif);
            yield return CreateGenericFontParagraph(GenericFontFamily.SansSerif);
            yield return CreateGenericFontParagraph(GenericFontFamily.Monospace);
        }

        private class Paragraph : IDisposable
        {
            public Paragraph(TextPage owner, Font font, string genericFamilyName)
            {
                Owner = owner;
                Font = font;
                GenericFamilyName = genericFamilyName;
                FontInfo = $"{GenericFamilyName}: {Font.Name}, {Font.SizeInPoints}pt";
            }

            public TextPage Owner { get; }

            public Font Font { get; }

            public string GenericFamilyName { get; }

            public string FontInfo { get; }

            public void Dispose()
            {
                Owner.formattedText.Font = null;
                Font.Dispose();
            }
        }
    }
}