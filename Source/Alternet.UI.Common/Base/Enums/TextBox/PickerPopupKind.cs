using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the kind of popup window used by the control.
    /// </summary>
    public enum PickerPopupKind
    {
        /// <summary>
        /// Popup window inherited from <see cref="PopupListBox"/>.
        /// </summary>
        ListBox,

        /// <summary>
        /// Use context menu as a popup window.
        /// </summary>
        ContextMenu,
    }
}
