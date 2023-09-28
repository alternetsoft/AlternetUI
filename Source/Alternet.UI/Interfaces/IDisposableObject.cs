using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="IDisposable"/> with new events and properties.
    /// </summary>
    public interface IDisposableObject : IDisposable
    {
        /// <summary>
        /// Occurs when control is disposed.
        /// </summary>
        event EventHandler? Disposed;

        /// <summary>
        /// Gets whether object is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets handle to unmanaged resources.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Gets or sets whether to dispose <see cref="Handle"/>.
        /// </summary>
        bool DisposeHandle { get; }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if
        /// <see cref="IsDisposed"/> is <c>true</c>.
        /// </summary>
        void CheckDisposed();
    }
}