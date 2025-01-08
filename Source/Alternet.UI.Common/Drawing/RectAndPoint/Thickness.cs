using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /*
     Please do not remove StructLayout(LayoutKind.Sequential) atrtribute.
     Also do not change order of the fields.
    */

    /// <summary>
    /// Thickness is a value type used to describe the thickness of frame around
    /// a rectangle.
    /// It contains four values each corresponding to a side:
    /// Left, Top, Right, Bottom.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Thickness : IEquatable<Thickness>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Thickness'/> structure with
        /// <see cref="Coord.NaN"/> in all bounds.
        /// </summary>
        public static readonly Thickness NaN = new(Coord.NaN, Coord.NaN, Coord.NaN, Coord.NaN);

        /// <summary>
        /// Gets an empty <see cref="Thickness"/> object with
        /// Left, Top, Right, Bottom properties equal to zero.
        /// </summary>
        public static readonly Thickness Empty = new();

        /// <summary>
        /// Gets an empty <see cref="Thickness"/> object with
        /// Left, Top, Right, Bottom properties equal to 1.
        /// </summary>
        public static readonly Thickness One = new(CoordD.One);

        private Coord left;
        private Coord top;
        private Coord right;
        private Coord bottom;

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> struct
        /// with the same value on every side.
        /// </summary>
        /// <param name="uniform">The specified uniform length.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Thickness(Coord uniform)
        {
            left = top = right = bottom = uniform;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> struct
        /// with the specified values for the each side.
        /// </summary>
        /// <param name="left">The thickness for the left side.</param>
        /// <param name="top">The thickness for the top side.</param>
        /// <param name="right">The thickness for the right side.</param>
        /// <param name="bottom">The thickness for the bottom side.</param>
        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Thickness(Coord left, Coord top, Coord right, Coord bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> structure
        /// with the specified values for the horizontal and vertical sides.
        /// </summary>
        /// <param name="horizontal">The thickness on the left and right sides.</param>
        /// <param name="vertical">The thickness on the top and bottom sides.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Thickness(Coord horizontal, Coord vertical)
        {
            left = right = horizontal;
            top = bottom = vertical;
        }

        /// <summary>
        /// Returns whether all values on every side are equal.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsUniform =>
            (left == right) && (top == bottom) && (left == top);

        /// <summary>
        /// Returns whether all values on every side are positive
        /// (greater than 0).
        /// </summary>
        [Browsable(false)]
        public readonly bool IsPositive =>
            (left > CoordD.Empty) && (right > CoordD.Empty)
            && (top > CoordD.Empty) && (bottom > CoordD.Empty);

        /// <summary>
        /// Returns whether any value on the side is positive
        /// (greater than 0).
        /// </summary>
        [Browsable(false)]
        public readonly bool IsAnyPositive =>
            (left > CoordD.Empty) || (right > CoordD.Empty)
            || (top > CoordD.Empty) || (bottom > CoordD.Empty);

        /// <summary>
        /// Gets the combined padding information in the form of a
        /// <see cref="Alternet.Drawing.SizeD"/>.
        /// </summary>
        /// <value>A <see cref="Alternet.Drawing.SizeD"/> containing the padding
        /// information.</value>
        /// <remarks>
        /// The <see cref="Horizontal"/> property corresponds to the
        /// <see cref="SizeD.Width"/> property,
        /// and the <see cref="Vertical"/> property corresponds to the
        /// <see cref="SizeD.Height"/> property.
        /// </remarks>
        [Browsable(false)]
        public readonly SizeD Size => new(Horizontal, Vertical);

        /// <summary>
        /// Gets <see cref="Left"/> and <see cref="Top"/> as <see cref="SizeD"/>.
        /// </summary>
        [Browsable(false)]
        public readonly SizeD LeftTop => new(Left, Top);

        /// <summary>
        /// Gets <see cref="Right"/> and <see cref="Bottom"/> as <see cref="SizeD"/>.
        /// </summary>
        [Browsable(false)]
        public readonly SizeD RightBottom => new(Right, Bottom);

        /// <summary>
        /// Gets the combined padding for the right and left edges.
        /// </summary>
        /// <value>Gets the sum, of the <see cref="Left"/> and
        /// <see cref="Right"/> padding values.</value>
        [Browsable(false)]
        public readonly Coord Horizontal => left + right;

        /// <summary>
        /// Gets the combined padding for the top and bottom edges.
        /// </summary>
        /// <value>Gets the sum, of the <see cref="Top"/> and
        /// <see cref="Bottom"/> padding values.</value>
        [Browsable(false)]
        public readonly Coord Vertical => top + bottom;

        /// <summary>
        /// This property is the length on the thickness' left side.
        /// </summary>
        public Coord Left
        {
            readonly get
            {
                return left;
            }

            set
            {
                left = value;
            }
        }

        /// <summary>This property is the length on the thickness' top side.</summary>
        public Coord Top
        {
            readonly get
            {
                return top;
            }

            set
            {
                top = value;
            }
        }

        /// <summary>
        /// This property is the Length on the thickness' right side.
        /// </summary>
        public Coord Right
        {
            readonly get
            {
                return right;
            }

            set
            {
                right = value;
            }
        }

        /// <summary>This property is the length on the thickness' bottom side.</summary>
        public Coord Bottom
        {
            readonly get
            {
                return bottom;
            }

            set
            {
                bottom = value;
            }
        }

        /// <summary>
        /// Implicit operator convertion from a single value to
        /// <see cref="Thickness"/>. All fields of thickness instance are assigned
        /// with the same value.
        /// </summary>
        /// <param name="d">New thickness value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Thickness(Coord d) => new(d);

        /// <summary>
        /// Implicit operator convertion from <see cref="int"/> to
        /// <see cref="Thickness"/>. All fields of thickness instance are assigned
        /// with the same int value.
        /// </summary>
        /// <param name="d">New thickness value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Thickness(int d) => new(d);

        /// <summary>
        /// Implicit operator convertion from tuple with four values
        /// to <see cref="Thickness"/>.
        /// </summary>
        /// <param name="d">New thickness value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Thickness((Coord, Coord, Coord, Coord) d) =>
            new(d.Item1, d.Item2, d.Item3, d.Item4);

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for equality.
        /// </summary>
        /// <param name="t1">first Thickness to compare</param>
        /// <param name="t2">second Thickness to compare</param>
        /// <returns>True if all sides of the Thickness are equal, false
        /// otherwise</returns>
        public static bool operator ==(Thickness t1, Thickness t2)
        {
            static bool EqualOrNaN(Coord a1, Coord a2)
            {
                var result = a1 == a2 || (Coord.IsNaN(a1) && Coord.IsNaN(a2));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Thickness t1, Thickness t2)
        {
            return !(t1 == t2);
        }

        /// <summary>
        /// Creates <see cref="Thickness"/> from array of 1, 2 or 4 values.
        /// </summary>
        /// <param name="coord">Array with values.</param>
        /// <returns></returns>
        public static Thickness? FromArray(float[]? coord)
        {
            switch (coord?.Length)
            {
                case 1:
                    return new(coord[0]);
                case 2:
                    return new(coord[0], coord[1]);
                case 4:
                    return new(coord[0], coord[1], coord[2], coord[3]);
            }

            return null;
        }

        /// <summary>
        /// Parses a <see cref="Thickness"/> string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The <see cref="Thickness"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Thickness Parse(string s)
        {
            const string exceptionMessage
                = "Invalid Thickness. Specify 1, 2 or 4 float numbers separated by comma.";

            if (!TryParse(s, out var result))
                throw new FormatException(exceptionMessage);

            return result;
        }

        /// <summary>
        /// Parses a <see cref="Thickness"/> string.
        /// </summary>
        public static bool TryParse(string s, out Thickness value)
        {
            var array = ConversionUtils.ParseOneOrTwoOrFourFloats(s);

            var result = FromArray(array);

            if(result is null)
            {
                value = Thickness.Empty;
                return false;
            }

            value = result.Value;
            return true;
        }

        /// <summary>
        /// Inflate all fields of the <see cref="Thickness"/> instance on
        /// the same value.
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Inflate(Coord value = 1)
        {
            left += value;
            top += value;
            right += value;
            bottom += value;
        }

        /// <summary>
        /// This function compares to the provided object for type and
        /// value equality.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if object is a Thickness and all sides of it are equal
        /// to this Thickness.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Thickness thickness)
        {
            return this == thickness;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override readonly int GetHashCode()
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
            string[] names =
            {
                PropNameStrings.Default.Left,
                PropNameStrings.Default.Top,
                PropNameStrings.Default.Right,
                PropNameStrings.Default.Bottom,
            };

            Coord[] values = { left, top, right, bottom };

            return StringUtils.ToStringWithOrWithoutNames<Coord>(names, values);
        }

        /// <summary>
        /// Gets a copy of this object with changed <see cref="Top"/> property.
        /// </summary>
        /// <param name="value">Property value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Thickness WithTop(Coord value)
        {
            return new(left, value, right, bottom);
        }

        /// <summary>
        /// Gets a copy of this object with changed <see cref="Bottom"/> property.
        /// </summary>
        /// <param name="value">Property value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Thickness WithBottom(Coord value)
        {
            return new(left, top, right, value);
        }

        /// <summary>
        /// Gets a copy of this object with changed <see cref="Right"/> property.
        /// </summary>
        /// <param name="value">Property value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Thickness WithRight(Coord value)
        {
            return new(left, top, value, bottom);
        }

        /// <summary>
        /// Gets a copy of this object with changed <see cref="Left"/> property.
        /// </summary>
        /// <param name="value">Property value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Thickness WithLeft(Coord value)
        {
            return new(value, top, right, bottom);
        }

        /// <summary>
        /// Gets <see cref="Vertical"/> or <see cref="Horizontal"/> depending on the
        /// <paramref name="isVert"/> parameter value.
        /// </summary>
        /// <param name="isVert">The flag which detremines whether to return
        /// <see cref="Vertical"/> or <see cref="Horizontal"/> property value.
        /// </param>
        /// <returns></returns>
        public readonly Coord GetSize(bool isVert)
        {
            if (isVert)
                return Vertical;
            else
                return Horizontal;
        }

        /// <summary>
        /// Sets all fields to 0 if <paramref name="reset"/> is True.
        /// </summary>
        /// <param name="reset"></param>
        public void Reset(bool reset = true)
        {
            if (!reset)
                return;
            left = CoordD.Empty;
            top = CoordD.Empty;
            right = CoordD.Empty;
            bottom = CoordD.Empty;
        }

        /// <summary>
        /// Apply minimal limits to all the fields of the
        /// <see cref="Thickness"/> instance.
        /// </summary>
        /// <param name="min">Minimal possible thickness.</param>
        public void ApplyMin(Thickness min)
        {
            if (left < min.left)
                left = min.left;
            if (top < min.top)
                top = min.top;
            if (right < min.right)
                right = min.right;
            if (bottom < min.bottom)
                bottom = min.bottom;
        }

        /// <summary>
        /// Apply minimal and maximal limits to all the fields of the
        /// <see cref="Thickness"/> instance.
        /// </summary>
        /// <param name="min">Minimal possible thickness.</param>
        /// <param name="max">Maximal possible thikness.</param>
        public void ApplyMinMax(Coord min, Coord max)
        {
            Coord SetMinMaxValue(Coord value)
            {
                if (value > max)
                    return max;
                if (value < min)
                    return min;
                return value;
            }

            left = SetMinMaxValue(left);
            top = SetMinMaxValue(top);
            right = SetMinMaxValue(right);
            bottom = SetMinMaxValue(bottom);
        }
    }
}