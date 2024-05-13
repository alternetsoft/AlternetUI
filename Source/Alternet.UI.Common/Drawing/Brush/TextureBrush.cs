namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a brush of a single color. Brushes are used to fill graphics shapes, such
    /// as rectangles, ellipses, pies, polygons, and paths.
    /// </summary>
    public class TextureBrush : Brush
    {
        private Image image;

        /// <summary>
        /// Initializes a new <see cref="TextureBrush"/> object of the specified color.
        /// </summary>
        /// <param name="image">An <see cref="Image"/> that represents the texture of
        /// this brush.</param>
        public TextureBrush(Image image)
            : base(false)
        {
            this.image = image;
        }

        /// <summary>
        /// Gets the texture of this brush object.
        /// </summary>
        /// <value>An <see cref="Image"/> structure that represents the
        /// texture of this brush.</value>
        public Image Image
        {
            get => image;

            set
            {
                if (image == value || Immutable)
                    return;
                image = value;
                UpdateRequired = true;
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.Texture;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"TextureBrush";
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Image.GetHashCode();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public override bool Equals(object? other)
        {
            var o = other as TextureBrush;
            if (o == null)
                return false;
            CheckDisposed();
            return Image == o.Image;
        }

        /// <inheritdoc/>
        protected override object CreateHandler()
        {
            return NativeDrawing.Default.CreateTextureBrush();
        }

        /// <inheritdoc/>
        protected override void UpdateHandler()
        {
            NativeDrawing.Default.UpdateTextureBrush(this);
        }
    }
}