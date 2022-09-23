using System;
using System.ComponentModel;
using System.Threading;

namespace Alternet.UI
{
    /// <summary>
    /// SynchronizationContext subclass used by AlterNET UI.
    /// </summary>
    public class SynchronizationContext : System.Threading.SynchronizationContext
    {
        [ThreadStatic]
        private static bool doNotAutoInstall;

        [ThreadStatic]
        private static bool contextInstallationInProgress;

        [ThreadStatic]
        private static System.Threading.SynchronizationContext? previousSynchronizationContext;

        private WeakReference? destinationThreadReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizationContext"/> class.
        /// </summary>
        public SynchronizationContext()
        {
            // store the current thread to ensure its still alive during an invoke.
            DestinationThread = Thread.CurrentThread;
        }

        private SynchronizationContext(Thread? destinationThread)
        {
            DestinationThread = destinationThread;
        }

        /// <summary>
        /// Determines whether we install the <see cref="SynchronizationContext"/> when we create a control, or
        /// when we start a message loop. Default: true.
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
                if ((destinationThreadReference != null) && (destinationThreadReference.IsAlive))
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
        /// Uninstalls the currently installed <see cref="SynchronizationContext"/> and replaces it with the previously installed context.
        /// </summary>
        /// <remarks>
        /// If the previously installed <see cref="SynchronizationContext"/> is <c>null</c>, the current context is set to <c>null</c>.
        /// If the currently installed synchronization context is not a <see cref="SynchronizationContext"/>, this method does nothing.
        /// </remarks>
        public static void Uninstall() => Uninstall(false);

        /// <summary>
        /// Dispatches a synchronous message to a synchronization context
        /// </summary>
        /// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        /// <remarks>
        /// If the destination thread no longer exists or the value of its Thread.IsAlive property is false, the Send method raises an InvalidAsynchronousStateException.
        /// It is up to the caller to determine what further action to take.
        /// </remarks>
        public override void Send(SendOrPostCallback d, object? state)
        {
            var destinationThread = DestinationThread;
            if (destinationThread == null || !destinationThread.IsAlive)
                throw new InvalidAsynchronousStateException();

            SynchronizationService.Invoke(d, new object?[] { state });
        }

        /// <summary>
        /// Copies the synchronization context.
        /// </summary>
        /// <returns>A copy of the synchronization context.</returns>
        public override System.Threading.SynchronizationContext CreateCopy() => new SynchronizationContext(DestinationThread);

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
            SynchronizationService.BeginInvoke(d, new object?[] { state });
        }

        /// <summary>
        /// Instantiate and install a op sync context, and save off the old one.
        /// </summary>
        internal static void InstallIfNeeded()
        {
            if (!AutoInstall || contextInstallationInProgress)
                return;

            if (Current == null)
                previousSynchronizationContext = null;

            if (previousSynchronizationContext != null)
                return;

            contextInstallationInProgress = true;
            try
            {
                var currentContext = AsyncOperationManager.SynchronizationContext;

                // Make sure we either have no sync context or that we have one of type SynchronizationContext
                if (currentContext == null || currentContext.GetType() == typeof(System.Threading.SynchronizationContext))
                {
                    previousSynchronizationContext = currentContext;
                    AsyncOperationManager.SynchronizationContext = new SynchronizationContext();
                }
            }
            finally
            {
                contextInstallationInProgress = false;
            }
        }

        internal static void Uninstall(bool turnOffAutoInstall)
        {
            if (AutoInstall)
            {
                var syncContext = AsyncOperationManager.SynchronizationContext as SynchronizationContext;
                if (syncContext != null)
                {
                    try
                    {
                        if (previousSynchronizationContext == null)
                        {
                            AsyncOperationManager.SynchronizationContext = new System.Threading.SynchronizationContext();
                        }
                        else
                        {
                            AsyncOperationManager.SynchronizationContext = previousSynchronizationContext;
                        }
                    }
                    finally
                    {
                        previousSynchronizationContext = null;
                    }
                }
            }

            if (turnOffAutoInstall)
                AutoInstall = false;
        }
    }
}