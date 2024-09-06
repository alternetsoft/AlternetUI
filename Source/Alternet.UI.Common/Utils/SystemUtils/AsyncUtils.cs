using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties related to async execution and tasks.
    /// </summary>
    public static class AsyncUtils
    {
        /// <summary>
        /// Runs the specified task synchroniously.
        /// </summary>
        /// <param name="func"></param>
        public static void Sync(Func<Task> func) => Task.Run(func).ConfigureAwait(false);

        /// <summary>
        /// Runs the specified task synchroniously and returns result.
        /// </summary>
        /// <param name="func"></param>
        public static T Sync<T>(Func<Task<T>> func) => Task.Run(func).Result;
    }
}
