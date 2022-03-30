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
    /// Represents the size of a rectangular region with an ordered pair of width and height.
    /// </summary>
    [Serializable]
    //[System.Runtime.CompilerServices.TypeForwardedFrom("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    //[TypeConverter("System.Drawing.SizeFConverter, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public struct Size : IEquatable<Size>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Size'/> class.
        /// </summary>
        public static readonly Size Empty;
        private double width; // Do not rename (binary serialization)
        private double height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Size'/> class from the specified
        /// existing <see cref='Drawing.Size'/>.
        /// </summary>
        public Size(Size size)
        {
            width = size.width;
            height = size.height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Size'/> class from the specified
        /// <see cref='Drawing.Point'/>.
        /// </summary>
        public Size(Point pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Size'/> struct from the specified
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public Size(Vector2 vector)
        {
            width = vector.X;
            height = vector.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.Size'/> class from the specified dimensions.
        /// </summary>
        public Size(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        /* TODO: uncommment when Double System.Numerics is availble. See https://github.com/dotnet/runtime/issues/24168
        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector2"/> from this <see cref="Drawing.Size"/>.
        /// </summary>
        public Vector2 ToVector2() => new Vector2(width, height);

        /// <summary>
        /// Converts the specified <see cref="Drawing.Size"/> to a <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public static explicit operator Vector2(Size size) => size.ToVector2();*/

        /// <summary>
        /// Converts the specified <see cref="System.Numerics.Vector2"/> to a <see cref="Drawing.Size"/>.
        /// </summary>
        public static explicit operator Size(Vector2 vector) => new Size(vector);

        /// <summary>
        /// Performs vector addition of two <see cref='Drawing.Size'/> objects.
        /// </summary>
        public static Size operator +(Size sz1, Size sz2) => Add(sz1, sz2);

        /// <summary>
        /// Contracts a <see cref='Drawing.Size'/> by another <see cref='Drawing.Size'/>
        /// </summary>
        public static Size operator -(Size sz1, Size sz2) => Subtract(sz1, sz2);

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Size data </param>
        /// </summary>
        public static Size Parse(string source)
        {
            IFormatProvider formatProvider = TypeConverterHelper.InvariantEnglishUS;

            TokenizerHelper th = new TokenizerHelper(source, formatProvider);

            Size value;

            String firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty")
            {
                value = Empty;
            }
            else
            {
                value = new Size(
                    Convert.ToDouble(firstToken, formatProvider),
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
                                 "{1:" + format + "}{0}{2:" + format + "}",
                                 separator,
                                 width,
                                 height);
        }

        /// <summary>
        /// Multiplies <see cref="Size"/> by a <see cref="double"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="double"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="Size"/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        public static Size operator *(double left, Size right) => Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="Size"/> by a <see cref="double"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="Size"/>.</param>
        /// <param name="right">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        public static Size operator *(Size left, double right) => Multiply(left, right);

        /// <summary>
        /// Divides <see cref="Size"/> by a <see cref="double"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="Size"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="Size"/>.</returns>
        public static Size operator /(Size left, double right)
            => new Size(left.width / right, left.height / right);

        /// <summary>
        /// Tests whether two <see cref='Drawing.Size'/> objects are identical.
        /// </summary>
        public static bool operator ==(Size sz1, Size sz2) => sz1.Width == sz2.Width && sz1.Height == sz2.Height;

        /// <summary>
        /// Tests whether two <see cref='Drawing.Size'/> objects are different.
        /// </summary>
        public static bool operator !=(Size sz1, Size sz2) => !(sz1 == sz2);

        /// <summary>
        /// Converts the specified <see cref='Drawing.Size'/> to a <see cref='Drawing.Point'/>.
        /// </summary>
        public static explicit operator Point(Size size) => new Point(size.Width, size.Height);

        /// <summary>
        /// Tests whether this <see cref='Drawing.Size'/> has zero width and height.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => width == 0 && height == 0;

        /// <summary>
        /// Represents the horizontal component of this <see cref='Drawing.Size'/>.
        /// </summary>
        public double Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Represents the vertical component of this <see cref='Drawing.Size'/>.
        /// </summary>
        public double Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Performs vector addition of two <see cref='Drawing.Size'/> objects.
        /// </summary>
        public static Size Add(Size sz1, Size sz2) => new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);

        /// <summary>
        /// Contracts a <see cref='Drawing.Size'/> by another <see cref='Drawing.Size'/>.
        /// </summary>
        public static Size Subtract(Size sz1, Size sz2) => new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);

        /// <summary>
        /// Tests to see whether the specified object is a <see cref='Drawing.Size'/>  with the same dimensions
        /// as this <see cref='Drawing.Size'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Size && Equals((Size)obj);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(Size other) => this == other;

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(Width, Height);

        /// <summary>
        /// Converts a <see cref="Size"/> structure to a <see cref="Point"/> structure.
        /// </summary>
        /// <returns>A <see cref="Point"/> structure.</returns>
        public readonly Point ToPointF() => (Point)this;

        /// <summary>
        /// Converts a <see cref="Size"/> structure to a <see cref="Int32Size"/> structure.
        /// </summary>
        /// <returns>A <see cref="Int32Size"/> structure.</returns>
        /// <remarks>The <see cref="Size"/> structure is converted to a <see cref="Int32Size"/> structure by
        /// truncating the values of the <see cref="Size"/> to the next lower integer values.</remarks>
        public readonly Int32Size ToSize() => Int32Size.Truncate(this);

        /// <summary>
        /// Creates a human-readable string that represents this <see cref='Drawing.Size'/>.
        /// </summary>
        public override readonly string ToString() => $"{{Width={width}, Height={height}}}";

        /// <summary>
        /// Multiplies <see cref="Size"/> by a <see cref="double"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="Size"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref="double"/>.</param>
        /// <returns>Product of type SizeF.</returns>
        private static Size Multiply(Size size, double multiplier) =>
            new Size(size.width * multiplier, size.height * multiplier);
    }
}
