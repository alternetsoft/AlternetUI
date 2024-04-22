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
        public override object CreateNativeBrush()
        {
            return NativeDrawing.Default.CreateTextureBrush();
        }

        /// <inheritdoc/>
        protected override void UpdateNativeBrush()
        {
            ((UI.Native.TextureBrush)NativeBrush).Initialize(image.NativeImage);
        }

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
    }
}