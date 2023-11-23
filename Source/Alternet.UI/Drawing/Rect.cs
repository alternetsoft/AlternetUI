// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
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
    public struct Rect : IEquatable<Rect>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Rect'/> class.
        /// </summary>
        public static readonly Rect Empty;

        private double x; // Do not rename (binary serialization)
        private double y; // Do not rename (binary serialization)
        private double width; // Do not rename (binary serialization)
        private double height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Rect'/> class with the
        /// specified location
        /// and size.
        /// </summary>
        public Rect(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Rect'/> class with the
        /// specified location
        /// and size.
        /// </summary>
        public Rect(Point location, Size size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Rect'/> struct from the specified
        /// <see cref="Vector4"/>.
        /// </summary>
        public Rect(Vector4 vector)
        {
            x = vector.X;
            y = vector.Y;
            width = vector.Z;
            height = vector.W;
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular
        /// region represented by this <see cref='Rect'/>.
        /// </summary>
        [Browsable(false)]
        public Point Location
        {
            readonly get => new(x, y);
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='Rect'/>.
        /// </summary>
        [Browsable(false)]
        public Size Size
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
        /// <see cref='Rect'/>.
        /// </summary>
        public double X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular
        /// region defined by this
        /// <see cref='Rect'/>.
        /// </summary>
        public double Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this
        /// <see cref='Rect'/>.
        /// </summary>
        public double Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Gets or sets the height of the rectangular region defined by this
        /// <see cref='Rect'/>.
        /// </summary>
        public double Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Left => x;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Top => y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Right => x + width;

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Bottom => y + height;

        /// <summary>
        /// Tests whether this <see cref='Rect'/> has a <see cref='Width'/>
        /// or a <see cref='Height'/> less than or equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => (width <= 0) || (height <= 0);

        /// <summary>
        /// Tests whether this <see cref='Rect'/> has all properties equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsZero => (width == 0) && (height == 0) && (x == 0) && (y == 0);

        /// <summary>
        /// TopLeft Property - This is a read-only alias for the Point which is at X, Y
        /// If this is the empty rectangle, the value will be positive infinity, positive infinity.
        /// </summary>
        [Browsable(false)]
        public readonly Point TopLeft
        {
            get
            {
                return new Point(x, y);
            }
        }

        /// <summary>
        /// TopRight Property - This is a read-only alias for the Point which is at X + Width, Y
        /// If this is the empty rectangle, the value will be negative infinity, positive infinity.
        /// </summary>
        [Browsable(false)]
        public readonly Point TopRight
        {
            get
            {
                return new Point(Right, y);
            }
        }

        /// <summary>
        /// BottomLeft Property - This is a read-only alias for the Point which is at X, Y + Height
        /// If this is the empty rectangle, the value will be positive infinity, negative infinity.
        /// </summary>
        [Browsable(false)]
        public readonly Point BottomLeft
        {
            get
            {
                return new Point(x, Bottom);
            }
        }

        /// <summary>
        /// BottomRight Property - This is a read-only alias for the Point
        /// which is at X + Width, Y + Height
        /// If this is the empty rectangle, the value will be negative infinity, negative infinity.
        /// </summary>
        [Browsable(false)]
        public readonly Point BottomRight
        {
            get
            {
                return new Point(Right, Bottom);
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
        /// Gets the center point of this <see cref="Rect"/>.
        /// </summary>
        [Browsable(false)]
        public readonly Point Center => Location + (Size / 2);

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
        /// <see cref="Rect"/>.
        /// </summary>
        public static explicit operator Rect(Vector4 vector) => new(vector);

        /// <summary>
        /// Implicit operator convertion from tuple with four <see cref="double"/> values
        /// to <see cref="Rect"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        public static implicit operator Rect((double, double, double, double) d) =>
            new(d.Item1, d.Item2, d.Item3, d.Item4);

        /// <summary>
        /// Converts the specified <see cref='Drawing.Int32Rect'/> to a
        /// <see cref='Rect'/>.
        /// </summary>
        public static implicit operator Rect(Int32Rect r) => new(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Tests whether two <see cref='Rect'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(Rect left, Rect right) => !(left == right);

        /// <summary>
        /// Tests whether two <see cref='Rect'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(Rect left, Rect right) =>
            left.x == right.x && left.y == right.y && left.width == right.width
            && left.height == right.height;

        /// <summary>
        /// Creates a <see cref='Rect'/> that is inflated by the specified amount.
        /// </summary>
        public static Rect Inflate(Rect rect, double x, double y)
        {
            Rect r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Creates a new <see cref="Rect"/> from a centerpoint and size.
        /// </summary>
        public static Rect FromCenter(Point center, Size size)
        {
            return new Rect(
                center.X - (size.Width / 2),
                center.Y - (size.Height / 2),
                size.Width,
                size.Height);
        }

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static Rect Union(Rect a, Rect b)
        {
            double x1 = Math.Min(a.x, b.x);
            double x2 = Math.Max(a.x + a.width, b.x + b.width);
            double y1 = Math.Min(a.y, b.y);
            double y2 = Math.Max(a.y + a.height, b.y + b.height);

            return new Rect(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there
        /// is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static Rect Intersect(Rect a, Rect b)
        {
            double x1 = Math.Max(a.x, b.x);
            double x2 = Math.Min(a.x + a.width, b.x + b.width);
            double y1 = Math.Max(a.y, b.Y);
            double y2 = Math.Min(a.y + a.height, b.y + b.height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Rect(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source">string with Rect data.</param>
        /// </summary>
        public static Rect Parse(string source)
        {
            IFormatProvider formatProvider = TypeConverterHelper.InvariantEnglishUS;

            TokenizerHelper th = new(source, formatProvider);

            Rect value;

            string firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty")
            {
                value = Empty;
            }
            else
            {
                value = new Rect(
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
        /// Creates a new <see cref='Rect'/> with the specified location and size.
        /// </summary>
        public static Rect FromLTRB(double left, double top, double right, double bottom) =>
            new(left, top, right - left, bottom - top);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined
        /// by this
        /// <see cref='Rect'/> .
        /// </summary>
        public readonly bool Contains(double x, double y) =>
            X <= x && x < X + Width && Y <= y && y < Y + Height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined
        /// by this
        /// <see cref='Rect'/> .
        /// </summary>
        public readonly bool Contains(Point pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Gets percentage of <see cref="Width"/>.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        public readonly double PercentOfWidth(double percent) => MathUtils.PercentOf(width, percent);

        /// <summary>
        /// Gets percentage of minimal size. Chooses minimum of <see cref="Width"/> and
        /// <see cref="Height"/>) and calculates <paramref name="percent"/> of this value.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        public readonly double PercentOfMinSize(double percent) =>
            MathUtils.PercentOf(MinWidthHeight, percent);

        /// <summary>
        /// Gets percentage of <see cref="Height"/>.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        public readonly double PercentOfHeight(double percent) => MathUtils.PercentOf(height, percent);

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='Rect'/> with the
        /// same location and
        /// size of this <see cref='Rect'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Rect rect && Equals(rect);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        public readonly bool Equals(Rect other) => this == other;

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is
        /// entirely contained within
        /// the rectangular region represented by this <see cref='Rect'/> .
        /// </summary>
        public readonly bool Contains(Rect rect) =>
            (x <= rect.x) && (rect.x + rect.width <= x + width)
            && (y <= rect.y) && (rect.y + rect.height <= y + height);

        /// <summary>
        /// Gets the hash code for this <see cref='Rect'/>.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(x, y, width, height);

        /// <summary>
        /// Inflates this <see cref='Rect'/> by the specified amount.
        /// </summary>
        public void Inflate(double dx, double dy)
        {
            x -= dx;
            y -= dy;
            width += 2 * dx;
            height += 2 * dy;
        }

        /// <summary>
        /// Creates a <see cref='Rect'/> that is inflated by the specified amount.
        /// </summary>
        public readonly Rect InflatedBy(double x, double y) => Inflate(this, x, y);

        /// <summary>
        /// Creates a <see cref='Rect'/> that is offset by the specified amount.
        /// </summary>
        public readonly Rect OffsetBy(double dx, double dy)
        {
            var r = this;
            r.x += dx;
            r.y += dy;
            return r;
        }

        /// <summary>
        /// Creates a <see cref='Rect'/> with the specified size.
        /// </summary>
        public readonly Rect WithSize(double width, double height)
        {
            var r = this;
            r.width = width;
            r.height = height;
            return r;
        }

        /// <summary>
        /// Creates a <see cref='Rect'/> with the specified location.
        /// </summary>
        public readonly Rect WithLocation(double x, double y)
        {
            var r = this;
            r.x = x;
            r.y = y;
            return r;
        }

        /// <summary>
        /// Inflates this <see cref='Rect'/> by the specified amount.
        /// </summary>
        public void Inflate(Size size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(Rect rect)
        {
            Rect result = Intersect(rect, this);

            x = result.x;
            y = result.y;
            width = result.width;
            height = result.height;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(Rect rect) =>
            (rect.x < x + width) && (x < rect.x + rect.width) && (rect.y < y + height)
            && (y < rect.y + rect.height);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(Point pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(double dx, double dy)
        {
            x += dx;
            y += dy;
        }

        /// <summary>
        /// Converts the <see cref='Location'/> and <see cref='Size'/>
        /// of this <see cref='Rect'/> to a human-readable string.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names =
            [
                PropNameStrings.Default.X,
                PropNameStrings.Default.Y,
                PropNameStrings.Default.Width,
                PropNameStrings.Default.Height,
            ];

            double[] values = [x, y, width, height];

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
            if (IsEmpty)
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
    }
}
