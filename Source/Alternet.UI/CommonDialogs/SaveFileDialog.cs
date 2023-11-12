using System;

namespace Alternet.UI
{
    /// <summary>
    /// Prompts the user to select a location for saving a file.
    /// </summary>
    [ControlCategory("Dialogs")]
    public class SaveFileDialog : FileDialog
    {
        /// <summary>
        /// Gets or sets prompt for a confirmation if a file will be overwritten.
        /// </summary>
        /// <remarks>
        /// This style is for Save dialog only, is always enabled on MacOs and cannot be disabled.
        /// </remarks>
        public bool OverwritePrompt
        {
            get
            {
                CheckDisposed();
                return NativeDialog.OverwritePrompt;
            }

            set
            {
                CheckDisposed();
                NativeDialog.OverwritePrompt = value;
            }
        }

        /// <summary>
        /// Gets or sets whether an empty file name is allowed when dialog result
        /// is <see cref="ModalResult.Accepted"/>.
        /// </summary>
        /// <remarks>
        /// Default value is <c>false</c>. When empty file name is not allowed, dialog
        /// result is set to <see cref="ModalResult.Canceled"/> when file name is empty or
        /// <c>null</c>.
        /// </remarks>
        public bool AllowNullFileName { get; set; } = false;

        private protected override Native.FileDialogMode Mode => Native.FileDialogMode.Save;

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            var result = base.ShowModalCore(owner);
            if (result == ModalResult.Accepted)
            {
                if (!AllowNullFileName)
                {
                   if(string.IsNullOrEmpty(FileName))
                        return ModalResult.Canceled;
                }
            }

            return result;
        }

    }
}