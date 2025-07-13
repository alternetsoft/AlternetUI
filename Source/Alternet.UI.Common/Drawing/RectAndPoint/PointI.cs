using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Alternet.UI;
using Alternet.UI.Localization;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point
    /// in a two-dimensional plane.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct PointI : IEquatable<PointI>
    {
        /// <summary>
        /// Gets an empty point with (0, 0) coordinates.
        /// </summary>
        public static readonly PointI Empty;

        /// <summary>
        /// Gets a point with (-1, -1) coordinates.
        /// </summary>
        public static readonly PointI MinusOne = new(-1, -1);

        /// <summary>
        /// Gets a point with (1, 1) coordinates.
        /// </summary>
        public static readonly PointI One = new(1, 1);

        [FieldOffset(0)]
        private readonly ulong xy;

        [FieldOffset(0)]
        private int x;

        [FieldOffset(4)]
        private int y;

        [FieldOffset(0)]
        private SKPointI point;

        [FieldOffset(0)]
        private SizeI size;

        /// <summary>
        /// Initializes a new instance of the <see cref='PointI'/>
        /// class with the specified coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointI(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='PointI'/>
        /// class with the specified coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointI(SKPointI point)
        {
            this.point = point;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='PointI'/>
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
        /// Gets or sets this point as <see cref="SKPoint"/>.
        /// </summary>
        [Browsable(false)]
        public SKPointI SkiaPoint
        {
            readonly get => point;
            set => point = value;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref='PointI'/>
        /// is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => xy == 0UL;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='PointI'/>.
        /// </summary>
        public int X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='PointI'/>.
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
        /// Creates a <see cref='SKPointI'/> with the coordinates of the
        /// specified <see cref='PointI'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SKPointI(PointI p) => p.point;

        /// <summary>
        /// Creates a <see cref='PointI'/> with the coordinates of the
        /// specified <see cref='SKPointI'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointI(SKPointI p) => new(p);

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
        public static explicit operator SizeI(PointI p) => p.size;

        /// <summary>
        /// Implicit operator conversion from tuple with two <see cref="int"/> values
        /// to <see cref="PointI"/>.
        /// </summary>
        /// <param name="d">New point value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointI((int X, int Y) d) => new(d.X, d.Y);

        /// <summary>
        /// Translates a <see cref='PointI'/> by a given
        /// <see cref='SizeI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI operator +(PointI pt, SizeI sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointI'/> by the negative of
        /// a given <see cref='SizeI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI operator -(PointI pt, SizeI sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='PointI'/> objects. The result
        /// specifies whether the values of the
        /// <see cref='X'/> and
        /// <see cref='Y'/> properties of the two
        /// <see cref='PointI'/> objects are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PointI left, PointI right) => left.xy == right.xy;

        /// <summary>
        /// Compares two <see cref='PointI'/> objects.
        /// The result specifies whether the values of the
        /// <see cref='X'/> or
        /// <see cref='Y'/> properties of the two
        /// <see cref='PointI'/>  objects are unequal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PointI left, PointI right) => left.xy != right.xy;

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
        /// Converts a <see cref="PointD"/> to a <see cref="PointI"/>
        /// by performing a ceiling operation on all the coordinates.
        /// </summary>
        /// <remarks>
        /// Ceiling operation returns the smallest integer that is greater than or equal
        /// to the specified floating-point number.
        /// </remarks>
        public static PointI Ceiling(PointD value)
        {
            return new(
                unchecked((int)Math.Ceiling(value.X)),
                unchecked((int)Math.Ceiling(value.Y)));
        }

        /// <summary>
        /// Converts a <see cref="PointD"/> to a <see cref="PointI"/>
        /// by performing a truncate operation
        /// on all the coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI Truncate(PointD value) =>
            new(unchecked((int)value.X), unchecked((int)value.Y));

        /// <summary>
        /// Converts a <see cref="PointD"/> to a <see cref="PointI"/>
        /// by performing a round operation on the coordinates.
        /// </summary>
        /// <param name="rounding">The <see cref="MidpointRounding"/> to use when
        /// <see cref="Math.Round(Coord,MidpointRounding)"/> is called.</param>
        /// <remarks>
        /// Rounds a floating-point value to the nearest integer.
        /// </remarks>
        /// <param name="value">The <see cref="PointD" /> to be converted.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointI Round(PointD value, MidpointRounding? rounding = null)
        {
            rounding ??= RectD.DefaultMidpointRounding;
            return new(
                unchecked((int)Math.Round(value.X, rounding.Value)),
                unchecked((int)Math.Round(value.Y, rounding.Value)));
        }

        /// <summary>
        /// Specifies whether this <see cref='PointI'/> contains
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
        public override readonly int GetHashCode() => (x, y).GetHashCode();

        /// <summary>
        /// Translates this <see cref='Drawing.PointI'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(int dx, int dy)
        {
            unchecked
            {
                x += dx;
                y += dy;
            }
        }

        /// <summary>
        /// Converts this point to device-independent units using the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor. Optional. If not specified, the default
        /// value is used.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD PixelToDip(Coord scaleFactor)
        {
            return GraphicsFactory.PixelToDip(this, scaleFactor);
        }

        /// <summary>
        /// Translates this <see cref='Drawing.PointI'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(PointI p) => Offset(p.x, p.y);

        /// <summary>
        /// Converts this <see cref='Drawing.PointI'/> to a human readable string.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names = { PropNameStrings.Default.X, PropNameStrings.Default.Y };
            int[] values = { x, y };

            return StringUtils.ToStringWithOrWithoutNames<int>(names, values);
        }
    }
}
