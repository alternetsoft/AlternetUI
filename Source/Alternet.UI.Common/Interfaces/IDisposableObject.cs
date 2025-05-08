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
    public interface IDisposableObject : IDisposable, IBaseObject
    {
        /// <summary>
        /// Occurs when object is disposed.
        /// </summary>
        event EventHandler? Disposed;

        /// <summary>
        /// Gets whether object is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets whether object is disposing or disposed.
        /// </summary>
        bool DisposingOrDisposed { get; }

        /// <summary>
        /// Gets whether to dispose resources.
        /// </summary>
        bool DisposeHandle { get; }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if
        /// <see cref="IsDisposed"/> is <c>true</c>.
        /// </summary>
        void CheckDisposed();
    }
}