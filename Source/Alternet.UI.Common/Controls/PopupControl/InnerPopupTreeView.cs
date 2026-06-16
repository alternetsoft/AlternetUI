using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a tree view control used in inner popups.
    /// This control is supposed to be shown inside the client area of another control.
    /// </summary>
    public partial class InnerPopupTreeView : ResizablePopupControl<StdTreeView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InnerPopupTreeView"/> class.
        /// </summary>
        public InnerPopupTreeView()
        {
        }
    }
}
