// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
    /*[TypeConverter("System.Drawing.SizeConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]*/
    public struct Int32Size : IEquatable<Int32Size>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Int32Size'/> class.
        /// </summary>
        public static readonly Int32Size Empty;

        /// <summary>
        /// Gets a size with (-1, -1) values.
        /// </summary>
        public static readonly Int32Size MinusOne = new(-1, -1);

        /// <summary>
        /// Gets a size with (1, 1) values.
        /// </summary>
        public static readonly Int32Size One = new(1, 1);

        private int width; // Do not rename (binary serialization)
        private int height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Int32Size'/>
        /// class from the specified
        /// <see cref='Int32Point'/>.
        /// </summary>
        public Int32Size(Int32Point pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Int32Size'/>
        /// class from the specified dimensions.
        /// </summary>
        public Int32Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int32Size"/> struct with
        /// equal width and height.
        /// </summary>
        /// <param name="size">Width and Height value.</param>
        public Int32Size(int size)
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
        /// Tests whether this <see cref='Int32Size'/> has zero
        /// width and height.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => width == 0 && height == 0;

        /// <summary>
        /// Represents the horizontal component of this
        /// <see cref='Int32Size'/>.
        /// </summary>
        public int Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Represents the vertical component of this
        /// <see cref='Int32Size'/>.
        /// </summary>
        public int Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Converts the specified <see cref='Int32Size'/> to a
        /// <see cref='Int32Point'/>.
        /// </summary>
        public static explicit operator Int32Point(Int32Size size) =>
            new(size.Width, size.Height);

        /// <summary>
        /// Implicit operator convertion from tuple with two <see cref="int"/> values
        /// to <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="d">New size value.</param>
        public static implicit operator Int32Size((int, int) d) => new(d.Item1, d.Item2);

        /// <summary>
        /// Creates a <see cref='System.Drawing.Size'/> with the coordinates of the
        /// specified <see cref='Int32Size'/>
        /// </summary>
        public static implicit operator System.Drawing.Size(Int32Size p) => new(p.Width, p.Height);

        /// <summary>
        /// Creates a <see cref='Int32Size'/> with the coordinates of the
        /// specified <see cref='System.Drawing.Size'/>
        /// </summary>
        public static implicit operator Int32Size(System.Drawing.Size p) => new(p.Width, p.Height);

        /// <summary>
        /// Converts the specified <see cref='Int32Size'/> to a <see cref='Size'/>.
        /// </summary>
        public static implicit operator Size(Int32Size p) => new(p.Width, p.Height);

        /// <summary>
        /// Converts the specified <see cref='int'/> to a <see cref='Int32Size'/>.
        /// Width and height are set to the <paramref name="value"/>.
        /// </summary>
        public static implicit operator Int32Size(int value) => new(value, value);

        /// <summary>
        /// Performs vector addition of two <see cref='Int32Size'/> objects.
        /// </summary>
        public static Int32Size operator +(Int32Size sz1, Int32Size sz2) =>
            Add(sz1, sz2);

        /// <summary>
        /// Contracts a <see cref='Int32Size'/> by another
        /// <see cref='Int32Size'/>
        /// </summary>
        public static Int32Size operator -(Int32Size sz1, Int32Size sz2) =>
            Subtract(sz1, sz2);

        /// <summary>
        /// Multiplies a <see cref="Int32Size"/> by an <see cref="int"/>
        /// producing <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="int"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <returns>Product of type <see cref="Int32Size"/>.</returns>
        public static Int32Size operator *(int left, Int32Size right) =>
            Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by an <see cref="int"/>
        /// producing <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <param name="right">Multiplier of type <see cref="int"/>.</param>
        /// <returns>Product of type <see cref="Int32Size"/>.</returns>
        public static Int32Size operator *(Int32Size left, int right) =>
            Multiply(left, right);

        /// <summary>
        /// Divides <see cref="Int32Size"/> by an <see cref="int"/>
        /// producing <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="Int32Size"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="Int32Size"/>.</returns>
        public static Int32Size operator /(Int32Size left, int right) =>
            new(unchecked(left.width / right), unchecked(left.height / right));

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by a <see cref="double"/>
        /// producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="double"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        public static Size operator *(double left, Int32Size right) =>
            Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by a <see cref="double"/>
        /// producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <param name="right">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        public static Size operator *(Int32Size left, double right) =>
            Multiply(left, right);

        /// <summary>
        /// Divides <see cref="Int32Size"/> by a <see cref="double"/>
        /// producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="Int32Size"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="Size"/>.</returns>
        public static Size operator /(Int32Size left, double right)
            => new(left.width / right, left.height / right);

        /// <summary>
        /// Tests whether two <see cref='Int32Size'/> objects are identical.
        /// </summary>
        public static bool operator ==(Int32Size sz1, Int32Size sz2) =>
            sz1.Width == sz2.Width && sz1.Height == sz2.Height;

        /// <summary>
        /// Tests whether two <see cref='Int32Size'/> objects are different.
        /// </summary>
        public static bool operator !=(Int32Size sz1, Int32Size sz2) =>
            !(sz1 == sz2);

        /// <summary>
        /// Gets maximal width and height from the two specified <see cref="Int32Size"/> values.
        /// </summary>
        /// <param name="v1">First <see cref="Int32Size"/> value.</param>
        /// <param name="v2">Second <see cref="Int32Size"/> value.</param>
        /// <returns></returns>
        public static Int32Size Max(Int32Size v1, Int32Size v2)
        {
            return new Int32Size(Math.Max(v1.width, v2.width), Math.Max(v1.height, v2.height));
        }

        /// <summary>
        /// Performs vector addition of two <see cref='Int32Size'/> objects.
        /// </summary>
        public static Int32Size Add(Int32Size sz1, Int32Size sz2) =>
            new(unchecked(sz1.Width + sz2.Width),
                unchecked(sz1.Height + sz2.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a ceiling operation
        /// on all the coordinates.
        /// </summary>
        public static Int32Size Ceiling(Size value) =>
            new(unchecked((int)Math.Ceiling(value.Width)),
                unchecked((int)Math.Ceiling(value.Height)));

        /// <summary>
        /// Contracts a <see cref='Int32Size'/> by another
        /// <see cref='Int32Size'/> .
        /// </summary>
        public static Int32Size Subtract(Int32Size sz1, Int32Size sz2) =>
            new(unchecked(sz1.Width - sz2.Width),
                unchecked(sz1.Height - sz2.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a truncate operation
        /// on all the coordinates.
        /// </summary>
        public static Int32Size Truncate(Size value) =>
            new(unchecked((int)value.Width), unchecked((int)value.Height));

        /// <summary>
        /// Converts a SizeF to a Size by performing a round operation on
        /// all the coordinates.
        /// </summary>
        public static Int32Size Round(Size value) =>
            new(unchecked((int)Math.Round(value.Width)),
                unchecked((int)Math.Round(value.Height)));

        /// <summary>
        /// Tests to see whether the specified object is a
        /// <see cref='Int32Size'/>  with the same dimensions
        /// as this <see cref='Int32Size'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Int32Size size && Equals(size);

        /// <summary>
        /// Indicates whether the current object is equal to another object
        /// of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other;
        /// otherwise, <c>false</c>.</returns>
        public readonly bool Equals(Int32Size other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() =>
            HashCode.Combine(Width, Height);

        /// <summary>
        /// Creates a human-readable string that represents this
        /// <see cref='Int32Size'/>.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names = [PropNameStrings.Default.Width, PropNameStrings.Default.Height];
            int[] values = [width, height];

            return StringUtils.ToString<int>(names, values);
        }

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by an <see cref="int"/>
        /// producing <see cref="Int32Size"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref='int'/>.</param>
        /// <returns>Product of type <see cref="Int32Size"/>.</returns>
        private static Int32Size Multiply(Int32Size size, int multiplier) =>
            new(unchecked(size.width * multiplier),
                unchecked(size.height * multiplier));

        /// <summary>
        /// Multiplies <see cref="Int32Size"/> by a <see cref="double"/>
        /// producing <see cref="Size"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="Int32Size"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type SizeF.</returns>
        private static Size Multiply(Int32Size size, double multiplier) =>
            new(size.width * multiplier, size.height * multiplier);
    }
}
