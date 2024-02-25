using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// OS related static methods
    /// </summary>
    public static class OSUtils
    {
        /// <summary>
        /// Suspends the execution of the current thread for a specified interval.
        /// </summary>
        /// <param name="milliSeconds">Specifies time, in milliseconds, for which
        /// to suspend execution.</param>
        public static void Sleep(int milliSeconds)
        {
            Thread.Sleep(milliSeconds);
        }

        /// <summary>
        /// Retrieves the low-order word from the specified value.
        /// </summary>
        /// <param name="value">Specifies the value to be converted.</param>
        /// <returns>The return value is the low-order word of the specified value.</returns>
        public static short LoWord(IntPtr value)
        {
            return (short)value.ToInt32();
        }

        /// <summary>
        /// Retrieves the high-order word from the given value.
        /// </summary>
        /// <param name="value">Specifies the value to be converted.</param>
        /// <returns>The return value is the high-order word of the specified value.</returns>
        public static short HiWord(IntPtr value)
        {
            return (short)(value.ToInt32() >> 16);
        }

        internal static short BytesToInt16(byte ch0, byte ch1)
        {
            int num = ch1;
            num |= ch0 << 8;
            return (short)num;
        }

        internal static int BytesToInt(byte ch0, byte ch1, byte ch2, byte ch3)
        {
            return MmioFOURCC((char)ch3, (char)ch2, (char)ch1, (char)ch0);
        }

        internal static int MmioFOURCC(char ch0, char ch1, char ch2, char ch3)
        {
            int num = 0;
            num |= ch0;
            num |= (int)((uint)ch1 << 8);
            num |= (int)((uint)ch2 << 16);
            return num | (int)((uint)ch3 << 24);
        }
    }
}
