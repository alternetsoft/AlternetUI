#pragma warning disable
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public enum FileDialogMode
    {
        Open,
        Save
    }

    // https://docs.wxwidgets.org/3.2/classwx_file_dialog.html
    public class FileDialog
    {
        /// <summary>
        /// Gets or sets prompt for a confirmation if a file will be overwritten.
        /// </summary>
        /// <remarks>
        /// This style is for Save dialog only, is always enabled on MacOs and cannot be disabled. 
        /// </remarks>
        public bool OverwritePrompt { get; set; }

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
        public bool NoShortcutFollow { get; set; }

        /// <summary>
        /// Gets or sets whether the user may only select files that actually exist.
        /// </summary>
        /// <remarks>
        /// For open dialog only. Notice that under MacOS the file open dialog always behaves as
        /// if this style was specified, because it is impossible to choose a file that
        /// doesn't exist from a standard MacOS file dialog.
        /// </remarks>
        public bool FileMustExist { get; set; }

        /// <summary>
        /// Gets or sets whether to change the current working directory (when the dialog is
        /// dismissed) to the directory where the file(s) chosen by the user are.
        /// </summary>
        public bool ChangeDir { get; set; }

        /// <summary>
        /// Gets or sets whether to show the preview of the selected files (currently only
        /// supported by Linux port).
        /// </summary>
        public bool PreviewFiles { get; set; }

        /// <summary>
        /// Gets or sets whether to show hidden files. 
        /// </summary>
        public bool ShowHiddenFiles { get; set; }

        public FileDialogMode Mode { get; set; }

        public string? InitialDirectory { get; set; }

        public string? Title { get; set; }

        public string? Filter { get; set; }

        public int SelectedFilterIndex { get; set; }

        public string? FileName { get; set; }

        public bool AllowMultipleSelection { get; set; }

        public string[] FileNames { get; }

        public ModalResult ShowModal(Window? owner) => throw new Exception();
    }
}