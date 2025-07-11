using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
#if DEBUG
        /// <summary>
        /// Internal use only.
        /// </summary>
        public static ObjectUniqueId? DebugElementId;
#endif

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
        /// Draws text inside the bounds with the specified font, background and foreground colors.
        /// </summary>
        /// <param name="rect">Bounding rectangle used to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        public virtual void DrawText(
            string text,
            RectD rect,
            Font font,
            Color foreColor,
            Color backColor)
        {
            DoInsideClipped(rect, () =>
            {
                DrawText(
                            text,
                            rect.Location,
                            font,
                            foreColor,
                            backColor);
            });
        }

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
        public virtual RectD DrawText(
            object? text,
            Font font,
            Brush brush,
            RectD rect,
            TextFormat format)
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

            wrappedText.DoInsideLayout(() =>
            {
                document.Size = rect.Size;
                wrappedText.SetFormat(format.AsRecord);
                wrappedText.Text = string.Empty;
                wrappedText.Text = s;
                wrappedText.Font = font;
                wrappedText.ForegroundColor = brush.AsColor;
            });

            TemplateUtils.RaisePaintClipped(wrappedText, this, rect.Location, isClipped: true);
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
            var convertedText = RegexUtils.GetBoldTagSplitted(text);
            return DrawTextWithFontStyle(
                        convertedText,
                        location,
                        font,
                        foreColor,
                        backColor);
        }

        /// <summary>
        /// Draws an array of text elements with font styles.
        /// </summary>
        public virtual SizeD DrawTextWithFontStyle(
            TextAndFontStyle[] text,
            PointD location,
            Font font,
            Color foreColor,
            Color? backColor = null)
        {
            DebugFontAssert(font);

            SizeD result = 0;

            bool visible = foreColor.IsOk && (foreColor != Color.Empty);

            foreach (var item in text)
            {
                var itemFont = font.WithStyle(item.FontStyle);
                var measure = GetTextExtent(item.Text, itemFont);
                if (visible)
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
        public virtual RectD DrawLabel(
            string text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            HVAlignment? alignment = null,
            int indexAccel = -1)
        {
            DrawLabelParams prm = new(
                text,
                font,
                foreColor,
                backColor,
                image,
                rect,
                alignment,
                indexAccel);

            var result = DrawLabel(ref prm);
            return result;
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors,
        /// optional image, alignment and underlined mnemonic character.
        /// </summary>
        /// <param name="prm">Parameters specified using
        /// <see cref="DrawLabelParams"/> structure.</param>
        /// <returns></returns>
        public virtual RectD DrawLabel(ref DrawLabelParams prm)
        {
            DrawElementsParams.ElementParams imageElement = DrawElementsParams.ElementParams.Default;
            var image = prm.Image;
            var indexAccel = prm.IndexAccel;
            var s = prm.Text;
            string[]? splitText = null;
            var font = prm.Font;
            var foreColor = prm.ForegroundColor;
            var backColor = prm.BackgroundColor;
            var isVertical = prm.IsVertical;
            var drawDebugCorners = prm.DrawDebugCorners;

            var textHorizontalAlignment = prm.TextHorizontalAlignment;
            var lineDistance = prm.LineDistance;

            var hasNewLineChars = prm.Flags.HasFlag(DrawLabelFlags.TextHasNewLineChars);

            if (hasNewLineChars)
            {
                if (StringUtils.ContainsNewLineChars(s))
                {
                    splitText = StringUtils.Split(s, false);
                }
                else
                {
                    hasNewLineChars = false;
                }
            }

            if (image is not null)
            {
                imageElement = new()
                {
                    GetSize = () =>
                    {
                        return image.SizeDip(ScaleFactor);
                    },
                    Draw = (dc, rect) =>
                    {
                        dc.DrawImage(image, rect.Location);

#if DEBUG
                        if (drawDebugCorners)
                            BorderSettings.DrawDesignCorners(dc, rect, BorderSettings.DebugBorderBlue);
#endif
                    },
                    Alignment = GetElementAlignment(),
                };
            }

            HVAlignment GetElementAlignment()
            {
                if (isVertical)
                    return (HorizontalAlignment.Center, VerticalAlignment.Top);
                else
                    return (HorizontalAlignment.Left, VerticalAlignment.Center);
            }

            TextAndFontStyle[]? parsed = prm.TextAndFontStyle;

            if(parsed is null)
            {
                if (prm.Flags.HasFlag(DrawLabelFlags.TextHasBold))
                {
                    parsed = RegexUtils.GetBoldTagSplitted(s);
                }
                else
                if (indexAccel >= 0)
                {
                    parsed = StringUtils.ParseTextWithIndexAccel(
                        s,
                        indexAccel,
                        FontStyle.Underline);
                }
            }

            DrawElementsParams.ElementParams textElement = new()
            {
                GetSize = () =>
                {
                    if (parsed is null)
                    {
                        SizeD result;

                        if(splitText is null)
                            result = MeasureText(s, font);
                        else
                        {
                            result = DrawStrings(
                                RectD.Empty,
                                font,
                                splitText,
                                textHorizontalAlignment,
                                lineDistance);
                        }

                        return result;
                    }
                    else
                    {
                        var result = DrawTextWithFontStyle(
                                    parsed,
                                    PointD.Empty,
                                    font,
                                    Color.Empty);
                        return result;
                    }
                },
                Draw = (dc, rect) =>
                {
                    if (parsed is null)
                    {
                        if (splitText is null)
                        {
                            DrawText(s, rect.Location, font, foreColor, backColor);
                        }
                        else
                        {
                            DrawStrings(
                                (rect.Location, SizeD.Empty),
                                font,
                                splitText,
                                textHorizontalAlignment,
                                lineDistance,
                                foreColor,
                                backColor);
                        }
                    }
                    else
                    {
                        DrawTextWithFontStyle(parsed, rect.Location, font, foreColor, backColor);
                    }

#if DEBUG
                    if (drawDebugCorners)
                        BorderSettings.DrawDesignCorners(dc, rect, BorderSettings.DebugBorderBlue);
#endif
                },
                Alignment = GetElementAlignment(),
            };

            DrawElementsParams drawParams = new();

            if (image is null)
                drawParams.Elements = [textElement];
            else
                drawParams.Elements = [imageElement, textElement];

            drawParams.IsVertical = isVertical;
            drawParams.Distance = prm.ImageLabelDistance;
            drawParams.Rect = prm.Rect;
            drawParams.Alignment = prm.Alignment;
            drawParams.Visible = prm.Visible;
            drawParams.DrawDebugCorners = prm.DrawDebugCorners;
            drawParams.DebugId = prm.DebugId;

            var result = DrawElements(ref drawParams);

            prm.ResultRects = drawParams.ResultRects;
            prm.ResultSizes = drawParams.ResultSizes;
            prm.ResultBounds = result;
            prm.ImageLabelDistance = drawParams.Distance;

            return result;
        }

        /// <summary>
        /// Draws range of strings.
        /// </summary>
        /// <param name="rect">The bounding rectangle used to specify location
        /// and maximal width of the text. Height of the rectangle is not used. If width
        /// of the rectangle is not specified <paramref name="textHorizontalAlignment"/>
        /// is not used.</param>
        /// <param name="font">The font used to draw the text.</param>
        /// <param name="wrappedText">The text to draw.</param>
        /// <param name="textHorizontalAlignment">The horizontal alignment of the text</param>
        /// <param name="lineDistance">The vertical distance between the lines of text.
        /// Optional. If not specified, 0 is used.</param>
        /// <param name="foreColor">The foreground color of the text. If <c>null</c>, text
        /// will be only measured and no drawing will be performed.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <returns></returns>
        public virtual SizeD DrawStrings(
            RectD rect,
            Font font,
            IEnumerable<string>? wrappedText,
            TextHorizontalAlignment textHorizontalAlignment = TextHorizontalAlignment.Left,
            Coord lineDistance = 0,
            Color? foreColor = null,
            Color? backColor = null)
        {
            var wrappedWidth = 0;

            if (wrappedText is null)
                return SizeD.Empty;

            var origin = rect.Location;
            SizeD totalMeasure = (wrappedWidth, 0);

            foreach (var s in wrappedText)
            {
                var measure = MeasureText(s, font).Ceiling();

                if (foreColor is not null)
                {
                    PointD location;

                    if (rect.SizeIsEmpty)
                    {
                        location = origin;
                    }
                    else
                    {
                        RectD itemRect = (origin, measure);
                        RectD itemContainer = itemRect;
                        itemContainer.Width = rect.Width;

                        var alignment = AlignUtils.Convert(textHorizontalAlignment);

                        var alignedItemRect = AlignUtils.AlignRectInRect(
                            false,
                            itemRect,
                            itemContainer,
                            (CoordAlignment)alignment);
                        location = alignedItemRect.Location;
                    }

                    DrawText(
                        s,
                        location,
                        font,
                        foreColor,
                        backColor ?? Color.Empty);
                }

                var increment = measure.Height + lineDistance;
                origin.Y += increment;
                totalMeasure.Height += increment;

                totalMeasure.Width = Math.Max(totalMeasure.Width, measure.Width);
            }

            return totalMeasure.ApplyMax(rect.Size);
        }

        /// <summary>
        /// Draws array of elements specified with <see cref="DrawElementsParams"/>.
        /// </summary>
        /// <param name="prm">Method arguments.</param>
        /// <returns></returns>
        public virtual RectD DrawElements(ref DrawElementsParams prm)
        {
            var drawDebugCorners = prm.DrawDebugCorners;
            var visible = prm.Visible;

            var length = prm.Elements.Length;
            if (length == 0)
                return prm.Rect;

            SizeD[] elementSizes = new SizeD[length];

            for (int i = 0; i < length; i++)
            {
                elementSizes[i] = prm.Elements[i].GetSize();
            }

            prm.ResultSizes = elementSizes;

            var elementDistance = prm.Distance ?? SpeedButton.DefaultImageLabelDistance;
            prm.Distance = elementDistance;
            Coord sumDistance = elementDistance * (length - 1);

            var sumSize = SizeD.Sum(elementSizes);
            var maxSize = SizeD.MaxWidthHeights(elementSizes);

            SizeD size;

            if (prm.IsVertical)
            {
                size = (maxSize.Width, sumSize.Height + sumDistance);
            }
            else
            {
                size = (sumSize.Width + sumDistance, maxSize.Height);
            }

            RectD beforeAlign = (prm.Rect.Location, size);
            RectD afterAlign;

            if (prm.Rect.Size.AnyIsEmptyOrNegative)
            {
                afterAlign = beforeAlign;
            }
            else
            {
                afterAlign = AlignUtils.AlignRectInRect(
                    beforeAlign,
                    prm.Rect,
                    prm.Alignment.Horizontal,
                    prm.Alignment.Vertical,
                    shrinkSize: true);
            }

#if DEBUG
            if (drawDebugCorners)
            {
                BorderSettings.DrawDesignCorners(this, afterAlign, BorderSettings.DebugBorder);
            }
#endif

            prm.ResultBounds = afterAlign;

            RectD[] bounds = new RectD[length];
            prm.ResultRects = bounds;

            var rect = afterAlign;

            for (int i = 0; i < length; i++)
            {
                var element = prm.Elements[i];
                var elementSize = elementSizes[i];
                RectD elementBeforeAlign = (rect.Location, elementSize);

                if (prm.IsVertical)
                {
                    var elementAfterAlign = AlignUtils.AlignRectInRect(
                        elementBeforeAlign,
                        rect,
                        element.Alignment.Horizontal,
                        VerticalAlignment.Top,
                        shrinkSize: false);
                    bounds[i] = elementAfterAlign;
                    DrawElement(in element, elementAfterAlign);
                    rect.Y = elementAfterAlign.Bottom + elementDistance;
                    rect.Height = rect.Height - elementAfterAlign.Height - elementDistance;
                }
                else
                {
#if DEBUG
                    if(length > 1)
                    {
                        if(DebugElementId == prm.DebugId && prm.DebugId is not null)
                        {
                        }
                    }
#endif
                    var elementAfterAlign = AlignUtils.AlignRectInRect(
                        elementBeforeAlign,
                        rect,
                        HorizontalAlignment.Left,
                        element.Alignment.Vertical,
                        shrinkSize: false);
                    bounds[i] = elementAfterAlign;
                    DrawElement(in element, elementAfterAlign);
                    rect.X = elementAfterAlign.Right + elementDistance;
                    rect.Width = rect.Width - elementAfterAlign.Width - elementDistance;
                }
            }

            void DrawElement(in DrawElementsParams.ElementParams element, RectD rect)
            {
                if (!visible)
                    return;
                element.Draw(this, rect);

#if DEBUG
                if (!drawDebugCorners)
                    return;
                BorderSettings.DrawDesignCorners(this, rect, BorderSettings.DebugBorder);
#endif
            }

            return afterAlign;
        }

        /// <summary>
        /// Contains parameters for the draw elements method.
        /// </summary>
        public struct DrawElementsParams
        {
            /// <summary>
            /// Internal use only.
            /// </summary>
            public ObjectUniqueId? DebugId;

            /// <summary>
            /// Gets or sets whether to draw debug corners around elements.
            /// </summary>
            public bool DrawDebugCorners;

            /// <summary>
            /// Gets or sets array of elements to draw.
            /// </summary>
            public ElementParams[] Elements = [];

            /// <summary>
            /// Gets elements bounding rectangle after drawing.
            /// This is filled even if <see cref="Visible"/> is False.
            /// </summary>
            public RectD ResultBounds;

            /// <summary>
            /// Gets element sizes after drawing was performed.
            /// This is filled with sizes even if <see cref="Visible"/> is False.
            /// </summary>
            public SizeD[]? ResultSizes;

            /// <summary>
            /// Gets element bounds after drawing was performed.
            /// This is filled with bounds even if <see cref="Visible"/> is False.
            /// </summary>
            public RectD[]? ResultRects;

            /// <summary>
            /// Gets or sets whether elements are painted as vertical or horizontal stack.
            /// </summary>
            public bool IsVertical = false;

            /// <summary>
            /// Gets or sets whether painting is actually performed. This property may be useful
            /// when you need to calculate element sizes without painting.
            /// </summary>
            public bool Visible = true;

            /// <summary>
            /// Gets or sets distance between elements. If Null,
            /// <see cref="SpeedButton.DefaultImageLabelDistance"/> is used.
            /// </summary>
            public Coord? Distance;

            /// <summary>
            /// Gets or sets rectangle in which drawing is performed.
            /// </summary>
            public RectD Rect;

            /// <summary>
            /// Gets or sets alignment of the element's block.
            /// Default is <see cref="HVAlignment.TopLeft"/>.
            /// </summary>
            public HVAlignment Alignment = HVAlignment.TopLeft;

            /// <summary>
            /// Initializes a new instance of the <see cref="DrawElementsParams"/> struct.
            /// </summary>
            public DrawElementsParams()
            {
            }

            /// <summary>
            /// Contains element parameters
            /// </summary>
            public struct ElementParams
            {
                /// <summary>
                /// Gets default element.
                /// </summary>
                public static readonly ElementParams Default = new();

                /// <summary>
                /// Gets or sets element size function.
                /// </summary>
                public Func<SizeD> GetSize;

                /// <summary>
                /// Gets or sets element draw function.
                /// </summary>
                public Action<Graphics, RectD> Draw;

                /// <summary>
                /// Gets or sets element alignment.
                /// </summary>
                public HVAlignment Alignment;

                /// <summary>
                /// Initializes a new instance of the <see cref="ElementParams"/> struct.
                /// </summary>
                public ElementParams()
                {
                    GetSize = () => SizeD.Empty;
                    Draw = (_, _) => { };
                    Alignment = (HorizontalAlignment.Left, VerticalAlignment.Center);
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="ElementParams"/> struct
                /// with the specified parameters.
                /// </summary>
                public ElementParams(
                    Func<SizeD> getSize,
                    Action<Graphics, RectD> draw,
                    HVAlignment alignment)
                {
                    GetSize = getSize;
                    Draw = draw;
                    Alignment = alignment;
                }
            }
        }

        /// <summary>
        /// Contains parameters for the draw label method.
        /// </summary>
        public struct DrawLabelParams
        {
            /// <summary>
            /// Gets or sets distance between lines of text.
            /// </summary>
            public Coord LineDistance;

            /// <summary>
            /// Gets or sets horizontal alignment of the text line within the text block.
            /// </summary>
            public TextHorizontalAlignment TextHorizontalAlignment = TextHorizontalAlignment.Left;

            /// <summary>
            /// Internal use only.
            /// </summary>
            public ObjectUniqueId? DebugId;

            /// <summary>
            /// Gets or sets a value which specifies whether image to text are aligned vertically
            /// or horizontally.
            /// </summary>
            public bool IsVertical = false;

            /// <summary>
            /// Gets or sets distance between image and label. If Null,
            /// <see cref="SpeedButton.DefaultImageLabelDistance"/> is used.
            /// </summary>
            public Coord? ImageLabelDistance;

            /// <summary>
            /// Gets or sets whether painting is actually performed. This property may be useful
            /// when you need to calculate element sizes without painting.
            /// </summary>
            public bool Visible = true;

            /// <summary>
            /// Gets or sets text to draw.
            /// </summary>
            public string Text;

            /// <summary>
            /// Gets or sets text with attributes to use instead of <see cref="Text"/>.
            /// </summary>
            public TextAndFontStyle[]? TextAndFontStyle;

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
            /// Gets or sets alignment of the text block. Default is <see cref="HVAlignment.TopLeft"/>.
            /// </summary>
            public HVAlignment Alignment = HVAlignment.TopLeft;

            /// <summary>
            /// Gets or sets index of underlined mnemonic character.
            /// </summary>
            public int IndexAccel = -1;

            /// <summary>
            /// Gets or sets flags that can be used to customize label painting.
            /// </summary>
            public DrawLabelFlags Flags;

            /// <summary>
            /// Gets element sizes after drawing was performed.
            /// This is filled with sizes even if <see cref="Visible"/> is False.
            /// </summary>
            public SizeD[]? ResultSizes;

            /// <summary>
            /// Gets element bounds after drawing was performed.
            /// This is filled with bounds only if <see cref="Visible"/> is True.
            /// </summary>
            public RectD[]? ResultRects;

            /// <summary>
            /// Gets all elements bounding rectangle after drawing.
            /// This is filled even if <see cref="Visible"/> is False.
            /// </summary>
            public RectD ResultBounds;

            /// <summary>
            /// Gets or sets whether to draw debug corners around elements.
            /// </summary>
            public bool DrawDebugCorners;

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
                HVAlignment? alignment = null,
                int indexAccel = -1)
            {
                Text = text;
                Font = font;
                ForegroundColor = foreColor;
                BackgroundColor = backColor;
                Image = image;
                Rect = rect;
                IndexAccel = indexAccel;
                Alignment = alignment ?? HVAlignment.TopLeft;
            }
        }
    }
}
