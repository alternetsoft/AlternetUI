using System;
using Alternet.Drawing;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Thickness is a value type used to describe the thickness of frame around a rectangle.
    /// It contains four doubles each corresponding to a side: Left, Top, Right, Bottom.
    /// </summary>
    public struct Thickness : IEquatable<Thickness>
    {
        private float left;

        private float top;

        private float right;

        private float bottom;

        /// <summary>
        /// This constructur builds a Thickness with a specified value on every side.
        /// </summary>
        /// <param name="uniformLength">The specified uniform length.</param>
        public Thickness(float uniformLength)
        {
            left = top = right = bottom = uniformLength;
        }

        /// <summary>
        /// This constructor builds a Thickness with the specified number of pixels on each side.
        /// </summary>
        /// <param name="left">The thickness for the left side.</param>
        /// <param name="top">The thickness for the top side.</param>
        /// <param name="right">The thickness for the right side.</param>
        /// <param name="bottom">The thickness for the bottom side.</param>
        public Thickness(float left, float top, float right, float bottom)
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
        public Thickness(float horizontal, float vertical)
        {
            left = right = horizontal;
            top = bottom = vertical;
        }

        /// <summary>
        /// Gets the combined padding information in the form of a <see cref="SizeF"/>.
        /// </summary>
        /// <value>A <see cref="SizeF"/> containing the padding information.</value>
        /// <remarks>
        /// The <see cref="Horizontal"/> property corresponds to the <see cref="SizeF.Width"/> property,
        /// and the <see cref="Vertical"/> property corresponds to the <see cref="SizeF.Height"/> property.
        /// </remarks>
        public SizeF Size => new SizeF(Horizontal, Vertical);

        /// <summary>
        /// Gets the combined padding for the right and left edges.
        /// </summary>
        /// <value>Gets the sum, of the <see cref="Left"/> and <see cref="Right"/> padding values.</value>
        public float Horizontal => left + right;

        /// <summary>
        /// Gets the combined padding for the top and bottom edges.
        /// </summary>
        /// <value>Gets the sum, of the <see cref="Top"/> and <see cref="Bottom"/> padding values.</value>
        public float Vertical => top + bottom;

        /// <summary>This property is the Length on the thickness' left side</summary>
        public float Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>This property is the Length on the thickness' top side</summary>
        public float Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>This property is the Length on the thickness' right side</summary>
        public float Right
        {
            get { return right; }
            set { right = value; }
        }

        /// <summary>This property is the Length on the thickness' bottom side</summary>
        public float Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for equality.
        /// </summary>
        /// <param name="t1">first Thickness to compare</param>
        /// <param name="t2">second Thickness to compare</param>
        /// <returns>True if all sides of the Thickness are equal, false otherwise</returns>
        //  SEEALSO
        public static bool operator ==(Thickness t1, Thickness t2)
        {
            return ((t1.left == t2.left || (float.IsNaN(t1.left) && float.IsNaN(t2.left)))
                    && (t1.top == t2.top || (float.IsNaN(t1.top) && float.IsNaN(t2.top)))
                    && (t1.right == t2.right || (float.IsNaN(t1.right) && float.IsNaN(t2.right)))
                    && (t1.bottom == t2.bottom || (float.IsNaN(t1.bottom) && float.IsNaN(t2.bottom)))
                    );
        }

        /// <summary>
        /// Parses a <see cref="Thickness"/> string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The <see cref="Thickness"/>.</returns>
        public static Thickness Parse(string s)
        {
            const string exceptionMessage = "Invalid Thickness.";

            using (var tokenizer = new StringTokenizer(s, CultureInfo.InvariantCulture, exceptionMessage))
            {
                if (tokenizer.TryReadSingle(out var a))
                {
                    if (tokenizer.TryReadSingle(out var b))
                    {
                        if (tokenizer.TryReadSingle(out var c))
                        {
                            return new Thickness(a, b, c, tokenizer.ReadSingle());
                        }

                        return new Thickness(a, b);
                    }

                    return new Thickness(a);
                }

                throw new FormatException(exceptionMessage);
            }
        }

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for inequality.
        /// </summary>
        /// <param name="t1">first Thickness to compare</param>
        /// <param name="t2">second Thickness to compare</param>
        /// <returns>False if all sides of the Thickness are equal, true otherwise</returns>
        //  SEEALSO
        public static bool operator !=(Thickness t1, Thickness t2)
        {
            return (!(t1 == t2));
        }

        /// <summary>
        /// This function compares to the provided object for type and value equality.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if object is a Thickness and all sides of it are equal to this Thickness'.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Thickness)
            {
                Thickness otherObj = (Thickness)obj;
                return (this == otherObj);
            }
            return (false);
        }

        /// <summary>
        /// Compares this instance of Thickness with another instance.
        /// </summary>
        /// <param name="thickness">Thickness instance to compare.</param>
        /// <returns><c>true</c>if this Thickness instance has the same value
        /// and unit type as thickness.</returns>
        public bool Equals(Thickness thickness)
        {
            return (this == thickness);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return left.GetHashCode() ^ top.GetHashCode() ^ right.GetHashCode() ^ bottom.GetHashCode();
        }

        /// <summary>
        /// Converts this Thickness object to a string.
        /// </summary>
        /// <returns>String conversion.</returns>
        public override string ToString()
        {
            return $"({left}, {top}, {right}, {bottom})";
        }
    }
}