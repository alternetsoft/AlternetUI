using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with select directory dialog window.
    /// </summary>
    public interface ISelectDirectoryDialogHandler : IDialogHandler
    {
        /// <inheritdoc cref="SelectDirectoryDialog.InitialDirectory"/>
        string? InitialDirectory { get; set; }

        /// <inheritdoc cref="SelectDirectoryDialog.DirectoryName"/>
        string? DirectoryName { get; set; }
    }
}
