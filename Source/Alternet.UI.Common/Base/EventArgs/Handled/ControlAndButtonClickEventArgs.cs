using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="ControlAndButton.ButtonClick"/> event.
    /// </summary>
    public class ControlAndButtonClickEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Gets id of the clicked button.
        /// </summary>
        public virtual ObjectUniqueId? ButtonId { get; set; }
    }
}
