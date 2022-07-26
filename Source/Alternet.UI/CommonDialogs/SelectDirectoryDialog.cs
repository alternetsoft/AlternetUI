namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a directory.
    /// </summary>
    public sealed class SelectDirectoryDialog : CommonDialog
    {
        private Native.SelectDirectoryDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="SelectDirectoryDialog"/>.
        /// </summary>
        public SelectDirectoryDialog()
        {
            nativeDialog = new Native.SelectDirectoryDialog();
        }

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            CheckDisposed();
            var nativeOwner = owner == null ? null : ((NativeWindowHandler)owner.Handler).NativeControl;
            return (ModalResult)nativeDialog.ShowModal(nativeOwner);
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

        private protected override string? TitleCore
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
    }
}