using System;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a standard dialog window that prompts the user to open a file.
    /// </summary>
    public sealed class OpenFileDialog : FileDialog
    {
        private protected override Native.FileDialogMode Mode => Native.FileDialogMode.Open;

        /// <summary>
        /// Gets or sets a value indicating whether the dialog window allows multiple files to be selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if the dialog window allows multiple files to be selected together or concurrently;
        /// otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        public bool AllowMultipleSelection
        {
            get
            {
                CheckDisposed();
                return NativeDialog.AllowMultipleSelection;
            }

            set
            {
                CheckDisposed();
                NativeDialog.AllowMultipleSelection = value;
            }
        }

        /// <summary>
        /// Gets the file names of all selected files in the dialog window.
        /// </summary>
        public string[] FileNames
        {
            get
            {
                CheckDisposed();
                return NativeDialog.FileNames;
            }
        }
    }
}