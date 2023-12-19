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
    }
}
