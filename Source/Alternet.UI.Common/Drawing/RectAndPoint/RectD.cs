using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Alternet.UI;
using Alternet.UI.Localization;
using Alternet.UI.Markup;

using SkiaSharp;

namespace Alternet.Drawing
{
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

        /// <summary>
        /// Initializes a new instance of the <see cref='RectD'/> class with
        /// <see cref="Coord.NaN"/> in all bounds.
        /// </summary>
        public static readonly RectD NaN = new(Coord.NaN, Coord.NaN, Coord.NaN, Coord.NaN);

        /// <summary>
        /// Initializes a new instance of the <see cref='RectD'/> with -1
        /// in all bounds.
        /// </summary>
        public static readonly RectD MinusOne
            = new(CoordD.MinusOne, CoordD.MinusOne, CoordD.MinusOne, CoordD.MinusOne);

        /// <summary>
        /// Gets or sets default <see cref="MidpointRounding"/> used in round coordinate
        /// operations.
        /// </summary>
        public static MidpointRounding DefaultMidpointRounding = MidpointRounding.AwayFromZero;

        private Coord x;
        private Coord y;
        private Coord width;
        private Coord height;

        /// <summary>
        /// Initializes a new instance of the <see cref='RectD'/> class with the
        /// specified location
        /// and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectD(Coord x, Coord y, Coord width, Coord height)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => new(x, y);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        /// <summary>
        /// Gets a value indicating whether any property is negative.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsAnyNegative
        {
            get
            {
                return x < CoordD.Empty || y < CoordD.Empty
                    || width < CoordD.Empty || height < CoordD.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether location is negative or size is negative or zero.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsLocationNegativeOrSizeEmpty
        {
            get
            {
                return x < CoordD.Empty || y < CoordD.Empty
                    || width <= CoordD.Empty || height <= CoordD.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether location is negative.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsLocationNegative
        {
            get
            {
                return x < CoordD.Empty || y < CoordD.Empty;
            }
        }

        /// <summary>
        /// Gets whether height is less or equal 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool HasEmptyHeight => height <= CoordD.Empty;

        /// <summary>
        /// Gets whether width is less or equal 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool HasEmptyWidth => width <= CoordD.Empty;

        /// <summary>
        /// Gets or sets the size of this <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public SizeD Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => new(width, height);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        public Coord X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular
        /// region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        public Coord Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => y;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        public Coord Width
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => width;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => width = value;
        }

        /// <summary>
        /// Gets or sets the height of the rectangular region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        public Coord Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => height;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => height = value;
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular
        /// region defined by this <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public Coord Left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => x;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular
        /// region defined by this <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public Coord Top
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => y;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => y = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular
        /// region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public Coord Right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => x + width;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => x = value - width;
        }

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular
        /// region defined by this
        /// <see cref='RectD'/>.
        /// </summary>
        [Browsable(false)]
        public Coord Bottom
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => y + height;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => y = value - height;
        }

        /// <summary>
        /// Gets a value indicating whether the rectangle is not empty.
        /// </summary>
        public readonly bool NotEmpty
        {
            get
            {
                return width > 0 && height > 0;
            }
        }

        /// <summary>
        /// Tests whether this <see cref='RectD'/> has a <see cref='Width'/>
        /// or a <see cref='Height'/> less than or equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool SizeIsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (width <= CoordD.Empty) || (height <= CoordD.Empty);
        }

        /// <summary>
        /// Tests whether this <see cref='RectD'/> has all properties equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsZero
        {
            get
            {
                return (width == CoordD.Empty) && (height == CoordD.Empty)
                    && (x == CoordD.Empty) && (y == CoordD.Empty);
            }
        }

        /// <summary>
        /// Tests whether this <see cref='RectD'/> has all properties equal to 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty
        {
            get
            {
                return (width == CoordD.Empty) && (height == CoordD.Empty)
                    && (x == CoordD.Empty) && (y == CoordD.Empty);
            }
        }

