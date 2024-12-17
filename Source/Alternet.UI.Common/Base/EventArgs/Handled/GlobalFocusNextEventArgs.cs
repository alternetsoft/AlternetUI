using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides event arguments for the
    /// <see cref="AbstractControl.GlobalFocusNextControl"/> event.
    /// </summary>
    public class GlobalFocusNextEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalFocusNextEventArgs"/> class.
        /// </summary>
        /// <param name="forward"><see langword="true"/> to move forward in the
        /// tab order; <see langword="false"/> to move backward in the tab
        /// order.</param>
        /// <param name="nested"><see langword="true"/> to include nested
        /// (children of child controls) child controls; otherwise,
        /// <see langword="false"/>.</param>
        public GlobalFocusNextEventArgs(bool forward, bool nested)
        {
            Forward = forward;
            Nested = nested;
        }

        /// <summary>
        /// Gets whether to move forward or backward in the
        /// tab order.
        /// </summary>
        public bool Forward { get; set; }

        /// <summary>
        /// Gets whether to include nested (children of child controls) child controls.
        /// </summary>
        public bool Nested { get; set; }
    }
}