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
        /// <summary>
        /// Gets or sets whether debug corners are painted in some of the graphics methods.
        /// </summary>
        public static bool DrawDebugCorners = false;

#if DEBUG
        /// <summary>
        /// Internal use only.
        /// </summary>
        public static ObjectUniqueId? DebugElementId;
#endif

        /// <summary>
        /// Gets the dimensions of the string using the specified font.
        /// </summary>
        /// <param name="text">The text string to measure.</param>
        /// <param name="font">The Font used to get text dimensions.</param>
        /// <returns><see cref="SizeD"/> with the total calculated width and height
        /// of the text.</returns>
        /// <remarks>
        /// This function only works with single-line strings.
        /// It works faster than MeasureText methods.
        /// </remarks>
        public abstract SizeD GetTextExtent(string text, Font font);

        /// <summary>
        /// Draws text with the specified angle, font, background and foreground colors.
        /// </summary>
        /// <param name="location">Location used to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        /// <param name="angle">The angle, in degrees, relative to the (default) horizontal
        /// direction to draw the string.</param>
        public abstract void DrawTextWithAngle(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            double angle);

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
            RectD result = RectD.Empty;

            if (prm.Rect.SizeIsEmpty || prm.Rect.SizeIsPositiveInfinity)
            {
                result = DrawLabelUnclipped(ref prm);
            }
            else
            {
                try
                {
                    Save();
                    ClipRect(prm.Rect);
                    result = DrawLabelUnclipped(ref prm);
                }
                finally
                {
                    Restore();
                }
            }

            return result;
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors,
        /// optional image, alignment and underlined mnemonic character.
        /// This is unclipped version of <see cref="DrawLabel(ref DrawLabelParams)"/>.
        /// </summary>
        /// <param name="prm">Parameters specified using
        /// <see cref="DrawLabelParams"/> structure.</param>
        /// <returns></returns>
        public virtual RectD DrawLabelUnclipped(ref DrawLabelParams prm)
        {
            var image = prm.Image;

            DrawElementParams imageElement = DrawElementParams.CreateImageElement(ref prm);
            DrawElementParams textElement = DrawElementParams.CreateTextElement(ref prm);

            DrawElementsParams drawParams = new();

            if(prm.PrefixElements is not null || prm.SuffixElements is not null)
            {
                if (image is null)
                {
                    drawParams.Elements = ArrayUtils.CombineArrays<DrawElementParams>(
                        prm.PrefixElements,
                        [textElement],
                        prm.SuffixElements);
                }
                else
                {
                    drawParams.Elements = ArrayUtils.CombineArrays<DrawElementParams>(
                        prm.PrefixElements,
                        [imageElement, textElement],
                        prm.SuffixElements);
                }
            }
            else
            {
                if (image is null)
                {
                    drawParams.Elements = [textElement];
                }
                else
                {
                    drawParams.Elements = [imageElement, textElement];
                }
            }

            drawParams.IsVertical = prm.IsVertical;
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

            double? emptyStringMeasure = null;

            foreach (var s in wrappedText)
            {
                SizeD measure;

                var isEmpty = s is null || s.Length == 0;

                if (isEmpty)
                {
                    emptyStringMeasure ??= font.GetHeight(this);
                    measure = (0d, emptyStringMeasure.Value);
                }
                else
                {
                    measure = MeasureText(s!, font).Ceiling();
                }

                if (!isEmpty && foreColor is not null)
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
                        s!,
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
                elementSizes[i] = prm.Elements[i].GetRealSize(this);
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
                    shrinkSize: false);
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

                    if (element.IsImage)
                    {
                    }
#endif
                    var elementAfterAlign = AlignUtils.AlignRectInRect(
                        elementBeforeAlign,
                        rect,
                        element.Alignment.Horizontal,
                        element.Alignment.Vertical,
                        shrinkSize: false);
                    bounds[i] = elementAfterAlign;
                    DrawElement(in element, elementAfterAlign);
                    if (element.Alignment.Horizontal == HorizontalAlignment.Left)
                    {
                        rect.X = elementAfterAlign.Right + elementDistance;
                    }
                    else
                    {
                    }

                    rect.Width = rect.Width - elementAfterAlign.Width - elementDistance;
                }
            }

            void DrawElement(in DrawElementParams element, RectD rect)
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
            public DrawElementParams[] Elements = [];

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
        }

        /// <summary>
        /// Contains draw element parameters.
        /// </summary>
        public struct DrawElementParams
        {
            /// <summary>
            /// Gets default element.
            /// </summary>
            public static readonly DrawElementParams Default = new();

            /// <summary>
            /// Gets or sets an object that provides additional data
            /// or metadata about the current instance.
            /// </summary>
            public object? Tag;

            /// <summary>
            /// Gets or sets the name associated with the object.
            /// </summary>
            public string? Name;

            /// <summary>
            /// Gets or sets element size function.
            /// </summary>
            public Func<Graphics, SizeD> GetSize;

            /// <summary>
            /// Gets or sets element draw function.
            /// </summary>
            public Action<Graphics, RectD> Draw;

            /// <summary>
            /// Gets or sets element alignment.
            /// </summary>
            public HVAlignment Alignment;

            /// <summary>
            /// Gets or sets whether element is image.
            /// </summary>
            public bool IsImage;

            /// <summary>
            /// Gets or sets minimal width of the element.
            /// </summary>
            public Coord MinWidth;

            /// <summary>
            /// Initializes a new instance of the <see cref="DrawElementParams"/> struct.
            /// </summary>
            public DrawElementParams()
            {
                GetSize = (_) => SizeD.Empty;
                Draw = (_, _) => { };
                Alignment = (HorizontalAlignment.Left, VerticalAlignment.Center);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DrawElementParams"/> struct
            /// with the specified parameters.
            /// </summary>
            public DrawElementParams(
                Func<Graphics, SizeD> getSize,
                Action<Graphics, RectD> draw,
                HVAlignment alignment)
            {
                GetSize = getSize;
                Draw = draw;
                Alignment = alignment;
            }

            /// <summary>
            /// Creates a <see cref="DrawElementParams"/> object for rendering an image
            /// element based on the specified SVG image, size, and optional color.
            /// </summary>
            /// <remarks>If both <paramref name="size"/> and <paramref name="color"/>
            /// are <see langword="null"/>, the method uses default values derived
            /// from the provided <paramref name="control"/>.
            /// The resulting image element is created with the specified or default
            /// size and color.</remarks>
            /// <param name="control">The control associated with the image element.
            /// This is used to determine default size and color if not
            /// explicitly provided. Can be <see langword="null"/>.</param>
            /// <param name="svg">The <see cref="SvgImage"/> to be rendered
            /// as the image element.</param>
            /// <param name="size">The size of the image in pixels.
            /// If <see langword="null"/>, a default size is determined based on the
            /// <paramref name="control"/>.</param>
            /// <param name="color">The color to apply to the SVG image.
            /// If <see langword="null"/>, a default color is determined based on
            /// the <paramref name="control"/>.</param>
            /// <returns>A <see cref="DrawElementParams"/> object representing
            /// the configured image element.</returns>
            public static DrawElementParams CreateImageElement(
                AbstractControl? control,
                SvgImage svg,
                int? size,
                Color? color = null)
            {
                size ??= ToolBarUtils.GetDefaultImageSize(control).Width;
                color ??= control?.GetSvgColor(KnownSvgColor.Normal);
                var normalImage = svg.ImageWithColor(size.Value, color);

                return CreateImageElement(normalImage);
            }

            /// <summary>
            /// Creates a new image element for rendering using the specified image.
            /// </summary>
            /// <param name="image">The image to be used for the element.
            /// Cannot be <see langword="null"/>.</param>
            /// <returns>A <see cref="DrawElementParams"/> object configured
            /// to render the specified image.</returns>
            public static DrawElementParams CreateImageElement(Image? image)
            {
                DrawLabelParams prm = new();
                prm.Image = image;
                return CreateImageElement(ref prm);
            }

            /// <summary>
            /// Creates an image element based on the specified drawing parameters.
            /// </summary>
            /// <remarks>The returned <see cref="DrawElementParams"/> object includes
            /// the size calculation and drawing logic for the image, as well
            /// as alignment settings derived from the
            /// provided parameters.</remarks>
            /// <param name="prm">A reference to the <see cref="DrawLabelParams"/>
            /// structure containing the parameters for drawing
            /// the image, including the image source and alignment settings.</param>
            /// <param name="imageOverride">The <see cref="Image"/> to use as the image
            /// for the element, overriding any image specified in <paramref name="prm"/>.</param>
            /// <returns>An <see cref="DrawElementParams"/> object representing
            /// the image element to be drawn, or <see langword="null"/> if
            /// the <see cref="DrawLabelParams.Image"/> property
            /// is <see langword="null"/>.</returns>
            public static DrawElementParams CreateImageElement(
                ref DrawLabelParams prm,
                Image? imageOverride = null)
            {
                var image = imageOverride ?? prm.Image;

                if (image is null)
                    return DrawElementParams.Default;

#if DEBUG
                var drawDebugCorners = prm.DrawDebugCorners || Graphics.DrawDebugCorners;
#endif

                DrawElementParams imageElement = new()
                {
                    IsImage = true,
                    GetSize = (dc) =>
                    {
                        var result = image.SizeDip(dc.ScaleFactor);
                        return result;
                    },
                    Draw = (dc, rect) =>
                    {
                        dc.DrawImage(image, rect.Location);

#if DEBUG
                        if (drawDebugCorners)
                        {
                            BorderSettings.DrawDesignCorners(
                                dc,
                                rect,
                                BorderSettings.DebugBorderBlue);
                        }
#endif
                    },
                    Alignment = prm.GetImageAlignment(),
                };

                return imageElement;
            }

            /// <summary>
            /// Creates a spacer element with the specified size and alignment.
            /// </summary>
            /// <param name="size">The size of the spacer element,
            /// defined as a <see cref="SizeD"/>.</param>
            /// <param name="alignment">The alignment of the spacer element,
            /// specified as a <see cref="HVAlignment"/>.</param>
            /// <returns>A <see cref="DrawElementParams"/> object representing
            /// the spacer element with the specified size and
            /// alignment.</returns>
            public static DrawElementParams CreateSpacerElement(
                SizeD size,
                HVAlignment? alignment = null)
            {
                SizeD GetSize(Graphics dc)
                {
                    return size;
                }

                void Draw(Graphics dc, RectD rect)
                {
                }

                return new DrawElementParams(GetSize, Draw, alignment ?? HVAlignment.CenterLeft);
            }

            /// <summary>
            /// Creates a text element with specified parameters for rendering
            /// text and associated styles.
            /// </summary>
            /// <remarks>This method processes the input parameters to handle
            /// various text rendering scenarios, such as vertical text,
            /// multi-line text, and text with special formatting
            /// (e.g., bold or underlined).</remarks>
            /// <param name="prm">A reference to the <see cref="DrawLabelParams"/>
            /// structure containing the text, font, colors, alignment,
            /// and other rendering options.</param>
            /// <param name="textOverride">The <see langword="string"/> to use as the text
            /// content for the element, overriding any text specified in <paramref name="prm"/>.</param>
            /// <returns>An <see cref="DrawElementParams"/> object that encapsulates
            /// the logic for measuring and drawing the text
            /// element, including alignment and font styles.</returns>
            public static DrawElementParams CreateTextElement(
                ref DrawLabelParams prm,
                string? textOverride = null)
            {
                var image = prm.Image;
                var minTextWidth = prm.MinTextWidth;
                var indexAccel = prm.IndexAccel;
                var s = textOverride ?? prm.Text;
                string[]? splitText = null;
                var font = prm.Font;
                var foreColor = prm.ForegroundColor;
                var backColor = prm.BackgroundColor;
                var isVertical = prm.IsVertical;
                var isVerticalText = prm.IsVerticalText;
                var drawDebugCorners = prm.DrawDebugCorners || Graphics.DrawDebugCorners;
                var imageVerticalAlignment = prm.ImageVerticalAlignment;
                var imageHorizontalAlignment = prm.ImageHorizontalAlignment;

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

                TextAndFontStyle[]? parsed = prm.TextAndFontStyle;

                if(textOverride is null)
                {
                    if (parsed is null)
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
                }
                else
                {
                    parsed = null;
                }

                DrawElementParams textElement = new()
                {
                    GetSize = (dc) =>
                    {
                        var result = Internal();

                        if(minTextWidth is not null)
                        {
                            result.Width = Math.Max(minTextWidth.Value, result.Width);
                        }

                        return result;

                        SizeD Internal()
                        {
                            if (parsed is null)
                            {
                                SizeD result;

                                if (splitText is null)
                                {
                                    result = dc.MeasureText(s, font);

                                    if (isVerticalText)
                                    {
                                        result.SwapWidthAndHeight();
                                    }
                                }
                                else
                                {
                                    result = dc.DrawStrings(
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
                                var result = dc.DrawTextWithFontStyle(
                                            parsed,
                                            PointD.Empty,
                                            font,
                                            Color.Empty);
                                return result;
                            }
                        }
                    },
                    Draw = (dc, rect) =>
                    {
                        if (parsed is null)
                        {
                            if (splitText is null)
                            {
                                if (isVerticalText)
                                {
                                    dc.DrawTextWithAngle(
                                        s,
                                        (rect.X + rect.Width, rect.Y),
                                        font,
                                        foreColor,
                                        backColor,
                                        270);
                                }
                                else
                                    dc.DrawText(s, rect.Location, font, foreColor, backColor);
                            }
                            else
                            {
                                dc.DrawStrings(
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
                            dc.DrawTextWithFontStyle(
                                parsed,
                                rect.Location,
                                font,
                                foreColor,
                                backColor);
                        }

#if DEBUG
                        if (drawDebugCorners)
                        {
                            BorderSettings.DrawDesignCorners(
                                dc,
                                rect,
                                BorderSettings.DebugBorderBlue);
                        }
#endif
                    },
                    Alignment = prm.GetTextAlignment(),
                };

                return textElement;
            }

            /// <summary>
            /// Calculates the actual size of the object, ensuring it meets the minimum width requirement.
            /// </summary>
            /// <remarks>The method uses the provided <see cref="Graphics"/> context to determine the
            /// size of the object.  If the calculated width is smaller than the
            /// minimum width, the width is adjusted to
            /// meet the minimum requirement while the height remains unchanged.</remarks>
            /// <param name="dc">The <see cref="Graphics"/> context used to measure the size.</param>
            /// <returns>A <see cref="SizeD"/> structure representing the calculated size.
            /// The width is adjusted to ensure it is
            /// not less than the minimum width.</returns>
            public readonly SizeD GetRealSize(Graphics dc)
            {
                var result = GetSize(dc);
                if (result.Width < MinWidth)
                    result.Width = MinWidth;
                return result;
            }
        }

        /// <summary>
        /// Contains parameters for the draw label method.
        /// </summary>
        public struct DrawLabelParams
        {
            /// <summary>
            /// Represents the minimum text width as a coordinate value.
            /// </summary>
            /// <remarks>This field is nullable, meaning it can hold a value of <see langword="null"/>
            /// to indicate that no minimum text width is specified.</remarks>
            public Coord? MinTextWidth;

            /// <summary>
            /// Gets or sets array of elements to draw before the label text and image.
            /// </summary>
            public DrawElementParams[]? PrefixElements;

            /// <summary>
            /// Gets or sets array of elements to draw after the label text and image.
            /// </summary>
            public DrawElementParams[]? SuffixElements;

            /// <summary>
            /// Gets or sets vertical alignment of the image.
            /// </summary>
            public VerticalAlignment? ImageVerticalAlignment;

            /// <summary>
            /// Gets or sets horizontal alignment of the image.
            /// </summary>
            public HorizontalAlignment? ImageHorizontalAlignment;

            /// <summary>
            /// Gets or sets distance between lines of text.
            /// </summary>
            public Coord LineDistance;

            /// <summary>
            /// Gets or sets a value indicating whether the text should be rendered vertically.
            /// </summary>
            public bool IsVerticalText;

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
            /// Gets or sets alignment of the image and text block.
            /// Default is <see cref="HVAlignment.TopLeft"/>.
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
            /// <param name="alignment">Alignment of the image and text block.</param>
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

            /// <summary>
            /// Determines the image alignment based on the current orientation.
            /// </summary>
            /// <returns>A tuple containing the horizontal and vertical alignment.</returns>
            public readonly HVAlignment GetImageAlignment()
            {
                if (IsVertical)
                {
                    return (
                        HorizontalAlignment.Center,
                        ImageVerticalAlignment ?? VerticalAlignment.Top);
                }
                else
                {
                    return (
                        ImageHorizontalAlignment ?? HorizontalAlignment.Left,
                        ImageVerticalAlignment ?? VerticalAlignment.Center);
                }
            }

            /// <summary>
            /// Determines the text alignment based on the current orientation.
            /// </summary>
            /// <returns>A tuple containing the horizontal and vertical alignment.</returns>
            public readonly HVAlignment GetTextAlignment()
            {
                if (IsVertical)
                    return (HorizontalAlignment.Center, VerticalAlignment.Top);
                else
                    return (HorizontalAlignment.Left, VerticalAlignment.Center);
            }
        }
    }
}
