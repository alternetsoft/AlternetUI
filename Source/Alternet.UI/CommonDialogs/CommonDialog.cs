using System;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the base class used for displaying standard system dialog windows on the screen.
    /// </summary>
    public abstract class CommonDialog : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Gets or sets the file dialog window title.
        /// </summary>
        public string? Title
        {
            get
            {
                CheckDisposed();
                return TitleCore;
            }

            set
            {
                CheckDisposed();
                TitleCore = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed of.
        /// </summary>
        public bool IsDisposed { get; private set; }

        private protected abstract string? TitleCore { get; set; }

        /// <summary>
        /// Runs a common dialog window with a default owner.
        /// </summary>
        /// <returns>
        /// <see cref="ModalResult.Accepted"/> if the user clicks OK in the dialog window;
        /// otherwise, <see cref="ModalResult.Canceled"/>.
        /// </returns>
        public ModalResult ShowModal()
        {
            CheckDisposed();
            return ShowModal(null);
        }

        /// <summary>
        /// Runs a common dialog window with the specified owner.
        /// </summary>
        /// <param name="owner">
        /// A window that will own the modal dialog.
        /// </param>
        /// <returns>
        /// <see cref="ModalResult.Accepted"/> if the user clicks OK in the dialog window;
        /// otherwise, <see cref="ModalResult.Canceled"/>.
        /// </returns>
        public ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();
            return ShowModalCore(owner);
        }

        private protected void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        private protected abstract ModalResult ShowModalCore(Window? owner);

        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                }

                isDisposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}