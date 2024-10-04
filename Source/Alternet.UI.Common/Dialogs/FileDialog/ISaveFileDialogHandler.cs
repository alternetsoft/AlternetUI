using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="IFileDialogHandler"/> with properties related to the save dialog window.
    /// </summary>
    public interface ISaveFileDialogHandler : IFileDialogHandler
    {
        /// <inheritdoc cref="SaveFileDialog.OverwritePrompt"/>
        bool OverwritePrompt { get; set; }
    }
}
