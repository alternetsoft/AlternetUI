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
