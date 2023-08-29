using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Keyboard actions used in <see cref="PropertyGrid.AddActionTrigger"/>
    /// and <see cref="PropertyGrid.ClearActionTriggers"/>.
    /// </summary>
    public enum PropertyGridKeyboardAction
    {
        /// <summary>
        /// Invalid action.
        /// </summary>
        ActionInvalid = 0,

        /// <summary>
        /// Select the next property.
        /// </summary>
        ActionNextProperty,

        /// <summary>
        /// Select the previous property.
        /// </summary>
        ActionPrevProperty,

        /// <summary>
        /// Expand the selected property, if it has child items.
        /// </summary>
        ActionExpandProperty,

        /// <summary>
        /// Collapse the selected property, if it has child items.
        /// </summary>
        ActionCollapseProperty,

        /// <summary>
        /// Cancel and undo any editing done in the currently active property editor.
        /// </summary>
        ActionCancelEdit,

        /// <summary>
        /// Move focus to the editor control of the currently selected property.
        /// </summary>
        ActionEdit,

        /// <summary>
        /// Causes editor's button (if any) to be pressed.
        /// </summary>
        ActionPressButton,
    }
}
