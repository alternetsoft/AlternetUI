using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Flags for <see cref="PropertyGrid.SaveEditableState"/> and
    /// <see cref="PropertyGrid.RestoreEditableState"/>.
    /// </summary>
    public enum PropertyGridEditableState
    {
        /// <summary>
        /// Include selected property.
        /// </summary>
        SelectionState = 0x01,

        /// <summary>
        /// Include expanded/collapsed property information.
        /// </summary>
        ExpandedState = 0x02,

        /// <summary>
        /// Include scrolled position.
        /// </summary>
        ScrollPosState = 0x04,

        /// <summary>
        /// Include selected page information.
        /// Only applies to wxPropertyGridManager.
        /// </summary>
        PageState = 0x08,

        /// <summary>
        /// Include splitter position. Stored for each page.
        /// </summary>
        SplitterPosState = 0x10,

        /// <summary>
        /// Include description box size.
        /// Only applies to wxPropertyGridManager.
        /// </summary>
        DescBoxState = 0x20,

        /// <summary>
        /// Include all supported user editable state information.
        /// This is usually the default value.
        /// </summary>
        AllStates = SelectionState | ExpandedState | ScrollPosState | PageState |
                           SplitterPosState | DescBoxState,
    }
}
