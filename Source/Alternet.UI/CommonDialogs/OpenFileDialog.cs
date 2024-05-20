using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a standard dialog window that prompts the user to open a file.
    /// </summary>
    [ControlCategory("Dialogs")]
    public class OpenFileDialog : FileDialog
    {
        /// <summary>
        /// Gets default <see cref="OpenFileDialog"/> instance.
        /// </summary>
        public static OpenFileDialog Default = defaultDialog ??= new OpenFileDialog();

        private static OpenFileDialog? defaultDialog;

        /// <summary>
        /// Gets or sets whether the user may only select files that actually exist.
        /// </summary>
        /// <remarks>
        /// For open dialog only. Notice that under MacOS the file open dialog always behaves as
        /// if this style was specified, because it is impossible to choose a file that
        /// doesn't exist from a standard MacOS file dialog.
        /// </remarks>
        public virtual bool FileMustExist
        {
            get
            {
                CheckDisposed();
                return NativeDialog.FileMustExist;
            }

            set
            {
                CheckDisposed();
                NativeDialog.FileMustExist = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog window allows multiple files
        /// to be selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if the dialog window allows multiple files to be selected together
        /// or concurrently;
        /// otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        public virtual bool AllowMultipleSelection
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
        [Browsable(false)]
        public virtual string[] FileNames
        {
            get
            {
                CheckDisposed();
                return NativeDialog.FileNames;
            }
        }

        private protected override bool IsOpenDialog => true;
    }
}