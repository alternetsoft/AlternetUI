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

        /// <inheritdoc cref="FileDialog.InitialDirectory"/>
        string? InitialDirectory { get; set; }

        /// <inheritdoc cref="FileDialog.Filter"/>
        string? Filter { get; set; }

        /// <inheritdoc cref="FileDialog.SelectedFilterIndex"/>
        int SelectedFilterIndex { get; set; }

        /// <inheritdoc cref="FileDialog.FileName"/>
        string? FileName { get; set; }
    }
}
