using System;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes the location and color of a transition point in a gradient.
    /// </summary>
    public class GradientStop
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GradientStop"/> class.
        /// </summary>
        public GradientStop()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientStop"/> class with the specified color and offset.
        /// </summary>
        /// <param name="color">The color value of the gradient stop.</param>
        /// <param name="offset">The location in the gradient where the gradient stop is placed.</param>
        public GradientStop(Color color, double offset)
        {
            Color = color;
            Offset = offset;
        }

        /// <summary>
        /// Gets or sets the color of the gradient stop.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the location of the gradient stop within the gradient vector.
        /// </summary>
        public double Offset { get; set; }
    }
}