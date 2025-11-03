using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI.Threading
{
    /// <summary>
    /// Implements background tasks queue.
    /// </summary>
    public class BackgroundTaskQueue : BaseObject
    {
        private readonly ConcurrentQueue<Func<Task>> tasks = new ();
        private readonly SemaphoreSlim signal = new (0);

        /// <summary>
        /// Gets a value indicating whether the queue has tasks.
        /// </summary>
        public virtual bool HasTasks
        {
            get
            {
                return !tasks.IsEmpty;
            }
        }

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
        public virtual async Task<Func<Task>?> DequeueAsync(CancellationToken cancellationToken)
        {
            await signal.WaitAsync(cancellationToken);

            tasks.TryDequeue(out var task);
            return task;
        }
    }
}