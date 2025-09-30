using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Alternet.UI;
using Alternet.UI.Localization;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// <see cref="ColorStruct"/> used inside the <see cref="Color"/> object in order to store
    /// color value. This structure stores color as BGRA and is compatible with <see cref="SKColor"/>.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ColorStruct : IEquatable<ColorStruct>
    {
        /// <summary>
        /// Gets default value with all fields assigned to default values.
        /// </summary>
        public static readonly ColorStruct Default = new();

        /// <summary>
        /// Gets or sets blue component of the color.
        /// </summary>
        [FieldOffset(0)]
        public byte B;

        /// <summary>
        /// Gets or sets green component of the color.
        /// </summary>
        [FieldOffset(1)]
        public byte G;

        /// <summary>
        /// Gets or sets red component of the color.
        /// </summary>
        [FieldOffset(2)]
        public byte R;

        /// <summary>
        /// Gets or sets alpha component of the color.
        /// </summary>
        [FieldOffset(3)]
        public byte A;

        /// <summary>
        /// Gets or sets color as <see cref="SKColor"/>.
        /// </summary>
        [FieldOffset(0)]
        public SKColor Color;

        /// <summary>
        /// Gets or sets color as <see cref="uint"/>.
        /// </summary>
        [FieldOffset(0)]
        public uint Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> with the specified parameters.
        /// </summary>
        /// <param name="red">Red component of the color.</param>
        /// <param name="green">Green component of the color.</param>
        /// <param name="blue">Blue component of the color.</param>
        /// <remarks>
        /// Alpha component is assigned with 255.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(byte red, byte green, byte blue)
        {
            A = 255;
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> with the specified parameters.
        /// </summary>
        /// <param name="red">Red component of the color.</param>
        /// <param name="green">Green component of the color.</param>
        /// <param name="blue">Blue component of the color.</param>
        /// <param name="alpha">Alpha component of the color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(byte alpha, byte red, byte green, byte blue)
        {
            A = alpha;
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        /// <param name="color">Color value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(SKColor color)
        {
            Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        /// <param name="value">Color value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(uint value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStruct"/> struct.
        /// </summary>
        /// <param name="value">Color value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(int value)
        {
            Value = unchecked((uint)value);
        }

        /// <summary>
        /// Converts the specified <see cref='SKColor'/> to a <see cref='ColorStruct'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ColorStruct(SKColor color)
        {
            return new(color);
        }

        /// <summary>
        /// Converts the specified <see cref='Color'/> to a <see cref='ColorStruct'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ColorStruct(Color color)
        {
            return new(color.Value);
        }

        /// <summary>
        /// Converts the specified <see cref='SKColor'/> to a <see cref='ColorStruct'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SKColor(ColorStruct color)
        {
            return new(color);
        }

        /// <summary>
        /// Converts the specified <see cref='int'/> to a <see cref='ColorStruct'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ColorStruct(int d) => new(d);

        /// <summary>
        /// Converts the specified <see cref='uint'/> to a <see cref='ColorStruct'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ColorStruct(uint d) => new(d);

        /// <summary>
        /// Converts the specified <see cref='ColorStruct'/> to a <see cref='uint'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator uint(ColorStruct d) => d.Value;

        /// <summary>
        /// Implicit operator conversion from tuple with three <see cref="byte"/> values
        /// to <see cref="Color"/>. Tuple values define RGB of the color.
        /// </summary>
        /// <param name="d">New color value.</param>
        /// <remarks>
        /// This operator uses
        /// <see cref="Color.FromRgb(byte, byte, byte)"/> internally.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ColorStruct((byte Red, byte Green, byte Blue) d) =>
            new(d.Red, d.Green, d.Blue);

        /// <summary>
        /// Implicit operator conversion from tuple with three <see cref="byte"/> values
        /// to <see cref="Color"/>. Tuple values define ARGB of the color.
        /// </summary>
        /// <param name="d">New color value.</param>
        /// <remarks>
        /// This operator uses
        /// <see cref="Color.FromArgb(byte, byte, byte, byte)"/> internally.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ColorStruct((byte Alpha, byte Red, byte Green, byte Blue) d) =>
            new(d.Alpha, d.Red, d.Green, d.Blue);

        /// <summary>
        /// Tests whether two specified <see cref="ColorStruct"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="Color"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="Color"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="ColorStruct"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ColorStruct left, ColorStruct right)
        {
            return left.Color == right.Color;
        }

        /// <summary>
        /// Tests whether two specified <see cref="ColorStruct"/> structures are not equivalent.
        /// </summary>
        /// <param name="left">The <see cref="Color"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="Color"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="ColorStruct"/> structures
        /// are not equal; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ColorStruct left, ColorStruct right)
        {
            return left.Color != right.Color;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the
        /// current object; otherwise, <c>false</c>.</returns>
        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is ColorStruct color)
                return color == this;
            return false;
        }

        /// <summary>
        /// Returns a new color based on this current instance, but with the new alpha channel value.
        /// </summary>
        /// <param name="alpha">The new alpha component.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ColorStruct WithAlpha(byte alpha)
        {
            var result = this;
            result.A = alpha;
            return new(result);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override readonly int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc/>
        public override readonly string ToString()
        {
            return $"A={A}, R={R}, G={G}, B={B}";
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(ColorStruct other)
        {
            return this == other;
        }
    }
}
