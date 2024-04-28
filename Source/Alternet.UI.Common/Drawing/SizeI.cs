using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /*
        Please do not remove StructLayout(LayoutKind.Sequential) atrtribute.
        Also do not change order of the fields.
    */

    /// <summary>
    /// Represents the size of a rectangular region with an ordered pair
    /// of width and height.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SizeI : IEquatable<SizeI>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='SizeI'/> class.
        /// </summary>
        public static readonly SizeI Empty;

        /// <summary>
        /// Gets a size with (-1, -1) values.
        /// </summary>
        public static readonly SizeI MinusOne = new(-1, -1);

        /// <summary>
        /// Gets a size with (1, 1) values.
        /// </summary>
        public static readonly SizeI One = new(1, 1);

        private int width; // Do not rename (binary serialization)
        private int height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeI'/>
        /// class from the specified
        /// <see cref='PointI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeI(PointI pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='SizeI'/>
        /// class from the specified dimensions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeI(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeI"/> struct with
        /// equal width and height.
        /// </summary>
        /// <param name="size">Width and Height value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeI(int size)
        {
            this.width = size;
            this.height = size;
        }

        /// <summary>
        /// Gets number of pixels (Width * Height).
        /// </summary>
        [Browsable(false)]
        public readonly int PixelCount => width * height;

        /// <summary>
        /// Tests whether this <see cref='SizeI'/> has zero width or height.
        /// </summary>
        [Browsable(false)]
        public readonly bool AnyIsEmpty => width == 0 || height == 0;

        /// <summary>
        /// Tests whether this <see cref='SizeI'/> has zero
        /// width and height.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => width == 0 && height == 0;

        /// <summary>
        /// Represents the horizontal component of this
        /// <see cref='SizeI'/>.
        /// </summary>
        public int Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Represents the vertical component of this
        /// <see cref='SizeI'/>.
        /// </summary>
        public int Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Converts the specified <see cref='SizeI'/> to a
        /// <see cref='PointI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PointI(SizeI size) =>
            new(size.Width, size.Height);

        /// <summary>
        /// Implicit operator convertion from tuple with two <see cref="int"/> values
        /// to <see cref="SizeI"/>.
        /// </summary>
        /// <param name="d">New size value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeI((int, int) d) => new(d.Item1, d.Item2);

        /// <summary>
        /// Creates a <see cref='System.Drawing.Size'/> with the coordinates of the
        /// specified <see cref='SizeI'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Size(SizeI p) => new(p.Width, p.Height);

        /// <summary>
        /// Creates a <see cref='SizeI'/> with the coordinates of the
        /// specified <see cref='System.Drawing.Size'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeI(System.Drawing.Size p) => new(p.Width, p.Height);

        /// <summary>
        /// Converts the specified <see cref='SizeI'/> to a <see cref='SizeD'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeD(SizeI p) => new(p.Width, p.Height);

        /// <summary>
        /// Converts the specified <see cref='int'/> to a <see cref='SizeI'/>.
        /// Width and height are set to the <paramref name="value"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SizeI(int value) => new(value, value);

        /// <summary>
        /// Performs vector addition of two <see cref='SizeI'/> objects.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeI operator +(SizeI sz1, SizeI sz2) =>
            Add(sz1, sz2);

        /// <summary>
        /// Contracts a <see cref='SizeI'/> by another
        /// <see cref='SizeI'/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeI operator -(SizeI sz1, SizeI sz2) =>
            Subtract(sz1, sz2);

        /// <summary>
        /// Multiplies a <see cref="SizeI"/> by an <see cref="int"/>
        /// producing <see cref="SizeI"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="int"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="SizeI"/>.</param>
        /// <returns>Product of type <see cref="SizeI"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeI operator *(int left, SizeI right) =>
            Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="SizeI"/> by an <see cref="int"/>
        /// producing <see cref="SizeI"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="SizeI"/>.</param>
        /// <param name="right">Multiplier of type <see cref="int"/>.</param>
        /// <returns>Product of type <see cref="SizeI"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeI operator *(SizeI left, int right) =>
            Multiply(left, right);

        /// <summary>
        /// Divides <see cref="SizeI"/> by an <see cref="int"/>
        /// producing <see cref="SizeI"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="SizeI"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="SizeI"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeI operator /(SizeI left, int right) =>
            new(unchecked(left.width / right), unchecked(left.height / right));

        /// <summary>
        /// Multiplies <see cref="SizeI"/> by a <see cref="double"/>
        /// producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="double"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="SizeI"/>.</param>
        /// <returns>Product of type <see cref="SizeD"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD operator *(double left, SizeI right) =>
            Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="SizeI"/> by a <see cref="double"/>
        /// producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="SizeI"/>.</param>
        /// <param name="right">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type <see cref="SizeD"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD operator *(SizeI left, double right) =>
            Multiply(left, right);

        /// <summary>
        /// Divides <see cref="SizeI"/> by a <see cref="double"/>
        /// producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="SizeI"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="SizeD"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SizeD operator /(SizeI left, double right)
            => new(left.width / right, left.height / right);

        /// <summary>
        /// Tests whether two <see cref='SizeI'/> objects are identical.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(SizeI sz1, SizeI sz2) =>
            sz1.Width == sz2.Width && sz1.Height == sz2.Height;

        /// <summary>
        /// Tests whether two <see cref='SizeI'/> objects are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(SizeI sz1, SizeI sz2) =>
            !(sz1 == sz2);

        /// <summary>
        /// Gets maximal width and height from the two specified <see cref="SizeI"/> values.
        /// </summary>
        /// <param name="v1">First <see cref="SizeI"/> value.</param>
        /// <param name="v2">Second <see cref="SizeI"/> value.</param>
        /// <returns></returns>
        public static SizeI Max(SizeI v1, SizeI v2)
        {
            return new SizeI(Math.Max(v1.width, v2.width), Math.Max(v1.height, v2.height));
        }

        /// <summary>
        /// Performs vector addition of two <see cref='SizeI'/> objects.
        /// </summary>
        public static SizeI Add(SizeI sz1, SizeI sz2) =>
            new(unchecked(sz1.Width + sz2.Width),
                unchecked(sz1.Height + sz2.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a ceiling operation
        /// on all the coordinates.
        /// </summary>
        public static SizeI Ceiling(SizeD value) =>
            new(unchecked((int)Math.Ceiling(value.Width)),
                unchecked((int)Math.Ceiling(value.Height)));

        /// <summary>
        /// Contracts a <see cref='SizeI'/> by another
        /// <see cref='SizeI'/> .
        /// </summary>
        public static SizeI Subtract(SizeI sz1, SizeI sz2) =>
            new(unchecked(sz1.Width - sz2.Width),
                unchecked(sz1.Height - sz2.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a truncate operation
        /// on all the coordinates.
        /// </summary>
        public static SizeI Truncate(SizeD value) =>
            new(unchecked((int)value.Width), unchecked((int)value.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a round operation on
        /// all the coordinates.
        /// </summary>
        public static SizeI Round(SizeD value) =>
            new(unchecked((int)Math.Round(value.Width)),
                unchecked((int)Math.Round(value.Height)));

        /// <summary>
        /// Tests to see whether the specified object is a
        /// <see cref='SizeI'/>  with the same dimensions
        /// as this <see cref='SizeI'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is SizeI size && Equals(size);

        /// <summary>
        /// Indicates whether the current object is equal to another object
        /// of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other;
        /// otherwise, <c>false</c>.</returns>
        public readonly bool Equals(SizeI other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() =>
            HashCode.Combine(Width, Height);

        /// <summary>
        /// Creates a human-readable string that represents this
        /// <see cref='SizeI'/>.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names = { PropNameStrings.Default.Width, PropNameStrings.Default.Height };
            int[] values = { width, height };

            return StringUtils.ToString<int>(names, values);
        }

        /// <summary>
        /// Multiplies <see cref="SizeI"/> by an <see cref="int"/>
        /// producing <see cref="SizeI"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="SizeI"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref='int'/>.</param>
        /// <returns>Product of type <see cref="SizeI"/>.</returns>
        private static SizeI Multiply(SizeI size, int multiplier) =>
            new(unchecked(size.width * multiplier),
                unchecked(size.height * multiplier));

        /// <summary>
        /// Multiplies <see cref="SizeI"/> by a <see cref="double"/>
        /// producing <see cref="SizeD"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="SizeI"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type SizeF.</returns>
        private static SizeD Multiply(SizeI size, double multiplier) =>
            new(size.width * multiplier, size.height * multiplier);
    }
}
