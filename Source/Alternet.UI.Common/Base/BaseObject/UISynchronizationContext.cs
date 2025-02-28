using System;
using System.ComponentModel;
using System.Threading;

namespace Alternet.UI
{
    /// <summary>
    /// SynchronizationContext subclass used by AlterNET UI.
    /// </summary>
    internal class UISynchronizationContext : System.Threading.SynchronizationContext
    {
        [ThreadStatic]
        private static bool doNotAutoInstall = true;

        [ThreadStatic]
        private static bool contextInstallationInProgress;

        [ThreadStatic]
        private static System.Threading.SynchronizationContext? previousSynchronizationContext;

        private WeakReference? destinationThreadReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="UISynchronizationContext"/> class.
        /// </summary>
        public UISynchronizationContext()
        {
            // store the current thread to ensure its still alive during an invoke.
            DestinationThread = Thread.CurrentThread;
        }

        private UISynchronizationContext(Thread? destinationThread)
        {
            DestinationThread = destinationThread;
        }

        /// <summary>
        /// Determines whether we install the <see cref="UISynchronizationContext"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static bool AutoInstall
        {
            get
            {
                return !doNotAutoInstall;
            }

            set
            {
                doNotAutoInstall = !value;
            }
        }

        private Thread? DestinationThread
        {
            get
            {
                if ((destinationThreadReference != null) && destinationThreadReference.IsAlive)
                    return destinationThreadReference.Target as Thread;

                return null;
            }

            set
            {
                if (value != null)
                {
                    destinationThreadReference = new WeakReference(value);
                }
            }
        }

        /// <summary>
        /// Uninstalls the currently installed <see cref="UISynchronizationContext"/> and
        /// replaces it with the previously installed context.
        /// </summary>
        /// <remarks>
        /// If the previously installed <see cref="UISynchronizationContext"/> is <c>null</c>,
        /// the current context is set to <c>null</c>.
        /// If the currently installed synchronization context is not
        /// a <see cref="UISynchronizationContext"/>, this method does nothing.
        /// </remarks>
        public static void Uninstall()
        {
            if (AutoInstall)
            {
                if (AsyncOperationManager.SynchronizationContext is UISynchronizationContext)
                {
                    try
                    {
                        if (previousSynchronizationContext == null)
                        {
                            AsyncOperationManager.SynchronizationContext =
                                new System.Threading.SynchronizationContext();
                        }
                        else
                        {
                            AsyncOperationManager.SynchronizationContext =
                                previousSynchronizationContext;
                        }
                    }
                    finally
                    {
                        previousSynchronizationContext = null;
                    }
                }
            }
        }

        /// <summary>
        /// Instantiate and install a op sync context, and save off the old one.
        /// </summary>
        public static void InstallIfNeeded()
        {
            if (!AutoInstall || contextInstallationInProgress)
                return;
            if (App.IsMaui)
                return;

            if (Current == null)
                previousSynchronizationContext = null;

            if (previousSynchronizationContext != null)
                return;

            contextInstallationInProgress = true;
            try
            {
                var currentContext = AsyncOperationManager.SynchronizationContext;

                // Make sure we either have no sync context or that we have
                // one of type SynchronizationContext
                if (currentContext == null
                    || currentContext.GetType() == typeof(System.Threading.SynchronizationContext))
                {
                    previousSynchronizationContext = currentContext;
                    AsyncOperationManager.SynchronizationContext = new UISynchronizationContext();
                }
            }
            finally
            {
                contextInstallationInProgress = false;
            }
        }

        /// <summary>
        /// Dispatches a synchronous message to a synchronization context
        /// </summary>
        /// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        /// <remarks>
        /// If the destination thread no longer exists or the value of its
        /// Thread.IsAlive property is false, the Send method raises
        /// an InvalidAsynchronousStateException.
        /// It is up to the caller to determine what further action to take.
        /// </remarks>
        public override void Send(SendOrPostCallback d, object? state)
        {
            var destinationThread = DestinationThread;
            if (destinationThread == null || !destinationThread.IsAlive)
                throw new InvalidAsynchronousStateException();

            BaseObject.Invoke(d, new object?[] { state });
        }

        /// <summary>
        /// Copies the synchronization context.
        /// </summary>
        /// <returns>A copy of the synchronization context.</returns>
        public override System.Threading.SynchronizationContext CreateCopy()
            => new UISynchronizationContext(DestinationThread);

        /// <summary>
        /// Dispatches an asynchronous message to a synchronization context.
        /// </summary>
        /// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        /// <remarks>
        /// The Post method starts an asynchronous request to post a message.
        /// </remarks>
        public override void Post(SendOrPostCallback d, object? state)
        {
            BaseObject.Invoke(d, new object?[] { state });
        }
    }
}