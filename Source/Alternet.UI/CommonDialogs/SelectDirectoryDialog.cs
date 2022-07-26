namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a directory.
    /// </summary>
    public sealed class SelectDirectoryDialog : CommonDialog
    {
        private protected override ModalResult ShowModalCore(Window? owner)
        {
            CheckDisposed();
            return NativeApi.FileDialog_ShowModal(NativePointer, owner?.NativePointer ?? IntPtr.Zero);
        }

        /// <summary>
        /// Gets or sets the initial directory displayed by the file dialog window.
        /// </summary>
        public string? InitialDirectory
        {
            get
            {
                CheckDisposed();
                return NativeApi.FileDialog_GetInitialDirectory(NativePointer);
            }

            set
            {
                CheckDisposed();
                NativeApi.FileDialog_SetInitialDirectory(NativePointer, value);
            }
        }

        private protected override string TitleCore
        {
            get
            {
                CheckDisposed();
                return NativeApi.FileDialog_GetTitle(NativePointer);
            }

            set
            {
                CheckDisposed();
                NativeApi.FileDialog_SetTitle(NativePointer, value);
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
                return NativeApi.FileDialog_GetFileName(NativePointer);
            }

            set
            {
                CheckDisposed();
                NativeApi.FileDialog_SetFileName(NativePointer, value);
            }
        }
    }
}