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
        /// Gets or sets default value for the <see cref="AllowMultipleSelection"/> property.
        /// </summary>
        public static bool DefaultAllowMultipleSelection = false;

        /// <summary>
        /// Gets or sets default value for the <see cref="FileMustExist"/> property.
        /// </summary>
        public static bool DefaultFileMustExist = true;

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
                if (DisposingOrDisposed)
                    return default;
                return Handler.FileMustExist;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.FileMustExist = value;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.AllowMultipleSelection;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.AllowMultipleSelection = value;
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
                if (DisposingOrDisposed)
                    return [];
                return Handler.FileNames;
            }
        }

        /// <summary>
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public new IOpenFileDialogHandler Handler => (IOpenFileDialogHandler)base.Handler;

        /// <summary>
        /// Displays a file selection dialog and invokes the specified callback with
        /// the selected file's path.
        /// </summary>
        /// <remarks>The dialog enforces that the selected file must exist and allows
        /// only a single file
        /// to be selected. The file filter is set to display all files.</remarks>
        /// <param name="onSelectFile">A callback to be invoked with the full
        /// path of the selected file. </param>
        /// <param name="filter">The value for <see cref="FileDialog.Filter"/> property.
        /// If not specified, <see cref="FileMaskUtils.FileDialogFilterAllFiles"/> is used.</param>
        public static void SelectFile(Action<string> onSelectFile, string? filter = null)
        {
            var dialog = OpenFileDialog.Default;
            dialog.FileMustExist = true;
            dialog.AllowMultipleSelection = false;
            dialog.Filter = FileMaskUtils.FileDialogFilterAllFiles;
            dialog.ShowAsync(() =>
            {
                if(dialog.FileName is not null)
                    onSelectFile(dialog.FileName);
            });
        }

        /// <inheritdoc/>
        public override void Reset()
        {
            base.Reset();
            AllowMultipleSelection = DefaultAllowMultipleSelection;
            FileMustExist = DefaultFileMustExist;
        }

        /// <inheritdoc/>
        protected override IDialogHandler CreateHandler()
        {
            return DialogFactory.Handler.CreateOpenFileDialogHandler(this);
        }
    }
}