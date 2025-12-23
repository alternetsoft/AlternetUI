using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using Microsoft.Maui.Controls;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="ContentPage"/> with <see cref="IDisposable"/> support.
    /// </summary>
    public partial class DisposableContentPage : ContentPage, IDisposable, IDisposableObject
    {
        private bool insideDisposing;
        private bool disposed;
        private bool runOnceAlreadyExecuted;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableContentPage"/> class.
        /// </summary>
        public DisposableContentPage()
        {
            this.SafeAreaEdges = Microsoft.Maui.SafeAreaEdges.All;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableContentPage"/> class.
        /// </summary>
        ~DisposableContentPage()
        {
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
        public bool IsDisposed
        {
            get => disposed;
            private set => disposed = true;
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

        bool IDisposableObject.DisposeHandle => false;

        /// <inheritdoc/>
        public bool DisposingOrDisposed => insideDisposing || disposed;

        /// <inheritdoc cref="BaseObject.SafeDispose"/>
        public static void SafeDispose<T>(ref T? disposable)
            where T : IDisposable
        {
            BaseObject.SafeDispose(ref disposable);
        }

        /// <inheritdoc cref="BaseObject.SafeDisposeObject"/>
        public static void SafeDisposeObject<T>(ref T? disposable)
            where T : IDisposableObject
        {
            BaseObject.SafeDisposeObject(ref disposable);
        }

        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        [Browsable(false)]
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
        [Conditional("DEBUG")]
        [Browsable(false)]
        public void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Required()
        {
        }

        void IDisposableObject.CheckDisposed() => this.CheckDisposed();


        /// <summary>
        /// Called once when the page appears for the first time.
        /// </summary>
        protected virtual void OnAppearingOnce()
        {
        }

        /// <inheritdoc/>
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Alternet.UI.TimerUtils.RunOnce(ref runOnceAlreadyExecuted, () =>
            {
                OnAppearingOnce();
            });
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
        /// <param name="disposing">Disposing scenario.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            Disposing = true;

            try
            {
                Disposed?.Invoke(this, EventArgs.Empty);

                if (disposing)
                    DisposeResources();
            }
            finally
            {
                Disposing = false;
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Override to dispose resources.
        /// Here we dispose all used object references.
        /// </summary>
        protected virtual void DisposeResources()
        {
        }
    }
}
