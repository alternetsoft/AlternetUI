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
        /// <summary>
        /// Gets the initial directory for the select directory dialog.
        /// </summary>
        /// <returns></returns>
        string? GetInitialDirectory();

        /// <summary>
        /// Sets the initial directory for the select directory dialog.
        /// </summary>
        /// <param name="value"></param>
        void SetInitialDirectory(string? value);

        /// <summary>
        /// Gets the directory name for the select directory dialog.
        /// </summary>
        /// <returns></returns>
        string? GetDirectoryName();

        /// <summary>
        /// Sets the directory name for the select directory dialog.
        /// </summary>
        /// <param name="value">The directory name.</param>
        void SetDirectoryName(string? value);
    }
}
