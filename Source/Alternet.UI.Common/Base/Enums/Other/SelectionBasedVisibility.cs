using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies when an item's border should be visible based on selection mode.
    /// </summary>
    public enum SelectionBasedVisibility
    {
        /// <summary>Border is never shown.</summary>
        None,

        /// <summary>Border is shown only when single item selection mode is active.</summary>
        Single,

        /// <summary>Border is shown only when multiple items selection mode is active.</summary>
        Multiple,

        /// <summary>Border is always shown regardless of selection mode.</summary>
        Always
    }
}
