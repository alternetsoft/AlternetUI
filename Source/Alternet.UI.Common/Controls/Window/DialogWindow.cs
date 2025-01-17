﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// A dialog box is a <see cref="Window"/> descendant with a title bar and sometimes
    /// a system menu, which can be moved around the screen. It can contain controls and
    /// other windows and is often used to allow the user to make some choice
    /// or to answer a question.
    /// </summary>
    public partial class DialogWindow : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogWindow"/> class.
        /// </summary>
        public DialogWindow()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogWindow"/> class.
        /// </summary>
        /// <param name="windowKind">Window kind to use instead of default value.</param>
        /// <remarks>
        /// Fo example, this constructor allows to use window as control
        /// (specify <see cref="WindowKind.Control"/>) as a parameter.
        /// </remarks>
        public DialogWindow(WindowKind windowKind)
            : base(windowKind)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this window is displayed modally.
        /// </summary>
        [Browsable(false)]
        public override bool Modal
        {
            get
            {
                if (DisposingOrDisposed)
                    return false;
                return Handler.IsModal;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ModalResult"/> of the ESC key.
        /// </summary>
        /// <remarks>
        /// Set this property to <see cref="ModalResult.Canceled"/> if you want to
        /// close modal dialog when ESC key is pressed.
        /// </remarks>
        public virtual ModalResult EscModalResult { get; set; } = ModalResult.None;

        /// <summary>
        /// Gets or sets <see cref="ModalResult"/> of the ENTER key.
        /// </summary>
        /// <remarks>
        /// Set this property to <see cref="ModalResult.Accepted"/> if you want to
        /// close modal dialog when ENTER key is pressed.
        /// </remarks>
        public virtual ModalResult EnterModalResult { get; set; } = ModalResult.None;

        /// <summary>
        /// Gets or sets the modal result value, which is the value that is used when dialog
        /// is shown in order to close it immediately.
        /// </summary>
        [Browsable(false)]
        public virtual ModalResult ModalResult
        {
            get
            {
                if (DisposingOrDisposed)
                    return ModalResult.Canceled;

                return Handler.ModalResult;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.ModalResult = value;
            }
        }

        /// <summary>
        /// Runs a dialog asynchroniously.
        /// </summary>
        /// <param name="onClose">Action to call after dialog is closed.</param>
        /// <param name="owner">A window that will own this window.</param>
        /// <remarks>
        /// On some platforms dialogs are shown synchroniously and application waits
        /// until dialog is closed.
        /// </remarks>
        public virtual void ShowDialogAsync(Window? owner = null, Action<bool>? onClose = null)
        {
            if (DisposingOrDisposed)
                return;

            App.DoEvents();
            ApplyStartLocationOnce(owner);
            ActiveControl?.SetFocusIfPossible();
            App.DoEvents();

            Handler.ShowModalAsync(owner, (result) =>
            {
                onClose?.Invoke(result == ModalResult.Accepted);
            });
        }

        /// <inheritdoc/>
        public override WindowKind GetWindowKind() => GetWindowKindOverride() ?? WindowKind.Dialog;

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            base.OnKeyDown(e);

            if (!Modal || e.Handled || e.ModifierKeys != UI.ModifierKeys.None)
                return;

            if(EscModalResult != ModalResult.None && e.Key == Key.Escape)
            {
                ModalResult = EscModalResult;
                e.Handled = true;
            }
            else if (EnterModalResult != ModalResult.None && e.Key == Key.Enter)
            {
                ModalResult = EnterModalResult;
                e.Handled = true;
            }
        }
    }
}
