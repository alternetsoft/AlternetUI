using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Alternet.UI;
using Alternet.UI.Localization;
using Alternet.UI.Markup;

using SkiaSharp;

namespace Alternet.Drawing
{
    /*
        Please do not remove StructLayout(LayoutKind.Sequential) attribute.
        Also do not change order of the fields.
    */

    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point in a
    /// two-dimensional plane.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PointD : IEquatable<PointD>
    {
        /// <summary>
        /// Gets minimal possible value.
        /// </summary>
        public static readonly PointD MinValue = new(Coord.MinValue);

        /// <summary>
        /// Gets maximal possible value.
        /// </summary>
        public static readonly PointD MaxValue = new(Coord.MaxValue);

        /// <summary>
        /// Gets an empty point with (0, 0) coordinates.
        /// </summary>
        public static readonly PointD Empty;

        /// <summary>
        /// Gets a point with (-1, -1) coordinates.
        /// </summary>
        public static readonly PointD MinusOne = new(CoordD.MinusOne, CoordD.MinusOne);

        /// <summary>
        /// Gets a point with (1, 1) coordinates.
        /// </summary>
        public static readonly PointD One = new(CoordD.One, CoordD.One);

        private Coord x; // Do not rename (binary serialization)
        private Coord y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointD'/> class with the
        /// specified coordinates.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointD(Coord x, Coord y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointD'/> class with the
        /// specified coordinate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointD(Coord xy)
        {
            this.x = xy;
            this.y = xy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointD'/>
        /// struct from the specified
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointD(Vector2 vector)
        {
            x = vector.X;
            y = vector.Y;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref='PointD'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == CoordD.Empty && y == CoordD.Empty;

        /// <summary>
        /// Gets a value indicating whether this <see cref='PointD'/> has X or Y negative.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsAnyNegative => x < CoordD.Empty || y < CoordD.Empty;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='Drawing.PointD'/>.
        /// </summary>
        public Coord X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='Drawing.PointD'/>.
        /// </summary>
        public Coord Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Converts the specified <see cref="PointD"/> to a
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2(PointD point) => point.ToVector2();

        /// <summary>
        /// Converts the specified <see cref="System.Numerics.Vector2"/> to a
        /// <see cref="PointD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PointD(Vector2 vector) => new(vector);

        /// <summary>
        /// Creates a <see cref='PointD'/> with the properties of the
        /// specified <see cref='System.Drawing.Point'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointD(System.Drawing.Point p) => new(p.X, p.Y);

        /// <summary>
        /// Creates a <see cref='System.Drawing.Point'/> with the properties of the
        /// specified <see cref='PointD'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Point(PointD p) =>
            new(RectD.CoordToInt(p.X), RectD.CoordToInt(p.Y));

        /// <summary>
        /// Implicit operator conversion from tuple with two values
        /// to <see cref="PointD"/>.
        /// </summary>
        /// <param name="d">New point value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointD((Coord X, Coord Y) d) => new(d.X, d.Y);

        /// <summary>
        /// Converts the specified <see cref="SKPoint"/> to a
        /// <see cref="PointD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointD(SKPoint value)
        {
            return new(value.X, value.Y);
        }

        /// <summary>
        /// Converts the specified <see cref="PointD"/> to a
        /// <see cref="SKPoint"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SKPoint(PointD point)
        {
            return new SKPoint((float)point.x, (float)point.y);
        }

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='SizeI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator +(PointD pt, SizeI sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointD'/> by a given <see cref='PointD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator +(PointD pt, PointD pt2) => Add(pt, pt2);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.SizeI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator -(PointD pt, SizeI sz) => Subtract(pt, sz);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.PointD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator -(PointD pt, PointD pt2) => Subtract(pt, pt2);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by a given <see cref='SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator +(PointD pt, SizeD sz) => Add(pt, sz);

        /// <summary>
        /// Moves a <see cref='PointD'/> by a given value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator +(PointD pt, Coord offset) =>
            new(pt.X + offset, pt.Y + offset);

        /// <summary>
        /// Translates a <see cref='PointD'/> by the negative of a given
        /// <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator -(PointD pt, SizeD sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='Drawing.PointD'/> objects. The result specifies whether
        /// the values of the
        /// <see cref='Drawing.PointD.X'/> and <see cref='Drawing.PointD.Y'/>
        /// properties of the two
        /// <see cref='Drawing.PointD'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointD left, PointD right)
            => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='Drawing.PointD'/> objects. The result specifies whether
        /// the values of the
        /// <see cref='Drawing.PointD.X'/> or <see cref='Drawing.PointD.Y'/>
        /// properties of the two
        /// <see cref='Drawing.PointD'/> objects are unequal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PointD left, PointD right)
            => !(left == right);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by a given <see cref='Drawing.SizeI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Add(PointD pt, SizeI sz)
            => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by a given <see cref='Drawing.PointD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Add(PointD pt, PointD pt2)
            => new(pt.X + pt2.X, pt.Y + pt2.Y);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.SizeI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Subtract(PointD pt, SizeI sz)
            => new(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.PointD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Subtract(PointD pt, PointD pt2)
            => new(pt.X - pt2.X, pt.Y - pt2.Y);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by a given <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Add(PointD pt, SizeD sz)
            => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Subtract(PointD pt, SizeD sz)
            => new(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Returns an instance converted from the provided string using
        /// <see cref="App.InvariantEnglishUS"/> culture.
        /// <param name="source">String with data.</param>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Parse(string source)
        {
            return ConversionUtils.ParseTwoFloats(source);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object;
        /// otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is PointD point && Equals(point);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(PointD other) => this == other;

        /// <summary>
        /// Gets <see cref="X"/> or <see cref="Y"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to return <see cref="X"/>
        /// or <see cref="Y"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Coord GetLocation(bool vert)
        {
            if (vert)
                return y;
            else
                return x;
        }

        /// <summary>
        /// Gets this point as a rectangle with the specified size.
        /// </summary>
        [Browsable(false)]
        public readonly RectD AsRect(SizeD size)
        {
            return (this, size);
        }

        /// <summary>
        /// Increments <see cref="X"/> or <see cref="Y"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to increment <see cref="X"/>
        /// or <see cref="Y"/>.</param>
        /// <param name="value">Value to add to the location.</param>
        public void IncLocation(bool vert, Coord value)
        {
            var oldLocation = GetLocation(vert);
            SetLocation(vert, oldLocation + value);
        }

        /// <summary>
        /// Sets <see cref="X"/> or <see cref="Y"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to set <see cref="X"/> or <see cref="Y"/>.</param>
        /// <param name="value">New location value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLocation(bool vert, Coord value)
        {
            if (vert)
                y = value;
            else
                x = value;
        }

        /// <summary>
        /// Translates this <see cref='PointD'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(Coord dx, Coord dy)
        {
            unchecked
            {
                x += dx;
                y += dy;
            }
        }

        /// <summary>
        /// Returns new <see cref='PointD'/> with coordinates of this point translated
        /// by the specified amount.
        /// </summary>
        public readonly PointD OffsetBy(Coord dx, Coord dy)
        {
            var newX = x;
            var newY = y;

            unchecked
            {
                newX += dx;
                newY += dy;
            }

            return new(newX, newY);
        }

        /// <summary>
        /// Returns new <see cref='PointD'/> with coordinates of this point translated
        /// by the specified amount specified in <paramref name="d"/>.
        /// </summary>
        public readonly PointD OffsetBy(PointD d) => OffsetBy(d.x, d.y);

        /// <summary>
        /// Translates this <see cref='PointD'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(PointI p) => Offset(p.X, p.Y);

        /// <summary>
        /// Converts a <see cref="PointD"/> structure to a
        /// <see cref="PointI"/> structure.
        /// </summary>
        /// <returns>A <see cref="PointI"/> structure.</returns>
        /// <remarks>The <see cref="PointD"/> structure is converted
        /// to a <see cref="PointI"/> structure by
        /// truncating the values of the <see cref="PointD"/> to the next
        /// lower integer values.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointI ToPoint() => PointI.Truncate(this);

        /// <summary>
        /// Returns new <see cref="PointD"/> value with ceiling of the <see cref="X"/> and
        /// <see cref="Y"/>. Uses <see cref="Math.Ceiling(Coord)"/> on values.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Ceiling operation returns the smallest integer that is greater than or equal
        /// to the specified floating-point number.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD Ceiling()
        {
            return new(Math.Ceiling(x), Math.Ceiling(y));
        }

        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector2"/> from
        /// this <see cref="System.Drawing.PointF"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Vector2 ToVector2()
        {
            return new(RectD.CoordToFloat(x), RectD.CoordToFloat(y));
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }

        /// <summary>
        /// Clamps the coordinates of the current <see cref="PointD"/> instance
        /// to zero if they are less than zero.
        /// </summary>
        /// <remarks>This method ensures that the resulting coordinates are non-negative
        /// by replacing any negative values with zero.</remarks>
        /// <returns>A new <see cref="PointD"/> instance with the X and Y coordinates
        /// clamped to zero if they are less than zero.</returns>
        public readonly PointD ClampToZero()
        {
            return new PointD(
                x < CoordD.Empty ? CoordD.Empty : x,
                y < CoordD.Empty ? CoordD.Empty : y);
        }

        /// <summary>
        /// Clamps the current point's coordinates to the specified point's coordinates.
        /// </summary>
        /// <param name="clampTo">The point whose coordinates define the
        /// minimum values for clamping.</param>
        /// <returns>A new <see cref="PointD"/> instance where the X and Y coordinates
        /// are clamped to the corresponding X and Y
        /// coordinates of the <paramref name="clampTo"/> point.</returns>
        public readonly PointD ClampTo(PointD clampTo)
        {
            return new PointD(
                x < clampTo.X ? clampTo.X : x,
                y < clampTo.Y ? clampTo.Y : y);
        }

        /// <summary>
        /// Clamps the current point's coordinates to the specified minimum values.
        /// </summary>
        /// <param name="clampX">The minimum allowable value for the X-coordinate.</param>
        /// <param name="clampY">The minimum allowable value for the Y-coordinate.</param>
        /// <returns>A new <see cref="PointD"/> instance with the X and Y
        /// coordinates clamped to the specified minimum values.</returns>
        public readonly PointD ClampTo(Coord clampX, Coord clampY)
        {
            return new PointD(
                x < clampX ? clampX : x,
                y < clampY ? clampY : y);
        }

        /// <summary>
        /// Clamps the current point's coordinates to the specified minimum and maximum bounds.
        /// </summary>
        /// <remarks>If the current point's coordinates are already within the specified bounds,
        /// the returned point will have the same coordinates as the current point.</remarks>
        /// <param name="min">The point representing the minimum allowable values for the coordinates.
        /// The <see cref="PointD.X"/> and <see cref="PointD.Y"/> values of this point define
        /// the lower bounds.</param>
        /// <param name="max">The point representing the maximum allowable values for the coordinates.
        /// The <see cref="PointD.X"/> and <see cref="PointD.Y"/> values of this point define
        /// the upper bounds.</param>
        /// <returns>A new <see cref="PointD"/> instance with its coordinates clamped
        /// to the specified bounds.</returns>
        public readonly PointD ClampToMinMax(PointD min, PointD max)
        {
            Coord clampedX = Math.Min(Math.Max(x, min.x), max.y);
            Coord clampedY = Math.Min(Math.Max(y, min.y), max.y);
            return new PointD(clampedX, clampedY);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override readonly string ToString()
        {
            string[] names = { PropNameStrings.Default.X, PropNameStrings.Default.Y };
            Coord[] values = { x, y };

            return StringUtils.ToStringWithOrWithoutNames<Coord>(names, values);
        }

        /// <summary>
        /// Converts this point to the pixel point using the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor. Optional. If not specified, the default
        /// scale factor is used for the conversion.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointI PixelFromDip(Coord? scaleFactor = null)
        {
            return GraphicsFactory.PixelFromDip(this, scaleFactor);
        }

        /// <summary>
        /// Calls <paramref name="coerceFunc"/> for x and y.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Coerce(Func<Coord, Coord> coerceFunc)
        {
            x = coerceFunc(x);
            y = coerceFunc(y);
        }

        /// <summary>
        /// Calls <see cref="SizeD.CoerceCoordFunc"/> for x and y.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Coerce()
        {
            if (SizeD.CoerceCoordFunc is null)
                return;
            Coerce(SizeD.CoerceCoordFunc);
        }

        /// <summary>
        /// Creates a string representation of this object based on the format string
        /// and <see cref="IFormatProvider"/> passed in.
        /// If the provider is null, the CurrentCulture is used.
        /// See the documentation for <see cref="IFormattable"/> for more information.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        public readonly string ConvertToString(string format, IFormatProvider provider)
        {
            // Helper to get the numeric list separator for a given culture.
            char separator = TokenizerHelper.GetNumericListSeparator(provider);
            return string.Format(
                provider,
                "{1:" + format + "}{0}{2:" + format + "}",
                separator,
                X,
                Y);
        }
    }
}
