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
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct ColorStruct
    {
        public static readonly ColorStruct Default = new();

        [FieldOffset(0)]
        public byte B;

        [FieldOffset(1)]
        public byte G;

        [FieldOffset(2)]
        public byte R;

        [FieldOffset(3)]
        public byte A;

        [FieldOffset(0)]
        public SKColor Color;

        [FieldOffset(0)]
        public uint Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(byte red, byte green, byte blue)
        {
            A = 255;
            R = red;
            G = green;
            B = blue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(byte alpha, byte red, byte green, byte blue)
        {
            A = alpha;
            R = red;
            G = green;
            B = blue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(SKColor color)
        {
            Color = color;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorStruct(uint value)
        {
            Value = value;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ColorStruct(uint d) => new(d);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator uint(ColorStruct d) => d.Value;

        /// <summary>
        /// Implicit operator convertion from tuple with three <see cref="byte"/> values
        /// to <see cref="Color"/>. Tuple values define RGB of the color.
        /// </summary>
        /// <param name="d">New color value.</param>
        /// <remarks>
        /// This operator uses
        /// <see cref="Color.FromRgb(byte, byte, byte)"/> internally.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ColorStruct((byte, byte, byte) d) =>
            new(d.Item1, d.Item2, d.Item3);

        /// <summary>
        /// Implicit operator convertion from tuple with three <see cref="byte"/> values
        /// to <see cref="Color"/>. Tuple values define ARGB of the color.
        /// </summary>
        /// <param name="d">New color value.</param>
        /// <remarks>
        /// This operator uses
        /// <see cref="Color.FromArgb(byte, byte, byte, byte)"/> internally.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ColorStruct((byte, byte, byte, byte) d) =>
            new(d.Item1, d.Item2, d.Item3, d.Item4);

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

        public override readonly bool Equals(object? obj)
        {
            if (obj is ColorStruct color)
                return color == this;
            return false;
        }

        public override readonly int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
