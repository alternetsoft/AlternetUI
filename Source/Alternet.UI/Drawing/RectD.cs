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
    /// Stores the location and size of a rectangular region.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RectD : IEquatable<RectD>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='RectD'/> class.
        /// </summary>
        public static readonly RectD Empty;

        private double x; // Do not rename (binary serialization)
        private double y; // Do not rename (binary serialization)
        private double width; // Do not rename (binary serialization)
        private double height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='RectD'/> class with the
        /// specified location
        /// and size.
        /// </summary>
        public RectD(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='RectD'/> class with the
        /// specified location
        /// and size.
        /// </summary>
        public RectD(PointD location, SizeD size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='RectD'/> struct from the specified
        /// <see cref="Vector4"/>.
        /// </summary>
        public RectD(Vector4 vector)
        {
            x = vector.X;
            y = vector.Y;
            width = vector.Z;
            height = vector.W;
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular
        /// region represented by this <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public PointD Location
        {
            readonly get => new(x, y);
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public SizeD Size
        {
            readonly get => new(width, height);
            set
            {
                width = value.Width;
                height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region
        /// defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        public double X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular
        /// region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        public double Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        public double Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Gets or sets the height of the rectangular region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        public double Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular
        /// region defined by this <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public double Left
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular
        /// region defined by this <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public double Top
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public double Right
        {
            readonly get => x + width;
            set => x = value - width;
        }

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public double Bottom
        {
            readonly get => y + height;
            set => y = value - height;
        }

        /// <summary>
        /// Tests whether this <see cref='RectD'/> has a <see cref='Width'/>
        /// or a <see cref='Height'/> less than or equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool SizeIsEmpty => (width <= 0) || (height <= 0);

        /// <summary>
        /// Tests whether this <see cref='RectD'/> has all properties equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsZero => (width == 0) && (height == 0) && (x == 0) && (y == 0);

        /// <summary>
        /// Tests whether this <see cref='RectD'/> has all properties equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => (width == 0) && (height == 0) && (x == 0) && (y == 0);

        /// <summary>
        /// This is a read-only alias for the point which is at (X, Y).
        /// </summary>
        [Browsable(false)]
        public readonly PointD TopLeft
        {
            get
            {
                return new PointD(x, y);
            }
        }

        /// <summary>
        /// Gets the point which is at (X + Width, Y).
        /// </summary>
        [Browsable(false)]
        public readonly PointD TopRight
        {
            get
            {
                return new PointD(Right, y);
            }
        }

        /// <summary>
        /// Gets the point which is at (X, Y + Height).
        /// </summary>
        [Browsable(false)]
        public readonly PointD BottomLeft
        {
            get
            {
                return new PointD(x, Bottom);
            }
        }

        /// <summary>
        /// Gets the point which is at (X + Width, Y + Height).
        /// </summary>
        [Browsable(false)]
        public readonly PointD BottomRight
        {
            get
            {
                return new PointD(Right, Bottom);
            }
        }

        /// <summary>
        /// Gets minimum of <see cref="Width"/> and <see cref="Height"/>.
        /// </summary>
        public readonly double MinWidthHeight => Math.Min(width, height);

        /// <summary>
        /// Gets maximum of <see cref="Width"/> and <see cref="Height"/>.
        /// </summary>
        public readonly double MaxWidthHeight => Math.Max(width, height);

        /// <summary>
        /// Gets the center point of this <see cref="RectD"/>.
        /// </summary>
        [Browsable(false)]
        public readonly PointD Center => Location + (Size / 2);

        /* TODO: uncommment when Double System.Numerics is availble.
         * See https://github.com/dotnet/runtime/issues/24168
        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector4"/> from this <see cref="Rect"/>.
        /// </summary>
        public Vector4 ToVector4() => new Vector4(x, y, width, height);

        /// <summary>
        /// Converts the specified <see cref="Rect"/> to a
        /// <see cref="System.Numerics.Vector4"/>.
        /// </summary>
        public static explicit operator Vector4(Rect rectangle) => rectangle.ToVector4();*/

        /// <summary>
        /// Converts the specified <see cref="Vector2"/> to a
        /// <see cref="RectD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator RectD(Vector4 vector) => new(vector);

        /// <summary>
        /// Implicit operator convertion from tuple with four <see cref="double"/> values
        /// to <see cref="RectD"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectD((double, double, double, double) d) =>
            new(d.Item1, d.Item2, d.Item3, d.Item4);

        /// <summary>
        /// Creates a <see cref='RectD'/> with the properties of the
        /// specified <see cref='System.Drawing.Rectangle'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectD(System.Drawing.Rectangle p) => new(p.X, p.Y, p.Width, p.Height);

        /// <summary>
        /// Creates a <see cref='System.Drawing.Rectangle'/> with the properties of the
        /// specified <see cref='RectD'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Rectangle(RectD p) =>
            new(CoordToInt(p.X), CoordToInt(p.Y), CoordToInt(p.Width), CoordToInt(p.Height));

        /// <summary>
        /// Creates a <see cref='System.Drawing.RectangleF'/> with the properties of the
        /// specified <see cref='RectD'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.RectangleF(RectD p) =>
            new((float)p.X, (float)p.Y, (float)p.Width, (float)p.Height);

        /// <summary>
        /// Implicit operator convertion from tuple (<see cref="PointD"/>, <see cref="Size"/>)
        /// to <see cref="RectD"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectD((PointD, SizeD) d) => new(d.Item1, d.Item2);

        /// <summary>
        /// Converts the specified <see cref='Drawing.RectI'/> to a
        /// <see cref='RectD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectD(RectI r) => new(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Tests whether two <see cref='RectD'/> objects differ in location or size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RectD left, RectD right) => !(left == right);

        /// <summary>
        /// Tests whether two <see cref='RectD'/> objects have equal location and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RectD left, RectD right) =>
            left.x == right.x && left.y == right.y && left.width == right.width
            && left.height == right.height;

        /// <summary>
        /// Creates a <see cref='RectD'/> that is inflated by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD Inflate(RectD rect, double x, double y)
        {
            RectD r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Creates a new <see cref="RectD"/> from a centerpoint and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD FromCenter(PointD center, SizeD size)
        {
            return new RectD(
                center.X - (size.Width / 2),
                center.Y - (size.Height / 2),
                size.Width,
                size.Height);
        }

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static RectD Union(RectD a, RectD b)
        {
            double x1 = Math.Min(a.x, b.x);
            double x2 = Math.Max(a.x + a.width, b.x + b.width);
            double y1 = Math.Min(a.y, b.y);
            double y2 = Math.Max(a.y + a.height, b.y + b.height);

            return new RectD(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there
        /// is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static RectD Intersect(RectD a, RectD b)
        {
            double x1 = Math.Max(a.x, b.x);
            double x2 = Math.Min(a.x + a.width, b.x + b.width);
            double y1 = Math.Max(a.y, b.Y);
            double y2 = Math.Min(a.y + a.height, b.y + b.height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new RectD(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source">string with Rect data.</param>
        /// </summary>
        public static RectD Parse(string source)
        {
            IFormatProvider formatProvider = TypeConverterHelper.InvariantEnglishUS;

            TokenizerHelper th = new(source, formatProvider);

            RectD value;

            string firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty")
            {
                value = Empty;
            }
            else
            {
                value = new RectD(
                    Convert.ToDouble(firstToken, formatProvider),
                    Convert.ToDouble(th.NextTokenRequired(), formatProvider),
                    Convert.ToDouble(th.NextTokenRequired(), formatProvider),
                    Convert.ToDouble(th.NextTokenRequired(), formatProvider));
            }

            // There should be no more tokens in this string.
            th.LastTokenRequired();

            return value;
        }

        /// <summary>
        /// Creates a new <see cref='RectD'/> using
        /// (<paramref name="left"/>, <paramref name="top"/>) and
        /// (<paramref name="right"/>, <paramref name="bottom"/>) points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD FromLTRB(double left, double top, double right, double bottom) =>
            new(left, top, right - left, bottom - top);

        /// <summary>
        /// Creates a new <see cref='RectD'/> specified by two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD FromLTRB(PointD leftTop, PointD rightBottom) =>
            new(
                leftTop.X,
                leftTop.Y,
                rightBottom.X - leftTop.X,
                rightBottom.Y - leftTop.Y);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined
        /// by this
        /// <see cref='RectD'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(double x, double y) =>
            X <= x && x < X + Width && Y <= y && y < Y + Height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined
        /// by this
        /// <see cref='RectD'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(PointD pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Gets percentage of <see cref="Width"/>.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double PercentOfWidth(double percent) => MathUtils.PercentOf(width, percent);

        /// <summary>
        /// Gets percentage of minimal size. Chooses minimum of <see cref="Width"/> and
        /// <see cref="Height"/>) and calculates <paramref name="percent"/> of this value.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double PercentOfMinSize(double percent) =>
            MathUtils.PercentOf(MinWidthHeight, percent);

        /// <summary>
        /// Gets percentage of <see cref="Height"/>.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double PercentOfHeight(double percent) => MathUtils.PercentOf(height, percent);

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='RectD'/> with the
        /// same location and
        /// size of this <see cref='RectD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is RectD rect && Equals(rect);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(RectD other) => this == other;

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is
        /// entirely contained within
        /// the rectangular region represented by this <see cref='RectD'/> .
        /// </summary>
        public readonly bool Contains(RectD rect) =>
            (x <= rect.x) && (rect.x + rect.width <= x + width)
            && (y <= rect.y) && (rect.y + rect.height <= y + height);

        /// <summary>
        /// Converts a <see cref="RectD"/> structure to a
        /// <see cref="RectI"/> structure.
        /// </summary>
        /// <returns>A <see cref="RectI"/> structure.</returns>
        /// <remarks>The <see cref="RectD"/> structure is converted
        /// to a <see cref="RectI"/> structure by
        /// truncating the values of the <see cref="RectD"/> to the next
        /// lower integer values.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectI ToRect() => RectI.Truncate(this);

        /// <summary>
        /// Gets <see cref="X"/> or <see cref="Y"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to return <see cref="X"/> or <see cref="Y"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double GetLocation(bool vert)
        {
            if (vert)
                return y;
            else
                return x;
        }

        /// <summary>
        /// Sets <see cref="X"/> or <see cref="Y"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to set <see cref="X"/> or <see cref="Y"/>.</param>
        /// <param name="value">New location value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLocation(bool vert, double value)
        {
            if (vert)
                y = value;
            else
                x = value;
        }

        /// <summary>
        /// Gets <see cref="Width"/> or <see cref="Height"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to return <see cref="Width"/>
        /// or <see cref="Height"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double GetSize(bool vert)
        {
            if (vert)
                return height;
            else
                return width;
        }

        /// <summary>
        /// Sets <see cref="Width"/> or <see cref="Height"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to set <see cref="Width"/>
        /// or <see cref="Height"/>.</param>
        /// <returns></returns>
        /// <param name="value">New size value.</param>
        public void SetSize(bool vert, double value)
        {
            if (vert)
                height = value;
            else
                width = value;
        }

        /// <summary>
        /// Gets the hash code for this <see cref='RectD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.Combine(x, y, width, height);

        /// <summary>
        /// Inflates this <see cref='RectD'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Inflate(double dx, double dy)
        {
            x -= dx;
            y -= dy;
            width += 2 * dx;
            height += 2 * dy;
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> that is inflated by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD InflatedBy(double x, double y) => Inflate(this, x, y);

        /// <summary>
        /// Creates a <see cref='RectD'/> that is offset by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD OffsetBy(double dx, double dy)
        {
            var r = this;
            r.x += dx;
            r.y += dy;
            return r;
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> with the specified size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithSize(double width, double height)
        {
            var r = this;
            r.width = width;
            r.height = height;
            return r;
        }

        /// <summary>
        /// Returns new <see cref="RectD"/> value with ceiling of location and size.
        /// Uses <see cref="Math.Ceiling(double)"/> on values.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD Ceiling()
        {
            return new(Math.Ceiling(x), Math.Ceiling(y), Math.Ceiling(width), Math.Ceiling(height));
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> with the specified location.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithLocation(double x, double y)
        {
            var r = this;
            r.x = x;
            r.y = y;
            return r;
        }

        /// <summary>
        /// Inflates this <see cref='RectD'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Inflate(SizeD size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Inflates this <see cref='RectD'/> by 1. <see cref="X"/> and <see cref="Y"/>
        /// are decremented by 1, <see cref="Width"/> and <see cref="Height"/> are
        /// incremented by 2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Inflate()
        {
            x -= 1;
            y -= 1;
            width += 2;
            height += 2;
        }

        /// <summary>
        /// Deflates this <see cref='RectD'/> by 1. <see cref="X"/> and <see cref="Y"/>
        /// are incremented by 1, <see cref="Width"/> and <see cref="Height"/> are
        /// decremented by 2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deflate()
        {
            x += 1;
            y += 1;
            width -= 2;
            height -= 2;
        }

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Intersect(RectD rect)
        {
            RectD result = Intersect(rect, this);

            x = result.x;
            y = result.y;
            width = result.width;
            height = result.height;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(RectD rect) =>
            (rect.x < x + width) && (x < rect.x + rect.width) && (rect.y < y + height)
            && (y < rect.y + rect.height);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(PointD pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Centers this rectangle in the given (usually, but not necessarily, larger) one.
        /// </summary>
        /// <param name="r">Container rectangle.</param>
        /// <param name="centerHorz">Specifies whether to center horizontally.</param>
        /// <param name="centerVert">Specifies whether to center vertically.</param>
        /// <returns></returns>
        public readonly RectD CenterIn(RectD r, bool centerHorz = true, bool centerVert = true)
        {
            return new RectD(
                centerHorz ? r.x + ((r.width - width) / 2) : x,
                centerVert ? r.y + ((r.height - height) / 2) : y,
                width,
                height);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(double dx, double dy)
        {
            x += dx;
            y += dy;
        }

        /// <summary>
        /// Converts the <see cref='Location'/> and <see cref='Size'/>
        /// of this <see cref='RectD'/> to a human-readable string.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names =
            {
                PropNameStrings.Default.X,
                PropNameStrings.Default.Y,
                PropNameStrings.Default.Width,
                PropNameStrings.Default.Height,
            };

            double[] values = { x, y, width, height };

            return StringUtils.ToString<double>(names, values);
        }

        /// <summary>
        /// Creates a string representation of this object based on the format string
        /// and <see cref="IFormatProvider"/> passed in.
        /// If the provider is null, the
        /// <see cref="System.Globalization.CultureInfo.CurrentCulture"/> is used.
        /// See the documentation for <see cref="IFormattable"/> for more information.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        public readonly string ConvertToString(string format, IFormatProvider provider)
        {
            if (IsZero)
            {
                return "Empty";
            }

            // Helper to get the numeric list separator for a given culture.
            char separator = TokenizerHelper.GetNumericListSeparator(provider);
            return string.Format(
                provider,
                "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}",
                separator,
                x,
                y,
                width,
                height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CoordToInt(double value)
        {
            int i = (int)Math.Round(value, MidpointRounding.AwayFromZero);
            return i;
        }
    }
}
