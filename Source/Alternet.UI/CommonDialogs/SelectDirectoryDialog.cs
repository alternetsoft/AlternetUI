namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a directory.
    /// </summary>
    [ControlCategory("Dialogs")]
    public class SelectDirectoryDialog : CommonDialog
    {
        private readonly Native.SelectDirectoryDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="SelectDirectoryDialog"/>.
        /// </summary>
        public SelectDirectoryDialog()
        {
            nativeDialog = new Native.SelectDirectoryDialog();
        }

        /// <summary>
        /// Gets or sets a string containing the directory name selected in the file dialog window.
        /// </summary>
        public string? DirectoryName
        {
            get
            {
                CheckDisposed();
                return nativeDialog.DirectoryName;
            }

            set
            {
                CheckDisposed();
                nativeDialog.DirectoryName = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial directory displayed by the file dialog window.
        /// </summary>
        public string? InitialDirectory
        {
            get
            {
                CheckDisposed();
                return nativeDialog.InitialDirectory;
            }

            set
            {
                CheckDisposed();
                nativeDialog.InitialDirectory = value;
            }
        }

        /// <inheritdoc/>
        public override string? Title
        {
            get
            {
                CheckDisposed();
                return nativeDialog.Title;
            }

            set
            {
                CheckDisposed();
                nativeDialog.Title = value;
            }
        }

        /// <inheritdoc/>
        public override ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();
            var nativeOwner = owner == null ?
                null : ((WindowHandler)owner.Handler).NativeControl;
            return (ModalResult)nativeDialog.ShowModal(nativeOwner);
        }
    }
}