using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to processor.
    /// </summary>
    public static class CpuUtils
    {
        /// <summary>
        /// Converts two bytes to <see cref="short"/> value.
        /// </summary>
        /// <param name="ch0">First byte.</param>
        /// <param name="ch1">Second byte.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short BytesToInt16(byte ch0, byte ch1)
        {
            int num = ch1;
            num |= ch0 << 8;
            return (short)num;
        }

        /// <summary>
        /// Converts four bytes to <see cref="int"/> value.
        /// </summary>
        /// <param name="ch0">First byte.</param>
        /// <param name="ch1">Second byte.</param>
        /// <param name="ch2">Third byte.</param>
        /// <param name="ch3">Fourth byte.</param>
        /// <returns></returns>
        public static int BytesToInt(byte ch0, byte ch1, byte ch2, byte ch3)
        {
            int num = ch0;
            num |= (int)((uint)ch1 << 8);
            num |= (int)((uint)ch2 << 16);
            return num | (int)((uint)ch3 << 24);
        }

        /// <summary>
        /// Converts four chars to <see cref="int"/> value.
        /// Only low byte of the <see cref="char"/> is used in the conversion.
        /// </summary>
        /// <param name="ch0">First character.</param>
        /// <param name="ch1">Second character.</param>
        /// <param name="ch2">Third character.</param>
        /// <param name="ch3">Fourth character.</param>
        /// <returns></returns>
        public static int CharsToInt(char ch0, char ch1, char ch2, char ch3)
        {
            int num = ch0;
            num |= (int)((uint)ch1 << 8);
            num |= (int)((uint)ch2 << 16);
            return num | (int)((uint)ch3 << 24);
        }
    }
}