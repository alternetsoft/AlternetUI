using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates text layout information (such as alignment and orientation),
    /// display manipulations (such as ellipsis insertion) and other formatting options.
    /// </summary>
    public partial class TextFormat : ImmutableWithRecord<TextFormat.Record>
    {
        /// <summary>
        /// Gets default horizontal alignment of the text;
        /// </summary>
        public const TextHorizontalAlignment DefaultHorizontalAlignment = TextHorizontalAlignment.Left;

        /// <summary>
        /// Gets default vertical alignment of the text;
        /// </summary>
        public const TextVerticalAlignment DefaultVerticalAlignment = TextVerticalAlignment.Top;

        /// <summary>
        /// Gets default trimming style of the text;
        /// </summary>
        public const TextTrimming DefaultTrimming = TextTrimming.Pixel;

        /// <summary>
        /// Gets default wrapping style of the text;
        /// </summary>
        public const TextWrapping DefaultWrapping = TextWrapping.Character;

        private static readonly TextFormat.Record defaultRecord = new();

        /// <summary>
        /// Gets default text format.
        /// </summary>
        public static TextFormat.Record DefaultRecord => defaultRecord;

        /// <summary>
        /// Gets or sets horizontal alignment of the text.
        /// Default is <see cref="TextHorizontalAlignment.Left"/>.
        /// </summary>
        /// <value>
        /// A <see cref="TextHorizontalAlignment"/> enumeration that specifies the
        /// horizontal alignment of the string.
        /// </value>
        public virtual TextHorizontalAlignment HorizontalAlignment
        {
            get
            {
                return record.HorizontalAlignment;
            }

            set
            {
                record.HorizontalAlignment = GetNewFieldValue(record.HorizontalAlignment, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the text.
        /// </summary>
        /// <value>
        /// A <see cref="TextVerticalAlignment"/> enumeration that represents the vertical alignment.
        /// Default is <see cref="TextVerticalAlignment.Top"/>.
        /// </value>
        public virtual TextVerticalAlignment VerticalAlignment
        {
            get
            {
                return record.VerticalAlignment;
            }

            set
            {
                record.VerticalAlignment = GetNewFieldValue(record.VerticalAlignment, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TextTrimming"/> enumeration value for this object.
        /// Default is <see cref="TextTrimming.None"/>.
        /// </summary>
        /// <value>
        /// A <see cref="TextTrimming"/> enumeration that indicates how text
        /// is trimmed when it exceeds the edges of the layout rectangle.
        /// </value>
        public virtual TextTrimming Trimming
        {
            get
            {
                return record.Trimming;
            }

            set
            {
                record.Trimming = GetNewFieldValue(record.Trimming, value);
            }
        }

        /// <summary>
        /// Gets or sets distance between lines of the text. Default is 0.
        /// </summary>
        public virtual Coord Distance
        {
            get
            {
                return record.Distance;
            }

            set
            {
                record.Distance = GetNewFieldValue(record.Distance, value);
            }
        }

        /// <summary>
        /// Gets or sets the top padding of the text.
        /// </summary>
        public Coord PaddingTop
        {
            get => Padding.Top;
            set => Padding = Padding.WithTop(value);
        }

        /// <summary>
        /// Gets or sets the bottom padding of the text.
        /// </summary>
        public Coord PaddingBottom
        {
            get => Padding.Bottom;
            set => Padding = Padding.WithBottom(value);
        }

        /// <summary>
        /// Gets or sets the right padding of the text.
        /// </summary>
        public Coord PaddingRight
        {
            get => Padding.Right;
            set => Padding = Padding.WithRight(value);
        }

        /// <summary>
        /// Gets or sets the left padding of the text.
        /// </summary>
        public Coord PaddingLeft
        {
            get => Padding.Left;
            set => Padding = Padding.WithLeft(value);
        }

        /// <summary>
        /// Gets or sets padding of the text.
        /// </summary>
        public virtual Thickness Padding
        {
            get
            {
                return record.Padding;
            }

            set
            {
                record.Padding = GetNewFieldValue(record.Padding, value);
            }
        }

        /// <summary>
        /// Gets or sets suggested height of the text.
        /// </summary>
        public Coord? SuggestedWidth
        {
            get
            {
                return record.SuggestedWidth;
            }

            set
            {
                record.SuggestedWidth = GetNewFieldValue(record.SuggestedWidth, value);
            }
        }

        /// <summary>
        /// Gets or sets suggested width of the text.
        /// </summary>
        public Coord? SuggestedHeight
        {
            get
            {
                return record.SuggestedHeight;
            }

            set
            {
                record.SuggestedHeight = GetNewFieldValue(record.SuggestedHeight, value);
            }
        }

        /// <summary>
        /// Gets or sets maximal height of the text.
        /// </summary>
        public Coord? MaxWidth
        {
            get
            {
                return record.MaxWidth;
            }

            set
            {
                record.MaxWidth = GetNewFieldValue(record.MaxWidth, value);
            }
        }

        /// <summary>
        /// Gets or sets maximal width of the text.
        /// </summary>
        public Coord? MaxHeight
        {
            get
            {
                return record.MaxHeight;
            }

            set
            {
                record.MaxHeight = GetNewFieldValue(record.MaxHeight, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TextWrapping"/> enumeration value for this object.
        /// Default is <see cref="TextWrapping.Character"/>.
        /// </summary>
        /// <value>
        /// A <see cref="TextWrapping"/> enumeration that indicates how text is
        /// wrapped when it exceeds the edges
        /// of the layout rectangle.
        /// </value>
        public virtual TextWrapping Wrapping
        {
            get
            {
                return record.Wrapping;
            }

            set
            {
                record.Wrapping = GetNewFieldValue(record.Wrapping, value);
            }
        }

        /// <summary>
        /// Creates a copy of default object.
        /// </summary>
        /// <returns></returns>
        public static TextFormat Default() => new();

        /// <summary>
        /// Sets horizontal alignment specified in the
        /// <paramref name="horizontalAlignment"/> parameter.
        /// </summary>
        /// <param name="horizontalAlignment">New horizontal text alignment.</param>
        /// <returns></returns>
        public TextFormat Alignment(TextHorizontalAlignment horizontalAlignment)
        {
            HorizontalAlignment = horizontalAlignment;
            return this;
        }

        /// <summary>
        /// Sets vertical alignment specified in the <paramref name="verticalAlignment"/> parameter.
        /// </summary>
        /// <param name="verticalAlignment">New vertical text alignment.</param>
        /// <returns></returns>
        public TextFormat Alignment(TextVerticalAlignment verticalAlignment)
        {
            VerticalAlignment = verticalAlignment;
            return this;
        }

        /// <summary>
        /// Creates new text format which is a clone of this object but with new horizontal
        /// alignment specified in the <paramref name="horizontalAlignment"/>.
        /// </summary>
        /// <param name="horizontalAlignment">Horizontal text alignment property
        /// of the created text format.</param>
        /// <returns></returns>
        public virtual TextFormat WithAlignment(TextHorizontalAlignment horizontalAlignment)
        {
            var result = Clone();
            result.HorizontalAlignment = horizontalAlignment;
            return result;
        }

        /// <summary>
        /// Creates new text format which is a clone of this object but with new suggested width
        /// specified in the <paramref name="suggestedWidth"/>.
        /// </summary>
        /// <param name="suggestedWidth">Suggested text width property
        /// of the created text format.</param>
        /// <returns></returns>
        public virtual TextFormat WithSuggestedWidth(Coord suggestedWidth)
        {
            var result = Clone();
            result.SuggestedWidth = suggestedWidth;
            return result;
        }

        /// <summary>
        /// Sets suggested width.
        /// </summary>
        /// <param name="suggestedWidth">New suggested text width.</param>
        /// <returns></returns>
        public virtual TextFormat SuggestWidth(Coord suggestedWidth)
        {
            SuggestedWidth = suggestedWidth;
            return this;
        }

        /// <summary>
        /// Sets maximal width.
        /// </summary>
        /// <param name="maxWidth">New maximal text width.</param>
        /// <returns></returns>
        public virtual TextFormat MaximalWidth(Coord maxWidth)
        {
            MaxWidth = maxWidth;
            return this;
        }

        /// <summary>
        /// Creates new text format which is a clone of this object but with new maximal width
        /// specified in the <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="maxWidth">Maximal text width property of the created text format.</param>
        /// <returns></returns>
        public virtual TextFormat WithMaxWidth(Coord maxWidth)
        {
            var result = Clone();
            result.MaxWidth = maxWidth;
            return result;
        }

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual TextFormat Clone()
        {
            TextFormat result = new();
            result.Assign(this);
            return result;
        }

        private static TextFormat CreateImmutable()
        {
            TextFormat result = new();
            result.SetImmutable();
            return result;
        }

        /// <summary>
        /// Contains all properties of the <see cref="TextFormat"/>.
        /// </summary>
        public struct Record
        {
            /// <see cref="TextFormat.SuggestedWidth"/>
            public Coord? SuggestedWidth;

            /// <see cref="TextFormat.SuggestedHeight"/>
            public Coord? SuggestedHeight;

            /// <see cref="TextFormat.MaxWidth"/>
            public Coord? MaxWidth;

            /// <see cref="TextFormat.MaxHeight"/>
            public Coord? MaxHeight;

            /// <see cref="TextFormat.HorizontalAlignment"/>
            public TextHorizontalAlignment HorizontalAlignment = DefaultHorizontalAlignment;

            /// <see cref="TextFormat.VerticalAlignment"/>
            public TextVerticalAlignment VerticalAlignment = DefaultVerticalAlignment;

            /// <see cref="TextFormat.Trimming"/>
            public TextTrimming Trimming = DefaultTrimming;

            /// <see cref="TextFormat.Wrapping"/>
            public TextWrapping Wrapping = DefaultWrapping;

            /// <see cref="TextFormat.Distance"/>
            public Coord Distance;

            /// <see cref="TextFormat.Padding"/>
            public Thickness Padding;

            /// <summary>
            /// Initializes a new instance of the <see cref="Record"/> struct.
            /// </summary>
            public Record()
            {
            }
        }
    }
}