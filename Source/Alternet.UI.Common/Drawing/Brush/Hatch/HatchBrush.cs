using System;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a brush with a hatch style and a color.
    /// </summary>
    /// <remarks>
    /// A hatch pattern is made from the lines of a color defined by the <see cref="Color"/> property.
    /// The <see cref="HatchStyle"/> property defines what type of pattern the brush has and can
    /// be any value from the <see cref="BrushHatchStyle"/> enumeration.
    /// </remarks>
    public class HatchBrush : Brush
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HatchBrush"/> class with the specified
        /// <see cref="BrushHatchStyle"/> enumeration, and the color.
        /// </summary>
        /// <param name="hatchStyle">One of the <see cref="BrushHatchStyle"/> values that
        /// represents the pattern drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="color">The <see cref="Drawing.Color"/> structure that represents the
        /// color of lines drawn by this <see cref="HatchBrush"/>.</param>
        public HatchBrush(BrushHatchStyle hatchStyle, Color color)
            : base(immutable: false)
        {
            HatchStyle = hatchStyle;
            Color = color;
        }

        /// <summary>
        /// Gets the color of hatch lines drawn by this <see cref="HatchBrush"/> object.
        /// </summary>
        /// <value>A <see cref="Drawing.Color"/> structure that represents the color for this
        /// <see cref="HatchBrush"/>.</value>
        public Color Color
        {
            get;
        }

        /// <summary>
        /// Gets the hatch style of this <see cref="HatchBrush"/> object.
        /// </summary>
        /// <value>One of the <see cref="BrushHatchStyle"/> values that represents the pattern of
        /// this <see cref="HatchBrush"/>.</value>
        public BrushHatchStyle HatchStyle
        {
            get;
        }

        /// <inheritdoc/>
        public override BrushType BrushType => BrushType.Hatch;

        /// <inheritdoc/>
        public override Color AsColor => this.Color;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"HatchBrush ({HatchStyle}, {Color})";
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => (HatchStyle, Color).GetHashCode();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public override bool Equals(object? other)
        {
            var o = other as HatchBrush;
            if (o == null)
                return false;

            CheckDisposed();
            return Color == o.Color && HatchStyle == o.HatchStyle;
        }

        /// <inheritdoc/>
        protected override IBrushHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateHatchBrushHandler(this);
        }

        /// <inheritdoc/>
        protected override void UpdateHandler()
        {
            ((IHatchBrushHandler)Handler).Update(this);
        }

        /// <inheritdoc/>
        protected override SKPaint CreateSkiaPaint()
        {
            return base.CreateSkiaPaint();
        }
    }
}