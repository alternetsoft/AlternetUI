namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a brush of a single color. Brushes are used to fill graphics shapes, such
    /// as rectangles, ellipses, pies, polygons, and paths.
    /// </summary>
    public class SolidBrush : Brush
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
            : base(immutable)
        {
            this.color = color;
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
                UpdateRequired = true;
            }
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.Solid;

        /// <inheritdoc/>
        public override Color BrushColor => this.Color;

        /// <summary>
        /// Same as <see cref="Color"/> property implemented as method.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            Color = color;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"SolidBrush ({Color})";
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Color.GetHashCode();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public override bool Equals(object? other)
        {
            var o = other as SolidBrush;
            if (o == null)
                return false;

            CheckDisposed();
            return Color == o.Color;
        }

        /// <inheritdoc/>
        protected override object CreateHandler()
        {
            return NativeDrawing.Default.CreateSolidBrush();
        }

        /// <inheritdoc/>
        protected override void UpdateHandler()
        {
            NativeDrawing.Default.UpdateSolidBrush(this);
        }
    }
}