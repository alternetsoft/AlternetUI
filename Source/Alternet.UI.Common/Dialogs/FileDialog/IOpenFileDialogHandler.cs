using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="IFileDialogHandler"/> with properties related to the open file dialog window.
    /// </summary>
    public interface IOpenFileDialogHandler : IFileDialogHandler
    {
        /// <inheritdoc cref="OpenFileDialog.FileMustExist"/>
        bool FileMustExist { get; set; }

        /// <inheritdoc cref="OpenFileDialog.AllowMultipleSelection"/>
        bool AllowMultipleSelection { get; set; }

        /// <inheritdoc cref="OpenFileDialog.FileNames"/>
        string[] FileNames { get; }
    }
}
