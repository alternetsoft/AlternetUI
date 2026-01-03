using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents the view container for an inner popup toolbar within the user interface.
    /// <see cref="InnerPopupToolBarContainerView"/> doesn't create its own control but serves as a bridge
    /// to the underlying <see cref="Alternet.UI.InnerPopupToolBar"/> control.
    /// Do not use this class directly in your code, it is used by the context menu system.
    /// </summary>
    public partial class InnerPopupToolBarContainerView : Alternet.UI.ControlView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InnerPopupToolBarContainerView"/> class.
        /// </summary>
        public InnerPopupToolBarContainerView()
        {
        }
    }
}