        /// <summary>
        /// This is a read-only alias for the point which is at (X, Y).
        /// </summary>
        [Browsable(false)]
        public readonly PointD TopLeft
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new PointD(Right, Bottom);
            }
        }

        /// <summary>
        /// Gets minimum of <see cref="Width"/> and <see cref="Height"/>.
        /// </summary>
        public readonly Coord MinWidthHeight => Math.Min(width, height);

        /// <summary>
        /// Gets maximum of <see cref="Width"/> and <see cref="Height"/>.
        /// </summary>
        public readonly Coord MaxWidthHeight => Math.Max(width, height);

        /// <summary>
        /// Gets or sets the center point of this <see cref="RectD"/>.
        /// </summary>
        [Browsable(false)]
        public PointD Center
        {
            readonly get
            {
                return Location + (Size / CoordD.Two);
            }

            set
            {
                x = value.X - (width / CoordD.Two);
                y = value.Y - (height / CoordD.Two);
            }
        }

        /// <summary>
        /// Gets center point on the top border of the rectangle.
        /// </summary>
        [Browsable(false)]
        public readonly PointD TopLineCenter
        {
            get
            {
                return (x + (width / CoordD.Two), y);
            }
        }

        /// <summary>
        /// Gets center point on the left border of the rectangle.
        /// </summary>
        [Browsable(false)]
        public readonly PointD LeftLineCenter
        {
            get
            {
                return (x, y + (height / CoordD.Two));
            }
        }

        /// <summary>
        /// Gets center point on the bottom border of the rectangle.
        /// </summary>
        [Browsable(false)]
        public readonly PointD BottomLineCenter
        {
            get
            {
                return (x + (width / CoordD.Two), Bottom);
            }
        }

        /// <summary>
        /// Converts the specified <see cref="RectD"/> to a
        /// <see cref="System.Numerics.Vector4"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4(RectD rectangle) => rectangle.ToVector4();

        /// <summary>
        /// Converts the specified <see cref="Vector2"/> to a
        /// <see cref="RectD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator RectD(Vector4 vector) => new(vector);

        /// <summary>
        /// Implicit operator conversion from tuple with four values
        /// to <see cref="RectD"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectD((Coord X, Coord Y, Coord Width, Coord Height) d) =>
            new(d.X, d.Y, d.Width, d.Height);

        /// <summary>
        /// Implicit operator conversion from <see cref="RectD"/>
        /// to <see cref="SKRect"/>.
        /// </summary>
        /// <param name="rect">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SKRect(RectD rect)
        {
            SKRect result = SKRect.Create((float)rect.Width, (float)rect.Height);
            result.Offset((float)rect.X, (float)rect.Y);
            return result;
        }

        /// <summary>
        /// Implicit operator conversion from <see cref="SKRect"/>
        /// to <see cref="RectD"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectD(SKRect value)
        {
            return new(value.Left, value.Top, value.Width, value.Height);
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> with the properties of the
        /// specified <see cref='System.Drawing.Rectangle'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectD(System.Drawing.Rectangle p)
            => new(p.X, p.Y, p.Width, p.Height);

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
        /// Implicit operator conversion from tuple (<see cref="PointD"/>, <see cref="Size"/>)
        /// to <see cref="RectD"/>.
        /// </summary>
        /// <param name="d">New rectangle value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RectD((PointD Location, SizeD Size) d)
            => new(d.Location, d.Size);

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
        public static RectD Inflate(RectD rect, Coord x, Coord y)
        {
            rect.Inflate(x, y);
            return rect;
        }

        /// <summary>
        /// Creates bounding rectangle for the circle specified using radius and its center location.
        /// </summary>
        /// <returns></returns>
        /// <param name="center">Circle center location.</param>
        /// <param name="radius">Circle radius.</param>
        public static RectD GetCircleBoundingBox(PointD center, Coord radius)
        {
            var diameter = radius * CoordD.Two;
            RectD rect = (center.X - radius, center.Y - radius, diameter, diameter);
            return rect;
        }

        /// <summary>
        /// Creates new rectangle with the specified width and height.
        /// </summary>
        /// <param name="width">Rectangle width.</param>
        /// <param name="height">Rectangle height.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD Create(Coord width, Coord height)
        {
            return new RectD(CoordD.Empty, CoordD.Empty, width, height);
        }

        /// <summary>
        /// Converts <see cref="Coord"/> coordinate to the integer value.
        /// </summary>
        /// <param name="value">Coordinate.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CoordToInt(Coord value)
        {
            int i = (int)Math.Round(value, DefaultMidpointRounding);
            return i;
        }

        /// <summary>
        /// Converts <see cref="Coord"/> coordinate to the <see cref="float"/> value.
        /// </summary>
        /// <param name="value">Coordinate.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CoordToFloat(Coord value)
        {
            return Convert.ToSingle(value);
        }

        /// <summary>
        /// Creates a new <see cref="RectD"/> from a center point and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD FromCenter(PointD center, SizeD size)
        {
            return new RectD(
                center.X - (size.Width / CoordD.Two),
                center.Y - (size.Height / CoordD.Two),
                size.Width,
                size.Height);
        }

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static RectD Union(RectD a, RectD b)
        {
            var x1 = Math.Min(a.x, b.x);
            var x2 = Math.Max(a.x + a.width, b.x + b.width);
            var y1 = Math.Min(a.y, b.y);
            var y2 = Math.Max(a.y + a.height, b.y + b.height);

            return new RectD(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there
        /// is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static RectD Intersect(RectD a, RectD b)
        {
            var x1 = Math.Max(a.x, b.x);
            var x2 = Math.Min(a.x + a.width, b.x + b.width);
            var y1 = Math.Max(a.y, b.Y);
            var y2 = Math.Min(a.y + a.height, b.y + b.height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new RectD(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Converts the specified <see cref="RectD" /> structure to a <see cref="RectI" />
        /// structure by rounding the <see cref="RectD" /> values to the next higher
        /// integer values.</summary>
        /// <param name="value">The <see cref="RectD" /> structure to be converted.</param>
        /// <returns>Returns a <see cref="RectI" />.</returns>
        /// <remarks>
        /// Ceiling operation returns the smallest integer that is greater than or equal
        /// to the specified floating-point number.
        /// </remarks>
        public static RectI Ceiling(RectD value)
        {
            return new(
                (int)Math.Ceiling(value.x),
                (int)Math.Ceiling(value.y),
                (int)Math.Ceiling(value.width),
                (int)Math.Ceiling(value.height));
        }

        /// <summary>
        /// Converts the specified <see cref="RectD" /> to a <see cref="RectI" />
        /// by truncating the <see cref="RectD" /> values.
        /// </summary>
        /// <param name="value">The <see cref="RectD" /> to be converted.</param>
        /// <returns>The truncated value of the  <see cref="RectI" />.</returns>
        public static RectI Truncate(RectD value)
        {
            return new((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
        }

        /// <summary>
        /// Converts the specified <see cref="RectD" /> to a <see cref="RectI" />
        /// by rounding the <see cref="RectD" /> values to the nearest integer values.
        /// </summary>
        /// <param name="value">The <see cref="RectD" /> to be converted.</param>
        /// <returns>The rounded integer value of the <see cref="RectI" />.</returns>
        /// <param name="rounding">The <see cref="MidpointRounding"/> to use when
        /// <see cref="Math.Round(Coord,MidpointRounding)"/> is called.</param>
        /// <remarks>
        /// Rounds a floating-point value to the nearest integer.
        /// </remarks>
        public static RectI Round(RectD value, MidpointRounding? rounding = null)
        {
            rounding ??= DefaultMidpointRounding;

            return new(
                unchecked((int)Math.Round(value.x, rounding.Value)),
                unchecked((int)Math.Round(value.y, rounding.Value)),
                unchecked((int)Math.Round(value.width, rounding.Value)),
                unchecked((int)Math.Round(value.height, rounding.Value)));
        }

        /// <summary>
        /// Returns an instance converted from the provided string using
        /// the culture "en-US".
        /// <param name="source">String with rectangle data.</param>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD Parse(string source)
        {
            return ConversionUtils.ParseFourFloats(source);
        }

        /// <summary>
        /// Creates a new <see cref='RectD'/> using
        /// (<paramref name="left"/>, <paramref name="top"/>) and
        /// (<paramref name="right"/>, <paramref name="bottom"/>) points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD FromLTRB(Coord left, Coord top, Coord right, Coord bottom) =>
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
        public readonly bool Contains(Coord ax, Coord ay) =>
            (ax >= X) && (ax < X + Width) && (ay >= Y) && (ay < Y + Height);

        /// <summary>
        /// Gets whether rectangle is not empty and contains the specified point.
        /// </summary>
        /// <param name="x">X coordinate of the point to test.</param>
        /// <param name="y">Y coordinate of the point to test.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool NotEmptyAndContains(Coord x, Coord y)
        {
            return !SizeIsEmpty && Contains(x, y);
        }

        /// <summary>
        /// Gets whether rectangle is not empty and contains the specified point.
        /// </summary>
        /// <param name="pt">Point to test.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool NotEmptyAndContains(PointD pt) => NotEmptyAndContains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined
        /// by this <see cref='RectD'/> .
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(PointD pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Gets percentage of <see cref="Width"/>.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Coord PercentOfWidth(Coord percent)
            => MathUtils.PercentOf(width, percent);

        /// <summary>
        /// Gets percentage of minimal size. Chooses minimum of <see cref="Width"/> and
        /// <see cref="Height"/>) and calculates <paramref name="percent"/> of this value.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Coord PercentOfMinSize(Coord percent) =>
            MathUtils.PercentOf(MinWidthHeight, percent);

        /// <summary>
        /// Gets percentage of <see cref="Height"/>.
        /// </summary>
        /// <param name="percent">Value from 0 to 100.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Coord PercentOfHeight(Coord percent)
            => MathUtils.PercentOf(height, percent);

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='RectD'/> with the
        /// same location and
        /// size of this <see cref='RectD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals([NotNullWhen(true)] object? obj)
            => obj is RectD rect && Equals(rect);

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
        /// Gets horizontal or vertical coordinate of the center point
        /// depending on <paramref name="isVert"/> parameter value.
        /// </summary>
        /// <param name="isVert">Defines whether to return horizontal or vertical coordinate
        /// of the center point.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Coord GetCenter(bool isVert)
        {
            if (isVert)
                return Center.Y;
            else
                return Center.X;
        }

        /// <summary>
        /// Sets horizontal or vertical coordinate of the center point
        /// depending on <paramref name="isVert"/> parameter value.
        /// </summary>
        /// <param name="isVert">Defines whether to set horizontal or vertical coordinate
        /// of the center point.</param>
        /// <param name="value">New value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCenter(bool isVert, Coord value)
        {
            if (isVert)
                Center = (Center.X, value);
            else
                Center = (value, Center.Y);
        }

        /// <summary>
        /// Sets <see cref="X"/> or <see cref="Y"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to set <see cref="X"/> or <see cref="Y"/>.</param>
        /// <param name="value">New value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLocation(bool vert, Coord value)
        {
            if (vert)
                y = value;
            else
                x = value;
        }

        /// <summary>
        /// Gets <see cref="Right"/> or <see cref="Bottom"/> depending
        /// on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to get <see cref="Right"/>
        /// or <see cref="Bottom"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Coord GetFarLocation(bool vert)
        {
            if (vert)
                return Bottom;
            else
                return Right;
        }

        /// <summary>
        /// Sets <see cref="Right"/> or <see cref="Bottom"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to set <see cref="Right"/>
        /// or <see cref="Bottom"/>.</param>
        /// <param name="value">New value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFarLocation(bool vert, Coord value)
        {
            if (vert)
                Bottom = value;
            else
                Right = value;
        }

        /// <summary>
        /// Gets <see cref="Width"/> or <see cref="Height"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to return <see cref="Width"/>
        /// or <see cref="Height"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Coord GetSize(bool vert)
        {
            if (vert)
                return height;
            else
                return width;
        }

        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector4"/> from this <see cref="RectD"/>.
        /// </summary>
        public readonly Vector4 ToVector4()
            => new(CoordToFloat(x), CoordToFloat(y), CoordToFloat(width), CoordToFloat(height));

        /// <summary>
        /// Sets <see cref="Width"/> or <see cref="Height"/> depending on <paramref name="vert"/>
        /// parameter value.
        /// </summary>
        /// <param name="vert">Defines whether to set <see cref="Width"/>
        /// or <see cref="Height"/>.</param>
        /// <returns></returns>
        /// <param name="value">New size value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSize(bool vert, Coord value)
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
        public override readonly int GetHashCode() => (x, y, width, height).GetHashCode();

        /// <summary>
        /// Inflates this <see cref='RectD'/> by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Inflate(Coord dx, Coord dy)
        {
            x -= dx;
            y -= dy;
            width += CoordD.Two * dx;
            height += CoordD.Two * dy;
        }

        /// <summary>
        /// Inflates this <see cref='RectD'/> vertically by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InflateVert(Coord dx, Coord dy)
        {
            y -= dy;
            height += CoordD.Two * dy;
        }

        /// <summary>
        /// Inflates this <see cref='RectD'/> horizontally by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InflateHorz(Coord dx)
        {
            x -= dx;
            width += CoordD.Two * dx;
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> that is inflated by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD InflatedBy(Coord x, Coord y) => Inflate(this, x, y);

        /// <summary>
        /// Creates a <see cref='RectD'/> that is inflated by 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD Inflated() => Inflate(this, 1, 1);

        /// <summary>
        /// Creates a <see cref='RectD'/> that is offset by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD OffsetBy(Coord dx, Coord dy)
        {
            var r = this;
            r.x += dx;
            r.y += dy;
            return r;
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> with the specified size and the location of this rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithSize(Coord width, Coord height)
        {
            var r = this;
            r.width = width;
            r.height = height;
            return r;
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> with the specified size and the location of this rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithSize(SizeD size)
        {
            return new(Location, size);
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> with an empty size and the location of this rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithEmptySize()
        {
            return new(Location, SizeD.Empty);
        }

        /// <summary>
        /// Returns new <see cref="RectD"/> value with ceiling of location and size.
        /// Uses <see cref="Math.Ceiling(Coord)"/> on values.
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
        public readonly RectD WithLocation(Coord x, Coord y)
        {
            var r = this;
            r.x = x;
            r.y = y;
            return r;
        }

        /// <summary>
        /// Creates a <see cref='RectD'/> with the specified location.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithLocation(PointD location)
        {
            return (location, Size);
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
            x -= CoordD.One;
            y -= CoordD.One;
            width += CoordD.Two;
            height += CoordD.Two;
        }

        /// <summary>
        /// Deflates this <see cref='RectD'/> by 1. <see cref="X"/> and <see cref="Y"/>
        /// are incremented by 1, <see cref="Width"/> and <see cref="Height"/> are
        /// decremented by 2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deflate()
        {
            x += CoordD.One;
            y += CoordD.One;
            width -= CoordD.Two;
            height -= CoordD.Two;
        }

        /// <summary>
        /// Changes this rectangle to the rectangle that represents the
        /// intersection between this rectangle and the rectangle specified
        /// in the <paramref name="rect"/> parameter.
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
                centerHorz ? r.x + ((r.width - width) / CoordD.Two) : x,
                centerVert ? r.y + ((r.height - height) / CoordD.Two) : y,
                width,
                height);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(Coord dx, Coord dy)
        {
            x += dx;
            y += dy;
        }

        /// <summary>
        /// Applies margins to this rectangle. <see cref="X"/> is incremented with
        /// <see cref="Thickness.Left"/>, <see cref="Y"/> is incremented with
        /// <see cref="Thickness.Top"/>, <see cref="Width"/> is decremented with horizontal
        /// margins (<see cref="Thickness.Horizontal"/>)
        /// and <see cref="Height"/> is decremented with
        /// vertical margins (<see cref="Thickness.Vertical"/>).
        /// </summary>
        /// <param name="margin"></param>
        public void ApplyMargin(Thickness margin)
        {
            x += margin.Left;
            y += margin.Top;
            width -= margin.Horizontal;
            height -= margin.Vertical;
        }

        /// <summary>
        /// Gets this rectangle with applied margin. Uses <see cref="ApplyMargin"/>, see it
        /// for details.
        /// </summary>
        /// <param name="margin">Margin to apply</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithMargin(Thickness margin)
        {
            RectD result = this;
            result.ApplyMargin(margin);
            return result;
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

            Coord[] values = { x, y, width, height };

            return StringUtils.ToStringWithOrWithoutNames<Coord>(names, values);
        }

        /// <summary>
        /// Gets pixel rectangle from this object using specified scaling factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectI PixelFromDip(Coord? scaleFactor = null)
        {
            return GraphicsFactory.PixelFromDip(this, scaleFactor);
        }

        /// <summary>
        /// Scales rectangle (multiplies all fields) using the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor.</param>
        public void Scale(Coord scaleFactor)
        {
            x *= scaleFactor;
            y *= scaleFactor;
            width *= scaleFactor;
            height *= scaleFactor;
        }

        /// <summary>
        /// Scales X and Y (multiplies them) using the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScaleLocation(Coord scaleFactor)
        {
            x *= scaleFactor;
            y *= scaleFactor;
        }

        /// <summary>
        /// Returns new rectangle with location and width of this rectangle and the specified height.
        /// </summary>
        /// <param name="aHeight">New height.</param>
        /// <returns>Rectangle object with the new height.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithHeight(Coord aHeight)
        {
            return new(x, y, width, aHeight);
        }

        /// <summary>
        /// Returns new rectangle with location and height of this rectangle and the specified width.
        /// </summary>
        /// <param name="aWidth">New width.</param>
        /// <returns>Rectangle object with the new width.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithWidth(Coord aWidth)
        {
            return new(x, y, aWidth, height);
        }

        /// <summary>
        /// Returns this rectangle deflated by the specified padding.
        /// </summary>
        /// <param name="padding">Specifies padding settings.</param>
        /// <returns>Rectangle object with changed size and location.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD DeflatedWithPadding(Thickness padding)
        {
            return new(
                x + padding.Left,
                y + padding.Top,
                width - padding.Horizontal,
                height - padding.Vertical);
        }

        /// <summary>
        /// Returns this rectangle inflated by the specified padding.
        /// </summary>
        /// <param name="padding">Specifies padding settings.</param>
        /// <returns>Rectangle object with changed size and location.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD InflatedWithPadding(Thickness padding)
        {
            return new(
                x - padding.Left,
                y - padding.Top,
                width + padding.Horizontal,
                height + padding.Vertical);
        }

        /// <summary>
        /// Returns new rectangle with size and y-coordinate of this rectangle
        /// and the specified x-coordinate.
        /// </summary>
        /// <param name="ax">New X position.</param>
        /// <returns>Rectangle object with the new x-coordinate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithX(Coord ax)
        {
            return new(ax, y, width, height);
        }

        /// <summary>
        /// Returns new rectangle with size and x-coordinate of this rectangle
        /// and the specified y-coordinate.
        /// </summary>
        /// <param name="ay">New Y position.</param>
        /// <returns>Rectangle object with the new y-coordinate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RectD WithY(Coord ay)
        {
            return new(x, ay, width, height);
        }

        /// <summary>
        /// Calls <see cref="SizeD.CoerceCoordFunc"/> for the location and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Coerce()
        {
            if (SizeD.CoerceCoordFunc is null)
                return;
            Coerce(SizeD.CoerceCoordFunc);
        }

        /// <summary>
        /// Calls <paramref name="coerceFunc"/> for the location and size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Coerce(Func<Coord, Coord> coerceFunc)
        {
            x = coerceFunc(x);
            y = coerceFunc(y);
            width = coerceFunc(width);
            height = coerceFunc(height);
        }

        /// <summary>
        /// Returns location of the upper-left corner of this rectangle.
        /// </summary>
        /// <returns>Point that contains location of the upper-left rectangle corner.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD LeftTop() => new(x, y);

        /// <summary>
        /// Returns location of the upper-right corner of this rectangle.
        /// </summary>
        /// <returns>Point that contains location of the upper-right rectangle corner.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD RightTop() => new(Right, y);

        /// <summary>
        /// Offsets the location of given rectangle by the specified offset.
        /// </summary>
        /// <param name="offsetX">Horizontal offset.</param>
        /// <param name="offsetY">Vertical offset.</param>
        /// <returns>Rectangle object with new location.</returns>
        public readonly RectD WithOffset(Coord offsetX, Coord offsetY)
        {
            var r = this;
            r.Offset(offsetX, offsetY);
            return r;
        }

        /// <summary>
        /// Offsets the location of given rectangle by the specified offset.
        /// </summary>
        /// <param name="offset">Horizontal and vertical offset.</param>
        /// <returns>Rectangle object with new location.</returns>
        public readonly RectD WithOffset(SizeD offset)
        {
            var r = this;
            r.Offset((PointD)offset);
            return r;
        }

        /// <summary>
        /// Returns location of the bottom-right corner of this rectangle.
        /// </summary>
        /// <returns>Point that contains location of the bottom-right rectangle corner.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD RightBottom() => new(Right, Bottom);

        /// <summary>
        /// Returns location of the bottom-left corner of this rectangle.
        /// </summary>
        /// <returns>Point that contains location of the bottom-left rectangle corner.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD LeftBottom() => new(x, Bottom);

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
    }
}