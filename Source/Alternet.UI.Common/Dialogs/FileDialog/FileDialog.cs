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
        /// Gets or sets default value for the <see cref="NoShortcutFollow"/> property.
        /// </summary>
        public static bool DefaultNoShortcutFollow = false;

        /// <summary>
        /// Gets or sets default value for the <see cref="ChangeDir"/> property.
        /// </summary>
        public static bool DefaultChangeDir = false;

        /// <summary>
        /// Gets or sets default value for the <see cref="PreviewFiles"/> property.
        /// </summary>
        public static bool DefaultPreviewFiles = false;

        /// <summary>
        /// Gets or sets default value for the <see cref="ShowHiddenFiles"/> property.
        /// </summary>
        public static bool DefaultShowHiddenFiles = false;

        /// <summary>
        /// Gets or sets default value for the <see cref="InitialDirectory"/> property.
        /// </summary>
        public static string? DefaultInitialDirectory = null;

        /// <summary>
        /// Gets or sets default value for the <see cref="Filter"/> property.
        /// </summary>
        public static string? DefaultFilter = null;

        /// <summary>
        /// Gets or sets default value for the <see cref="FileName"/> property.
        /// </summary>
        public static string? DefaultFileName = null;

        /// <summary>
        /// Initializes a new instance of <see cref="FileDialog"/>.
        /// </summary>
        public FileDialog()
        {
            Reset();
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.NoShortcutFollow;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.ChangeDir;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.PreviewFiles;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.ShowHiddenFiles;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.InitialDirectory;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.Filter;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.SelectedFilterIndex;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
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
                if (DisposingOrDisposed)
                    return default;
                return Handler.FileName;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.FileName = value;
            }
        }

        /// <summary>
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public new IFileDialogHandler Handler => (IFileDialogHandler)base.Handler;

        /// <summary>
        /// Resets properties to the default values.
        /// </summary>
        public virtual void Reset()
        {
            NoShortcutFollow = DefaultNoShortcutFollow;
            ChangeDir = DefaultChangeDir;
            PreviewFiles = DefaultPreviewFiles;
            ShowHiddenFiles = DefaultShowHiddenFiles;
            InitialDirectory = DefaultInitialDirectory;
            Filter = DefaultFilter;
            FileName = DefaultFileName;
        }
    }
}