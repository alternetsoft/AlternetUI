using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Describes how an object obtained focus.
    /// </summary>
    public enum FocusState
    {
        /// <summary>
        /// Object is not currently focused.
        /// </summary>
        Unfocused,

        /// <summary>
        /// Object obtained focus through a pointer action.
        /// </summary>
        Pointer,

        /// <summary>
        /// Object obtained focus through a keyboard action, such as tab sequence traversal.
        /// </summary>
        Keyboard,

        /// <summary>
        /// Object obtained focus through a deliberate call to Focus or a related API.
        /// </summary>
        Programmatic,
    }
}
