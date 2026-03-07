using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Skia;
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
        /// Returns the size of a single character when drawn with the specified font.
        /// </summary>
        /// <param name="ch">The character to measure.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <returns>
        /// A <see cref="SizeD"/> structure representing the width and height of the character.
        /// </returns>
        public SizeD CharSize(char ch, Font font)
        {
#pragma warning disable
            Span<char> buffer = stackalloc char[1];
            buffer[0] = ch;
#pragma warning restore
            return GetTextExtent(buffer, font);
        }

        /// <summary>
        /// Returns the size of a pair of identical characters when drawn with the specified font.
        /// </summary>
        /// <param name="ch">The character to measure as a pair.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <returns>
        /// A <see cref="SizeD"/> structure representing the width and height of the character pair.
        /// </returns>
        public SizeD CharPairSize(char ch, Font font)
        {
#pragma warning disable
            Span<char> buffer = stackalloc char[2];
            buffer[0] = ch;
            buffer[1] = ch;
#pragma warning restore
            return GetTextExtent(buffer, font);
        }

        /// <summary>
        /// Calculates the size of a single character and the spacing between two consecutive characters.
        /// </summary>
        /// <param name="ch">The character to measure.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <param name="charSize">When this method returns, contains the size of the character.</param>
        /// <param name="spacing">When this method returns, contains the spacing between
        /// two consecutive characters.</param>
        public void InterCharSpacing(char ch, Font font, out SizeD charSize, out Coord spacing)
        {
            charSize = CharSize(ch, font);
            spacing = CharPairSize(ch, font).Width - (2 * charSize.Width);
        }

        /// <summary>
        /// Returns the size of a sequence of identical characters when drawn with the specified font.
        /// </summary>
        /// <param name="ch">The character to measure.</param>
        /// <param name="count">The number of times the character is repeated.</param>
        /// <param name="font">The font used for measurement.</param>
        /// <returns>
        /// A <see cref="SizeD"/> structure representing the width and height of the repeated characters.
        /// </returns>
        public SizeD CharSize(char ch, int count, Font font)
        {
            if (count == 1)
                return CharSize(ch, font);
            if (count <= 0)
                return SizeD.Empty;

            SizeD result = SizeD.Empty;

            SkiaHelper.InvokeWithFilledSpan(
                count,
                ch,
                span =>
                {
                    result = GetTextExtent(span, font);
                });

            return result;
        }

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
        public abstract SizeD GetTextExtent(ReadOnlySpan<char> text, Font font);

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
            ReadOnlySpan<char> text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            Coord angle);

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
        public abstract void DrawText(ReadOnlySpan<char> text, Font font, Brush brush, RectD bounds);

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
        public abstract void DrawText(ReadOnlySpan<char> text, Font font, Brush brush, PointD origin);

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
            ReadOnlySpan<char> text,
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
        public void DrawText(ReadOnlySpan<char> text, PointD origin)
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
            ReadOnlySpan<char> text,
            RectD rect,
            Font font,
            Color foreColor,
            Color backColor)
        {
            Save();

            try
            {
                ClipRect(rect);
                DrawText(
                    text,
                    rect.Location,
                    font,
                    foreColor,
                    backColor);
            }
            finally
            {
                Restore();
            }
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
            ReadOnlySpan<char> text,
            Font font,
            Brush brush,
            RectD rect,
            TextFormat format)
        {
            if (text.IsEmpty)
                return rect.WithEmptySize();

            var document = SafeDocument;
            var wrappedText = document.WrappedText;

            if (rect.HasEmptyWidth)
                rect.Width = MaxCoord;

            if (rect.HasEmptyHeight)
                rect.Height = MaxCoord;

            wrappedText.SuspendLayout();
            try
            {
                document.Size = rect.Size;
                wrappedText.SetFormat(format.AsRecord);
                wrappedText.Text = string.Empty;
                wrappedText.Text = text.ToString();
                wrappedText.Font = font;
                wrappedText.ForegroundColor = brush.AsColor;
            }
            finally
            {
                wrappedText.ResumeLayout(true, true);
            }

            TemplateUtils.RaisePaintClipped(wrappedText, this, rect.Location, isClipped: true);
            var result = wrappedText.Bounds.WithLocation(rect.Location);
            return result;
        }

        /// <summary>
        /// Draws text with html bold tags.
        /// </summary>
        public virtual SizeD DrawTextWithBoldTags(
            ReadOnlySpan<char> text,
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
            ReadOnlySpan<char> text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            HVAlignment? alignment = null,
            int indexAccel = -1)
        {
            DrawLabelParams prm = new(
                text.ToString(),
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

            DrawElementsParams drawParams = new();

            if (image is null)
            {
                if (prm.TextVisible)
                    drawParams.Elements = [DrawElementParams.CreateTextElement(ref prm)];
            }
            else
            {
                var imageElement = DrawElementParams.CreateImageElement(ref prm);

                if (prm.TextVisible)
                {
                    var textElement = DrawElementParams.CreateTextElement(ref prm);

                    drawParams.Elements = prm.IsImageAfterText ? [textElement, imageElement] : [imageElement, textElement];
                }
                else
                {
                    drawParams.Elements = [imageElement];
                }
            }

            if (prm.PrefixElements is not null || prm.SuffixElements is not null)
            {
                drawParams.Elements = ArrayUtils.CombineArrays<DrawElementParams>(
                    prm.PrefixElements,
                    drawParams.Elements,
                    prm.SuffixElements);
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

            Coord? emptyStringMeasure = null;

            foreach (var s in wrappedText)
            {
                SizeD measure;

                var isEmpty = s is null || s.Length == 0;

                if (isEmpty)
                {
                    emptyStringMeasure ??= font.GetHeight(this);
                    measure = (CoordD.Empty, emptyStringMeasure.Value);
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

#if DEBUG
                AlignUtils.DebugIdentifier = element.DebugIdentifier;
#endif

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
            /// Gets or sets a debug identifier for the element.
            /// </summary>
            public string? DebugIdentifier;

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

                var imageMargin = prm.ImageMargin;

#if DEBUG
                var drawDebugCorners = prm.DrawDebugCorners || Graphics.DrawDebugCorners;
#endif

                DrawElementParams imageElement = new()
                {
                    IsImage = true,
                    GetSize = (dc) =>
                    {
                        var result = image.SizeDip(dc.ScaleFactor);
                        result += imageMargin.Size;
                        return result;
                    },
                    Draw = (dc, rect) =>
                    {
                        var deflated = rect.DeflatedWithPadding(imageMargin);

                        dc.DrawImage(image, deflated.Location);

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

#if DEBUG
                imageElement.DebugIdentifier = $"Image{prm.Text}";
#endif
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

                if (textOverride is null)
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

                        if (minTextWidth is not null)
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

#if DEBUG
                textElement.DebugIdentifier = s;
#endif
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
            /// Gets or sets a value which specifies whether image to text are aligned vertically or horizontally.
            /// </summary>
            public bool IsVertical;

            /// <summary>
            /// Gets or sets margin around the image. This is used when image is drawn.
            /// </summary>
            public Thickness ImageMargin;

            /// <summary>
            /// Gets or sets a value indicating whether the image is displayed after the text.
            /// </summary>
            public bool IsImageAfterText;

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
            /// Gets or sets a value indicating whether the text is visible. If set to <see langword="false"/>,
            /// the text will not be drawn. Default value is <see langword="true"/>. This property can be used
            /// to control the visibility of the text independently from other elements, such as images
            /// or prefixes/suffixes, allowing for flexible rendering options based on the specific requirements of the drawing operation.
            /// </summary>
            public bool TextVisible = true;

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
            /// Sets value of the <see cref="MinTextWidth"/> property.
            /// </summary>
            /// <param name="value">The minimum text width.</param>
            public void SetMinTextWidth(Coord? value)
            {
                MinTextWidth = value;
            }

            /// <summary>
            /// Sets the collection of prefix elements to be used in subsequent drawing operations.
            /// This is the same as setting the <see cref="PrefixElements"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <remarks>Setting prefix elements affects how future drawing operations are rendered,
            /// as these elements will be applied before the main drawing content. If <paramref name="value"/> is <see
            /// langword="null"/>, any existing prefix elements are removed.</remarks>
            /// <param name="value">An array of <see cref="DrawElementParams"/> objects that represent the prefix elements to set. This
            /// parameter can be <see langword="null"/> to clear the current prefix elements.</param>
            public void SetPrefixElements(DrawElementParams[]? value)
            {
                PrefixElements = value;
            }

            /// <summary>
            /// Sets the collection of suffix elements used for drawing operations.
            /// This is the same as setting the <see cref="SuffixElements"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <remarks>Setting this value to <see langword="null"/> clears any previously assigned
            /// suffix elements. The provided array is used to update the internal state for subsequent drawing
            /// operations.</remarks>
            /// <param name="value">An array of <see cref="DrawElementParams"/> objects that represent the suffix elements to assign.
            /// Specify <see langword="null"/> to remove all existing suffix elements.</param>
            public void SetSuffixElements(DrawElementParams[]? value)
            {
                SuffixElements = value;
            }

            /// <summary>
            /// Sets the vertical alignment for the image associated with the label.
            /// This is the same as setting the <see cref="ImageVerticalAlignment"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The vertical alignment to set for the image.</param>
            public void SetImageVerticalAlignment(VerticalAlignment? value)
            {
                ImageVerticalAlignment = value;
            }

            /// <summary>
            /// Sets the horizontal alignment for the image associated with the label.
            /// This is the same as setting the <see cref="ImageHorizontalAlignment"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The horizontal alignment to set for the image.</param>
            public void SetImageHorizontalAlignment(HorizontalAlignment? value)
            {
                ImageHorizontalAlignment = value;
            }

            /// <summary>
            /// Sets the distance between lines of the text.
            /// This is the same as setting the <see cref="LineDistance"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The coordinate value representing the desired line distance.</param>
            public void SetLineDistance(Coord value)
            {
                LineDistance = value;
            }

            /// <summary>
            /// Sets a value indicating whether the text is displayed vertically.
            /// This is the same as setting the <see cref="IsVerticalText"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">A boolean value that determines if the text is vertical. Set to <see langword="true"/> to display text
            /// vertically; otherwise, set to <see langword="false"/>.</param>
            public void SetIsVerticalText(bool value)
            {
                IsVertical = value;
            }

            /// <summary>
            /// Sets the horizontal alignment of the text line within the text block.
            /// This is the same as setting the <see cref="TextHorizontalAlignment"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">the horizontal alignment of the text line within the text block.</param>
            public void SetTextHorizontalAlignment(TextHorizontalAlignment value)
            {
                TextHorizontalAlignment = value;
            }

            /// <summary>
            /// Sets a value which specifies whether image to text are aligned vertically or horizontally.
            /// This is the same as setting the <see cref="IsVertical"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            public void SetIsVertical(bool value)
            {
                IsVertical = value;
            }

            /// <summary>
            /// Sets a value indicating whether the image is displayed after the text.
            /// This is the same as setting the <see cref="IsImageAfterText"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">A boolean value that determines if the image is displayed after the text.
            /// Set to <see langword="true"/> to display the image
            /// after the text; otherwise, set to <see langword="false"/>.</param>
            public void SetIsImageAfterText(bool value)
            {
                IsImageAfterText = value;
            }

            /// <summary>
            /// Sets the distance between the image and its label.
            /// If value is <see langword="null"/>, <see cref="SpeedButton.DefaultImageLabelDistance"/> is used as the default distance.
            /// This is the same as setting the <see cref="ImageLabelDistance"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <remarks>Use this method to adjust the spacing between the image and its label to meet
            /// specific layout requirements. Passing <see langword="null"/> removes any previously set
            /// distance.</remarks>
            /// <param name="value">The distance to set between the image and its label.
            /// Specify <see langword="null"/> to indicate that the
            /// default distance should be used.</param>
            public void SetImageLabelDistance(Coord? value)
            {
                ImageLabelDistance = value;
            }

            /// <summary>
            /// Sets a value indicating whether the label should be drawn.
            /// This is the same as setting the <see cref="Visible"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">A boolean value that determines if the label should be drawn.
            /// Set to <see langword="true"/> to draw the label; otherwise, set to <see langword="false"/>.</param>
            public void SetVisible(bool value)
            {
                Visible = value;
            }

            /// <summary>
            /// Sets the text content to the specified value.
            /// This is the same as setting the <see cref="Text"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The text to set as the content. This value cannot be null.</param>
            public void SetText(string value)
            {
                Text = value;
            }

            /// <summary>
            /// Sets the collection of text and font styles to be used instead of <see cref="Text"/>.
            /// This is the same as setting the <see cref="TextAndFontStyle"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <remarks>If <paramref name="value"/> is <see langword="null"/>, any previously set
            /// text and font styles are removed.</remarks>
            /// <param name="value">An array of <see cref="TextAndFontStyle"/> objects that specify the text and font styles to use. This
            /// parameter can be <see langword="null"/> to clear the current styles.</param>
            public void SetTextAndFontStyle(TextAndFontStyle[]? value)
            {
                TextAndFontStyle = value;
            }

            /// <summary>
            /// Sets the font used to render text.
            /// This is the same as setting the <see cref="Font"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The font to apply for text rendering. Cannot be null.</param>
            public void SetFont(Font value)
            {
                Font = value;
            }

            /// <summary>
            /// Sets the foreground color for the element.
            /// This is the same as setting the <see cref="ForegroundColor"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The color to set as the foreground color. This value cannot be null.</param>
            public void SetForegroundColor(Color value)
            {
                ForegroundColor = value;
            }

            /// <summary>
            /// Sets the background color of the control.
            /// If parameter value equals to <see cref="Color.Empty"/>, background will not be painted.
            /// This is the same as setting the <see cref="BackgroundColor"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The color to set as the background. This value cannot be null.</param>
            public void SetBackgroundColor(Color value)
            {
                BackgroundColor = value;
            }

            /// <summary>
            /// Sets the image to be displayed.
            /// This is the same as setting the <see cref="Image"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The image to set. Can be null to clear the current image.</param>
            public void SetImage(Image? value)
            {
                Image = value;
            }

            /// <summary>
            /// Sets the rectangle in which drawing is performed to the specified value.
            /// This is the same as setting the <see cref="Rect"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The new rectangle value to assign. This value must be a valid instance of <see cref="RectD"/>.</param>
            public void SetRect(RectD value)
            {
                Rect = value;
            }

            /// <summary>
            /// Sets the horizontal and vertical alignment of the label to the specified value.
            /// This aligment determines how the whole text and image block is positioned within the drawing rectangle.
            /// This is the same as setting the <see cref="Alignment"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The alignment value to apply. Specifies how the element is positioned horizontally and vertically.</param>
            public void SetAlignment(HVAlignment value)
            {
                Alignment = value;
            }

            /// <summary>
            /// Sets the index of the underlined mnemonic character.
            /// This is the same as setting the <see cref="IndexAccel"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The index of the underlined mnemonic character to set. Must be a non-negative integer.</param>
            public void SetIndexAccel(int value)
            {
                IndexAccel = value;
            }

            /// <summary>
            /// Sets the flags that determine the drawing behavior of the label.
            /// This is the same as setting the <see cref="Flags"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The flags that specify how the label is drawn. This value cannot be null.</param>
            public void SetFlags(DrawLabelFlags value)
            {
                Flags = value;
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
