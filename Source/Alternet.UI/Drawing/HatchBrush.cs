using System;
using System.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a rectangular brush with a hatch style, a foreground color, and a background color.
    /// </summary>
    /// <remarks>
    /// A hatch pattern is made from two colors: one defined by the <see cref="BackgroundColor"/>, which fills the background and
    /// one for the lines that form the pattern over the background defined by the <see cref="ForegroundColor"/> property.
    /// The <see cref="HatchStyle"/> property defines what type of pattern the brush has and can be any value from the <see cref="BrushHatchStyle"/> enumeration.
    /// </remarks>
    public sealed class HatchBrush : Brush
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HatchBrush"/> class with the specified
        /// <see cref="BrushHatchStyle"/> enumeration, foreground color, and background color.
        /// </summary>
        /// <param name="hatchStyle">One of the <see cref="BrushHatchStyle"/> values that represents the pattern drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="foregroundColor">The <see cref="Color"/> structure that represents the color of lines drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="backgroundColor">The <see cref="Color"/> structure that represents the color of spaces between the lines drawn by this <see cref="HatchBrush"/>.</param>
        public HatchBrush(BrushHatchStyle hatchStyle, Color foregroundColor, Color backgroundColor) : base(new Native.HatchBrush())
        {
            HatchStyle = hatchStyle;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;

            ((Native.HatchBrush)NativeBrush).Initialize((Native.BrushHatchStyle)hatchStyle, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HatchBrush"/> class with the specified
        /// <see cref="BrushHatchStyle"/> enumeration and foreground color.
        /// </summary>
        /// <param name="hatchStyle">One of the <see cref="BrushHatchStyle"/> values that represents the pattern drawn by this <see cref="HatchBrush"/>.</param>
        /// <param name="foregroundColor">The <see cref="Color"/> structure that represents the color of lines drawn by this <see cref="HatchBrush"/>.</param>
        /// <remarks>The background color is initialized to transparent.</remarks>
        public HatchBrush(BrushHatchStyle hatchStyle, Color foregroundColor) : this(hatchStyle, foregroundColor, Color.Transparent)
        {
        }

        /// <summary>
        /// Gets the color of hatch lines drawn by this <see cref="HatchBrush"/> object.
        /// </summary>
        /// <value>A <see cref="Color"/> structure that represents the foreground color for this <see cref="HatchBrush"/>.</value>
        public Color ForegroundColor { get; }

        /// <summary>
        /// Gets the color of spaces between the hatch lines drawn by this <see cref="HatchBrush"/> object.
        /// </summary>
        /// <value>A <see cref="Color"/> structure that represents the background color for this <see cref="HatchBrush"/>.</value>
        public Color BackgroundColor { get; }

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
            
            return ForegroundColor == o.ForegroundColor && BackgroundColor == o.BackgroundColor && HatchStyle == o.HatchStyle;
        }

        private protected override int GetHashCodeCore() => HashCode.Combine(HatchStyle, ForegroundColor, BackgroundColor);

        private protected override string ToStringCore() => $"HatchBrush ({HatchStyle}, {ForegroundColor}, {BackgroundColor})";
    }
}