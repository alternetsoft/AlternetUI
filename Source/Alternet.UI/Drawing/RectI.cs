// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
    public struct RectI : IEquatable<RectI>
    {
        /// <summary>
        /// Represents a <see cref="RectI"/> structure with its properties left uninitialized.
        /// </summary>
        public static readonly RectI Empty;

        private int x; // Do not rename (binary serialization)
        private int y; // Do not rename (binary serialization)
        private int width; // Do not rename (binary serialization)
        private int height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.RectI'/> class with the
        /// specified location
        /// and size.
        /// </summary>
        public RectI(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle class with the specified location and size.
        /// </summary>
        public RectI(PointI location, SizeI size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular
        /// region represented by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        [Browsable(false)]
        public PointI Location
        {
            readonly get => new(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='Drawing.RectI'/>.
        /// </summary>
        [Browsable(false)]
        public SizeI Size
        {
            readonly get => new(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region
        /// defined by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        public int X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region
        /// defined by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        public int Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        public int Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        public int Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Gets number of pixels (Width * Height).
        /// </summary>
        [Browsable(false)]
        public readonly int PixelCount => width * height;

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.RectI'/> .
        /// </summary>
        [Browsable(false)]
        public readonly int Left => X;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Right => unchecked(X + Width);

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Bottom => unchecked(Y + Height);

        /// <summary>
        /// Tests whether this <see cref='Drawing.RectI'/> has
        /// a <see cref='Drawing.RectI.Width'/>
        /// or a <see cref='Drawing.RectI.Height'/> of 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => height == 0 && width == 0 && x == 0 && y == 0;

        /// <summary>
        /// Implicit operator convertion from tuple with four <see cref="int"/> values
        /// to <see cref="RectI"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        public static implicit operator RectI((int, int, int, int) d) =>
            new(d.Item1, d.Item2, d.Item3, d.Item4);

        /// <summary>
        /// Implicit operator convertion from tuple (<see cref="PointI"/>, <see cref="SizeI"/>)
        /// to <see cref="RectI"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        public static implicit operator RectI((PointI, SizeI) d) => new(d.Item1, d.Item2);

        /// <summary>
        /// Creates a <see cref='System.Drawing.Rectangle'/> with the coordinates of the
        /// specified <see cref='RectI'/>
        /// </summary>
        public static implicit operator System.Drawing.Rectangle(RectI p) => new(p.X, p.Y, p.Width, p.Height);

        /// <summary>
        /// Creates a <see cref='RectI'/> with the coordinates of the
        /// specified <see cref='System.Drawing.Rectangle'/>
        /// </summary>
        public static implicit operator RectI(System.Drawing.Rectangle p) => new(p.X, p.Y, p.Width, p.Height);

        /// <summary>
        /// Tests whether two <see cref='Drawing.RectI'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(RectI left, RectI right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width
            && left.Height == right.Height;

        /// <summary>
        /// Tests whether two <see cref='Drawing.RectI'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(RectI left, RectI right) => !(left == right);

        /// <summary>
        /// Creates a new <see cref='Drawing.RectI'/> with the specified location and size.
        /// </summary>
        public static RectI FromLTRB(int left, int top, int right, int bottom) =>
            new(left, top, unchecked(right - left), unchecked(bottom - top));

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Int32Rect data </param>
        /// </summary>
        public static RectI Parse(string source)
        {
            IFormatProvider formatProvider = TypeConverterHelper.InvariantEnglishUS;

            TokenizerHelper th = new(source, formatProvider);

            RectI value;

            string firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty")
            {
                value = Empty;
            }
            else
            {
                value = new RectI(
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
        /// Converts a RectangleF to a Rectangle by performing a ceiling operation on all
        /// the coordinates.
        /// </summary>
        public static RectI Ceiling(RectD value)
        {
            unchecked
            {
                return new RectI(
                    (int)Math.Ceiling(value.X),
                    (int)Math.Ceiling(value.Y),
                    (int)Math.Ceiling(value.Width),
                    (int)Math.Ceiling(value.Height));
            }
        }

        /// <summary>
        /// Converts a RectangleF to a Rectangle by performing a truncate operation on all
        /// the coordinates.
        /// </summary>
        public static RectI Truncate(RectD value)
        {
            unchecked
            {
                return new RectI(
                    (int)value.X,
                    (int)value.Y,
                    (int)value.Width,
                    (int)value.Height);
            }
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there is
        /// no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static RectI Intersect(RectI a, RectI b)
        {
            int x1 = Math.Max(a.X, b.X);
            int x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Max(a.Y, b.Y);
            int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new RectI(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static RectI Union(RectI a, RectI b)
        {
            int x1 = Math.Min(a.X, b.X);
            int x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Min(a.Y, b.Y);
            int y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new RectI(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Creates a <see cref='Drawing.RectI'/> that is inflated by the specified amount.
        /// </summary>
        public static RectI Inflate(RectI rect, int x, int y)
        {
            RectI r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Converts a RectangleF to a Rectangle by performing a round operation on all
        /// the coordinates.
        /// </summary>
        public static RectI Round(RectD value)
        {
            unchecked
            {
                return new RectI(
                    (int)Math.Round(value.X),
                    (int)Math.Round(value.Y),
                    (int)Math.Round(value.Width),
                    (int)Math.Round(value.Height));
            }
        }

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='Drawing.RectI'/> with
        /// the same location
        /// and size of this Rectangle.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is RectI rect && Equals(rect);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        public readonly bool Equals(RectI other) => this == other;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region
        /// defined by this
        /// <see cref='Drawing.RectI'/> .
        /// </summary>
        public readonly bool Contains(int x, int y) => X <= x && x < X + Width
            && Y <= y && y < Y + Height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined
        /// by this
        /// <see cref='Drawing.RectI'/> .
        /// </summary>
        public readonly bool Contains(PointI pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is
        /// entirely contained within the
        /// rectangular region represented by this <see cref='Drawing.RectI'/> .
        /// </summary>
        public readonly bool Contains(RectI rect) =>
            (X <= rect.X) && (rect.X + rect.Width <= X + Width) &&
            (Y <= rect.Y) && (rect.Y + rect.Height <= Y + Height);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>
        /// Inflates this <see cref='Drawing.RectI'/> by the specified amount.
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
        /// Inflates this <see cref='Drawing.RectI'/> by the specified amount.
        /// </summary>
        public void Inflate(SizeI size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(RectI rect)
        {
            RectI result = Intersect(rect, this);

            X = result.X;
            Y = result.Y;
            Width = result.Width;
            Height = result.Height;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(RectI rect) =>
            (rect.X < X + Width) && (X < rect.X + rect.Width) &&
            (rect.Y < Y + Height) && (Y < rect.Y + rect.Height);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(PointI pos) => Offset(pos.X, pos.Y);

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
        /// Converts the attributes of this <see cref='Drawing.RectI'/> to a human
        /// readable string.
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

            int[] values = [x, y, width, height];

            return StringUtils.ToString<int>(names, values);
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
                X,
                Y,
                Width,
                Height);
        }
    }
}
