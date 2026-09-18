using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
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
                return CreateImageElement(ref prm, ref prm.ImageParams);
            }

            /// <summary>
            /// Creates a new image element based on the specified drawing parameters.
            /// </summary>
            /// <param name="labelPrm">A reference to the <see cref="DrawLabelParams"/>
            /// structure containing the parameters for drawing of the label.</param>
            /// <param name="imageOverride">The <see cref="Image"/> to use as the image for the element,
            /// overriding any image specified in <paramref name="labelPrm"/>.</param>
            /// <returns>A <see cref="DrawElementParams"/> object representing the image element to be drawn.</returns>
            public static DrawElementParams CreateImageElement(ref DrawLabelParams labelPrm, Image? imageOverride = null)
            {
                return CreateImageElement(ref labelPrm, ref labelPrm.ImageParams, imageOverride);
            }

            /// <summary>
            /// Creates an image element based on the specified drawing parameters.
            /// </summary>
            /// <remarks>The returned <see cref="DrawElementParams"/> object includes
            /// the size calculation and drawing logic for the image, as well
            /// as alignment settings derived from the
            /// provided parameters.</remarks>
            /// <param name="labelPrm">A reference to the <see cref="DrawLabelParams"/>
            /// structure containing the parameters for drawing of the label.</param>
            /// <param name="prm">A reference to the <see cref="DrawLabelImageParams"/>
            /// structure containing the parameters for drawing
            /// the image, including the image source and alignment settings.</param>
            /// <param name="imageOverride">The <see cref="Image"/> to use as the image
            /// for the element, overriding any image specified in <paramref name="prm"/>.</param>
            /// <returns>An <see cref="DrawElementParams"/> object representing
            /// the image element to be drawn, or <see langword="null"/> if
            /// the <see cref="DrawLabelParams.Image"/> property
            /// is <see langword="null"/>.</returns>
            public static DrawElementParams CreateImageElement(
                ref DrawLabelParams labelPrm,
                ref DrawLabelImageParams prm,
                Image? imageOverride = null)
            {
                var image = imageOverride ?? prm.Image;

                if (image is null)
                    return DrawElementParams.Default;

                var imageMargin = prm.ImageMargin;

#if DEBUG
                var drawDebugCorners = Graphics.DrawDebugCorners;
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
                                SystemSettings.AppearanceIsDark,
                                BorderSettings.DebugBorderBlue);
                        }
