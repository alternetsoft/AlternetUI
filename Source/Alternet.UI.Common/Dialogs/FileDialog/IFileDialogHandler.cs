using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with open/save file dialog window.
    /// </summary>
    public interface IFileDialogHandler : IDialogHandler
    {
        /// <inheritdoc cref="FileDialog.NoShortcutFollow"/>
        bool NoShortcutFollow { get; set; }

        /// <inheritdoc cref="FileDialog.ChangeDir"/>
        bool ChangeDir { get; set; }

        /// <inheritdoc cref="FileDialog.PreviewFiles"/>
        bool PreviewFiles { get; set; }

        /// <inheritdoc cref="FileDialog.ShowHiddenFiles"/>
        bool ShowHiddenFiles { get; set; }

        /// <summary>
        /// Gets initial directory for the file dialog.
        /// </summary>
        /// <returns></returns>
        string? GetInitialDirectory();

        /// <summary>
        /// Sets initial directory for the file dialog.
        /// </summary>
        /// <param name="value">The initial directory.</param>
        void SetInitialDirectory(string? value);

        /// <summary>
        /// Gets file name filter for the file dialog.
        /// </summary>
        /// <returns>The file name filter.</returns>
        string? GetFilter();

        /// <summary>
        /// Sets file name filter for the file dialog.
        /// </summary>
        /// <param name="value">The file name filter.</param>
        void SetFilter(string? value);

        /// <inheritdoc cref="FileDialog.SelectedFilterIndex"/>
        int SelectedFilterIndex { get; set; }

        /// <summary>
        /// Gets the file name for the file dialog.
        /// </summary>
        /// <returns>The file name.</returns>
        string? GetFileName();

        /// <summary>
        /// Sets the file name for the file dialog.
        /// </summary>
        /// <param name="value">The file name.</param>
        void SetFileName(string? value);
    }
}
