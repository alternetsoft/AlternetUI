using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to <see cref="DateTime"/>.
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// Gets <see cref="DateTime"/> format used in Javascript
        /// or in other situations.
        /// </summary>
        public static string DateFormatJs { get; set; } = "yyyy-MM-ddTHH:mm:ss.fffK";

        /// <summary>
        /// Subtracts <see cref="DateTime.Now"/>
        /// with the specified timestamp and gets absolute value of the result.
        /// </summary>
        /// <param name="timestamp">Value to subtract from the <see cref="DateTime.Now"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetAbsDistanceWithNow(long timestamp)
        {
            return Math.Abs(DateTime.Now.Ticks - timestamp);
        }

        /// <summary>
        /// Converts milliseconds to ticks.
        /// </summary>
        /// <param name="msec">Value to convert.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TicksFromMilliseconds(long msec)
        {
            return msec * TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Converts ticks to milliseconds.
        /// </summary>
        /// <param name="ticks">Value to convert.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long TicksToMilliseconds(long ticks)
        {
            return ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Gets current time in milliseconds.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetNowInMilliseconds()
            => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        /// <summary>
        /// Gets current time in milliseconds using
        /// <see cref="DateTimeOffset.ToUnixTimeMilliseconds()"/>.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetNowInUnixMilliseconds() => DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}
