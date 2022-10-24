using System;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes the location and color of a transition point in a gradient.
    /// </summary>
    public class GradientStop : IEquatable<GradientStop>
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

        /// <summary>
        /// Returns a value that indicates whether the two objects are not equal.
        /// </summary>
        public static bool operator !=(GradientStop? a, GradientStop? b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a value that indicates whether the two objects are equal.
        /// </summary>
        public static bool operator ==(GradientStop? a, GradientStop? b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Color, Offset);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(GradientStop? other)
        {
            if (other == null)
                return false;

            return Color == other.Color && Offset == other.Offset;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"({Color}, {Offset})";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            var o = obj as GradientStop;

            if (ReferenceEquals(o, null))
                return false;

            if (GetType() != obj?.GetType())
                return false;

            return Equals(o);
        }
    }
}