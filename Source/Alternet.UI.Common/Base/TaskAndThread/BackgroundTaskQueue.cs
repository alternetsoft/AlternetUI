using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI.Threading
{
    /// <summary>
    /// Implements background tasks queue.
    /// </summary>
    public class BackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<Task>> tasks = new ConcurrentQueue<Func<Task>>();
        private readonly SemaphoreSlim signal = new SemaphoreSlim(0);

        /// <summary>
        /// Enqueue a task.
        /// </summary>
        /// <param name="task">Task to enqueue.</param>
        public virtual void Enqueue(Func<Task> task)
        {
            tasks.Enqueue(task);

            // Signals that a task is available.
            signal.Release();
        }

        /// <summary>
        /// Dequeue and execute tasks.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the notification
        /// that operations should be canceled.</param>
        /// <returns></returns>
        public virtual async Task<Func<Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await signal.WaitAsync(cancellationToken);

            tasks.TryDequeue(out var task);
            return task;
        }
    }
}