using System;
using System.Collections.Generic;
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
    public class DialogWindow : Window
    {
        /// <summary>
        /// Gets a value indicating whether this window is displayed modally.
        /// </summary>
        [Browsable(false)]
        public override bool Modal
        {
            get
            {
                CheckDisposed();
                return NativeControl.Modal;
            }
        }

        /// <summary>
        /// Gets or sets the modal result value, which is the value that is returned from the
        /// <see cref="ShowModal()"/> method.
        /// This property is set to <see cref="ModalResult.None"/> at the moment
        /// <see cref="ShowModal()"/> is called.
        /// </summary>
        [Browsable(false)]
        public virtual ModalResult ModalResult
        {
            get
            {
                CheckDisposed();
                return (ModalResult)NativeControl.ModalResult;
            }

            set
            {
                CheckDisposed();
                NativeControl.ModalResult = (Native.ModalResult)value;
            }
        }

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed.
        /// User interaction with all other windows in the application is disabled until the
        /// modal window is closed.
        /// </summary>
        /// <returns>
        /// The return value is the value of the <see cref="ModalResult"/> property before
        /// window closes.
        /// </returns>
        public virtual ModalResult ShowModal()
        {
            return ShowModal(Owner);
        }

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed.
        /// User interaction with all other windows in the application is disabled until the
        /// modal window is closed.
        /// </summary>
        /// <param name="owner">
        /// A window that will own this window.
        /// </param>
        /// <returns>
        /// The return value is the value of the <see cref="ModalResult"/> property before
        /// window closes.
        /// </returns>
        public virtual ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();

            ModalResult = ModalResult.None;

            ApplyStartLocation(owner);
            NativeControl.ShowModal(owner?.WxWidget ?? default);

            return ModalResult;
        }

        internal override WindowKind GetWindowKind() => WindowKind.Dialog;
    }
}
