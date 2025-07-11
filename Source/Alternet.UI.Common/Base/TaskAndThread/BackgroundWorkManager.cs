using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI.Threading
{
    /// <summary>
    /// Implements background worker which can execute tasks in the background.
    /// Use <see cref="TaskQueue"/> property in order to manage tasks.
    /// Use <see cref="Start"/> and <see cref="Stop"/> methods to control
    /// background worker execution.
    /// </summary>
    public class BackgroundWorkManager : BaseObject
    {
        private static readonly object defaultWorkerLocker = new();
        private static BackgroundWorkManager? defaultWorker;

        private readonly BackgroundTaskQueue taskQueue;
        private readonly CancellationTokenSource cancellationTokenSource = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorkManager"/> class
        /// </summary>
        public BackgroundWorkManager()
            : this(new BackgroundTaskQueue())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorkManager"/> class
        /// with the specified tasks queue.
        /// </summary>
        /// <param name="taskQueue">Tasks queue.</param>
        public BackgroundWorkManager(BackgroundTaskQueue taskQueue)
        {
            this.taskQueue = taskQueue;
        }

        /// <summary>
        /// Get default background worker. <see cref="Start"/> is called automatically when
        /// it is first accessed.
        /// </summary>
        public static BackgroundWorkManager Default
        {
            get
            {
                lock (defaultWorkerLocker)
                {
                    if (defaultWorker is null)
                    {
                        defaultWorker = new();
                        defaultWorker.Start();
                    }
                }

                return defaultWorker;
            }

            set
            {
                defaultWorker = value;
            }
        }

        /// <summary>
        /// Gets tasks queue.
        /// </summary>
        public virtual BackgroundTaskQueue TaskQueue => taskQueue;

        /// <summary>
        /// Starts execution of the background tasks.
        /// </summary>
        public virtual void Start()
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
                            var t = task();
                            if (t.Status == TaskStatus.Created)
                                t.Start();
                            await t;
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                        catch (Exception ex)
                        {
                            Invoke(() =>
                            {
                                App.OnThreadException(ex);
                            });
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Stops execution of the background tasks.
        /// </summary>
        public virtual void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}