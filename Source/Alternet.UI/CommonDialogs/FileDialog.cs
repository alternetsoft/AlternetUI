using System;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a file.
    /// </summary>
    [ControlCategory("Hidden")]
    public abstract class FileDialog : CommonDialog
    {
        private readonly Native.FileDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="FileDialog"/>.
        /// </summary>
        public FileDialog()
        {
            nativeDialog = new Native.FileDialog
            {
                Mode = this.Mode,
            };
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
        public bool NoShortcutFollow
        {
            get
            {
                CheckDisposed();
                return nativeDialog.NoShortcutFollow;
            }

            set
            {
                CheckDisposed();
                nativeDialog.NoShortcutFollow = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to change the current working directory (when the dialog is
        /// dismissed) to the directory where the file(s) chosen by the user are.
        /// </summary>
        public bool ChangeDir
        {
            get
            {
                CheckDisposed();
                return nativeDialog.ChangeDir;
            }

            set
            {
                CheckDisposed();
                nativeDialog.ChangeDir = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show the preview of the selected files (currently only
        /// supported by Linux port).
        /// </summary>
        public bool PreviewFiles
        {
            get
            {
                CheckDisposed();
                return nativeDialog.PreviewFiles;
            }

            set
            {
                CheckDisposed();
                nativeDialog.PreviewFiles = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show hidden files.
        /// </summary>
        public bool ShowHiddenFiles
        {
            get
            {
                CheckDisposed();
                return nativeDialog.ShowHiddenFiles;
            }

            set
            {
                CheckDisposed();
                nativeDialog.ShowHiddenFiles = value;
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
        public string? Filter
        {
            get
            {
                CheckDisposed();
                return nativeDialog.Filter;
            }

            set
            {
                CheckDisposed();
                nativeDialog.Filter = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the filter currently selected in the file dialog window.
        /// </summary>
        public int SelectedFilterIndex
        {
            get
            {
                CheckDisposed();
                return nativeDialog.SelectedFilterIndex;
            }

            set
            {
                CheckDisposed();
                nativeDialog.SelectedFilterIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a string containing the file name selected in the file dialog window.
        /// </summary>
        public string? FileName
        {
            get
            {
                CheckDisposed();
                return nativeDialog.FileName;
            }

            set
            {
                CheckDisposed();
                nativeDialog.FileName = value;
            }
        }

        private protected Native.FileDialog NativeDialog => nativeDialog;

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

        private protected abstract Native.FileDialogMode Mode { get; }

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            CheckDisposed();
            var nativeOwner = owner == null ? null
                : ((NativeWindowHandler)owner.Handler).NativeControl;
            var result = (ModalResult)nativeDialog.ShowModal(nativeOwner);
            return result;
        }
    }
}