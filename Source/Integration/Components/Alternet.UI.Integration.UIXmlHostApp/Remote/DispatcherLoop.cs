using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Alternet.UI.Integration.Remoting;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    public sealed class DispatcherLoop : IDisposable
    {
        #region Instance
        private DispatcherLoop() { }

        static Dictionary<int, DispatcherLoop> dispatcherLoops = new();
        public static DispatcherLoop Current
        {
            get
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (dispatcherLoops.ContainsKey(threadId))
                    return dispatcherLoops[threadId];

                DispatcherLoop dispatcherLoop = new()
                {
                    ThreadId = Thread.CurrentThread.ManagedThreadId
                };
                dispatcherLoops.Add(threadId, dispatcherLoop);
                return dispatcherLoop;
            }
        }
        #endregion

        bool isDisposed = false;
        public void Dispose()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);

            _queue.CompleteAdding();
            _queue.Dispose();
            dispatcherLoops.Remove(ThreadId);
            isDisposed = true;
        }

        public int ThreadId { get; private set; } = -1;
        public bool IsRunning { get; private set; } = false;

        BlockingCollection<Task> _queue = new();
        public void Run()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);

            if (ThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new InvalidOperationException($"The {nameof(DispatcherLoop)} has been created for a different thread!");

            if (IsRunning)
                throw new InvalidOperationException("Already running!");

            IsRunning = true;

            try
            {
                // ToDo: `RunSynchronously` is not guaranteed to be executed on this thread (see comments below)!
                foreach (var task in _queue.GetConsumingEnumerable())
                    task?.RunSynchronously();
            }
            catch (ObjectDisposedException) { }

            IsRunning = false;
        }

        public void BeginInvoke(Task task)
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);

            if (ThreadId == Thread.CurrentThread.ManagedThreadId)
                task?.RunSynchronously();
            else
                _queue.Add(task);
        }

        public void Invoke(Action action)
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);

            Task task = new(action);
            BeginInvoke(task);
            task.GetAwaiter().GetResult();
        }

        public T Invoke<T>(Func<T> action)
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);

            Task<T> task = new(action);
            BeginInvoke(task);
            return task.GetAwaiter().GetResult();
        }
    }
}
