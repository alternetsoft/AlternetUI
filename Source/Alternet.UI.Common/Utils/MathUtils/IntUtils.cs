using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to <see cref="int"/>.
    /// </summary>
    public static class IntUtils
    {
        /// <summary>
        /// Determines whether two integers have the same evenness (both even or both odd).
        /// </summary>
        /// <param name="a">The first integer to compare.</param>
        /// <param name="b">The second integer to compare.</param>
        /// <returns><c>true</c> if both integers are either even
        /// or odd; otherwise, <c>false</c>.</returns>
        public static bool IsEqualEven(int a, int b)
        {
            return IsEven(a) == IsEven(b);
        }

        /// <summary>
        /// Determines whether the specified integer value is even.
        /// </summary>
        /// <param name="value">The integer value to check.</param>
        /// <returns><c>true</c> if the value is even; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// Gets an integer value as a binary string (containing 0 and 1).
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AsBinaryString(int value)
        {
            return Convert.ToString(value, 2).PadLeft(32, '0');
        }

        /// <summary>
        /// Sets a bit at the specified position to 1.
        /// </summary>
        /// <param name="value">Original value.</param>
        /// <param name="position">Bit position.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SetBit(int value, int position)
        {
            return value |= 1 << position;
        }

        /// <summary>
        /// Sets a bit at the specified position to 0.
        /// </summary>
        /// <param name="value">Original value.</param>
        /// <param name="position">Bit position.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int UnsetBit(int value, int position)
        {
            return value & ~(1 << position);
        }

        /// <summary>
        /// Checks whether a bit at the specified position is equal to 1.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="position">Bit position.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBitSet(int value, int position)
        {
            return (value & (1 << position)) != 0;
        }

        /// <summary>
        /// Checks whether a bit at the specified position is equal to 0.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="position">Bit position.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBitUnset(int value, int position)
        {
            return !IsBitSet(value, position);
        }
    }
}
