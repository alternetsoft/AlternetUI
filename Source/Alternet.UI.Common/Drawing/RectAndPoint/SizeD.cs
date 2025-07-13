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
        Please do not remove StructLayout(LayoutKind.Sequential) attribute.
        Also do not change order of the fields.
    */

    /// <summary>
    /// Represents the size of a rectangular region with an ordered pair of width
    /// and height.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SizeD : IEquatable<SizeD>
    {
        /// <summary>
        /// Gets <see cref="SizeD"/> with zero width and height.
        /// </summary>
        public static readonly SizeD Empty;

        /// <summary>
        /// Gets <see cref="SizeD"/> with width and height equal to -1.
        /// </summary>
        public static readonly SizeD Default = new(CoordD.MinusOne, CoordD.MinusOne);

        /// <summary>
        /// Gets <see cref="SizeD"/> with width and height equal to -1.
        /// </summary>
        public static readonly SizeD MinusOne = new(CoordD.MinusOne, CoordD.MinusOne);

        /// <summary>
        /// Gets <see cref="SizeD"/> with width and height equal to 1.
        /// </summary>
        public static readonly SizeD One = new(CoordD.One, CoordD.One);

        /// <summary>
        /// Gets <see cref="SizeD"/> with width and height equal
        /// to <see cref="Coord.PositiveInfinity"/>.
        /// </summary>
        public static readonly SizeD PositiveInfinity
            = new(Coord.PositiveInfinity, Coord.PositiveInfinity);

        /// <summary>
        /// Gets <see cref="SizeD"/> with width and height equal to
        /// (<see cref="Coord.NaN"/>, <see cref="Coord.NaN"/>).
        /// </summary>
        public static readonly SizeD NaN = new(Coord.NaN, Coord.NaN);

        /// <summary>
        /// Gets or sets coerce function used in <see cref="Coerce()"/> method.
        /// Default is Null. You can assign here for example <see cref="Math.Ceiling(Coord)"/>.
        /// </summary>
        public static Func<Coord, Coord>? CoerceCoordFunc = null;

        private Coord width; // Do not rename (binary serialization)
        private Coord height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.SizeD'/> class
        /// from the specified
        /// existing <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD(SizeD size)
        {
            width = size.width;
            height = size.height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.SizeD'/> class
        /// from the specified
        /// <see cref='Drawing.PointD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD(PointD pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.SizeD'/> struct
        /// from the specified
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD(Vector2 vector)
        {
            width = vector.X;
            height = vector.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.SizeD'/> class
        /// from the specified dimensions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD(Coord width, Coord height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeD"/> struct.
        /// </summary>
        /// <param name="widthAndHeight"><see cref="Width"/>
        /// and <see cref="Height"/> values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD(Coord widthAndHeight)
        {
            this.width = widthAndHeight;
            this.height = widthAndHeight;
        }

        /// <summary>
        /// Gets minimal of width and height.
        /// </summary>
        [Browsable(false)]
        public readonly Coord MinWidthHeight
        {
            get
            {
                return Math.Min(width, height);
            }
        }

        /// <summary>
        /// Gets maximal of width and height.
        /// </summary>
        [Browsable(false)]
        public readonly Coord MaxWidthHeight
        {
            get
            {
                return Math.Max(width, height);
            }
        }

        /// <summary>
        /// Gets diagonal of the rectangle with height and width specified in this object.
        /// </summary>
        [Browsable(false)]
        public readonly Coord Diagonal
        {
            get
            {
                var result = Math.Pow(Width, CoordD.Two) + Math.Pow(Height, CoordD.Two);
                result = Math.Sqrt(result);
                return result;
            }
        }

        /// <summary>
        /// Tests whether both <see cref='Width'/> and <see cref="Height"/> are not numbers
        /// (equal to <see cref="Coord.NaN"/>).
        /// </summary>
        [Browsable(false)]
        public readonly bool IsNanWidthAndHeight => Coord.IsNaN(width) && Coord.IsNaN(height);

        /// <summary>
        /// Tests whether <see cref='Width'/> or <see cref="Height"/> is not a number
        /// (equals <see cref="Coord.NaN"/>).
        /// </summary>
        [Browsable(false)]
        public readonly bool IsNanWidthOrHeight => Coord.IsNaN(width) || Coord.IsNaN(height);

        /// <summary>
        /// Tests whether <see cref='Width'/> is not a number (equals <see cref="Coord.NaN"/>).
        /// </summary>
        [Browsable(false)]
        public readonly bool IsNanWidth => Coord.IsNaN(width);

        /// <summary>
        /// Tests whether <see cref='Height'/> is not a number (equals <see cref="Coord.NaN"/>).
        /// </summary>
        [Browsable(false)]
        public readonly bool IsNanHeight => Coord.IsNaN(height);

        /// <summary>
        /// Tests whether this <see cref='Drawing.SizeD'/> has zero width and height.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => width == CoordD.Empty && height == CoordD.Empty;

        /// <summary>
        /// Gets <see cref="SizeD"/> with absolute values of (Width, Height).
        /// </summary>
        [Browsable(false)]
        public readonly SizeD Abs => new(Math.Abs(width), Math.Abs(height));

        /// <summary>
        /// Tests whether this <see cref='SizeD'/> has zero width or height.
        /// </summary>
        [Browsable(false)]
        public readonly bool AnyIsEmpty => width == CoordD.Empty || height == CoordD.Empty;

        /// <summary>
        /// Tests whether this <see cref='SizeD'/> has zero (or negative) width or height.
        /// </summary>
        [Browsable(false)]
        public readonly bool AnyIsEmptyOrNegative => width <= CoordD.Empty || height <= CoordD.Empty;

        /// <summary>
        /// Represents the horizontal component of this <see cref='Drawing.SizeD'/>.
        /// </summary>
        public Coord Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Gets <see cref="Coord.PositiveInfinity"/> instead of width or height
        /// if their value is 0.
        /// </summary>
        [Browsable(false)]
        public readonly SizeD InfinityIfEmpty
        {
            get
            {
                var w = width == CoordD.Empty ? Coord.PositiveInfinity : width;
                var h = height == CoordD.Empty ? Coord.PositiveInfinity : height;
                return (w, h);
            }
        }

        /// <summary>
        /// Represents the vertical component of this <see cref='Drawing.SizeD'/>.
        /// </summary>
        public Coord Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Converts the specified <see cref='SizeD'/> to a
        /// <see cref='PointD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PointD(SizeD size) =>
            new(size.Width, size.Height);

        /// <summary>
        /// Creates a <see cref='SizeD'/> with the properties of the
        /// specified <see cref='System.Drawing.Size'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeD(System.Drawing.Size p) => new(p.Width, p.Height);

        /// <summary>
        /// Creates a <see cref='System.Drawing.Size'/> with the properties of the
        /// specified <see cref='SizeD'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Size(SizeD p) =>
            new(RectD.CoordToInt(p.Width), RectD.CoordToInt(p.Height));

        /// <summary>
        /// Implicit operator conversion from tuple with two <see cref="Coord"/> values
        /// to <see cref="SizeD"/>.
        /// </summary>
        /// <param name="d">New size value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeD((Coord Width, Coord Height) d) => new(d.Width, d.Height);

        /// <summary>
        /// Converts the specified <see cref="SizeD"/> to a
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2(SizeD size) => size.ToVector2();

        /// <summary>
        /// Converts the specified <see cref="System.Numerics.Vector2"/> to
        /// a <see cref="SizeD"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SizeD(Vector2 vector) => new(vector);

        /// <summary>
        /// Initializes <see cref="Drawing.SizeD"/> with equal width and height.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeD(Coord widthAndHeight) => new(widthAndHeight);

        /// <summary>
        /// Initializes <see cref="Drawing.SizeD"/> with equal width and height.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeD(int widthAndHeight) => new(widthAndHeight);

        /// <summary>
        /// Performs vector addition of two <see cref='Drawing.SizeD'/> objects.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD operator +(SizeD sz1, SizeD sz2) => Add(sz1, sz2);

        /// <summary>
        /// Subtracts a <see cref='Drawing.SizeD'/> by another
        /// <see cref='Drawing.SizeD'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD operator -(SizeD sz1, SizeD sz2) => Subtract(sz1, sz2);

        /// <summary>
        /// Multiplies <see cref="SizeD"/> by a value producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Multiplier.</param>
        /// <param name="right">Multiplicand of type <see cref="SizeD"/>.</param>
        /// <returns>Product of type <see cref="SizeD"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD operator *(Coord left, SizeD right) =>
            Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="SizeD"/> by a value producing
        /// <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="SizeD"/>.</param>
        /// <param name="right">Multiplier.</param>
        /// <returns>Product of type <see cref="SizeD"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD operator *(SizeD left, Coord right) =>
            Multiply(left, right);

        /// <summary>
        /// Divides <see cref="SizeD"/> by a value producing
        /// <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="SizeD"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="SizeD"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD operator /(SizeD left, Coord right)
            => new(left.width / right, left.height / right);

        /// <summary>
        /// Tests whether two <see cref='SizeD'/> objects are identical.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(SizeD sz1, SizeD sz2) =>
            sz1.Width == sz2.Width && sz1.Height == sz2.Height;

        /// <summary>
        /// Tests whether two <see cref='SizeD'/> objects are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(SizeD sz1, SizeD sz2) => !(sz1 == sz2);

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Size data </param>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD Parse(string source)
        {
            return ConversionUtils.ParseTwoFloats(source);
        }

        /// <summary>
        /// Performs vector addition of two <see cref='Drawing.SizeD'/> objects.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD Add(SizeD sz1, SizeD sz2) =>
            new(sz1.Width + sz2.Width, sz1.Height + sz2.Height);

        /// <summary>
        /// Contracts a <see cref='Drawing.SizeD'/> by another
        /// <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD Subtract(SizeD sz1, SizeD sz2) =>
            new(sz1.Width - sz2.Width, sz1.Height - sz2.Height);

        /// <summary>
        /// Gets maximal width and height from the two specified <see cref="SizeD"/> values.
        /// </summary>
        /// <param name="v1">First <see cref="SizeD"/> value.</param>
        /// <param name="v2">Second <see cref="SizeD"/> value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD Max(SizeD v1, SizeD v2)
        {
            return new SizeD(Math.Max(v1.width, v2.width), Math.Max(v1.height, v2.height));
        }

        /// <summary>
        /// Gets minimal width and height from the two specified <see cref="SizeD"/> values.
        /// </summary>
        /// <param name="v1">First <see cref="SizeD"/> value.</param>
        /// <param name="v2">Second <see cref="SizeD"/> value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD Min(SizeD v1, SizeD v2)
        {
            return new SizeD(Math.Min(v1.width, v2.width), Math.Min(v1.height, v2.height));
        }

        /// <summary>Determines whether the specified value is (NaN, NaN).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(SizeD d) => Coord.IsNaN(d.width) && Coord.IsNaN(d.height);

        /// <summary>Determines whether the specified value has width or height
        /// equal to <see cref="Coord.NaN"/>.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyIsNaN(SizeD d) => Coord.IsNaN(d.width) || Coord.IsNaN(d.height);

        /// <summary>
        /// Returns an array filled with widths of the specified <see cref="SizeD"/> values.
        /// </summary>
        /// <param name="sizes">Array of <see cref="SizeD"/> values.</param>
        public static Coord[] GetWidths(SizeD[] sizes)
        {
            Coord[] result = new Coord[sizes.Length];
            for(int i = 0; i < sizes.Length; i++)
                result[i] = sizes[i].Width;
            return result;
        }

        /// <summary>
        /// Gets sum of the each element of the specified <see cref="SizeD"/> array.
        /// </summary>
        /// <param name="sizes">Array of <see cref="SizeD"/>.</param>
        /// <returns></returns>
        public static SizeD Sum(SizeD[] sizes)
        {
            var length = sizes.Length;
            SizeD result = SizeD.Empty;

            for (int i = 0; i < length; i++)
            {
                result.Width += sizes[i].Width;
                result.Height += sizes[i].Height;
            }

            return result;
        }

        /// <summary>
        /// Multiplies <see cref="SizeD"/> by a value
        /// producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="SizeD"/>.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <returns>Product of type SizeF.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD Multiply(SizeD size, Coord multiplier) =>
            new(size.width * multiplier, size.height * multiplier);

        /// <summary>
        /// Returns an array filled with heights of the specified <see cref="SizeD"/> values.
        /// </summary>
        /// <param name="sizes">Array of <see cref="SizeD"/> values.</param>
        public static Coord[] GetHeights(SizeD[] sizes)
        {
            Coord[] result = new Coord[sizes.Length];
            for (int i = 0; i < sizes.Length; i++)
                result[i] = sizes[i].Height;
            return result;
        }

        /// <summary>
        /// Returns the larger of the specified <see cref="SizeD"/> values.
        /// </summary>
        /// <param name="sizes">Array of <see cref="SizeD"/> values.</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="sizes"/> is
        /// an empty array.</exception>
        public static SizeD MaxWidthHeights(SizeD[] sizes)
        {
            var widths = GetWidths(sizes);
            var heights = GetHeights(sizes);

            var width = MathUtils.Max(widths);
            var height = MathUtils.Max(heights);

            return new(width, height);
        }

        /// <summary>
        /// Gets a new <see cref="SizeD"/> with swapped <see cref="Width"/> and
        /// Height values.
        /// </summary>
        /// <returns>A new <see cref="SizeD"/> instance with swapped width and height.</returns>
        public readonly SizeD WithSwappedWidthAndHeight()
        {
            return new SizeD(height, width);
        }

        /// <summary>
        /// Swaps <see cref="Width"/> and <see cref="Height"/> values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwapWidthAndHeight()
        {
            (height, width) = (width, height);
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
        /// Gets size with applied minimal and maximal limitations.
        /// </summary>
        /// <param name="min">Minimal width and height.</param>
        /// <param name="max">Maximal width and height.</param>
        /// <returns><see cref="SizeD"/> with width and height greater than specified in
        /// <paramref name="min"/> and less than specified in <paramref name="max"/>.</returns>
        public readonly SizeD ApplyMinMax(SizeD min, SizeD max)
        {
            var minApplied = this.ApplyMin(min);
            var maxApplied = minApplied.ApplyMax(max);
            return maxApplied;
        }

        /// <summary>
        /// Gets size with applied minimal limitations.
        /// </summary>
        /// <param name="min">Minimal width and height.</param>
        /// <returns><see cref="SizeD"/> with width and height greater than specified in
        /// <paramref name="min"/>.</returns>
        public readonly SizeD ApplyMin(SizeD min)
        {
            var result = this;
            if (min.Width >= CoordD.Empty && result.Width < min.Width)
                result.Width = min.Width;
            if (min.Height >= CoordD.Empty && result.Height < min.Height)
                result.Height = min.Height;
            return result;
        }

        /// <summary>
        /// Gets size with applied maximal limitations.
        /// </summary>
        /// <param name="max">Maximal width and height.</param>
        /// <returns><see cref="SizeD"/> with width and height less than
        /// specified in <paramref name="max"/>.</returns>
        public readonly SizeD ApplyMax(SizeD max)
        {
            var result = this;
            if (max.Width > CoordD.Empty && result.Width > max.Width)
                result.Width = max.Width;
            if (max.Height > CoordD.Empty && result.Height > max.Height)
                result.Height = max.Height;
            return result;
        }

        /// <summary>
        /// Tests to see whether the specified object is a
        /// <see cref='Drawing.SizeD'/>  with the same dimensions
        /// as this <see cref='Drawing.SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is SizeD size && Equals(size);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other;
        /// otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(SizeD other) => this == other;

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => (Width, Height).GetHashCode();

        /// <summary>
        /// Returns new <see cref="SizeD"/> value with ceiling of the <see cref="Width"/> and
        /// <see cref="Height"/>. Uses <see cref="Math.Ceiling(Coord)"/> on values.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Ceiling operation returns the smallest integer that is greater than or equal
        /// to the specified floating-point number.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SizeD Ceiling()
        {
            return new(Math.Ceiling(width), Math.Ceiling(height));
        }

        /// <summary>
        /// Converts a <see cref="SizeD"/> structure to a <see cref="PointD"/> structure.
        /// </summary>
        /// <returns>A <see cref="PointD"/> structure.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly PointD ToPointF() => (PointD)this;

        /// <summary>
        /// Get this object with applied min and max constraints.
        /// </summary>
        /// <param name="maxWidth">Maximal width.</param>
        /// <param name="maxHeight">Maximal height.</param>
        /// <returns></returns>
        public readonly SizeD Shrink(Coord? maxWidth, Coord? maxHeight)
        {
            var result = this;

            if (maxWidth is not null)
            {
                result.Width = Math.Min(result.Width, maxWidth.Value);
            }

            if (maxHeight is not null)
            {
                result.Height = Math.Min(result.Height, maxHeight.Value);
            }

            return result;
        }

        /// <summary>
        /// Returns this size (in pixels) converted to device-independent units.
        /// </summary>
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
        /// <returns></returns>
        public readonly SizeD PixelToDip(Coord? scaleFactor = null)
        {
            return GraphicsFactory.PixelToDip(this.ToSize(), scaleFactor);
        }

        /// <summary>
        /// Returns this size (in inches) converted to device-independent units.
        /// </summary>
        /// <returns></returns>
        public readonly SizeD InchesToDips()
        {
            var result = GraphicsUnitConverter.ConvertSize(
                GraphicsUnit.Inch,
                GraphicsUnit.Dip,
                0,
                this);
            return result;
        }

        /// <summary>
        /// Returns this size (in millimeters) converted to device-independent units.
        /// </summary>
        /// <returns></returns>
        public readonly SizeD MillimetersToDips()
        {
            var result = GraphicsUnitConverter.ConvertSize(
                GraphicsUnit.Millimeter,
                GraphicsUnit.Dip,
                0,
                this);
            return result;
        }

        /// <summary>
        /// Shrinks the specified size coordinate.
        /// </summary>
        /// <param name="vert">Whether to shrink vertical or horizontal size.</param>
        /// <param name="maxSize">Maximal possible value.</param>
        /// <returns></returns>
        public readonly SizeD Shrink(bool vert, Coord maxSize)
        {
            var result = this;
            result.SetSize(vert, Math.Min(result.GetSize(vert), maxSize));
            return result;
        }

        /// <summary>
        /// Converts a <see cref="SizeD"/> structure to a
        /// <see cref="SizeI"/> structure.
        /// </summary>
        /// <returns>A <see cref="SizeI"/> structure.</returns>
        /// <remarks>The <see cref="SizeD"/> structure is converted
        /// to a <see cref="SizeI"/> structure by
        /// truncating the values of the <see cref="SizeD"/> to the next
        /// lower integer values.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SizeI ToSize() => SizeI.Truncate(this);

        /// <summary>
        /// Creates a human-readable string that represents this
        /// <see cref='Drawing.SizeD'/>.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names = { PropNameStrings.Default.Width, PropNameStrings.Default.Height };
            Coord[] values = { width, height };

            return StringUtils.ToStringWithOrWithoutNames<Coord>(names, values);
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
        public readonly string ConvertToString(
            string format,
            IFormatProvider provider)
        {
            if (IsEmpty)
            {
                return "Empty";
            }

            // Helper to get the numeric list separator for a given culture.
            char separator = TokenizerHelper.GetNumericListSeparator(provider);
            return string.Format(
                provider,
                "{1:" + format + "}{0}{2:" + format + "}",
                separator,
                width,
                height);
        }

        /// <summary>
        /// Calls <paramref name="coerceFunc"/> for the height.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CoerceHeight(Func<Coord, Coord> coerceFunc)
        {
            height = coerceFunc(height);
        }

        /// <summary>
        /// Calls <paramref name="coerceFunc"/> for the width and height.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Coerce(Func<Coord, Coord> coerceFunc)
        {
            width = coerceFunc(width);
            height = coerceFunc(height);
        }

        /// <summary>
        /// Calls <see cref="CoerceCoordFunc"/> for the width and height.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Coerce()
        {
            if (CoerceCoordFunc is null)
                return;
            Coerce(CoerceCoordFunc);
        }

        /// <summary>
        /// Calls <see cref="CoerceCoordFunc"/> for the height.
        /// </summary>
        public void CoerceHeight()
        {
            if (CoerceCoordFunc is null)
                return;
            CoerceHeight(CoerceCoordFunc);
        }

        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector2"/> from this object.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Vector2 ToVector2()
            => new(RectD.CoordToFloat(width), RectD.CoordToFloat(height));
    }
}
