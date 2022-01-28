// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Alternet.UI;
using Alternet.UI.Markup;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Alternet.Drawing
{
    /// <summary>
    /// Stores the location and size of a rectangular region.
    /// </summary>
    [Serializable]
    public struct Rect : IEquatable<Rect>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Rect'/> class.
        /// </summary>
        public static readonly Rect Empty;

        private double x; // Do not rename (binary serialization)
        private double y; // Do not rename (binary serialization)
        private double width; // Do not rename (binary serialization)
        private double height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Rect'/> class with the specified location
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
        /// Initializes a new instance of the <see cref='Drawing.Rect'/> class with the specified location
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
        /// Initializes a new instance of the <see cref='Drawing.Rect'/> struct from the specified
        /// <see cref="System.Numerics.Vector4"/>.
        /// </summary>
        public Rect(Vector4 vector)
        {
            x = vector.X;
            y = vector.Y;
            width = vector.Z;
            height = vector.W;
        }

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Rect data </param>
        /// </summary>
        public static Rect Parse(string source)
        {
            IFormatProvider formatProvider = TypeConverterHelper.InvariantEnglishUS;

            TokenizerHelper th = new TokenizerHelper(source, formatProvider);

            Rect value;

            String firstToken = th.NextTokenRequired();

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
        /// Creates a string representation of this object based on the format string
        /// and IFormatProvider passed in.
        /// If the provider is null, the CurrentCulture is used.
        /// See the documentation for IFormattable for more information.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        internal string ConvertToString(string format, IFormatProvider provider)
        {
            if (IsEmpty)
            {
                return "Empty";
            }

            // Helper to get the numeric list separator for a given culture.
            char separator = TokenizerHelper.GetNumericListSeparator(provider);
            return String.Format(provider,
                                 "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}",
                                 separator,
                                 x,
                                 y,
                                 width,
                                 height);
        }

        /* TODO: uncommment when Double System.Numerics is availble. See https://github.com/dotnet/runtime/issues/24168
        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector4"/> from this <see cref="Drawing.Rect"/>.
        /// </summary>
        public Vector4 ToVector4() => new Vector4(x, y, width, height);

        /// <summary>
        /// Converts the specified <see cref="Drawing.Rect"/> to a <see cref="System.Numerics.Vector4"/>.
        /// </summary>
        public static explicit operator Vector4(Rect rectangle) => rectangle.ToVector4();*/

        /// <summary>
        /// Converts the specified <see cref="System.Numerics.Vector2"/> to a <see cref="Drawing.Rect"/>.
        /// </summary>
        public static explicit operator Rect(Vector4 vector) => new Rect(vector);

        /// <summary>
        /// Creates a new <see cref='Drawing.Rect'/> with the specified location and size.
        /// </summary>
        public static Rect FromLTRB(double left, double top, double right, double bottom) =>
            new Rect(left, top, right - left, bottom - top);

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular region represented by this
        /// <see cref='Drawing.Rect'/>.
        /// </summary>
        [Browsable(false)]
        public Point Location
        {
            readonly get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='Drawing.Rect'/>.
        /// </summary>
        [Browsable(false)]
        public Size Size
        {
            readonly get => new Size(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.Rect'/>.
        /// </summary>
        public double X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.Rect'/>.
        /// </summary>
        public double Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref='Drawing.Rect'/>.
        /// </summary>
        public double Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Gets or sets the height of the rectangular region defined by this <see cref='Drawing.Rect'/>.
        /// </summary>
        public double Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.Rect'/> .
        /// </summary>
        [Browsable(false)]
        public readonly double Left => X;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Drawing.Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Right => X + Width;

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Drawing.Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly double Bottom => Y + Height;

        /// <summary>
        /// Tests whether this <see cref='Drawing.Rect'/> has a <see cref='Drawing.Rect.Width'/> or a <see cref='Drawing.Rect.Height'/> of 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => (Width <= 0) || (Height <= 0);

        /// <summary>
        /// TopLeft Property - This is a read-only alias for the Point which is at X, Y
        /// If this is the empty rectangle, the value will be positive infinity, positive infinity.
        /// </summary>
        public Point TopLeft
        {
            get
            {
                return new Point(Left, Top);
            }
        }

        /// <summary>
        /// TopRight Property - This is a read-only alias for the Point which is at X + Width, Y
        /// If this is the empty rectangle, the value will be negative infinity, positive infinity.
        /// </summary>
        public Point TopRight
        {
            get
            {
                return new Point(Right, Top);
            }
        }

        /// <summary>
        /// BottomLeft Property - This is a read-only alias for the Point which is at X, Y + Height
        /// If this is the empty rectangle, the value will be positive infinity, negative infinity.
        /// </summary>
        public Point BottomLeft
        {
            get
            {
                return new Point(Left, Bottom);
            }
        }

        /// <summary>
        /// BottomRight Property - This is a read-only alias for the Point which is at X + Width, Y + Height
        /// If this is the empty rectangle, the value will be negative infinity, negative infinity.
        /// </summary>
        public Point BottomRight
        {
            get
            {
                return new Point(Right, Bottom);
            }
        }

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='Drawing.Rect'/> with the same location and
        /// size of this <see cref='Drawing.Rect'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Rect && Equals((Rect)obj);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(Rect other) => this == other;

        /// <summary>
        /// Tests whether two <see cref='Drawing.Rect'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(Rect left, Rect right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

        /// <summary>
        /// Tests whether two <see cref='Drawing.Rect'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(Rect left, Rect right) => !(left == right);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Drawing.Rect'/> .
        /// </summary>
        public readonly bool Contains(double x, double y) => X <= x && x < X + Width && Y <= y && y < Y + Height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Drawing.Rect'/> .
        /// </summary>
        public readonly bool Contains(Point pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within
        /// the rectangular region represented by this <see cref='Drawing.Rect'/> .
        /// </summary>
        public readonly bool Contains(Rect rect) =>
            (X <= rect.X) && (rect.X + rect.Width <= X + Width) && (Y <= rect.Y) && (rect.Y + rect.Height <= Y + Height);

        /// <summary>
        /// Gets the hash code for this <see cref='Drawing.Rect'/>.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>
        /// Inflates this <see cref='Drawing.Rect'/> by the specified amount.
        /// </summary>
        public void Inflate(double x, double y)
        {
            X -= x;
            Y -= y;
            Width += 2 * x;
            Height += 2 * y;
        }

        /// <summary>
        /// Inflates this <see cref='Drawing.Rect'/> by the specified amount.
        /// </summary>
        public void Inflate(Size size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates a <see cref='Drawing.Rect'/> that is inflated by the specified amount.
        /// </summary>
        public static Rect Inflate(Rect rect, double x, double y)
        {
            Rect r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(Rect rect)
        {
            Rect result = Intersect(rect, this);

            X = result.X;
            Y = result.Y;
            Width = result.Width;
            Height = result.Height;
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static Rect Intersect(Rect a, Rect b)
        {
            double x1 = Math.Max(a.X, b.X);
            double x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            double y1 = Math.Max(a.Y, b.Y);
            double y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Rect(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(Rect rect) =>
            (rect.X < X + Width) && (X < rect.X + rect.Width) && (rect.Y < Y + Height) && (Y < rect.Y + rect.Height);

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static Rect Union(Rect a, Rect b)
        {
            double x1 = Math.Min(a.X, b.X);
            double x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            double y1 = Math.Min(a.Y, b.Y);
            double y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new Rect(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(Point pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(double x, double y)
        {
            X += x;
            Y += y;
        }

        /// <summary>
        /// Converts the specified <see cref='Drawing.Int32Rect'/> to a
        /// <see cref='Drawing.Rect'/>.
        /// </summary>
        public static implicit operator Rect(Int32Rect r) => new Rect(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Converts the <see cref='Drawing.Rect.Location'/> and <see cref='Drawing.Rect.Size'/>
        /// of this <see cref='Drawing.Rect'/> to a human-readable string.
        /// </summary>
        public override readonly string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";
    }
}
