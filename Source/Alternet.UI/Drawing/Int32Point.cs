// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point in a two-dimensional plane.
    /// </summary>
    [Serializable]
    //[TypeConverter("System.Drawing.PointConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public struct Int32Point : IEquatable<Int32Point>
    {
        /// <summary>
        /// Creates a new instance of the <see cref='Drawing.Int32Point'/> class with member data left uninitialized.
        /// </summary>
        public static readonly Int32Point Empty;

        private int x; // Do not rename (binary serialization)
        private int y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Int32Point'/> class with the specified coordinates.
        /// </summary>
        public Int32Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Int32Point'/> class from a <see cref='Drawing.Size'/>.
        /// </summary>
        public Int32Point(Int32Size sz)
        {
            x = sz.Width;
            y = sz.Height;
        }

        /// <summary>
        /// Initializes a new instance of the Point class using coordinates specified by an integer value.
        /// </summary>
        public Int32Point(int dw)
        {
            x = LowInt16(dw);
            y = HighInt16(dw);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref='Drawing.Int32Point'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == 0 && y == 0;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='Drawing.Int32Point'/>.
        /// </summary>
        public int X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='Drawing.Int32Point'/>.
        /// </summary>
        public int Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Creates a <see cref='Drawing.Point'/> with the coordinates of the specified <see cref='Drawing.Int32Point'/>
        /// </summary>
        public static implicit operator Point(Int32Point p) => new Point(p.X, p.Y);

        /// <summary>
        /// Creates a <see cref='Drawing.Int32Size'/> with the coordinates of the specified <see cref='Drawing.Int32Point'/> .
        /// </summary>
        public static explicit operator Int32Size(Int32Point p) => new Int32Size(p.X, p.Y);

        /// <summary>
        /// Translates a <see cref='Drawing.Int32Point'/> by a given <see cref='Drawing.Int32Size'/> .
        /// </summary>
        public static Int32Point operator +(Int32Point pt, Int32Size sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='Drawing.Int32Point'/> by the negative of a given <see cref='Drawing.Int32Size'/> .
        /// </summary>
        public static Int32Point operator -(Int32Point pt, Int32Size sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='Drawing.Int32Point'/> objects. The result specifies whether the values of the
        /// <see cref='Drawing.Int32Point.X'/> and <see cref='Drawing.Int32Point.Y'/> properties of the two
        /// <see cref='Drawing.Int32Point'/> objects are equal.
        /// </summary>
        public static bool operator ==(Int32Point left, Int32Point right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='Drawing.Int32Point'/> objects. The result specifies whether the values of the
        /// <see cref='Drawing.Int32Point.X'/> or <see cref='Drawing.Int32Point.Y'/> properties of the two
        /// <see cref='Drawing.Int32Point'/>  objects are unequal.
        /// </summary>
        public static bool operator !=(Int32Point left, Int32Point right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='Drawing.Int32Point'/> by a given <see cref='Drawing.Int32Size'/> .
        /// </summary>
        public static Int32Point Add(Int32Point pt, Int32Size sz) => new Int32Point(unchecked(pt.X + sz.Width), unchecked(pt.Y + sz.Height));

        /// <summary>
        /// Translates a <see cref='Drawing.Int32Point'/> by the negative of a given <see cref='Drawing.Int32Size'/> .
        /// </summary>
        public static Int32Point Subtract(Int32Point pt, Int32Size sz) => new Int32Point(unchecked(pt.X - sz.Width), unchecked(pt.Y - sz.Height));

        /// <summary>
        /// Converts a PointF to a Point by performing a ceiling operation on all the coordinates.
        /// </summary>
        public static Int32Point Ceiling(Point value) => new Int32Point(unchecked((int)Math.Ceiling(value.X)), unchecked((int)Math.Ceiling(value.Y)));

        /// <summary>
        /// Converts a PointF to a Point by performing a truncate operation on all the coordinates.
        /// </summary>
        public static Int32Point Truncate(Point value) => new Int32Point(unchecked((int)value.X), unchecked((int)value.Y));

        /// <summary>
        /// Converts a PointF to a Point by performing a round operation on all the coordinates.
        /// </summary>
        public static Int32Point Round(Point value) => new Int32Point(unchecked((int)Math.Round(value.X)), unchecked((int)Math.Round(value.Y)));

        /// <summary>
        /// Specifies whether this <see cref='Drawing.Int32Point'/> contains the same coordinates as the specified
        /// <see cref='object'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Int32Point && Equals((Int32Point)obj);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(Int32Point other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        /// <summary>
        /// Translates this <see cref='Drawing.Int32Point'/> by the specified amount.
        /// </summary>
        public void Offset(int dx, int dy)
        {
            unchecked
            {
                X += dx;
                Y += dy;
            }
        }

        /// <summary>
        /// Translates this <see cref='Drawing.Int32Point'/> by the specified amount.
        /// </summary>
        public void Offset(Int32Point p) => Offset(p.X, p.Y);

        /// <summary>
        /// Converts this <see cref='Drawing.Int32Point'/> to a human readable string.
        /// </summary>
        public override readonly string ToString() => $"{{X={X},Y={Y}}}";

        private static short HighInt16(int n) => unchecked((short)((n >> 16) & 0xffff));

        private static short LowInt16(int n) => unchecked((short)(n & 0xffff));
    }
}
