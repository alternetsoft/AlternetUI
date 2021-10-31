using System;
using Alternet.Drawing;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a brush with a hatch style and a color.
    /// </summary>
    /// <remarks>
    /// A hatch pattern is made from the lines of a color defined by the <see cref="Color"/> property.
    /// The <see cref="HatchStyle"/> property defines what type of pattern the brush has and can be any value from the <see cref="BrushHatchStyle"/> enumeration.
    /// </remarks>
    public sealed class HatchBrush : Brush
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HatchBrush"/> class with the specified
        /// <see cref="BrushHatchStyle"/> enumeration, and the color.
        /// </summary>
        /// <param name="hatchStyle">One of the <see cref="BrushHatchStyle"/> values that represents the pattern drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="color">The <see cref="Drawing.Color"/> structure that represents the color of lines drawn by this <see cref="HatchBrush"/>.</param>
        public HatchBrush(BrushHatchStyle hatchStyle, Color color) : base(new UI.Native.HatchBrush(), immutable: false)
        {
            HatchStyle = hatchStyle;
            Color = color;

            ((UI.Native.HatchBrush)NativeBrush).Initialize((UI.Native.BrushHatchStyle)hatchStyle, color);
        }

        /// <summary>
        /// Gets the color of hatch lines drawn by this <see cref="HatchBrush"/> object.
        /// </summary>
        /// <value>A <see cref="Drawing.Color"/> structure that represents the color for this <see cref="HatchBrush"/>.</value>
        public Color Color { get; }

        /// <summary>
        /// Gets the hatch style of this <see cref="HatchBrush"/> object.
        /// </summary>
        /// <value>One of the <see cref="BrushHatchStyle"/> values that represents the pattern of this <see cref="HatchBrush"/>.</value>
        public BrushHatchStyle HatchStyle { get; }

        private protected override bool EqualsCore(Brush other)
        {
            var o = other as HatchBrush;
            if (o == null)
                return false;
            
            return Color == o.Color && HatchStyle == o.HatchStyle;
        }

        private protected override int GetHashCodeCore() => HashCode.Combine(HatchStyle, Color);

        private protected override string ToStringCore() => $"HatchBrush ({HatchStyle}, {Color})";
    }
}