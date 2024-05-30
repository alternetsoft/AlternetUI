using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a directory.
    /// </summary>
    [ControlCategory("Dialogs")]
    public class SelectDirectoryDialog : CommonDialog
    {
        /// <summary>
        /// Gets default <see cref="SelectDirectoryDialog"/> instance.
        /// </summary>
        public static SelectDirectoryDialog Default = defaultDialog ??= new();

        private static SelectDirectoryDialog? defaultDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="SelectDirectoryDialog"/>.
        /// </summary>
        public SelectDirectoryDialog()
        {
        }

        /// <summary>
        /// Gets or sets a string containing the directory name selected in the file dialog window.
        /// </summary>
        public virtual string? DirectoryName
        {
            get
            {
                CheckDisposed();
                return Handler.DirectoryName;
            }

            set
            {
                CheckDisposed();
                Handler.DirectoryName = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial directory displayed by the file dialog window.
        /// </summary>
        public virtual string? InitialDirectory
        {
            get
            {
                CheckDisposed();
                return Handler.InitialDirectory;
            }

            set
            {
                CheckDisposed();
                Handler.InitialDirectory = value;
            }
        }

        [Browsable(false)]
        public new ISelectDirectoryDialogHandler Handler => (ISelectDirectoryDialogHandler)base.Handler;

        protected override IDialogHandler CreateHandler()
        {
            return DialogFactory.Handler.CreateSelectDirectoryDialogHandler(this);
        }
    }
}