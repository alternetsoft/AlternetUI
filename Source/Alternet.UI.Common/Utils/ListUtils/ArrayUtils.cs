using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to arrays.
    /// </summary>
    public static class ArrayUtils
    {
        /// <summary>
        /// Checks whether two arrays are equal.
        /// </summary>
        /// <typeparam name="T">Type of the items.</typeparam>
        /// <param name="left">First array to compare.</param>
        /// <param name="right">Second array to compare.</param>
        /// <returns></returns>
        public static bool AreNotEqual<T>(T[]? left, T[]? right)
        {
            const bool Changed = true;
            const bool NotChanged = false;

            if (left is null)
            {
                if (right is null)
                    return NotChanged;
                else
                    return Changed;
            }
            else
            {
                if (right is null)
                    return Changed;
                else
                {
                    var length = left.Length;

                    if (length != right.Length)
                        return Changed;

                    for (int i = 0; i < length; i++)
                    {
                        if (!Equals(left[i], right[i]))
                            return Changed;
                    }

                    return NotChanged;
                }
            }
        }

        /// <summary>
        /// Checks whether portions of two byte arrays are equal.
        /// </summary>
        /// <param name="a1">First array to compare.</param>
        /// <param name="a2">Second array to compare.</param>
        /// <param name="start">Starting position for the compare operation.</param>
        /// <param name="length">Number of bytes to compare.</param>
        /// <returns></returns>
        public static bool AreEqual(byte[] a1, byte[] a2, int start, int length)
        {
            if (length <= 0)
                return true;

            var end = start + length;

            if (a1.Length < end || a2.Length < end)
                return false;

            var span1 = a1.AsSpan(start, length);
            var span2 = a2.AsSpan(start, length);

            return span1.SequenceEqual(span2);
        }

        /// <summary>
        /// Checks whether two byte arrays are equal.
        /// </summary>
        /// <param name="a1">First array to compare.</param>
        /// <param name="a2">Second array to compare.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreEqual(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
        {
            return a1.SequenceEqual(a2);
        }

        /// <summary>
        /// Fills specified array with the given value.
        /// </summary>
        /// <param name="a">Array to fill.</param>
        /// <param name="value">Value used to fill the array.</param>
        /// <typeparam name="T">Type of value.</typeparam>
        public static void Fill<T>(ref T[] a, T value)
        {
#if NETSTANDARD2_1_OR_GREATER
        #error Add using Array.Fill here
#else
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = value;
            }
#endif
        }

        /// <summary>
        /// Fills each element of the byte array with it's index.
        /// </summary>
        /// <param name="a">Array to fill.</param>
        public static void FillWithIndex(ref byte[] a)
        {
            var length = Math.Min(a.Length, byte.MaxValue);

            for (byte i = 0; i < length; i++)
            {
                a[i] = i;
            }
        }
    }
}
