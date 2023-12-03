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
    }
}
