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
    public abstract class DisposableObject : BaseObject, IDisposable, IDisposableObject
    {
        private bool disposeHandle;
        private bool disposing;
        private bool disposed = false;
        private IntPtr handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        /// <param name="handle">Handle to unmanaged resources.</param>
        /// <param name="disposeHandle">Specifies whether to dispose handle using
        /// <see cref="DisposeUnmanagedResources"/>.</param>
        public DisposableObject(IntPtr handle, bool disposeHandle)
        {
            this.handle = handle;
            this.disposeHandle = disposeHandle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        public DisposableObject()
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
        /// Occurs when control is disposed.
        /// </summary>
        public event EventHandler? Disposed;

        /// <summary>
        /// Gets whether object is disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed => disposed;

        /// <summary>
        /// Gets whether object is currently disposing.
        /// </summary>
        [Browsable(false)]
        public bool Disposing => disposing;

        /// <summary>
        /// Gets handle to unmanaged resources.
        /// </summary>
        [Browsable(false)]
        public IntPtr Handle
        {
            get => handle;
            protected set => handle = value;
        }

        /// <summary>
        /// Gets or sets whether to dispose <see cref="Handle"/>.
        /// </summary>
        [Browsable(false)]
        public bool DisposeHandle
        {
            get => disposeHandle;
            protected set => disposeHandle = value;
        }

        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if
        /// <see cref="IsDisposed"/> is <c>true</c>.
        /// </summary>
        /// <remarks>
        /// Does nothing if DEBUG is not defined.
        /// </remarks>
        public void CheckDisposed()
        {
#if DEBUG
            if (IsDisposed)
                throw new ObjectDisposedException(null);
#endif
        }

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
        /// <param name="disposing">Disposing scenario</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposing = true;

                try
                {
                    Disposed?.Invoke(this, EventArgs.Empty);

                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                        DisposeManagedResources();

                    DisposeResources();

                    // Call the appropriate methods to clean up
                    // unmanaged resources here.
                    // If disposing is false,
                    // only the following code is executed.
                    if (handle != default && disposeHandle)
                    {
                        DisposeUnmanagedResources();
                        handle = default;
                    }
                }
                finally
                {
                    this.disposing = false;
                    disposed = true;
                }
            }
        }

        /// <summary>
        /// Override to dispose managed resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
        }

        /// <summary>
        /// Override to dispose resources.
        /// </summary>
        protected virtual void DisposeResources()
        {
        }

        /// <summary>
        /// Override to dispose <see cref="Handle"/> or other unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }
    }
}
