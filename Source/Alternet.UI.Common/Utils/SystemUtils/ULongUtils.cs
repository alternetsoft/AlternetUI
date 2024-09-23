using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to <see cref="ulong"/>.
    /// </summary>
    public static class ULongUtils
    {
        /// <summary>
        /// Sets a bit at the specified position to 1.
        /// </summary>
        /// <param name="value">Original value.</param>
        /// <param name="position">Bit position.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong SetBit(ulong value, int position)
        {
            return value |= 1UL << position;
        }

        /// <summary>
        /// Sets a bit at the specified position to 0.
        /// </summary>
        /// <param name="value">Original value.</param>
        /// <param name="position">Bit position.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong UnsetBit(ulong value, int position)
        {
            return value & ~(1UL << position);
        }

        /// <summary>
        /// Checks whether a bit at the specified position is equal to 1.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="position">Bit position.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBitSet(ulong value, int position)
        {
            return (value & (1UL << position)) != 0;
        }

        /// <summary>
        /// Checks whether a bit at the specified position is equal to 0.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="position">Bit position.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBitUnset(ulong value, int position)
        {
            return (value & (1UL << position)) == 0;
        }
    }
}
