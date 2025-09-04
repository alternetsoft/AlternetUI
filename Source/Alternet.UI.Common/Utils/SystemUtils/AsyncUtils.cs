using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        /// Runs a task with a specified timeout.
        /// </summary>
        /// <param name="taskFunc">The asynchronous function to execute.</param>
        /// <param name="timeoutMs">The timeout duration in milliseconds.</param>
        /// <returns><c>true</c> if the task completes within the timeout;
        /// otherwise, <c>false</c>.</returns>
        public static async Task<bool> RunWithTimeout(Func<Task> taskFunc, int timeoutMs)
        {
            var task = taskFunc();
            var timeoutTask = Task.Delay(timeoutMs);

            if (await Task.WhenAny(task, timeoutTask) == task)
            {
                await task; // Ensure exceptions are propagated
                return true; // Task completed within timeout
            }

            return false; // Timeout occurred
        }

        /// <summary>
        /// Calls <see cref="Thread.Interrupt"/> and clears ref parameter.
        /// </summary>
        /// <param name="thread">The thread to interrupt.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndThread(ref Thread? thread)
        {
            thread?.Interrupt();
            thread = null;
        }

        /// <summary>
        /// Runs the specified task synchronously.
        /// </summary>
        /// <param name="func"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sync(Func<Task> func) => Task.Run(func).ConfigureAwait(false);

        /// <summary>
        /// Runs the specified task synchronously and returns result.
        /// </summary>
        /// <param name="func"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sync<T>(Func<Task<T>> func) => Task.Run(func).Result;

        /// <summary>
        /// A utility class that asynchronously waits until a value of type T is assigned.
        /// </summary>
        /// <typeparam name="T">The type of value to wait for.</typeparam>
        public class ValueWaiter<T>
        {
            private readonly TaskCompletionSource<T> tcs = new();

            /// <summary>
            /// Returns a task that waits until a value of type T is assigned.
            /// </summary>
            public Task<T> WaitForValueAsync() => tcs.Task;

            /// <summary>
            /// Assigns a value and completes the waiting task.
            /// </summary>
            /// <param name="value">The value to set.</param>
            public void SetValue(T value)
            {
                if (!tcs.Task.IsCompleted)
                    tcs.SetResult(value);
            }
        }
    }
}
