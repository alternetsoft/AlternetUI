using System;
using System.Threading;

namespace Alternet.UI
{
    static class SynchronizationService
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

        class Invocation : IAsyncResult
        {
            readonly Delegate m_TargetDelegate;
            readonly object?[] m_TargetDelegateArgs;
            ManualResetEvent? m_CompletedSyncEvent;

            object m_InvokeSyncObject = new object();
            bool m_Synchronous;

            public Invocation(Delegate targetDelegate, object?[] targetDelegateArgs, bool synchronous)
            {
                m_TargetDelegate = targetDelegate;
                m_TargetDelegateArgs = targetDelegateArgs;
                m_Synchronous = synchronous;
            }

            ~Invocation()
            {
                if (m_CompletedSyncEvent != null)
                    m_CompletedSyncEvent.Close();
            }

            public object? AsyncState => null;

            public object? ReturnValue { get; private set; }

            public Exception? Exception { get; private set; }

            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    if (m_CompletedSyncEvent == null)
                    {
                        lock (m_InvokeSyncObject)
                        {
                            if (m_CompletedSyncEvent == null)
                            {
                                m_CompletedSyncEvent = new ManualResetEvent(false);
                                if (IsCompleted)
                                    m_CompletedSyncEvent.Set();
                            }
                        }
                    }

                    return m_CompletedSyncEvent;
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
                            ReturnValue = m_TargetDelegate.DynamicInvoke(m_TargetDelegateArgs);
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

            void Complete()
            {
                lock (m_InvokeSyncObject)
                {
                    IsCompleted = true;
                    if (m_CompletedSyncEvent != null)
                    {
                        m_CompletedSyncEvent.Set();
                    }
                }
            }
        }
    }
}