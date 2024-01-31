// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Alternet.UI;
using Alternet.UI.Localization;
using Alternet.UI.Markup;

namespace Alternet.Drawing
{
    /*
        Please do not remove StructLayout(LayoutKind.Sequential) atrtribute.
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
        /// Gets an empty point with (0, 0) ccordinates.
        /// </summary>
        public static readonly PointD Empty;

        /// <summary>
        /// Gets a point with (-1, -1) ccordinates.
        /// </summary>
        public static readonly PointD MinusOne = new(-1d, -1d);

        /// <summary>
        /// Gets a point with (1, 1) ccordinates.
        /// </summary>
        public static readonly PointD One = new(1d, 1d);

        private double x; // Do not rename (binary serialization)
        private double y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointD'/> class with the
        /// specified coordinates.
        /// </summary>
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointD'/> struct from the specified
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public PointD(Vector2 vector)
        {
            x = vector.X;
            y = vector.Y;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref='Drawing.PointD'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == 0f && y == 0f;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='Drawing.PointD'/>.
        /// </summary>
        public double X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='Drawing.PointD'/>.
        /// </summary>
        public double Y
        {
            readonly get => y;
            set => y = value;
        }

        /* TODO: uncommment when Double System.Numerics is availble.
         * See https://github.com/dotnet/runtime/issues/24168
        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector2"/> from
        /// this <see cref="System.Drawing.PointF"/>.
        /// </summary>
        public Vector2 ToVector2() => new Vector2(x, y);

        /// <summary>
        /// Converts the specified <see cref="Drawing.Point"/> to a
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public static explicit operator Vector2(Point point) => point.ToVector2();*/

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
        /// Implicit operator convertion from tuple with two <see cref="double"/> values
        /// to <see cref="PointD"/>.
        /// </summary>
        /// <param name="d">New point value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PointD((double, double) d) => new(d.Item1, d.Item2);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by a given <see cref='Drawing.SizeI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator +(PointD pt, SizeI sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.SizeI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator -(PointD pt, SizeI sz) => Subtract(pt, sz);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by a given <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator +(PointD pt, SizeD sz) => Add(pt, sz);

        /// <summary>
        /// Moves a <see cref='PointD'/> by a given value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator +(PointD pt, double offset) =>
            new(pt.X + offset, pt.Y + offset);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD operator -(PointD pt, SizeD sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='Drawing.PointD'/> objects. The result specifies whether
        /// the values of the
        /// <see cref='Drawing.PointD.X'/> and <see cref='Drawing.PointD.Y'/> properties of the two
        /// <see cref='Drawing.PointD'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointD left, PointD right) => left.X == right.X &&
            left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='Drawing.PointD'/> objects. The result specifies whether
        /// the values of the
        /// <see cref='Drawing.PointD.X'/> or <see cref='Drawing.PointD.Y'/> properties of the two
        /// <see cref='Drawing.PointD'/> objects are unequal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PointD left, PointD right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by a given <see cref='Drawing.SizeI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Add(PointD pt, SizeI sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.SizeI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Subtract(PointD pt, SizeI sz) =>
            new(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by a given <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Add(PointD pt, SizeD sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointD'/> by the negative of a given
        /// <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD Subtract(PointD pt, SizeD sz) => new(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Point data </param>
        /// </summary>
        public static PointD Parse(string source)
        {
            IFormatProvider formatProvider = TypeConverterHelper.InvariantEnglishUS;

            TokenizerHelper th = new(source, formatProvider);

            PointD value;

            string firstToken = th.NextTokenRequired();

            value = new PointD(
                Convert.ToDouble(firstToken, formatProvider),
                Convert.ToDouble(th.NextTokenRequired(), formatProvider));

            // There should be no more tokens in this string.
            th.LastTokenRequired();

            return value;
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
        /// Translates this <see cref='PointD'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(double dx, double dy)
        {
            unchecked
            {
                X += dx;
                Y += dy;
            }
        }

        /// <summary>
        /// Returns new <see cref='PointD'/> with coordinates of this point translated
        /// by the specified amount.
        /// </summary>
        public readonly PointD OffsetBy(double dx, double dy)
        {
            double newX = x;
            double newY = y;

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
        /// <see cref="Y"/>. Uses <see cref="Math.Ceiling(double)"/> on values.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD Ceiling()
        {
            return new(Math.Ceiling(x), Math.Ceiling(y));
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() =>
            HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override readonly string ToString()
        {
            string[] names = { PropNameStrings.Default.X, PropNameStrings.Default.Y };
            double[] values = { x, y };

            return StringUtils.ToString<double>(names, values);
        }

        /// <summary>
        /// Creates a string representation of this object based on the format string
        /// and IFormatProvider passed in.
        /// If the provider is null, the CurrentCulture is used.
        /// See the documentation for IFormattable for more information.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        internal readonly string ConvertToString(string format, IFormatProvider provider)
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
