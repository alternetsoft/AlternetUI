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

        [FieldOffset(0)] public byte B;

        [FieldOffset(1)] public byte G;

        [FieldOffset(2)] public byte R;

        [FieldOffset(3)] public byte A;

        [FieldOffset(0)] public SKColor Color;

        [FieldOffset(0)] public uint Value;

        public ColorStruct()
        {
        }

        public ColorStruct(uint value)
        {
            Value = value;
        }

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
