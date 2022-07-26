using System;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a dialog window from which the user can select a file.
    /// </summary>
    public abstract class FileDialog : CommonDialog
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
        /// Gets or sets the current file name filter string, which determines the choices that appear
        /// in the "Save as file type" or "Files of type" box in the dialog window.
        /// </summary>
        /// <value>
        /// The file filtering options available in the dialog window.
        /// </value>
        public string Filter
        {
            get
            {
                CheckDisposed();
                NativeApi.FileDialog_GetFilters(
                    NativePointer,
                    out var filters,
                    out _);

                if (filters == null)
                    return new FileDialogFilter[0];

                return filters.Select(x => new FileDialogFilter(x.FileNamePattern, x.Description)).ToArray();
            }

            set
            {
                CheckDisposed();

                var filters =
                    value?.Select(x => new NativeApi.Filter { Description = x.Description, FileNamePattern = x.FileNamePattern }).ToArray() ??
                    new NativeApi.Filter[0];

                NativeApi.FileDialog_SetFilters(
                    NativePointer,
                    filters,
                    filters.Length);
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
                return NativeApi.FileDialog_GetSelectedFilterIndex(NativePointer);
            }

            set
            {
                CheckDisposed();
                NativeApi.FileDialog_SetSelectedFilterIndex(NativePointer, value);
            }
        }

        void CheckFileNameAccessWithMultipleSelectionAllowed()
        {
            if (DialogMode != NativeApi.DialogMode.OpenFile)
                return;

            if (((OpenFileDialog)this).AllowMultipleSelection)
                throw new InvalidOperationException("AllowMultipleSelection property is set to true. Please use FileNames property instead of FileName.");
        }

        /// <summary>
        /// Gets or sets a string containing the file name selected in the file dialog window.
        /// </summary>
        public string? FileName
        {
            get
            {
                CheckDisposed();
                CheckFileNameAccessWithMultipleSelectionAllowed();
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