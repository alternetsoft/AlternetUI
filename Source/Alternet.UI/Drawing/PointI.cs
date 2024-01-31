// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point
    /// in a two-dimensional plane.
    /// </summary>
    /*
     Please do not remove StructLayout(LayoutKind.Sequential) atrtribute.
     Also do not change order of the fields.
    */
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    /*[TypeConverter("System.Drawing.PointConverter, System.Drawing,
      Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]*/
    public struct PointI : IEquatable<PointI>
    {
        /// <summary>
        /// Gets an empty point with (0, 0) ccordinates.
        /// </summary>
        public static readonly PointI Empty;

        /// <summary>
        /// Gets a point with (-1, -1) ccordinates.
        /// </summary>
        public static readonly PointI MinusOne = new(-1, -1);

        /// <summary>
        /// Gets a point with (1, 1) ccordinates.
        /// </summary>
        public static readonly PointI One = new(1, 1);

        private int x; // Do not rename (binary serialization)
        private int y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointI'/>
        /// class with the specified coordinates.
        /// </summary>
        public PointI(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointI'/>
        /// class from a <see cref='Drawing.SizeD'/>.
        /// </summary>
        public PointI(SizeI sz)
        {
            x = sz.Width;
            y = sz.Height;
        }

        /// <summary>
        /// Initializes a new instance of the Point class using coordinates
        /// specified by an integer value.
        /// </summary>
        public PointI(int dw)
        {
            x = LowInt16(dw);
            y = HighInt16(dw);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref='Drawing.PointI'/>
        /// is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == 0 && y == 0;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='Drawing.PointI'/>.
        /// </summary>
        public int X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='Drawing.PointI'/>.
        /// </summary>
        public int Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Creates a <see cref='PointD'/> with the coordinates of the
        /// specified <see cref='PointI'/>
        /// </summary>
        public static implicit operator PointD(PointI p) => new(p.X, p.Y);

        /// <summary>
        /// Creates a <see cref='System.Drawing.Point'/> with the coordinates of the
        /// specified <see cref='PointI'/>
        /// </summary>
        public static implicit operator System.Drawing.Point(PointI p) => new(p.X, p.Y);

        /// <summary>
        /// Creates a <see cref='PointI'/> with the coordinates of the
        /// specified <see cref='System.Drawing.Point'/>
        /// </summary>
        public static implicit operator PointI(System.Drawing.Point p) => new(p.X, p.Y);

        /// <summary>
        /// Creates a <see cref='Drawing.SizeI'/> with the coordinates of
        /// the specified <see cref='Drawing.PointI'/> .
        /// </summary>
        public static explicit operator SizeI(PointI p) => new(p.X, p.Y);

        /// <summary>
        /// Implicit operator convertion from tuple with two <see cref="int"/> values
        /// to <see cref="PointI"/>.
        /// </summary>
        /// <param name="d">New point value.</param>
        public static implicit operator PointI((int, int) d) => new(d.Item1, d.Item2);

        /// <summary>
        /// Translates a <see cref='Drawing.PointI'/> by a given
        /// <see cref='Drawing.SizeI'/> .
        /// </summary>
        public static PointI operator +(PointI pt, SizeI sz) =>
            Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='Drawing.PointI'/> by the negative of
        /// a given <see cref='Drawing.SizeI'/> .
        /// </summary>
        public static PointI operator -(PointI pt, SizeI sz) =>
            Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='Drawing.PointI'/> objects. The result
        /// specifies whether the values of the
        /// <see cref='Drawing.PointI.X'/> and
        /// <see cref='Drawing.PointI.Y'/> properties of the two
        /// <see cref='Drawing.PointI'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointI left, PointI right) =>
            left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='Drawing.PointI'/> objects.
        /// The result specifies whether the values of the
        /// <see cref='Drawing.PointI.X'/> or
        /// <see cref='Drawing.PointI.Y'/> properties of the two
        /// <see cref='Drawing.PointI'/>  objects are unequal.
        /// </summary>
        public static bool operator !=(PointI left, PointI right) =>
            !(left == right);

        /// <summary>
        /// Translates a <see cref='Drawing.PointI'/> by a given
        /// <see cref='Drawing.SizeI'/> .
        /// </summary>
        public static PointI Add(PointI pt, SizeI sz) =>
            new(unchecked(pt.X + sz.Width), unchecked(pt.Y + sz.Height));

        /// <summary>
        /// Translates a <see cref='Drawing.PointI'/> by the negative
        /// of a given <see cref='Drawing.SizeI'/> .
        /// </summary>
        public static PointI Subtract(PointI pt, SizeI sz) =>
            new(unchecked(pt.X - sz.Width), unchecked(pt.Y - sz.Height));

        /// <summary>
        /// Converts a PointF to a Point by performing a ceiling operation
        /// on all the coordinates.
        /// </summary>
        public static PointI Ceiling(PointD value) =>
            new(unchecked((int)Math.Ceiling(value.X)),
                unchecked((int)Math.Ceiling(value.Y)));

        /// <summary>
        /// Converts a Point to a Int32Point by performing a truncate operation
        /// on all the coordinates.
        /// </summary>
        public static PointI Truncate(PointD value) =>
            new(unchecked((int)value.X), unchecked((int)value.Y));

        /// <summary>
        /// Converts a PointF to a Point by performing a round operation on
        /// all the coordinates.
        /// </summary>
        public static PointI Round(PointD value) =>
            new(unchecked((int)Math.Round(value.X)),
                unchecked((int)Math.Round(value.Y)));

        /// <summary>
        /// Specifies whether this <see cref='Drawing.PointI'/> contains
        /// the same coordinates as the specified
        /// <see cref='object'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is PointI point && Equals(point);

        /// <summary>
        /// Indicates whether the current object is equal to another
        /// object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other;
        /// otherwise, <c>false</c>.</returns>
        public readonly bool Equals(PointI other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        /// <summary>
        /// Translates this <see cref='Drawing.PointI'/> by the specified amount.
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
        /// Translates this <see cref='Drawing.PointI'/> by the specified amount.
        /// </summary>
        public void Offset(PointI p) => Offset(p.X, p.Y);

        /// <summary>
        /// Converts this <see cref='Drawing.PointI'/> to a human readable string.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names = { PropNameStrings.Default.X, PropNameStrings.Default.Y };
            int[] values = { x, y };

            return StringUtils.ToString<int>(names, values);
        }

        private static short HighInt16(int n) =>
            unchecked((short)((n >> 16) & 0xffff));

        private static short LowInt16(int n) => unchecked((short)(n & 0xffff));
    }
}
