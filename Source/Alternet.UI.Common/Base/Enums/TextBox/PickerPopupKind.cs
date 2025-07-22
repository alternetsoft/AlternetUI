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
        /// Show context menu as a popup window if there are a few items,
        /// otherwise use a popup window with list box.
        /// </summary>
        Auto,

        /// <summary>
        /// Show popup window with list box.
        /// </summary>
        ListBox,

        /// <summary>
        /// Show context menu as a popup window.
        /// </summary>
        ContextMenu,

        /// <summary>
        /// No popup window is shown.
        /// </summary>
        None,
    }
}
