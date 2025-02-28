using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI.Threading
{
    public class BackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<Task>> tasks = new ConcurrentQueue<Func<Task>>();
        private readonly SemaphoreSlim signal = new SemaphoreSlim(0);

        // Enqueue a task
        public void Enqueue(Func<Task> task)
        {
            tasks.Enqueue(task);
            signal.Release(); // Signal that a task is available
        }

        // Dequeue and execute tasks
        public async Task<Func<Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await signal.WaitAsync(cancellationToken);

            tasks.TryDequeue(out var task);
            return task;
        }
    }
}