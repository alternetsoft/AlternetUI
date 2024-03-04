namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a brush of a single color. Brushes are used to fill graphics shapes, such
    /// as rectangles, ellipses, pies, polygons, and paths.
    /// </summary>
    public sealed class TextureBrush : Brush
    {
        private Image image;

        /// <summary>
        /// Initializes a new <see cref="TextureBrush"/> object of the specified color.
        /// </summary>
        /// <param name="image">An <see cref="Image"/> that represents the texture of
        /// this brush.</param>
        public TextureBrush(Image image)
            : base(new UI.Native.TextureBrush(), false)
        {
            this.image = image;
            if(image is not null)
                Initialize(image);
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
                Initialize(image);
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.Texture;

        private protected override bool EqualsCore(Brush other)
        {
            var o = other as TextureBrush;
            if (o == null)
                return false;
            return Image == o.Image;
        }

        private protected override int GetHashCodeCore() => Image.GetHashCode();

        private protected override string ToStringCore()
        {
            return $"TextureBrush";
        }

        private void Initialize(Image image)
        {
            ((UI.Native.TextureBrush)NativeBrush).Initialize(image.NativeImage);
        }
    }
}