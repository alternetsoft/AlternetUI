using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies action which is performed on property value validation error.
    /// Used in <see cref="PropertyGrid"/>.
    /// </summary>
    [Flags]
    public enum PropertyGridValidationFailure
    {
        /// <summary>
        /// Prevents user from leaving property unless value is valid. If this
        /// behaviour flag is not used, then value change is instead cancelled.
        /// </summary>
        StayInProperty = 0x01,

        /// <summary>
        /// Beeps on validation failure.
        /// </summary>
        Beep = 0x02,

        /// <summary>
        /// Cell with invalid value will be marked (with red color).
        /// </summary>
        MarkCell = 0x04,

        /// <summary>
        /// Display a text message explaining the situation.
        /// Default behaviour is to display the text on
        /// the top-level frame's status bar, if present, and otherwise
        /// using MessageBox.
        /// </summary>
        ShowMessage = 0x08,

        /// <summary>
        /// Similar to <see cref="ShowMessage"/>, except always displays the
        /// message using MessageBox.
        /// </summary>
        ShowMessagebox = 0x10,

        /// <summary>
        /// Similar to <see cref="ShowMessage"/>, except always displays the
        /// message on the status bar.
        /// </summary>
        ShowMessageOnStatusbar = 0x20,

        /// <summary>
        /// Default value.
        /// </summary>
        Default = MarkCell | ShowMessagebox,
    }
}
