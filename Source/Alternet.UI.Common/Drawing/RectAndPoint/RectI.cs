using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Alternet.UI;
using Alternet.UI.Localization;
using Alternet.UI.Markup;

using SkiaSharp;

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
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct RectI : IEquatable<RectI>
    {
        /// <summary>
        /// Represents a <see cref="RectI"/> structure with its properties left uninitialized.
        /// </summary>
        public static readonly RectI Empty;

#pragma warning disable
        [FieldOffset(0)] private int x;
        [FieldOffset(4)] private int y;
        [FieldOffset(0)] private PointI location;
        [FieldOffset(0)] private ulong xy;

        [FieldOffset(8)] private int width;
        [FieldOffset(12)] private int height;
        [FieldOffset(8)] private SizeI size;
        [FieldOffset(8)] private ulong wh;
#pragma warning restore

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.RectI'/> class with the
        /// specified location
        /// and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectI(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle class with the specified
        /// location and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectI(PointI location, SizeI size)
        {
            this.location = location;
            this.size = size;
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular
        /// region represented by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        [Browsable(false)]
        public PointI Location
        {
            readonly get => location;
            set
            {
                location = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='Drawing.RectI'/>.
        /// </summary>
        [Browsable(false)]
        public SizeI Size
        {
            readonly get => size;
            set
            {
                size = value;
            }
        }

        /// <summary>
        /// Gets center point on the top border of the rectangle.
        /// </summary>
        [Browsable(false)]
        public readonly PointI TopLineCenter
        {
            get
            {
                return (x + (width / 2), y);
            }
        }

        /// <summary>
        /// Gets center point on the bottom border of the rectangle.
        /// </summary>
        [Browsable(false)]
        public readonly PointI BottomLineCenter
        {
            get
            {
                return (x + (width / 2), Bottom);
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
        /// defined by this <see cref='Drawing.RectI'/>.
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
        /// Gets the x-coordinate of the upper-left corner of the rectangular
        /// region defined by this
        /// <see cref='Drawing.RectI'/> .
        /// </summary>
        [Browsable(false)]
        public readonly int Left => x;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular
        /// region defined by this
        /// <see cref='Drawing.RectI'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Top => y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular
        /// region defined by this <see cref='RectI'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Right => unchecked(x + width);

        /// <summary>
        /// This is a read-only alias for the point which is at (X, Y).
        /// </summary>
        [Browsable(false)]
        public readonly PointI TopLeft
        {
            get
            {
                return location;
            }
        }

        /// <summary>
        /// Gets the point which is at (X + Width, Y).
        /// </summary>
        [Browsable(false)]
        public readonly PointI TopRight
        {
            get
            {
                return new(Right, y);
            }
        }

        /// <summary>
        /// Gets the point which is at (X, Y + Height).
        /// </summary>
        [Browsable(false)]
        public readonly PointI BottomLeft
        {
            get
            {
                return new(x, Bottom);
            }
        }

        /// <summary>
        /// Gets the point which is at (X + Width, Y + Height).
        /// </summary>
        [Browsable(false)]
        public readonly PointI BottomRight
        {
            get
            {
                return new(Right, Bottom);
            }
        }

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular
        /// region defined by this <see cref='RectI'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Bottom => unchecked(y + height);

        /// <summary>
        /// Tests whether this <see cref='RectI'/> has all properties equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => xy == 0UL && wh == 0UL;

        /// <summary>
        /// Tests whether this <see cref='RectI'/> has a <see cref='Width'/>
        /// or a <see cref='Height'/> less than or equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool SizeIsEmpty => (width <= 0) || (height <= 0);

        /// <summary>
        /// Gets or sets the center point of this <see cref="RectI"/>.
        /// </summary>
        [Browsable(false)]
        public PointI Center
        {
            readonly get
            {
                return Location + (Size / 2);
            }

            set
            {
                x = value.X - (width / 2);
                y = value.Y - (height / 2);
            }
        }

        /// <summary>
        /// Implicit operator convertion from tuple with four <see cref="int"/> values
        /// to <see cref="RectI"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectI((int, int, int, int) d) =>
            new(d.Item1, d.Item2, d.Item3, d.Item4);

        /// <summary>
        /// Implicit operator convertion from tuple (<see cref="PointI"/>, <see cref="SizeI"/>)
        /// to <see cref="RectI"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectI((PointI, SizeI) d) => new(d.Item1, d.Item2);

        /// <summary>
        /// Implicit operator convertion from <see cref="RectI"/> to <see cref="SKRectI"/>.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SKRectI(RectI rect)
        {
            SKRectI result = SKRectI.Create(rect.Width, rect.Height);
            result.Offset(rect.X, rect.Y);
            return result;
        }

        /// <summary>
        /// Implicit operator convertion from <see cref="SKRectI"/> to <see cref="RectI"/>.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectI(SKRectI rect)
        {
            return new(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        /// <summary>
        /// Creates a <see cref='System.Drawing.Rectangle'/> with the coordinates of the
        /// specified <see cref='RectI'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Rectangle(RectI p) =>
            new(p.x, p.y, p.width, p.height);

        /// <summary>
        /// Creates a <see cref='RectI'/> with the coordinates of the
        /// specified <see cref='System.Drawing.Rectangle'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectI(System.Drawing.Rectangle p) =>
            new(p.X, p.Y, p.Width, p.Height);

        /// <summary>
        /// Tests whether two <see cref='Drawing.RectI'/> objects have equal location and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RectI left, RectI right) =>
            left.xy == right.xy && left.wh == right.wh;

        /// <summary>
        /// Tests whether two <see cref='Drawing.RectI'/> objects differ in location or size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RectI left, RectI right) => !(left == right);

        /// <summary>
        /// Creates a new <see cref='RectI'/> using
        /// (<paramref name="left"/>, <paramref name="top"/>) and
        /// (<paramref name="right"/>, <paramref name="bottom"/>) points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectI FromLTRB(int left, int top, int right, int bottom) =>
            new(left, top, unchecked(right - left), unchecked(bottom - top));

        /// <summary>
        /// Creates a new <see cref='RectI'/> specified by two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectI FromLTRB(PointI leftTop, PointI rightBottom) =>
            new(
                leftTop.X,
                leftTop.Y,
                rightBottom.X - leftTop.X,
                rightBottom.Y - leftTop.Y);

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Int32Rect data </param>
        /// </summary>
        public static RectI Parse(string source)
        {
            IFormatProvider formatProvider = App.InvariantEnglishUS;

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
        /// Creates new rectangle with the specified width and height.
        /// </summary>
        /// <param name="width">Rectangle width.</param>
        /// <param name="height">Rectangle height.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectI Create(int width, int height)
        {
            return new(0, 0, width, height);
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
            int x1 = Math.Max(a.x, b.x);
            int x2 = Math.Min(a.x + a.width, b.x + b.width);
            int y1 = Math.Max(a.y, b.y);
            int y2 = Math.Min(a.y + a.height, b.y + b.height);

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
            int x1 = Math.Min(a.x, b.x);
            int x2 = Math.Max(a.x + a.width, b.x + b.width);
            int y1 = Math.Min(a.y, b.y);
            int y2 = Math.Max(a.y + a.height, b.y + b.height);

            return new RectI(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Creates a <see cref='Drawing.RectI'/> that is inflated by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is RectI rect && Equals(rect);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(RectI other) => this == other;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region
        /// defined by this <see cref='RectI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(int px, int py) => (x <= px) && (px < x + width)
            && (y <= py) && (py < y + height);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined
        /// by this <see cref='RectI'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(PointI pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is
        /// entirely contained within the
        /// rectangular region represented by this <see cref='Drawing.RectI'/> .
        /// </summary>
        public readonly bool Contains(RectI rect) =>
            (x <= rect.x) && (rect.x + rect.width <= x + width) &&
            (y <= rect.y) && (rect.y + rect.height <= y + height);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.Combine(x, y, width, height);

        /// <summary>
        /// Inflates this <see cref='RectI'/> by the specified amount.
        /// </summary>
        public void Inflate(int nwidth, int nheight)
        {
            unchecked
            {
                x -= nwidth;
                y -= nheight;

                width += 2 * nwidth;
                height += 2 * nheight;
            }
        }

        /// <summary>
        /// Inflates this <see cref='Drawing.RectI'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Inflate(SizeI size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(RectI rect)
        {
            RectI result = Intersect(rect, this);

            x = result.x;
            y = result.y;
            width = result.width;
            height = result.height;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(RectI rect) =>
            (rect.x < x + width) && (x < rect.x + rect.width) &&
            (rect.y < y + height) && (y < rect.y + rect.height);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(PointI pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(int px, int py)
        {
            unchecked
            {
                x += px;
                y += py;
            }
        }

        /// <summary>
        /// Converts the attributes of this <see cref='Drawing.RectI'/> to a human
        /// readable string.
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

            int[] values = { x, y, width, height };

            return StringUtils.ToStringWithOrWithoutNames<int>(names, values);
        }

        /// <summary>
        /// Converts this rectangle to rectangle with device-indepdenent units using
        /// the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor. Optional. If not specified, the default
        /// scale factor is used for the convertion.</param>
        /// <returns></returns>
        public readonly RectD PixelToDip(Coord? scaleFactor = null)
        {
            return GraphicsFactory.PixelToDip(this, scaleFactor);
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
        public readonly string ConvertToString(string format, IFormatProvider provider)
        {
            if (IsEmpty)
                return "Empty";

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

        /// <summary>
        /// Returns new rectangle with location and width of this rectangle and the specified height.
        /// </summary>
        /// <param name="aheight">New height.</param>
        /// <returns>Rectangle object with the new height.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectI WithHeight(int aheight)
        {
            return new(x, y, width, aheight);
        }

        /// <summary>
        /// Returns new rectangle with location and height of this rectangle and the specified width.
        /// </summary>
        /// <param name="awidth">New width.</param>
        /// <returns>Rectangle object with the new width.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectI WithWidth(int awidth)
        {
            return new(x, y, awidth, height);
        }

        /// <summary>
        /// Returns new rectangle with location of this rectangle and the specified size.
        /// </summary>
        /// <param name="newSize">New size.</param>
        /// <returns>Rectangle object with the new size.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectI WithSize(SizeI newSize)
        {
            return new(x, y, newSize.Width, newSize.Height);
        }

        /// <summary>
        /// Returns new rectangle with size of this rectangle and the specified location.
        /// </summary>
        /// <param name="newLocation">New location.</param>
        /// <returns>Rectangle object with the new location.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectI WithLocation(PointI newLocation)
        {
            return new(newLocation.X, newLocation.Y, width, height);
        }

        /// <summary>
        /// Returns new rectangle with size and y-coordinate of this rectangle and the specified x-coordinate.
        /// </summary>
        /// <param name="ax">New X position.</param>
        /// <returns>Rectangle object with the new x-coordinate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectI WithX(int ax)
        {
            return new(ax, y, width, height);
        }

        /// <summary>
        /// Returns new rectangle with size and x-coordinate of this rectangle and the specified y-coordinate.
        /// </summary>
        /// <param name="ay">New Y position.</param>
        /// <returns>Rectangle object with the new y-coordinate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectI WithY(int ay)
        {
            return new(x, ay, width, height);
        }
    }
}
