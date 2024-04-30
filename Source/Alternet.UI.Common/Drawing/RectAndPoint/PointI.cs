using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointI(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointI'/>
        /// class from a <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointI(SizeI sz)
        {
            x = sz.Width;
            y = sz.Height;
        }

        /// <summary>
        /// Initializes a new instance of the Point class using coordinates
        /// specified by an integer value.
        /// </summary>
        /// <remarks>
        /// <see cref="X"/> property is assigned with low part of the integer value
        /// and <see cref="Y"/> property is assigned with high part of the integer value.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointI(int dw)
        {
            x = MathUtils.LowInt16(dw);
            y = MathUtils.HighInt16(dw);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointD(PointI p) => new(p.X, p.Y);

        /// <summary>
        /// Creates a <see cref='System.Drawing.Point'/> with the coordinates of the
        /// specified <see cref='PointI'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Point(PointI p) => new(p.X, p.Y);

        /// <summary>
        /// Creates a <see cref='PointI'/> with the coordinates of the
        /// specified <see cref='System.Drawing.Point'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointI(System.Drawing.Point p) => new(p.X, p.Y);

        /// <summary>
        /// Creates a <see cref='Drawing.SizeI'/> with the coordinates of
        /// the specified <see cref='Drawing.PointI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SizeI(PointI p) => new(p.X, p.Y);

        /// <summary>
        /// Implicit operator convertion from tuple with two <see cref="int"/> values
        /// to <see cref="PointI"/>.
        /// </summary>
        /// <param name="d">New point value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointI((int, int) d) => new(d.Item1, d.Item2);

        /// <summary>
        /// Translates a <see cref='PointI'/> by a given
        /// <see cref='SizeI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI operator +(PointI pt, SizeI sz) =>
            Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointI'/> by the negative of
        /// a given <see cref='SizeI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI operator -(PointI pt, SizeI sz) =>
            Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='PointI'/> objects. The result
        /// specifies whether the values of the
        /// <see cref='X'/> and
        /// <see cref='Y'/> properties of the two
        /// <see cref='PointI'/> objects are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PointI left, PointI right) =>
            left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='PointI'/> objects.
        /// The result specifies whether the values of the
        /// <see cref='X'/> or
        /// <see cref='Y'/> properties of the two
        /// <see cref='PointI'/>  objects are unequal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PointI left, PointI right) =>
            !(left == right);

        /// <summary>
        /// Translates a <see cref='PointI'/> by a given
        /// <see cref='SizeI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI Add(PointI pt, SizeI sz) =>
            new(unchecked(pt.X + sz.Width), unchecked(pt.Y + sz.Height));

        /// <summary>
        /// Translates a <see cref='PointI'/> by the negative
        /// of a given <see cref='SizeI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI Truncate(PointD value) =>
            new(unchecked((int)value.X), unchecked((int)value.Y));

        /// <summary>
        /// Converts a PointF to a Point by performing a round operation on
        /// all the coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(PointI other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        /// <summary>
        /// Translates this <see cref='Drawing.PointI'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    }
}
