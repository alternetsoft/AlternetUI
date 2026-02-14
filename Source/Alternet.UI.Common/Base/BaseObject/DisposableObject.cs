using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    // https://learn.microsoft.com/ru-ru/dotnet/api/system.idisposable?view=net-7.0

    /// <summary>
    /// Provides a mechanism for releasing managed and unmanaged resources.
    /// </summary>
    public class DisposableObject : BaseObjectWithNotify, IDisposable, IDisposableObject
    {
        private bool disposeHandle;
        private bool insideDisposing;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        /// <param name="disposeHandle">Specifies whether to dispose handle using
        /// <see cref="DisposeUnmanaged"/>.</param>
        public DisposableObject(bool disposeHandle)
        {
            this.disposeHandle = disposeHandle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        public DisposableObject()
            : this(disposeHandle: true)
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        ~DisposableObject()
        {
            // Use C# finalizer syntax for finalization code.
            // This finalizer will run only if the Dispose method
            // does not get called.
            // It gives your base class the opportunity to finalize.
            // Do not provide finalizer in types derived from this class.

            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(disposing: false) is optimal in terms of
            // readability and maintainability.
            Dispose(disposing: false);
        }

        /// <summary>
        /// Occurs when object is disposed.
        /// This is static event, so it is raised for all instances of this class and its derived classes.
        /// </summary>
        public static event EventHandler? InstanceDisposed;

        /// <summary>
        /// Occurs when object is disposed.
        /// </summary>
        public event EventHandler? Disposed;

        /// <summary>
        /// Gets the disposable that does nothing when disposed.
        /// </summary>
        public static IDisposable EmptyDisposable => EmptyDisposableObject.Instance;

        /// <summary>
        /// Gets whether object is disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed
        {
            get => disposed;
            private set => disposed = true;
        }

        /// <summary>
        /// Gets whether this object is disposing or disposed.
        /// </summary>
        [Browsable(false)]
        public override bool DisposingOrDisposed
        {
            get
            {
                return disposed || insideDisposing || App.Terminating;
            }
        }

        /// <summary>
        /// Gets whether object is currently disposing.
        /// </summary>
        [Browsable(false)]
        public bool Disposing
        {
            get => insideDisposing;
            private set => insideDisposing = value;
        }

        /// <summary>
        /// Gets or sets whether to call <see cref="DisposeUnmanaged"/> method
        /// when this object is disposed.
        /// </summary>
        [Browsable(false)]
        public bool DisposeHandle
        {
            get => disposeHandle;
            protected set => disposeHandle = value;
        }

        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        [Browsable(false)]
        public void Dispose()
        {
            if (DisposingOrDisposed)
                return;

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds <paramref name="action"/> which will be executed one time
        /// when the application finished processing events and is
        /// about to enter the idle state.
        /// </summary>
        /// <param name="action">Action to call.</param>
        public virtual void RunWhenIdle(Action action)
        {
            App.AddIdleTask(() =>
            {
                if (IsDisposed)
                    return;
                action();
            });
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if
        /// <see cref="IsDisposed"/> is <c>true</c>.
        /// </summary>
        /// <remarks>
        /// Does nothing if DEBUG is not defined.
        /// </remarks>
        [Conditional("DEBUG")]
        [Browsable(false)]
        public virtual void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        void IDisposableObject.CheckDisposed() => this.CheckDisposed();

        /// <summary>
        /// Disposes object's resources.
        /// </summary>
        /// <remarks>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals <c>true</c>, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals <c>false</c>, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </remarks>
        /// <param name="disposing">Disposing scenario.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (DisposingOrDisposed)
                return;

            Disposing = true;

            try
            {
                Disposed?.Invoke(this, EventArgs.Empty);
                InstanceDisposed?.Invoke(this, EventArgs.Empty);

                if (disposing)
                    DisposeManaged();

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                if (disposeHandle)
                {
                    DisposeUnmanaged();
                }
            }
            finally
            {
                Disposing = false;
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Override to dispose managed resources.
        /// Here we dispose all used object references.
        /// </summary>
        protected virtual void DisposeManaged()
        {
        }

        /// <summary>
        /// Override to dispose unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanaged()
        {
        }

        /// <summary>
        /// Represents a disposable that does nothing on disposal.
        /// </summary>
        private sealed class EmptyDisposableObject : IDisposable
        {
            /// <summary>
            /// Singleton default disposable.
            /// </summary>
            public static readonly EmptyDisposableObject Instance = new();

            private EmptyDisposableObject()
            {
            }

            /// <summary>
            /// Does nothing.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}
