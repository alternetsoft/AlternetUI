using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the base class used for displaying standard system dialog windows on the screen.
    /// </summary>
    [ControlCategory("Hidden")]
    public abstract class CommonDialog : DisposableObject, IDisposable
    {
        /// <summary>
        /// Gets or sets the dialog window title.
        /// </summary>
        public abstract string? Title { get; set; }

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
        public abstract ModalResult ShowModal(Window? owner);
    }
}