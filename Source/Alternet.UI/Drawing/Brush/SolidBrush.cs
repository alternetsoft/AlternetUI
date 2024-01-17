namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a brush of a single color. Brushes are used to fill graphics shapes, such
    /// as rectangles, ellipses, pies, polygons, and paths.
    /// </summary>
    public sealed class SolidBrush : Brush
    {
        private Color color;

        /// <summary>
        /// Initializes a new <see cref="SolidBrush"/> object of the specified color.
        /// </summary>
        /// <param name="color">A <see cref="Color"/> structure that represents the color of
        /// this brush.</param>
        public SolidBrush(Color color)
            : this(color, immutable: false)
        {
        }

        internal SolidBrush(Color color, bool immutable)
            : base(new UI.Native.SolidBrush(), immutable)
        {
            this.color = color;
            ((UI.Native.SolidBrush)NativeBrush).Initialize(color);
        }

        /// <summary>
        /// Gets the color of this SolidBrush object.
        /// </summary>
        /// <value>A <see cref="Color"/> structure that represents the color of this brush.</value>
        public Color Color
        {
            get => color;

            set
            {
                if (color == value || Immutable)
                    return;
                color = value;
                ((UI.Native.SolidBrush)NativeBrush).Initialize(color);
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.Solid;

        internal override Color BrushColor => this.Color;

        /// <summary>
        /// Same as <see cref="Color"/> property implemented as method.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            Color = color;
        }

        private protected override bool EqualsCore(Brush other)
        {
            var o = other as SolidBrush;
            if (o == null)
                return false;

            return Color == o.Color;
        }

        private protected override int GetHashCodeCore() => Color.GetHashCode();

        private protected override string ToStringCore()
        {
            return $"SolidBrush ({Color})";
        }
    }
}