#endif
                    },
                    Alignment = prm.GetImageAlignment(labelPrm.IsVertical),
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
                var vertTextDirection = prm.VertDirection;
                var drawDebugCorners = prm.DrawDebugCorners || Graphics.DrawDebugCorners;

                var textHorizontalAlignment = prm.TextHorizontalAlignment;
                var lineDistance = prm.LineDistance;

                var hasNewLineChars = prm.Flags.HasFlag(DrawLabelFlags.TextHasNewLineChars);

                if (hasNewLineChars)
                {
                    if (StringUtils.ContainsNewLineChars(s))
                    {
                        splitText = StringUtils.TrimWithEllipsisOptional(StringUtils.Split(s, removeEmptyLines: false), prm.MaxLines);

                        if (splitText.Length <= 1)
                        {
                            splitText = null;
                            hasNewLineChars = false;
                        }
                    }
                    else
                    {
                        hasNewLineChars = false;
                    }
                }

                TextAndFontStyle[][]? parsedLines = prm.TextAndFontStyle is null ? null : new[] { prm.TextAndFontStyle };

                if (textOverride is null)
                {
                    if (parsedLines is null)
                    {
                        if (prm.Flags.HasFlag(DrawLabelFlags.TextHasBold))
                        {
                            if (hasNewLineChars)
                            {
                                parsedLines = RegexUtils.GetTextLinesBoldTagSplitted(splitText);
                            }
                            else
                            {
                                parsedLines = new[] { RegexUtils.GetBoldTagSplitted(s) };
                            }
                        }
                        else
                            if (indexAccel >= 0)
                            {
                                if (hasNewLineChars)
                                {
                                    parsedLines = StringUtils.ParseTextLinesWithIndexAccel(
                                        splitText,
                                        indexAccel,
                                        FontStyle.Underline);
                                }
                                else
                                {
                                    var parsed = StringUtils.ParseTextWithIndexAccel(
                                        s,
                                        indexAccel,
                                        FontStyle.Underline);
                                    parsedLines = new[] { parsed };
                                }
                            }
                    }
                }
                else
                {
                    parsedLines = null;
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
                            if (parsedLines is null)
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
                                var sizes = dc.DrawTextLinesWithFontStyle(
                                            parsedLines,
                                            lineDistance,
                                            PointD.Empty,
                                            font,
                                            Color.Empty);
                                var sumHeights = SizeD.SumHeights(sizes, lineDistance);
                                var maxWidth = SizeD.MaxWidth(sizes);
                                result = new SizeD(maxWidth, sumHeights);
                                return result;
                            }
                        }
                    },
                    Draw = (dc, rect) =>
                    {
                        if (parsedLines is null)
                        {
                            if (splitText is null)
                            {
                                if (isVerticalText)
                                {
                                    dc.DrawVertText(s, font, foreColor, rect, vertTextDirection);
                                }
                                else
                                {
                                    dc.DrawText(s, rect.Location, font, foreColor, backColor);
                                }
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
                            dc.DrawTextLinesWithFontStyle(
                                parsedLines,
                                lineDistance,
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
                                SystemSettings.AppearanceIsDark,
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
        /// Contains image parameters for the draw label method.
        /// </summary>
        public struct DrawLabelImageParams
        {
            /// <summary>
            /// Gets or sets vertical alignment of the image.
            /// </summary>
            public VerticalAlignment? ImageVerticalAlignment;

            /// <summary>
            /// Gets or sets horizontal alignment of the image.
            /// </summary>
            public HorizontalAlignment? ImageHorizontalAlignment;

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
            /// Gets or sets optional image which is shown near the text. Default is Null.
            /// </summary>
            public Image? Image;

            /// <summary>
            /// Determines the image alignment based on the current orientation.
            /// </summary>
            /// <returns>A tuple containing the horizontal and vertical alignment.</returns>
            public readonly HVAlignment GetImageAlignment(bool isVertical)
            {
                if (isVertical)
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
        }

        /// <summary>
        /// Contains parameters for the draw label method.
        /// </summary>
        public struct DrawLabelParams
        {
            /// <summary>
            /// Gets or sets image parameters, including alignment, margin, and other settings.
            /// </summary>
            public DrawLabelImageParams ImageParams;

            /// <summary>
            /// Gets or sets vertical alignment of the image.
            /// </summary>
            public VerticalAlignment? ImageVerticalAlignment
            {
                readonly get => ImageParams.ImageVerticalAlignment;
                set => ImageParams.ImageVerticalAlignment = value;
            }

            /// <summary>
            /// Gets or sets horizontal alignment of the image.
            /// </summary>
            public HorizontalAlignment? ImageHorizontalAlignment
            {
                readonly get => ImageParams.ImageHorizontalAlignment;
                set => ImageParams.ImageHorizontalAlignment = value;
            }

            /// <summary>
            /// Gets or sets margin around the image. This is used when image is drawn.
            /// </summary>
            public Thickness ImageMargin
            {
                readonly get => ImageParams.ImageMargin;
                set => ImageParams.ImageMargin = value;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the image is displayed after the text.
            /// </summary>
            public bool IsImageAfterText
            {
                readonly get => ImageParams.IsImageAfterText;
                set => ImageParams.IsImageAfterText = value;
            }

            /// <summary>
            /// Gets or sets distance between image and label. If Null,
            /// <see cref="SpeedButton.DefaultImageLabelDistance"/> is used.
            /// </summary>
            public Coord? ImageLabelDistance
            {
                readonly get => ImageParams.ImageLabelDistance;
                set => ImageParams.ImageLabelDistance = value;
            }

            /// <summary>
            /// Gets or sets optional image which is shown near the text. Default is Null.
            /// </summary>
            public Image? Image
            {
                readonly get => ImageParams.Image;
                set => ImageParams.Image = value;
            }

            /// <summary>
            /// Gets or sets a list of additional images to be drawn alongside the main image.
            /// </summary>
            public List<DrawLabelImageParams>? AdditionalImages;

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
            /// Gets or sets distance between lines of text.
            /// </summary>
            public Coord LineDistance;

            /// <summary>
            /// Gets or sets a value indicating whether the text should be rendered vertically.
            /// </summary>
            public bool IsVerticalText;

            /// <summary>
            /// Gets or sets the direction in which vertical text should be drawn.
            /// If not specified, the default direction is used (specified in <see cref="Graphics.DefaultVertTextDirection"/>).
            /// This property is relevant only when <see cref="IsVerticalText"/> is set to <see langword="true"/>.
            /// </summary>
            public VerticalTextDirection? VertDirection;

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
            /// Gets or sets maximum number of lines to draw. If Null, all lines are drawn.
            /// </summary>
            public int? MaxLines;

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
            /// or prefixes/suffixes, allowing for flexible rendering options based on
            /// the specific requirements of the drawing operation.
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
            /// Adds an additional image to the drawing parameters.
            /// This method is intended for use in scenarios where multiple images need to be associated
            /// with a single label or text element.
            /// </summary>
            /// <param name="image">The image to add as an additional image.</param>
            /// <param name="imageInfo">The image information associated with the additional image.</param>
            public void AddAdditionalImage(Image image, ListControlItem.ItemImageInfoRef imageInfo)
            {
                DrawLabelImageParams prm = new();
                prm.Image = image;
                prm.IsImageAfterText = imageInfo.IsAfterText;
                prm.ImageMargin = imageInfo.Margin;
                prm.ImageHorizontalAlignment = imageInfo.HorizontalAlignment;
                prm.ImageVerticalAlignment = imageInfo.VerticalAlignment;

                AdditionalImages ??= new();
                AdditionalImages.Add(prm);
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
            /// Sets the collection of images to be used in subsequent drawing operations.
            /// </summary>
            /// <param name="value">An array of <see cref="Image"/> objects to set.
            /// Specify <see langword="null"/> or an empty array to remove all existing images.</param>
            public void SetImages(Image[]? value)
            {
                AdditionalImages?.Clear();

                if (value is null || value.Length == 0)
                {
                    ImageParams.Image = null;
                    return;
                }

                ImageParams.Image = value[0];

                if (value.Length == 1)
                    return;

                AdditionalImages ??= new();

                for (int i = 1; i < value.Length; i++)
                {
                    var newItem = new DrawLabelImageParams
                    {
                        Image = value[i]
                    };

                    AdditionalImages.Add(newItem);
                }
            }

            /// <summary>
            /// Sets the collection of prefix elements to be used in subsequent drawing operations.
            /// This is the same as setting the <see cref="PrefixElements"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <remarks>Setting prefix elements affects how future drawing operations are rendered,
            /// as these elements will be applied before the main drawing content. If <paramref name="value"/> is <see
            /// langword="null"/>, any existing prefix elements are removed.</remarks>
            /// <param name="value">An array of <see cref="DrawElementParams"/> objects
            /// that represent the prefix elements to set. This
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
            /// <param name="value">An array of <see cref="DrawElementParams"/> objects
            /// that represent the suffix elements to assign.
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
            /// <param name="value">A boolean value that determines if the text is vertical.
            /// Set to <see langword="true"/> to display text
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
            /// If value is <see langword="null"/>, <see cref="SpeedButton.DefaultImageLabelDistance"/>
            /// is used as the default distance.
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
            /// <param name="value">An array of <see cref="TextAndFontStyle"/> objects that
            /// specify the text and font styles to use. This
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
            /// <param name="value">The new rectangle value to assign. This value must be
            /// a valid instance of <see cref="RectD"/>.</param>
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
            /// <param name="value">The alignment value to apply. Specifies how the element
            /// is positioned horizontally and vertically.</param>
            public void SetAlignment(HVAlignment value)
            {
                Alignment = value;
            }

            /// <summary>
            /// Sets the index of the underlined mnemonic character.
            /// This is the same as setting the <see cref="IndexAccel"/> property,
            /// but provided as a method for convenience and potential future extensibility.
            /// </summary>
            /// <param name="value">The index of the underlined mnemonic character to set.
            /// Must be a non-negative integer.</param>
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
