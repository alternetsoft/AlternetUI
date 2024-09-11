#define ObsoleteModalDialogs

using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the base class used for displaying standard system dialog windows on the screen.
    /// </summary>
    [ControlCategory("Hidden")]
    public abstract class CommonDialog : BaseComponent
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
#if ObsoleteModalDialogs
        [Obsolete("Method is deprecated. Use ShowAsync method instead of it.")]
#endif
        public ModalResult ShowModal()
        {
#pragma warning disable
            return ShowModal(null);
#pragma warning restore
        }

        /// <summary>
        /// Runs a common dialog window with a default owner asynchroniously.
        /// </summary>
        /// <param name="onClose">Action to call after dialog is closed.</param>
        /// <remarks>
        /// On some platforms dialogs are shown synchroniously and application waits
        /// until dialog is closed.
        /// </remarks>
        public void ShowAsync(Action<CommonDialog, bool>? onClose)
        {
            ShowAsync(null, onClose);
        }

        /// <summary>
        /// Runs a common dialog window with the specified owner asynchroniously.
        /// </summary>
        /// <param name="onClose">Action to call after dialog is closed.</param>
        /// <param name="owner">
        /// A window that will own the dialog.
        /// </param>
        /// <remarks>
        /// On some platforms dialogs are shown synchroniously and application waits
        /// until dialog is closed.
        /// </remarks>
        public virtual void ShowAsync(Window? owner, Action<CommonDialog, bool>? onClose)
        {
#pragma warning disable
            var result = ShowModal(owner);
#pragma warning restore
            var resultAsBool = result == ModalResult.Accepted;
            onClose?.Invoke(this, resultAsBool);
        }

        /// <summary>
        /// Same as <see cref="ShowModal()"/>. Added for compatibility.
        /// </summary>
        /// <returns></returns>
#if ObsoleteModalDialogs
        [Obsolete("Method is deprecated. Use ShowAsync method instead of it.")]
#endif
        public DialogResult ShowDialog()
        {
#pragma warning disable
            return EnumUtils.Convert(ShowModal(null));
#pragma warning restore
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
#if ObsoleteModalDialogs
        [Obsolete("Method is deprecated. Use ShowAsync method instead of it.")]
#endif
        public ModalResult ShowModal(Window? owner)
        {
            if (!IsValidShowDialog())
                return ModalResult.Canceled;
            CheckDisposed();

            var result = Handler.ShowModal(owner);

            if (result == ModalResult.Accepted)
            {
                var boolResult = CoerceDialogResult(true);

                if (boolResult)
                    result = ModalResult.Accepted;
                else
                    result = ModalResult.Canceled;
            }
            else
                result = ModalResult.Canceled;

            return result;
        }

        /// <summary>
        /// Called after dialog is closed. Override to coerce dialog result.
        /// </summary>
        /// <param name="dialogResult">Whether dialog is accepted.</param>
        /// <returns></returns>
        protected virtual bool CoerceDialogResult(bool dialogResult)
        {
            return dialogResult;
        }

        /// <summary>
        /// Called before dialog is shown. Override to check whether dialog can be shown
        /// using current settings. Should return <c>false</c> if dialog will not be shown.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsValidShowDialog()
        {
            return true;
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
            SafeDispose(ref handler);
        }
    }
}