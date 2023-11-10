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