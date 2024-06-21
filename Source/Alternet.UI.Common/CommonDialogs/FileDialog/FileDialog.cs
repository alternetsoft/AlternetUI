using System;
using System.ComponentModel;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a file.
    /// </summary>
    [ControlCategory("Hidden")]
    public abstract class FileDialog : CommonDialog
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FileDialog"/>.
        /// </summary>
        public FileDialog()
        {
        }

        /// <summary>
        /// Gets or sets whether to direct the dialog to return the path and file name of the
        /// selected shortcut file, not
        /// its target as it does by default.
        /// </summary>
        /// <remarks>
        /// Currently this flag is only implemented in Windows
        /// and MacOs (where it prevents aliases from being resolved).
        /// The non-dereferenced link path is always returned, even without this flag, under Linux
        /// and so using it there doesn't do anything.
        /// </remarks>
        public virtual bool NoShortcutFollow
        {
            get
            {
                CheckDisposed();
                return Handler.NoShortcutFollow;
            }

            set
            {
                CheckDisposed();
                Handler.NoShortcutFollow = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to change the current working directory (when the dialog is
        /// dismissed) to the directory where the file(s) chosen by the user are.
        /// </summary>
        public virtual bool ChangeDir
        {
            get
            {
                CheckDisposed();
                return Handler.ChangeDir;
            }

            set
            {
                CheckDisposed();
                Handler.ChangeDir = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show the preview of the selected files (currently only
        /// supported by Linux port).
        /// </summary>
        public virtual bool PreviewFiles
        {
            get
            {
                CheckDisposed();
                return Handler.PreviewFiles;
            }

            set
            {
                CheckDisposed();
                Handler.PreviewFiles = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show hidden files.
        /// </summary>
        public virtual bool ShowHiddenFiles
        {
            get
            {
                CheckDisposed();
                return Handler.ShowHiddenFiles;
            }

            set
            {
                CheckDisposed();
                Handler.ShowHiddenFiles = value;
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

        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that appear
        /// in the "Save as file type" or "Files of type" box in the dialog window.
        /// </summary>
        /// <value>
        /// The file filtering options available in the dialog window.
        /// </value>
        /// <remarks>
        /// For each filtering option, the filter string contains a description of the filter,
        /// followed by the vertical bar (|) and the filter pattern. The strings for different
        /// filtering options are separated by the vertical bar.
        /// The following is an example of a filter string:
        /// <code>Text files(*.txt)|*.txt|All files(*.*)|*.*</code>
        /// You can add several filter patterns to a filter by separating the file types with
        /// semicolons, for example:
        /// <code>Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files(*.*)|*.*</code>
        /// Use the <see cref="SelectedFilterIndex"/> property to set which filtering option
        /// is shown first to the user.
        /// </remarks>
        public virtual string? Filter
        {
            get
            {
                CheckDisposed();
                return Handler.Filter;
            }

            set
            {
                CheckDisposed();
                Handler.Filter = value;
            }
        }

        /// <summary>
        /// Same as <see cref="SelectedFilterIndex"/>. Added for the compatibility.
        /// </summary>
        [Browsable(false)]
        public int FilterIndex
        {
            get
            {
                return SelectedFilterIndex;
            }

            set
            {
                SelectedFilterIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the filter currently selected in the file dialog window.
        /// </summary>
        public virtual int SelectedFilterIndex
        {
            get
            {
                CheckDisposed();
                return Handler.SelectedFilterIndex;
            }

            set
            {
                CheckDisposed();
                Handler.SelectedFilterIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a string containing the file name selected in the file dialog window.
        /// </summary>
        public virtual string? FileName
        {
            get
            {
                CheckDisposed();
                return Handler.FileName;
            }

            set
            {
                CheckDisposed();
                Handler.FileName = value;
            }
        }

        /// <summary>
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public new IFileDialogHandler Handler => (IFileDialogHandler)base.Handler;
    }
}