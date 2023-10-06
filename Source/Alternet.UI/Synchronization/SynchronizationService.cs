using System;
using System.Threading;

namespace Alternet.UI
{
    internal static class SynchronizationService
    {
        public static bool InvokeRequired
        {
            get
            {
                var currentApplication = Application.Current;
                if (currentApplication?.IsDisposed ?? true)
                    return false;

                return currentApplication.InvokeRequired;
            }
        }

        public static IAsyncResult BeginInvoke(Delegate method, object?[] args)
        {
            var currentApplication = Application.Current;
            if (currentApplication?.IsDisposed ?? true)
                throw new InvalidOperationException();

            var invocation = new Invocation(method, args, synchronous: false);
            currentApplication.BeginInvoke(invocation.GetAction());
            return invocation;
        }

        public static object? EndInvoke(IAsyncResult result)
        {
            var invocation = result as Invocation;
            if (invocation == null)
                throw new ArgumentException("Invalid IAsyncResult.", nameof(result));

            result.AsyncWaitHandle.WaitOne();

            if (invocation.Exception != null)
                throw invocation.Exception;

            return invocation.ReturnValue;
        }

        public static object? Invoke(Delegate method, object?[] args)
        {
            if (!InvokeRequired)
                return method.DynamicInvoke(args);

            return EndInvoke(BeginInvoke(method, args));
        }

        internal class Invocation : IAsyncResult
        {
            private readonly Delegate mTargetDelegate;
            private readonly object?[] mTargetDelegateArgs;
            private ManualResetEvent? mCompletedSyncEvent;

            private object mInvokeSyncObject = new();
            private bool m_Synchronous;

            public Invocation(Delegate targetDelegate, object?[] targetDelegateArgs, bool synchronous)
            {
                mTargetDelegate = targetDelegate;
                mTargetDelegateArgs = targetDelegateArgs;
                m_Synchronous = synchronous;
            }

            ~Invocation()
            {
                mCompletedSyncEvent?.Close();
            }

            public object? AsyncState => null;

            public object? ReturnValue { get; private set; }

            public Exception? Exception { get; private set; }

            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    if (mCompletedSyncEvent == null)
                    {
                        lock (mInvokeSyncObject)
                        {
                            if (mCompletedSyncEvent == null)
                            {
                                mCompletedSyncEvent = new ManualResetEvent(false);
                                if (IsCompleted)
                                    mCompletedSyncEvent.Set();
                            }
                        }
                    }

                    return mCompletedSyncEvent;
                }
            }

            public bool CompletedSynchronously => IsCompleted && m_Synchronous;

            public bool IsCompleted { get; private set; }

            public Action GetAction()
            {
                var sink = new Action(
                    () =>
                    {
                        try
                        {
                            ReturnValue = mTargetDelegate.DynamicInvoke(mTargetDelegateArgs);
                        }
                        catch (Exception e)
                        {
                            Exception = e;
                        }
                        finally
                        {
                            Complete();
                        }
                    });

                return sink;
            }

            private void Complete()
            {
                lock (mInvokeSyncObject)
                {
                    IsCompleted = true;
                    if (mCompletedSyncEvent != null)
                    {
                        mCompletedSyncEvent.Set();
                    }
                }
            }
        }
    }
}