using Alternet.UI;
using System;
using System.CodeDom;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Margin is a value type used to describe the thickness of frame around a rectangle.
    /// It contains four doubles each corresponding to a side: Left, Top, Right, Bottom.
    /// </summary>
    public struct Margins : IEquatable<Margins>
    {
        private double left;

        private double top;

        private double right;

        private double bottom;

        /// <summary>
        /// This constructur builds a Margin with a specified value on every side.
        /// </summary>
        /// <param name="uniformLength">The specified uniform length.</param>
        public Margins(double uniformLength)
        {
            left = top = right = bottom = uniformLength;
        }

        /// <summary>
        /// This constructor builds a Margin with the specified number of pixels on each side.
        /// </summary>
        /// <param name="left">The thickness for the left side.</param>
        /// <param name="top">The thickness for the top side.</param>
        /// <param name="right">The thickness for the right side.</param>
        /// <param name="bottom">The thickness for the bottom side.</param>
        public Margins(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Margins"/> structure.
        /// </summary>
        /// <param name="horizontal">The thickness on the left and right.</param>
        /// <param name="vertical">The thickness on the top and bottom.</param>
        public Margins(double horizontal, double vertical)
        {
            left = right = horizontal;
            top = bottom = vertical;
        }

        internal Margins(Thickness t) : this(t.Left, t.Top, t.Right, t.Bottom)
        {
        }

        internal Thickness ToThickness() => new Thickness(Left, Top, Right, Bottom);

        /// <summary>
        /// Gets the combined padding information in the form of a <see cref="Drawing.Size"/>.
        /// </summary>
        /// <value>A <see cref="Drawing.Size"/> containing the padding information.</value>
        /// <remarks>
        /// The <see cref="Horizontal"/> property corresponds to the <see cref="Size.Width"/> property,
        /// and the <see cref="Vertical"/> property corresponds to the <see cref="Size.Height"/> property.
        /// </remarks>
        public Size Size => new Size(Horizontal, Vertical);

        /// <summary>
        /// Gets the combined padding for the right and left edges.
        /// </summary>
        /// <value>Gets the sum, of the <see cref="Left"/> and <see cref="Right"/> padding values.</value>
        public double Horizontal => left + right;

        /// <summary>
        /// Gets the combined padding for the top and bottom edges.
        /// </summary>
        /// <value>Gets the sum, of the <see cref="Top"/> and <see cref="Bottom"/> padding values.</value>
        public double Vertical => top + bottom;

        /// <summary>This property is the Length on the thickness' left side</summary>
        public double Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>This property is the Length on the thickness' top side</summary>
        public double Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>This property is the Length on the thickness' right side</summary>
        public double Right
        {
            get { return right; }
            set { right = value; }
        }

        /// <summary>This property is the Length on the thickness' bottom side</summary>
        public double Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for equality.
        /// </summary>
        /// <param name="t1">first Margin to compare</param>
        /// <param name="t2">second Margin to compare</param>
        /// <returns>True if all sides of the Margin are equal, false otherwise</returns>
        //  SEEALSO
        public static bool operator ==(Margins t1, Margins t2)
        {
            return ((t1.left == t2.left || (double.IsNaN(t1.left) && double.IsNaN(t2.left)))
                    && (t1.top == t2.top || (double.IsNaN(t1.top) && double.IsNaN(t2.top)))
                    && (t1.right == t2.right || (double.IsNaN(t1.right) && double.IsNaN(t2.right)))
                    && (t1.bottom == t2.bottom || (double.IsNaN(t1.bottom) && double.IsNaN(t2.bottom)))
                    );
        }

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for inequality.
        /// </summary>
        /// <param name="t1">first Margin to compare</param>
        /// <param name="t2">second Margin to compare</param>
        /// <returns>False if all sides of the Margin are equal, true otherwise</returns>
        //  SEEALSO
        public static bool operator !=(Margins t1, Margins t2)
        {
            return (!(t1 == t2));
        }

        /// <summary>
        /// This function compares to the provided object for type and value equality.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if object is a Margin and all sides of it are equal to this Margin'.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Margins)
            {
                Margins otherObj = (Margins)obj;
                return (this == otherObj);
            }
            return (false);
        }

        /// <summary>
        /// Compares this instance of Margin with another instance.
        /// </summary>
        /// <param name="thickness">Margin instance to compare.</param>
        /// <returns><c>true</c>if this Margin instance has the same value
        /// and unit type as thickness.</returns>
        public bool Equals(Margins thickness)
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
        /// Converts this Margin object to a string.
        /// </summary>
        /// <returns>String conversion.</returns>
        public override string ToString()
        {
            return $"({left}, {top}, {right}, {bottom})";
        }
    }
}