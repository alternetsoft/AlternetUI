using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies look and behavior of the <see cref="DateTimePicker"/> control
    /// in the date editing mode.
    /// </summary>
    public enum DateTimePickerPopupKind
    {
        /// <summary>
        /// Date is edited in the style which is default for the operating system.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Date is edited using spin editor.
        /// </summary>
        Spin = 1,

        /// <summary>
        /// Date is edited in the text editor with calendar popup.
        /// </summary>
        DropDown = 2,
    }
}
