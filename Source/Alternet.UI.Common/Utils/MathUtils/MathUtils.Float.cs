using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    public static partial class MathUtils
    {
        /// <summary>
        /// Optional override for single-precision floating-point equality comparison.
        /// If set, this delegate will be used by <c>AreClose(float, float)</c>
        /// instead of the default implementation.
        /// </summary>
        public static Func<float, float, bool>? AreCloseOverrideF;

        /// <summary>
        /// Determines whether a single-precision floating-point value represents an even integer.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="value"/> is an integer and divisible by 2;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// IsEven(4f);     // returns true
        /// IsEven(3f);     // returns false
        /// IsEven(2.5f);   // returns false
        /// IsEven(-6f);    // returns true
        /// IsEven(0f);     // returns true
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEvenInteger(float value)
        {
            // Check whether the value is an integer
            if (value % 1 != 0)
                return false;

            return ((int)value % 2) == 0;
        }

        /// <summary>
        /// Determines whether the specified single-precision floating-point value is NaN (Not a Number).
        /// This method uses bit-level inspection.
        /// </summary>
        /// <param name="value">The value to evaluate.</param>
        /// <returns><c>true</c> if the value is NaN; otherwise, <c>false</c>.</returns>
        /// <example>
        /// <code>
        /// IsNaNFast(float.NaN);               // returns true
        /// IsNaNFast(0f);                      // returns false
        /// IsNaNFast(float.PositiveInfinity);  // returns false
        /// IsNaNFast(float.NegativeInfinity);  // returns false
        /// IsNaNFast(1f / 0f);                 // returns false (infinity)
        /// IsNaNFast(0f / 0f);                 // returns true (NaN)
        /// </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaNFast(float value)
        {
            FloatUnion u = new() { FloatValue = value };
            uint exp = u.UintValue & 0x7F800000;
            uint man = u.UintValue & 0x007FFFFF;
            return exp == 0x7F800000 && man != 0;
        }

        /// <summary>
        /// Provides a union view of a single-precision floating-point value and its
        /// underlying 32-bit unsigned integer representation.
        /// </summary>
        /// <remarks>
        /// This struct allows bit-level inspection and manipulation of a <see cref="float"/>
        /// value by exposing its raw binary representation as a <see cref="uint"/>.
        /// </remarks>
        [StructLayout(LayoutKind.Explicit)]
        public struct FloatUnion
        {
            /// <summary>
            /// The single-precision floating-point value.
            /// </summary>
            [FieldOffset(0)] public float FloatValue;

            /// <summary>
            /// The 32-bit unsigned integer representation of the floating-point value.
            /// </summary>
            [FieldOffset(0)] public uint UintValue;
        }
    }
}
