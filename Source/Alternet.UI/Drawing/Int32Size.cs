// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents the size of a rectangular region with an ordered pair of width and height.
    /// </summary>
    [Serializable]
//    [TypeConverter("System.Drawing.SizeConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public struct Int32Size : IEquatable<Int32Size>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Int32Size'/> class.
        /// </summary>
        public static readonly Int32Size Empty;

        private int width; // Do not rename (binary serialization)
        private int height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Int32Size'/> class from the specified
        /// <see cref='Drawing.Int32Point'/>.
        /// </summary>
        public Int32Size(Int32Point pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Int32Size'/> class from the specified dimensions.
        /// </summary>
        public Int32Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Converts the specified <see cref='Drawing.Int32Size'/> to a <see cref='Drawing.Size'/>.
        /// </summary>
        public static implicit operator Size(Int32Size p) => new Size(p.Width, p.Height);

        /// <summary>
        /// Performs vector addition of two <see cref='Drawing.Int32Size'/> objects.
        /// </summary>
        public static Int32Size operator +(Int32Size sz1, Int32Size sz2) => Add(sz1, sz2);

        /// <summary>
        /// Contracts a <see cref='Drawing.Int32Size'/> by another <see cref='Drawing.Int32Size'/>
        /// </summary>
        public static Int32Size operator -(Int32Size sz1, Int32Size sz2) => Subtract(sz1, sz2);

        /// <summary>
        /// Multiplies a <see cref="Int32Size"/> by an <see cref="int"/> producing <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="int"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <returns>Product of type <see cref="Int32Size"/>.</returns>
        public static Int32Size operator *(int left, Int32Size right) => Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by an <see cref="int"/> producing <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <param name="right">Multiplier of type <see cref="int"/>.</param>
        /// <returns>Product of type <see cref="Int32Size"/>.</returns>
        public static Int32Size operator *(Int32Size left, int right) => Multiply(left, right);

        /// <summary>
        /// Divides <see cref="Int32Size"/> by an <see cref="int"/> producing <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="Int32Size"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="Int32Size"/>.</returns>
        public static Int32Size operator /(Int32Size left, int right) => new Int32Size(unchecked(left.width / right), unchecked(left.height / right));

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by a <see cref="double"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="double"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        public static Size operator *(double left, Int32Size right) => Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by a <see cref="double"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <param name="right">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        public static Size operator *(Int32Size left, double right) => Multiply(left, right);

        /// <summary>
        /// Divides <see cref="Int32Size"/> by a <see cref="double"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="Int32Size"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="Size"/>.</returns>
        public static Size operator /(Int32Size left, double right)
            => new Size(left.width / right, left.height / right);

        /// <summary>
        /// Tests whether two <see cref='Drawing.Int32Size'/> objects are identical.
        /// </summary>
        public static bool operator ==(Int32Size sz1, Int32Size sz2) => sz1.Width == sz2.Width && sz1.Height == sz2.Height;

        /// <summary>
        /// Tests whether two <see cref='Drawing.Int32Size'/> objects are different.
        /// </summary>
        public static bool operator !=(Int32Size sz1, Int32Size sz2) => !(sz1 == sz2);

        /// <summary>
        /// Converts the specified <see cref='Drawing.Int32Size'/> to a <see cref='Drawing.Int32Point'/>.
        /// </summary>
        public static explicit operator Int32Point(Int32Size size) => new Int32Point(size.Width, size.Height);

        /// <summary>
        /// Tests whether this <see cref='Drawing.Int32Size'/> has zero width and height.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => width == 0 && height == 0;

        /// <summary>
        /// Represents the horizontal component of this <see cref='Drawing.Int32Size'/>.
        /// </summary>
        public int Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Represents the vertical component of this <see cref='Drawing.Int32Size'/>.
        /// </summary>
        public int Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Performs vector addition of two <see cref='Drawing.Int32Size'/> objects.
        /// </summary>
        public static Int32Size Add(Int32Size sz1, Int32Size sz2) =>
            new Int32Size(unchecked(sz1.Width + sz2.Width), unchecked(sz1.Height + sz2.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a ceiling operation on all the coordinates.
        /// </summary>
        public static Int32Size Ceiling(Size value) =>
            new Int32Size(unchecked((int)Math.Ceiling(value.Width)), unchecked((int)Math.Ceiling(value.Height)));

        /// <summary>
        /// Contracts a <see cref='Drawing.Int32Size'/> by another <see cref='Drawing.Int32Size'/> .
        /// </summary>
        public static Int32Size Subtract(Int32Size sz1, Int32Size sz2) =>
            new Int32Size(unchecked(sz1.Width - sz2.Width), unchecked(sz1.Height - sz2.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a truncate operation on all the coordinates.
        /// </summary>
        public static Int32Size Truncate(Size value) => new Int32Size(unchecked((int)value.Width), unchecked((int)value.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a round operation on all the coordinates.
        /// </summary>
        public static Int32Size Round(Size value) =>
            new Int32Size(unchecked((int)Math.Round(value.Width)), unchecked((int)Math.Round(value.Height)));

        /// <summary>
        /// Tests to see whether the specified object is a <see cref='Drawing.Int32Size'/>  with the same dimensions
        /// as this <see cref='Drawing.Int32Size'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Int32Size && Equals((Int32Size)obj);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(Int32Size other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(Width, Height);

        /// <summary>
        /// Creates a human-readable string that represents this <see cref='Drawing.Int32Size'/>.
        /// </summary>
        public override readonly string ToString() => $"{{Width={width}, Height={height}}}";

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by an <see cref="int"/> producing <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref='int'/>.</param>
        /// <returns>Product of type <see cref="Int32Size"/>.</returns>
        private static Int32Size Multiply(Int32Size size, int multiplier) =>
            new Int32Size(unchecked(size.width * multiplier), unchecked(size.height * multiplier));

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by a <see cref="double"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type SizeF.</returns>
        private static Size Multiply(Int32Size size, double multiplier) =>
            new Size(size.width * multiplier, size.height * multiplier);
    }
}
