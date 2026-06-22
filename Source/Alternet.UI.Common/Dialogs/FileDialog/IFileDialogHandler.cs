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

        string? GetInitialDirectory();

        void SetInitialDirectory(string? value);

        string? GetFilter();

        void SetFilter(string? value);

        /// <inheritdoc cref="FileDialog.SelectedFilterIndex"/>
        int SelectedFilterIndex { get; set; }

        string? GetFileName();
        
        void SetFileName(string? value);
    }
}
