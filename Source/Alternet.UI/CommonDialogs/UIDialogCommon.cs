using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the base class used for displaying dialog windows inherited from
    /// <see cref="Window"/> on the screen.
    /// </summary>
    [ControlCategory("Hidden")]
    internal class UIDialogCommon : DisposableObject
    {
        private string? title;
        private DialogWindow? window;

        /// <summary>
        /// Gets <see cref="Window"/> instance which implements dialog window.
        /// </summary>
        public DialogWindow DialogWindow
        {
            get
            {
                if(window == null)
                {
                    window = CreateDialogWindow();
                    UpdateTitle();
                }

                return window;
            }
        }

        /// <summary>
        /// Gets or sets the dialog window title.
        /// </summary>
        public virtual string? Title
        {
            get
            {
                if(window == null)
                    return title;
                return window.Title;
            }

            set
            {
                title = value;
                UpdateTitle();
            }
        }

        /// <summary>
        /// Runs a dialog window.
        /// </summary>
        /// <returns>
        /// <see cref="ModalResult.Accepted"/> if the user clicks OK in the dialog window;
        /// otherwise, <see cref="ModalResult.Canceled"/>.
        /// </returns>
        public virtual ModalResult ShowModal()
        {
            return DialogWindow.ShowModal();
        }

        /// <summary>
        /// Runs a dialog window with the specified owner.
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
            return DialogWindow.ShowModal(owner);
        }

        /// <summary>
        /// Creates <see cref="Window"/> instance which implements dialog window.
        /// </summary>
        /// <returns></returns>
        protected virtual DialogWindow CreateDialogWindow()
        {
            return new DialogWindow();
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            if (window == null)
                return;
            window.Dispose();
            window = null;
        }

        private void UpdateTitle()
        {
            if (window == null || title == null)
                return;
            window.Title = title;
        }
    }
}
