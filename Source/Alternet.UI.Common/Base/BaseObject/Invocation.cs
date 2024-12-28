using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Alternet.UI
{
    internal class Invocation : IAsyncResult
    {
        private readonly Delegate? mTargetDelegate;
        private readonly object?[]? mTargetDelegateArgs;
        private readonly object mInvokeSyncObject = new();
        private readonly bool mSynchronous;
        private ManualResetEvent? mCompletedSyncEvent;

        public Invocation(
            Delegate? targetDelegate,
            object?[]? targetDelegateArgs,
            bool synchronous)
        {
            mTargetDelegate = targetDelegate;
            mTargetDelegateArgs = targetDelegateArgs;
            mSynchronous = synchronous;
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

        public bool CompletedSynchronously => IsCompleted && mSynchronous;

        public bool IsCompleted { get; private set; }

        public Action GetAction()
        {
            var sink = new Action(
                () =>
                {
                    try
                    {
                        ReturnValue = mTargetDelegate?.DynamicInvoke(mTargetDelegateArgs);
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
                mCompletedSyncEvent?.Set();
            }
        }
    }
}