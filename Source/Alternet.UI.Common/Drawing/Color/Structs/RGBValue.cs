using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// A simple class which stores red, green and blue values as 8 bit unsigned integers
    /// in the range of 0-255.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{AsDebugString}")]
    public struct RGBValue : IEquatable<RGBValue>
    {
        /// <summary>
        /// Red component of a color.
        /// </summary>
        public byte R = 0;

        /// <summary>
        /// Green component of a color.
        /// </summary>
        public byte G = 0;

        /// <summary>
        /// Blue component of a color.
        /// </summary>
        public byte B = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBValue"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBValue"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBValue(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Gets color name and ARGB for the debug purposes.
        /// </summary>
        public readonly string AsDebugString => $"{{RGB=({R}, {G}, {B})}}";

        /// <summary>
        /// Implicit operator conversion from tuple with three <see cref="byte"/> values
        /// to <see cref="RGBValue"/>.
        /// </summary>
        /// <param name="d">New <see cref="RGBValue"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RGBValue((byte Red, byte Green, byte Blue) d) =>
            new(d.Red, d.Green, d.Blue);

        /// <summary>
        /// Converts the specified <see cref='SKColor'/> to a <see cref='RGBValue'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RGBValue(SKColor color)
        {
            return new(color.Red, color.Green, color.Blue);
        }

        /// <summary>
        /// Converts the specified <see cref='RGBValue'/> to a <see cref='SKColor'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SKColor(RGBValue color)
        {
            return new(color.R, color.G, color.B);
        }

        /// <summary>
        /// Implicit operator conversion from <see cref="HSVValue"/> to <see cref="RGBValue"/>.
        /// </summary>
        /// <param name="d">New <see cref="RGBValue"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RGBValue(HSVValue d) => Color.HSVtoRGB(d);

        /// <summary>
        /// Implicit operator conversion from <see cref="RGBValue"/> to <see cref="HSVValue"/>.
        /// </summary>
        /// <param name="d">New <see cref="HSVValue"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HSVValue(RGBValue d) => Color.RGBtoHSV(d);

        /// <summary>
        /// Tests whether two <see cref='RGBValue'/> objects are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RGBValue left, RGBValue right) => !(left == right);

        /// <summary>
        /// Tests whether two <see cref='RGBValue'/> objects are equal.
        /// </summary>
        public static bool operator ==(RGBValue left, RGBValue right) =>
            left.R == right.R && left.G == right.G && left.B == right.B;

        /// <summary>
        /// Converts <see cref="RGBValue"/> to <see cref="SKColor"/>.
        /// </summary>
        /// <param name="color">Value to convert.</param>
        /// <param name="alpha">Alpha component.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKColor ToSkia(RGBValue color, byte alpha)
        {
            return new(color.R, color.G, color.B, alpha);
        }

        /// <summary>
        /// Converts <see cref="RGBValue"/> to <see cref="SKColor"/>.
        /// </summary>
        /// <param name="color">Value to convert.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKColor ToSkia(RGBValue color)
        {
            return new(color.R, color.G, color.B);
        }

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='RGBValue'/> and
        /// is equal to this object
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is RGBValue value && Equals(value);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(RGBValue other) => this == other;

        /// <summary>
        /// Gets the hash code for this <see cref='RectD'/>.
        /// </summary>
        public override readonly int GetHashCode() => (R, G, B).GetHashCode();

        /// <summary>
        /// Gets string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override readonly string ToString() => AsDebugString;

        /// <summary>
        /// Returns this object as <see cref="SKColor"/> with the specified alpha component.
        /// </summary>
        /// <param name="a">Alpha component of the color.</param>
        /// <returns></returns>
        public readonly SKColor WithAlpha(byte a) => new(R, G, B, a);
    }
}
