using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a resizable popup control with a window like border and title bar.
    /// </summary>
    public partial class ResizablePopupControl : PopupControl
    {
        private readonly ResizableWindowBorder border = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizablePopupControl"/> class.
        /// </summary>
        public ResizablePopupControl()
        {
            border.ParentFont = true;
            border.Parent = this;
        }

        /// <summary>
        /// Gets the border control of the popup.
        /// </summary>
        public ResizableWindowBorder BorderControl => border;
    }
}
