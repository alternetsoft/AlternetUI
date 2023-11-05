using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies an icon to display in a <see cref="MessageBox"/> or any other places.
    /// </summary>
    public enum MessageBoxIcon
    {
        /// <summary>
        /// The message box contains no icon where possible.
        /// </summary>
        None,

        /// <summary>
        /// The message box contains an 'Information' icon.
        /// </summary>
        Information,

        /// <summary>
        /// The message box contains a 'Warning' icon.
        /// </summary>
        Warning,

        /// <summary>
        /// The message box contains an 'Error' icon.
        /// </summary>
        Error,

        /// <summary>
        /// Displays a question mark symbol. This style is not supported for message dialogs under
        /// Windows when a task dialog is used to implement them (i.e. when running under
        /// Windows Vista or later) because Microsoft guidelines indicate that no icon
        /// should be used for routine confirmations. If it is specified, no icon will be displayed.
        /// </summary>
        Question,
    }
}
