using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the base class used for displaying standard system dialog windows on the screen.
    /// </summary>
    [ControlCategory("Hidden")]
    public abstract class CommonDialog : DisposableObject
    {
        private IDialogHandler? handler;

        /// <summary>
        /// Gets or sets the dialog window title.
        /// </summary>
        public virtual string? Title
        {
            get
            {
                CheckDisposed();
                return Handler.Title;
            }

            set
            {
                CheckDisposed();
                Handler.Title = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the the <b>Help</b> button is displayed.
        /// </summary>
        /// <remarks>
        /// Not implemented on all platforms.
        /// </remarks>
        public virtual bool ShowHelp
        {
            get
            {
                return Handler.ShowHelp;
            }

            set
            {
                Handler.ShowHelp = value;
            }
        }

        /// <summary>
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public IDialogHandler Handler => handler ??= CreateHandler();

        /// <summary>
        /// Runs a common dialog window with a default owner.
        /// </summary>
        /// <returns>
        /// <see cref="ModalResult.Accepted"/> if the user clicks OK in the dialog window;
        /// otherwise, <see cref="ModalResult.Canceled"/>.
        /// </returns>
        public ModalResult ShowModal()
        {
            return ShowModal(null);
        }

        /// <summary>
        /// Same as <see cref="ShowModal()"/>. Added for compatibility.
        /// </summary>
        /// <returns></returns>
        public DialogResult ShowDialog()
        {
            return EnumUtils.Convert(ShowModal(null));
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
        public virtual ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();
            return Handler.ShowModal(owner);
        }

        /// <summary>
        /// Creates dialog handler.
        /// </summary>
        /// <returns></returns>
        protected virtual IDialogHandler CreateHandler()
        {
            return null!;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            handler?.Dispose();
            handler = null;
        }
    }
}