using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies an icon to display in a <see cref="MessageBox"/>.
    /// </summary>
    public enum MessageBoxIcon
    {
        /// <summary>
        /// The message box contains no icon where possible.
        /// </summary>
        None,

        /// <summary>
        /// The message box contains an Information icon.
        /// </summary>
        Information,

        /// <summary>
        /// The message box contains a Warning icon.
        /// </summary>
        Warning,

        /// <summary>
        /// The message box contains an Error icon.
        /// </summary>
        Error,
    }
}
