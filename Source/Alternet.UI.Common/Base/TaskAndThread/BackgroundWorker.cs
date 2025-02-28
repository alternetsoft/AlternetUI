using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI.Threading
{
    public class BackgroundWorker
    {
        private readonly BackgroundTaskQueue taskQueue;
        private readonly CancellationTokenSource cancellationTokenSource
            = new CancellationTokenSource();

        private static BackgroundWorker? defaultWorker;

        public BackgroundWorker()
            : this(new BackgroundTaskQueue())
        {
        }

        public BackgroundWorker(BackgroundTaskQueue taskQueue)
        {
            this.taskQueue = taskQueue;
        }

        public static BackgroundWorker Default
        {
            get
            {
                if(defaultWorker is null)
                {
                    defaultWorker = new();
                    defaultWorker.Start();
                }

                return defaultWorker;
            }
        }

        public BackgroundTaskQueue TaskQueue => taskQueue;

        public void Start()
        {
            Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var task = await taskQueue.DequeueAsync(cancellationTokenSource.Token);
                    if (task != null)
                    {
                        try
                        {
                            await task();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error executing task: {ex}");
                        }
                    }
                }
            });
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}