using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a directory.
    /// </summary>
    [ControlCategory(KnownControlCategory.Dialogs)]
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
                return Handler.GetDirectoryName();
            }

            set
            {
                CheckDisposed();
                Handler.SetDirectoryName(value);
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
                return Handler.GetInitialDirectory();
            }

            set
            {
                CheckDisposed();
                Handler.SetInitialDirectory(value);
            }
        }

        /// <summary>
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public new ISelectDirectoryDialogHandler Handler => (ISelectDirectoryDialogHandler)base.Handler;

        /// <summary>
        /// Creates a new instance of <see cref="SelectDirectoryDialog"/> that is a copy of the current instance.
        /// </summary>
        /// <returns>A new instance of <see cref="SelectDirectoryDialog"/> that is a copy of the current instance.</returns>
        public virtual SelectDirectoryDialog Clone()
        {
            CheckDisposed();
            var clone = new SelectDirectoryDialog();
            clone.Assign(this);
            return clone;
        }

        /// <summary>
        /// Assigns the properties of another <see cref="SelectDirectoryDialog"/> to this instance.
        /// </summary>
        /// <param name="source">The source <see cref="SelectDirectoryDialog"/> to copy properties from.</param>
        public virtual void Assign(SelectDirectoryDialog? source)
        {
            CheckDisposed();

            if (source == null)
            {
                Reset();
                return;
            }

            InitialDirectory = source.InitialDirectory;
            DirectoryName = source.DirectoryName;
        }

        /// <summary>
        /// Resets the dialog to its default state.
        /// </summary>
        public virtual void Reset()
        {
            InitialDirectory = null;
            DirectoryName = null;
        }

        /// <inheritdoc/>
        protected override IDialogHandler CreateHandler()
        {
            return DialogFactory.Handler.CreateSelectDirectoryDialogHandler(this);
        }
    }

    /// <summary>
    /// Represents a dummy implementation of <see cref="SelectDirectoryDialog"/> class.
    /// It can be used as a storage for values of <see cref="SelectDirectoryDialog"/> properties.
    /// </summary>
    public class DummySelectDirectoryDialog : SelectDirectoryDialog
    {
        /// <inheritdoc/>
        protected override IDialogHandler CreateHandler()
        {
            return new BaseSelectDirectoryDialogHandler();
        }
    }
}