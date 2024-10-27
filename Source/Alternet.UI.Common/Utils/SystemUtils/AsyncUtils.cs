using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties related to async execution and tasks.
    /// </summary>
    public static class AsyncUtils
    {
        /// <summary>
        /// Calls <see cref="Thread.Interrupt"/> and clears ref parameter.
        /// </summary>
        /// <param name="thread">The thread to interrupt.</param>
        public static void EndThread(ref Thread? thread)
        {
            thread?.Interrupt();
            thread = null;
        }

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
