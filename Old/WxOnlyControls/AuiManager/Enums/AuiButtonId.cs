using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// These are the possible pane button identifiers in <see cref="AuiNotebook"/>
    /// and <see cref="AuiToolbar"/>
    /// </summary>
    internal enum AuiButtonId
    {
        /// <summary>
        /// Shows a close button on the pane.
        /// </summary>
        Close = 101,

        /// <summary>
        /// Shows a maximize/restore button on the pane.
        /// </summary>
        MaximizeRestore = 102,

        /// <summary>
        /// Shows a minimize button on the pane.
        /// </summary>
        Minimize = 103,

        /// <summary>
        /// Shows a pin button on the pane.
        /// </summary>
        Pin = 104,

        /// <summary>
        /// Shows an option button on the pane (not implemented).
        /// </summary>
        Options = 105,

        /// <summary>
        /// Shows a window list button on the pane (for <see cref="AuiNotebook"/>).
        /// </summary>
        WindowList = 106,

        /// <summary>
        /// Shows a left button on the pane (for <see cref="AuiNotebook"/>).
        /// </summary>
        Left = 107,

        /// <summary>
        /// Shows a right button on the pane (for <see cref="AuiNotebook"/>).
        /// </summary>
        Right = 108,

        /// <summary>
        /// Shows an up button on the pane (not implemented).
        /// </summary>
        Up = 109,

        /// <summary>
        /// Shows a down button on the pane (not implemented).
        /// </summary>
        Down = 110,

        /// <summary>
        /// Shows one of three possible custom buttons on the pane (not implemented).
        /// </summary>
        Custom1 = 201,

        /// <summary>
        /// Shows one of three possible custom buttons on the pane (not implemented).
        /// </summary>
        Custom2 = 202,

        /// <summary>
        /// Shows one of three possible custom buttons on the pane (not implemented).
        /// </summary>
        Custom3 = 203,
    }
}