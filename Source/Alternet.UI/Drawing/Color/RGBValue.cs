using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        public byte R;

        /// <summary>
        /// Green component of a color.
        /// </summary>
        public byte G;

        /// <summary>
        /// Blue component of a color.
        /// </summary>
        public byte B;

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBValue"/>.
        /// </summary>
        public RGBValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBValue"/>.
        /// </summary>
        public RGBValue(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Implicit operator convertion from tuple with three <see cref="byte"/> values
        /// to <see cref="RGBValue"/>.
        /// </summary>
        /// <param name="d">New <see cref="RGBValue"/>.</param>
        public static implicit operator RGBValue((byte, byte, byte) d) =>
            new(d.Item1, d.Item2, d.Item3);

        /// <summary>
        /// Implicit operator convertion from <see cref="HSVValue"/> to <see cref="RGBValue"/>.
        /// </summary>
        /// <param name="d">New <see cref="RGBValue"/>.</param>
        public static implicit operator RGBValue(HSVValue d) => Color.HSVtoRGB(d);

        /// <summary>
        /// Implicit operator convertion from <see cref="RGBValue"/> to <see cref="HSVValue"/>.
        /// </summary>
        /// <param name="d">New <see cref="HSVValue"/>.</param>
        public static implicit operator HSVValue(RGBValue d) => Color.RGBtoHSV(d);

        /// <summary>
        /// Tests whether two <see cref='RGBValue'/> objects are different.
        /// </summary>
        public static bool operator !=(RGBValue left, RGBValue right) => !(left == right);

        /// <summary>
        /// Tests whether two <see cref='RGBValue'/> objects are equal.
        /// </summary>
        public static bool operator ==(RGBValue left, RGBValue right) =>
            left.R == right.R && left.G == right.G && left.B == right.B;

        /// <summary>
        /// Gets color name and ARGB for the debug purposes.
        /// </summary>
        public readonly string AsDebugString => $"{{RGB=({R}, {G}, {B})}}";

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
        public readonly bool Equals(RGBValue other) => this == other;

        /// <summary>
        /// Gets the hash code for this <see cref='Rect'/>.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(R, G, B);

        /// <summary>
        /// Gets string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override readonly string ToString() => AsDebugString;
    }
}
