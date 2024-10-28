using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        /// <summary>
        /// Draws text string with the specified bounds, <see cref="Brush"/>
        /// and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture
        /// of the drawn text.</param>
        /// <param name="bounds"><see cref="RectD"/> structure that specifies the bounds of
        /// the drawn text.</param>
        public abstract void DrawText(string text, Font font, Brush brush, RectD bounds);

        /// <summary>
        /// Draws text string with the specified location, <see cref="Brush"/>
        /// and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of
        /// the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the text position on the canvas.</param>
        public abstract void DrawText(string text, Font font, Brush brush, PointD origin);

        /// <summary>
        /// Draws text with <see cref="AbstractControl.DefaultFont"/> and <see cref="Brush.Default"/>.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the text position on the canvas.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawText(string text, PointD origin)
        {
            DrawText(text, Control.DefaultFont, Brush.Default, origin);
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors.
        /// This is the fastest method to draw text.
        /// </summary>
        /// <param name="location">Location used to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        public abstract void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor);

        /// <summary>
        /// Draws the text string at the specified location with
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of
        /// the drawn text.</param>
        /// <param name="rect"><see cref="RectD"/> structure that specifies the bounds
        /// of the text.</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as alignment and trimming, that are applied to the drawn text.</param>
        /// <remarks>
        /// You can pass 0 as width of the <paramref name="rect"/>. In this case wrapping
        /// will not be performed, only line breaks will be applied.
        /// </remarks>
        /// <remarks>
        /// You can pass 0 as height of the <paramref name="rect"/>.
        /// </remarks>
        public virtual RectD DrawText(object? text, Font font, Brush brush, RectD rect, TextFormat format)
        {
            string s = text?.ToString() ?? string.Empty;

            if (s.Length == 0)
                return rect.WithEmptySize();

            var document = SafeDocument;
            var wrappedText = document.WrappedText;

            if (rect.HasEmptyWidth)
                rect.Width = HalfOfMaxValue;

            if (rect.HasEmptyHeight)
                rect.Height = HalfOfMaxValue;

            document.Size = rect.Size;

            wrappedText.DoInsideLayout(() =>
            {
                wrappedText.SetFormat(format.AsRecord);
                wrappedText.Text = s!;
                wrappedText.Font = font;
                wrappedText.ForegroundColor = brush.AsColor;
            });

            TemplateUtils.RaisePaintClipped(wrappedText, this, rect.Location);
            var result = wrappedText.Bounds.WithLocation(rect.Location);
            return result;
        }

        /// <summary>
        /// Draws text with html bold tags.
        /// </summary>
        public virtual SizeD DrawTextWithBoldTags(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color? backColor = null)
        {
            var splitted = RegexUtils.GetBoldTagSplitted(text);
            return DrawTextWithFontStyle(
                        splitted,
                        location,
                        font,
                        foreColor,
                        backColor);
        }

        /// <summary>
        /// Draws an array of text elements with font styles.
        /// </summary>
        public virtual SizeD DrawTextWithFontStyle(
            TextAndFontStyle[] splittedText,
            PointD location,
            Font font,
            Color foreColor,
            Color? backColor = null)
        {
            DebugFontAssert(font);
            DebugColorAssert(foreColor);

            SizeD result = 0;

            bool visible = foreColor != Color.Empty;

            foreach (var item in splittedText)
            {
                var itemFont = font.WithStyle(item.FontStyle);
                var measure = GetTextExtent(item.Text, itemFont);
                if(visible)
                    DrawText(item.Text, location, itemFont, foreColor, backColor ?? Color.Empty);
                location.X += measure.Width;
                result.Width += measure.Width;
                result.Height = Math.Max(result.Height, measure.Height);
            }

            return result;
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors,
        /// optional image, alignment and underlined mnemonic character.
        /// </summary>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted.</param>
        /// <param name="image">Optional image.</param>
        /// <param name="rect">Rectangle in which drawing is performed.</param>
        /// <param name="alignment">Alignment of the text.</param>
        /// <param name="indexAccel">Index of underlined mnemonic character.</param>
        /// <returns>The bounding rectangle.</returns>
        public abstract RectD DrawLabel(
            string text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            GenericAlignment alignment = GenericAlignment.TopLeft,
            int indexAccel = -1);

        /// <summary>
        /// Draws text with the specified font, background and foreground colors,
        /// optional image, alignment and underlined mnemonic character.
        /// </summary>
        /// <param name="prm">Parameters spedified using <see cref="DrawLabelParams"/> structure.</param>
        /// <returns></returns>
        internal virtual RectD DrawLabel(ref DrawLabelParams prm)
        {
            var info = prm;
            var indexAccel = prm.IndexAccel;
            var image = prm.Image;
            var s = info.Text;
            var font = info.Font;

            SizeD textSize;

            TextAndFontStyle[]? parsed;

            if (indexAccel == -1)
            {
                textSize = MeasureText(s, font);
            }
            else
            {
                parsed = StringUtils.ParseTextWithIndexAccel(
                    prm.Text,
                    indexAccel,
                    FontStyle.Underline);
                textSize = DrawTextWithFontStyle(
                            parsed,
                            PointD.Empty,
                            font,
                            Color.Empty);
            }

            var size = textSize;

            if (image is not null)
            {
                var imageLabelDistance = prm.ImageLabelDistance ?? SpeedButton.DefaultImageLabelDistance;

                if(prm.ImageToText == ImageToText.Horizontal)
                {
                    size.Width += image.PixelWidth + imageLabelDistance;
                }
                else
                {
                    size.Height += image.PixelHeight + imageLabelDistance;
                }
            }

            RectD beforeAlign = (prm.Rect.Location, size);

            var afterAlign = AlignUtils.AlignRectInRect(
                beforeAlign,
                prm.Rect,
                prm.Alignment.Horizontal,
                prm.Alignment.Vertical,
                shrinkSize: true);

            if(image is null)
            {
            }
            else
            {
            }

            return afterAlign;
        }

        /// <summary>
        /// Contains parameters for the draw label method.
        /// </summary>
        public struct DrawLabelParams
        {
            /// <summary>
            /// Gets or sets a value which specifies display modes for
            /// item image and text.
            /// </summary>
            public ImageToText ImageToText = ImageToText.Horizontal;

            /// <summary>
            /// Gets or sets distance between image and label. If Null,
            /// <see cref="SpeedButton.DefaultImageLabelDistance"/> is used.
            /// </summary>
            public Coord? ImageLabelDistance;

            /// <summary>
            /// Gets or sets text to draw.
            /// </summary>
            public string Text;

            /// <summary>
            /// Gets or sets font used to draw the text.
            /// </summary>
            public Font Font;

            /// <summary>
            /// Gets or sets foreground color of the text.
            /// </summary>
            public Color ForegroundColor;

            /// <summary>
            /// Gets or sets background color of the text. If property value equals
            /// to <see cref="Color.Empty"/> (default value), background will not be painted.
            /// </summary>
            public Color BackgroundColor = Color.Empty;

            /// <summary>
            /// Gets or sets optional image which is shown near the text. Default is Null.
            /// </summary>
            public Image? Image;

            /// <summary>
            /// Gets or sets rectangle in which drawing is performed.
            /// </summary>
            public RectD Rect;

            /// <summary>
            /// Gets or sets alignment of the text. Default is <see cref="HVAlignment.TopLeft"/>.
            /// </summary>
            public HVAlignment Alignment = HVAlignment.TopLeft;

            /// <summary>
            /// Gets or sets index of underlined mnemonic character.
            /// </summary>
            public int IndexAccel = -1;

            /// <summary>
            /// Initializes a new instance of the <see cref="DrawLabelParams"/> struct.
            /// </summary>
            public DrawLabelParams(
                string text,
                Font font,
                Color foreColor)
            {
                Text = text;
                Font = font;
                ForegroundColor = foreColor;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DrawLabelParams"/> struct
            /// with the specified initial values.
            /// </summary>
            /// <param name="text">Text to draw.</param>
            /// <param name="font">Font used to draw the text.</param>
            /// <param name="foreColor">Foreground color of the text.</param>
            /// <param name="backColor">Background color of the text. If parameter is equal
            /// to <see cref="Color.Empty"/>, background will not be painted.</param>
            /// <param name="image">Optional image.</param>
            /// <param name="rect">Rectangle in which drawing is performed.</param>
            /// <param name="alignment">Alignment of the text.</param>
            /// <param name="indexAccel">Index of underlined mnemonic character.</param>
            public DrawLabelParams(
                string text,
                Font font,
                Color foreColor,
                Color backColor,
                Image? image,
                RectD rect,
                GenericAlignment alignment = GenericAlignment.TopLeft,
                int indexAccel = -1)
            {
                Text = text;
                Font = font;
                ForegroundColor = foreColor;
                BackgroundColor = backColor;
                Image = image;
                Rect = rect;
                IndexAccel = indexAccel;
                Alignment = alignment;
            }
        }
    }
}
