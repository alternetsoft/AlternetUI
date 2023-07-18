using System;
using System.Globalization;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Thickness is a value type used to describe the thickness of frame around
    /// a rectangle.
    /// It contains four doubles each corresponding to a side:
    /// Left, Top, Right, Bottom.
    /// </summary>
    public struct Thickness : IEquatable<Thickness>
    {
        private double left;

        private double top;

        private double right;

        private double bottom;

        /// <summary>
        /// This constructur builds a Thickness with a specified value on every side.
        /// </summary>
        /// <param name="uniformLength">The specified uniform length.</param>
        public Thickness(double uniformLength)
        {
            left = top = right = bottom = uniformLength;
        }

        /// <summary>
        /// This constructor builds a Thickness with the specified number of
        /// pixels on each side.
        /// </summary>
        /// <param name="left">The thickness for the left side.</param>
        /// <param name="top">The thickness for the top side.</param>
        /// <param name="right">The thickness for the right side.</param>
        /// <param name="bottom">The thickness for the bottom side.</param>
        public Thickness(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure.
        /// </summary>
        /// <param name="horizontal">The thickness on the left and right.</param>
        /// <param name="vertical">The thickness on the top and bottom.</param>
        public Thickness(double horizontal, double vertical)
        {
            left = right = horizontal;
            top = bottom = vertical;
        }

        /// <summary>
        ///     Returns whether all values on every side are equal.
        /// </summary>
        public readonly bool IsUniform =>
            (left == right) && (top == bottom) && (left == top);

        /// <summary>
        ///     Returns whether all values on every side are positive
        ///     (greater than 0).
        /// </summary>
        public readonly bool IsPositive =>
            (left > 0) && (right > 0) && (top > 0) && (bottom > 0);

        /// <summary>
        /// Gets the combined padding information in the form of a
        /// <see cref="Drawing.Size"/>.
        /// </summary>
        /// <value>A <see cref="Drawing.Size"/> containing the padding
        /// information.</value>
        /// <remarks>
        /// The <see cref="Horizontal"/> property corresponds to the
        /// <see cref="Size.Width"/> property,
        /// and the <see cref="Vertical"/> property corresponds to the
        /// <see cref="Size.Height"/> property.
        /// </remarks>
        public readonly Size Size => new(Horizontal, Vertical);

        /// <summary>
        /// Gets the combined padding for the right and left edges.
        /// </summary>
        /// <value>Gets the sum, of the <see cref="Left"/> and
        /// <see cref="Right"/> padding values.</value>
        public readonly double Horizontal => left + right;

        /// <summary>
        /// Gets the combined padding for the top and bottom edges.
        /// </summary>
        /// <value>Gets the sum, of the <see cref="Top"/> and
        /// <see cref="Bottom"/> padding values.</value>
        public readonly double Vertical => top + bottom;

        /// <summary>
        /// This property is the Length on the thickness' left side
        /// </summary>
        public double Left
        {
            readonly get { return left; }
            set { left = value; }
        }

        /// <summary>This property is the Length on the thickness' top side</summary>
        public double Top
        {
            readonly get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// This property is the Length on the thickness' right side
        /// </summary>
        public double Right
        {
            readonly get { return right; }
            set { right = value; }
        }

        /// <summary>This property is the Length on the thickness' bottom
        /// side</summary>
        public double Bottom
        {
            readonly get { return bottom; }
            set { bottom = value; }
        }

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for equality.
        /// </summary>
        /// <param name="t1">first Thickness to compare</param>
        /// <param name="t2">second Thickness to compare</param>
        /// <returns>True if all sides of the Thickness are equal, false
        /// otherwise</returns>
        public static bool operator ==(Thickness t1, Thickness t2)
        {
            static bool EqualOrNaN(double a1, double a2)
            {
                var result = a1 == a2 || (double.IsNaN(a1) && double.IsNaN(a2));
                return result;
            }

            if (!EqualOrNaN(t1.left, t2.left))
                return false;
            if (!EqualOrNaN(t1.right, t2.right))
                return false;
            if (!EqualOrNaN(t1.top, t2.top))
                return false;
            return EqualOrNaN(t1.bottom, t2.bottom);
        }

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for inequality.
        /// </summary>
        /// <param name="t1">first Thickness to compare</param>
        /// <param name="t2">second Thickness to compare</param>
        /// <returns>False if all sides of the Thickness are equal, true
        /// otherwise</returns>
        public static bool operator !=(Thickness t1, Thickness t2)
        {
            return !(t1 == t2);
        }

        public void Inflate(double value)
        {
            left += value;
            top += value;
            right += value;
            bottom += value;
        }

        /// <summary>
        /// Parses a <see cref="Thickness"/> string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The <see cref="Thickness"/>.</returns>
        public static Thickness Parse(string s)
        {
            const string exceptionMessage = "Invalid Thickness.";

            if (!TryParse(s, out var result))
                throw new FormatException(exceptionMessage);

            return result;
        }

        /// <summary>
        /// Parses a <see cref="Thickness"/> string.
        /// </summary>
        public static bool TryParse(string s, out Thickness value)
        {
            value = new Thickness();

            using var tokenizer =
                new StringTokenizer(s, CultureInfo.InvariantCulture);
            if (tokenizer.TryReadSingle(out var a))
            {
                if (tokenizer.TryReadSingle(out var b))
                {
                    if (tokenizer.TryReadSingle(out var c))
                    {
                        if (tokenizer.TryReadSingle(out var d))
                        {
                            value = new Thickness(a, b, c, d);
                            return true;
                        }

                        return false;
                    }

                    value = new Thickness(a, b);
                    return true;
                }

                value = new Thickness(a);
                return true;
            }

            return false;
        }

        /// <summary>
        /// This function compares to the provided object for type and
        /// value equality.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if object is a Thickness and all sides of it are equal
        /// to this Thickness.</returns>
        public override readonly bool Equals(object? obj)
        {
            if (obj is Thickness otherObj)
                return this == otherObj;
            return false;
        }

        /// <summary>
        /// Compares this instance of Thickness with another instance.
        /// </summary>
        /// <param name="thickness">Thickness instance to compare.</param>
        /// <returns><c>true</c>if this Thickness instance has the same value
        /// and unit type as thickness.</returns>
        public readonly bool Equals(Thickness thickness)
        {
            return this == thickness;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return left.GetHashCode() ^ top.GetHashCode() ^
                right.GetHashCode() ^ bottom.GetHashCode();
        }

        /// <summary>
        /// Converts this Thickness object to a string.
        /// </summary>
        /// <returns>String conversion.</returns>
        public override readonly string ToString()
        {
            return $"({left}, {top}, {right}, {bottom})";
        }
    }
}