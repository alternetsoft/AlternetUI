using System;
using System.Collections.Generic;
using System.Linq;
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
