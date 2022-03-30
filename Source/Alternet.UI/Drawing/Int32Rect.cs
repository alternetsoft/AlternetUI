// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Alternet.UI;
using Alternet.UI.Markup;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Alternet.Drawing
{
    /// <summary>
    /// Stores the location and size of a rectangular region.
    /// </summary>
    [Serializable]
    //[TypeConverter("System.Drawing.RectangleConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public struct Int32Rect : IEquatable<Int32Rect>
    {
        /// <summary>
        /// Represents a <see cref="Int32Rect"/> structure with its properties left uninitialized.
        /// </summary>
        public static readonly Int32Rect Empty;

        private int x; // Do not rename (binary serialization)
        private int y; // Do not rename (binary serialization)
        private int width; // Do not rename (binary serialization)
        private int height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Int32Rect'/> class with the specified location
        /// and size.
        /// </summary>
        public Int32Rect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle class with the specified location and size.
        /// </summary>
        public Int32Rect(Int32Point location, Int32Size size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Creates a new <see cref='Drawing.Int32Rect'/> with the specified location and size.
        /// </summary>
        public static Int32Rect FromLTRB(int left, int top, int right, int bottom) =>
            new Int32Rect(left, top, unchecked(right - left), unchecked(bottom - top));

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Int32Rect data </param>
        /// </summary>
        public static Int32Rect Parse(string source)
        {
            IFormatProvider formatProvider = TypeConverterHelper.InvariantEnglishUS;

            TokenizerHelper th = new TokenizerHelper(source, formatProvider);

            Int32Rect value;

            String firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty")
            {
                value = Empty;
            }
            else
            {
                value = new Int32Rect(
                    Convert.ToInt32(firstToken, formatProvider),
                    Convert.ToInt32(th.NextTokenRequired(), formatProvider),
                    Convert.ToInt32(th.NextTokenRequired(), formatProvider),
                    Convert.ToInt32(th.NextTokenRequired(), formatProvider));
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
                                 X,
                                 Y,
                                 Width,
                                 Height);
        }


        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular region represented by this
        /// <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        [Browsable(false)]
        public Int32Point Location
        {
            readonly get => new Int32Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        [Browsable(false)]
        public Int32Size Size
        {
            readonly get => new Int32Size(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        public int X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        public int Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        public int Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        public int Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.Int32Rect'/> .
        /// </summary>
        [Browsable(false)]
        public readonly int Left => X;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Right => unchecked(X + Width);

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Drawing.Int32Rect'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Bottom => unchecked(Y + Height);

        /// <summary>
        /// Tests whether this <see cref='Drawing.Int32Rect'/> has a <see cref='Drawing.Int32Rect.Width'/>
        /// or a <see cref='Drawing.Int32Rect.Height'/> of 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => height == 0 && width == 0 && x == 0 && y == 0;

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='Drawing.Int32Rect'/> with the same location
        /// and size of this Rectangle.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Int32Rect && Equals((Int32Rect)obj);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(Int32Rect other) => this == other;

        /// <summary>
        /// Tests whether two <see cref='Drawing.Int32Rect'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(Int32Rect left, Int32Rect right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

        /// <summary>
        /// Tests whether two <see cref='Drawing.Int32Rect'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(Int32Rect left, Int32Rect right) => !(left == right);

        /// <summary>
        /// Converts a RectangleF to a Rectangle by performing a ceiling operation on all the coordinates.
        /// </summary>
        public static Int32Rect Ceiling(Rect value)
        {
            unchecked
            {
                return new Int32Rect(
                    (int)Math.Ceiling(value.X),
                    (int)Math.Ceiling(value.Y),
                    (int)Math.Ceiling(value.Width),
                    (int)Math.Ceiling(value.Height));
            }
        }

        /// <summary>
        /// Converts a RectangleF to a Rectangle by performing a truncate operation on all the coordinates.
        /// </summary>
        public static Int32Rect Truncate(Rect value)
        {
            unchecked
            {
                return new Int32Rect(
                    (int)value.X,
                    (int)value.Y,
                    (int)value.Width,
                    (int)value.Height);
            }
        }

        /// <summary>
        /// Converts a RectangleF to a Rectangle by performing a round operation on all the coordinates.
        /// </summary>
        public static Int32Rect Round(Rect value)
        {
            unchecked
            {
                return new Int32Rect(
                    (int)Math.Round(value.X),
                    (int)Math.Round(value.Y),
                    (int)Math.Round(value.Width),
                    (int)Math.Round(value.Height));
            }
        }

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Drawing.Int32Rect'/> .
        /// </summary>
        public readonly bool Contains(int x, int y) => X <= x && x < X + Width && Y <= y && y < Y + Height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Drawing.Int32Rect'/> .
        /// </summary>
        public readonly bool Contains(Int32Point pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within the
        /// rectangular region represented by this <see cref='Drawing.Int32Rect'/> .
        /// </summary>
        public readonly bool Contains(Int32Rect rect) =>
            (X <= rect.X) && (rect.X + rect.Width <= X + Width) &&
            (Y <= rect.Y) && (rect.Y + rect.Height <= Y + Height);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>
        /// Inflates this <see cref='Drawing.Int32Rect'/> by the specified amount.
        /// </summary>
        public void Inflate(int width, int height)
        {
            unchecked
            {
                X -= width;
                Y -= height;

                Width += 2 * width;
                Height += 2 * height;
            }
        }

        /// <summary>
        /// Inflates this <see cref='Drawing.Int32Rect'/> by the specified amount.
        /// </summary>
        public void Inflate(Int32Size size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates a <see cref='Drawing.Int32Rect'/> that is inflated by the specified amount.
        /// </summary>
        public static Int32Rect Inflate(Int32Rect rect, int x, int y)
        {
            Int32Rect r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(Int32Rect rect)
        {
            Int32Rect result = Intersect(rect, this);

            X = result.X;
            Y = result.Y;
            Width = result.Width;
            Height = result.Height;
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static Int32Rect Intersect(Int32Rect a, Int32Rect b)
        {
            int x1 = Math.Max(a.X, b.X);
            int x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Max(a.Y, b.Y);
            int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Int32Rect(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(Int32Rect rect) =>
            (rect.X < X + Width) && (X < rect.X + rect.Width) &&
            (rect.Y < Y + Height) && (Y < rect.Y + rect.Height);

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static Int32Rect Union(Int32Rect a, Int32Rect b)
        {
            int x1 = Math.Min(a.X, b.X);
            int x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Min(a.Y, b.Y);
            int y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new Int32Rect(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(Int32Point pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(int x, int y)
        {
            unchecked
            {
                X += x;
                Y += y;
            }
        }

        /// <summary>
        /// Converts the attributes of this <see cref='Drawing.Int32Rect'/> to a human readable string.
        /// </summary>
        public override readonly string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";
    }
}
