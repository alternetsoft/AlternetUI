using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class Window
    {
        private ModalResult modalResult = ModalResult.None;
        private bool isModal = false;
        private Action<bool>? onCloseModal;
        private ConcurrentStack<PropInstanceAndValue.SavedPropertiesItem>? disabledControls;

        /// <summary>
        /// Gets a value indicating whether this window is displayed modally.
        /// </summary>
        [Browsable(false)]
        public virtual bool Modal
        {
            get
            {
                return isModal;
            }
        }

        /// <summary>
        /// Gets or sets the modal result value, which is the value that is used when dialog
        /// is shown in order to close it immediately.
        /// </summary>
        [Browsable(false)]
        public virtual ModalResult ModalResult
        {
            get
            {
                return modalResult;
            }

            set
            {
                if (!Modal)
                    return;
                if (modalResult == value)
                    return;
                modalResult = value;

                PropInstanceAndValue.PopPropertiesMultiple(disabledControls);
                disabledControls = null;
                LastShownAsDialogTime = null;
                Close(WindowCloseAction.Hide);
                isModal = false;
                onCloseModal?.Invoke(modalResult == ModalResult.Accepted);
                onCloseModal = null;
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
            if (DisposingOrDisposed || Modal)
                return;

            modalResult = ModalResult.None;
            isModal = true;
            disabledControls = PropInstanceAndValue.DisableAllFormsChildrenExcept(this);
            onCloseModal = onClose;
            LastShownAsDialogTime = DateTime.Now;

            try
            {
                App.DoEvents();
                ApplyStartLocationOnce(owner);
                ActiveControl?.SetFocusIfPossible();
                App.DoEvents();
                ShowAndFocus();
            }
            catch
            {
                PropInstanceAndValue.PopPropertiesMultiple(disabledControls);
                disabledControls = null;
                throw;
            }
        }
    }
}
