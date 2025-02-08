#define ObsoleteModalDialogs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with system dialog window.
    /// </summary>
    public interface IDialogHandler : IDisposable
    {
        /// <summary>
        /// Gets or sets the dialog window title.
        /// </summary>
        string? Title { get; set; }

        /// <summary>
        /// Gets or sets whether or not to show the dialog window help.
        /// </summary>
        bool ShowHelp { get; set; }

        /// <summary>
        /// Shows dialog on screen asynchroniusly.
        /// </summary>
        /// <param name="owner">Dialog owner.</param>
        /// <param name="onClose">Action to call after dialog is closed.</param>
        void ShowAsync(Window? owner, Action<bool>? onClose);
    }
}
