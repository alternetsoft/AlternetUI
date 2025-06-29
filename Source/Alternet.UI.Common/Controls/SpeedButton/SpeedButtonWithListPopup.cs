using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a specialized button control that displays a popup list when clicked.
    /// </summary>
    /// <remarks>This class is a non-generic version of <see cref="SpeedButtonWithListPopup{T}"/>
    /// and uses <see cref="VirtualListBox"/> as the default type for the popup list.
    /// It provides functionality for scenarios
    /// where a quick selection from a list is required.</remarks>
    public class SpeedButtonWithListPopup : SpeedButtonWithListPopup<VirtualListBox>
    {
        /// <summary>
        /// Represents the kind of popup window used by the control.
        /// </summary>
        public enum PopupWindowKind
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
}
