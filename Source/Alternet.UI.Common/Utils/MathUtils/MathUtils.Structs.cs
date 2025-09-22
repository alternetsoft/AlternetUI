using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    public static partial class MathUtils
    {
        /// <summary>
        /// Provides a union for accessing the bitwise representation of a <see cref="double"/> value.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct DoubleUnion
        {
            /// <summary>
            /// The double-precision floating-point value.
            /// </summary>
            [FieldOffset(0)]
            public double DoubleValue;

            /// <summary>
            /// The unsigned 64-bit integer representation of the double value.
            /// </summary>
            [FieldOffset(0)]
            public ulong ULongValue;

            /// <summary>
            /// Represents the value of the field as a 64-bit signed integer.
            /// </summary>
            /// <remarks>This field is part of a structure with explicit layout,
            /// and its offset is set to 0.</remarks>
            [FieldOffset(0)]
            public long LongValue;

            /// <summary>
            /// Converts the specified double-precision floating-point number to its
            /// equivalent 64-bit signed integer representation.
            /// </summary>
            /// <param name="value">The double-precision floating-point number to convert.</param>
            /// <returns>The 64-bit signed integer representation of the specified double value.</returns>
            public static long AsLong(double value)
            {
                DoubleUnion u = new() { DoubleValue = value };
                return u.LongValue;
            }

            /// <summary>
            /// Converts the specified double-precision floating-point number to its
            /// equivalent 64-bit unsigned integer representation.
            /// </summary>
            /// <param name="value">The double-precision floating-point number to convert.</param>
            /// <returns>The 64-bit unsigned integer representation of the specified double value.</returns>
            public static ulong AsULong(double value)
            {
                DoubleUnion u = new() { DoubleValue = value };
                return u.ULongValue;
            }
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
            /// Represents the value of the field as a single-precision floating-point number.
            /// </summary>
            /// <remarks>This field is part of a structure that uses explicit layout.</remarks>
            [FieldOffset(0)]
            public float FloatValue;

            /// <summary>
            /// Represents the value of the field as an unsigned 32-bit integer.
            /// </summary>
            /// <remarks>This field is part of a structure with explicit layout,
            /// and its offset is set to 0.</remarks>
            [FieldOffset(0)]
            public uint UIntValue;

            /// <summary>
            /// Represents the integer value stored at the specified memory offset.
            /// </summary>
            /// <remarks>This field is part of a structure that uses explicit layout.
            /// It shares memory
            /// with other fields in the structure, as determined
            /// by the <see cref="FieldOffsetAttribute"/>.</remarks>
            [FieldOffset(0)]
            public int IntValue;

            /// <summary>
            /// Converts the specified single-precision floating-point number to its
            /// equivalent 32-bit integer representation.
            /// </summary>
            /// <remarks>
            /// This method performs a bitwise reinterpretation of the floating-point value
            /// as an integer. The result depends on the internal binary representation
            /// of the input value.</remarks>
            /// <param name="value">The single-precision floating-point number to convert.</param>
            /// <returns>The 32-bit integer representation of the specified floating-point number.</returns>
            public static int AsInt(float value)
            {
                FloatUnion u = new() { FloatValue = value };
                return u.IntValue;
            }

            /// <summary>
            /// Converts the specified single-precision floating-point number to its
            /// equivalent 32-bit unsigned integer representation.
            /// </summary>
            /// <remarks>
            /// This method performs a bitwise reinterpretation of the floating-point value
            /// as an unsigned integer. The result depends on the internal binary representation
            /// of the input value.</remarks>
            /// <param name="value">The single-precision floating-point number to convert.</param>
            /// <returns>The 32-bit unsigned integer representation
            /// of the specified floating-point number.</returns>
            public static uint AsUInt(float value)
            {
                FloatUnion u = new() { FloatValue = value };
                return u.UIntValue;
            }
        }
    }
}
