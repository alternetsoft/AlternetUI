namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates text layout information (such as alignment and orientation) display manipulations (such
    /// as ellipsis insertion). This class cannot be inherited.
    /// </summary>
    public sealed class TextFormat
    {
        /// <summary>
        /// Gets a default <see cref="TextFormat"/> object.
        /// </summary>
        public static TextFormat Default { get; } = new TextFormat();

        /// <summary>
        /// Gets or sets horizontal alignment of the text.
        /// </summary>
        /// <value>
        /// A <see cref="TextHorizontalAlignment"/> enumeration that specifies the horizontal alignment of the string.
        /// Default is <see cref="TextHorizontalAlignment.Left"/>.
        /// </value>
        public TextHorizontalAlignment HorizontalAlignment { get; set; } = TextHorizontalAlignment.Left;

        /// <summary>
        /// Gets or sets the vertical alignment of the text.
        /// </summary>
        /// <value>
        /// A <see cref="TextVerticalAlignment"/> enumeration that represents the vertical alignment.
        /// Default is <see cref="TextVerticalAlignment.Top"/>.
        /// </value>
        public TextVerticalAlignment VerticalAlignment { get; set; } = TextVerticalAlignment.Top;

        /// <summary>
        /// Gets or sets the <see cref="TextTrimming"/> enumeration for this <see cref="TextFormat"/> object.
        /// </summary>
        /// <value>A <see cref="TextTrimming"/> enumeration that indicates how text drawn with this
        /// <see cref="TextFormat"/> object is trimmed when it exceeds the edges of the layout rectangle. Default is <see cref="TextTrimming.None"/></value>
        public TextTrimming Trimming { get; set; } = TextTrimming.None;

        /// <summary>
        /// Gets or sets the <see cref="TextWrapping"/> enumeration for this <see cref="TextFormat"/> object.
        /// </summary>
        /// <value>A <see cref="TextWrapping"/> enumeration that indicates how text drawn with this
        /// <see cref="TextFormat"/> object is wrapped when it exceeds the edges of the layout rectangle. Default is <see cref="TextWrapping.Character"/></value>
        public TextWrapping Wrapping { get; set; } = TextWrapping.Character;
    }
}