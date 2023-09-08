using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies constants defining which buttons to display on a <see cref="MessageBox"/>.
    /// </summary>
    public enum MessageBoxButtons
    {
        /// <summary>
        /// The message box contains an OK button.
        /// </summary>
        OK,

        /// <summary>
        /// The message box contains OK and Cancel buttons.
        /// </summary>
        OKCancel,

        /// <summary>
        /// The message box contains Yes, No and Cancel buttons.
        /// </summary>
        YesNoCancel,

        /// <summary>
        /// The message box contains Yes and No buttons.
        /// </summary>
        YesNo,
    }
}
